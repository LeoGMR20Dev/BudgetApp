import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Observable } from 'rxjs';
import { Types } from 'src/app/constants/types';
import {
  IBudgetData,
  TBackendErrors,
} from 'src/app/interfaces/budget.interface';
import { TApiErrorResponse } from 'src/app/interfaces/response.interface';
import { BudgetService } from 'src/app/services/budget.service';
import { greaterThan } from 'src/app/validators/greaterThan';
import Swal from 'sweetalert2';

@Component({
  selector: 'add-transaction',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-transaction.component.html',
  styleUrl: 'add-transaction.component.css',
})
export class AddTransactionComponent {
  private budgetService = inject(BudgetService);
  public budgetData$: Observable<IBudgetData | null> =
    this.budgetService.budgetData$;
  private fb = inject(FormBuilder);
  public transactionForm: FormGroup;
  public transactionTypes = Types;
  private initialBackendErrors: TBackendErrors = {
    description: null,
    amount: null,
    type: null,
  };
  public backendErrors: TBackendErrors = {
    ...this.initialBackendErrors,
  };

  constructor() {
    this.transactionForm = this.fb.group({
      description: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(255),
        ],
      ],
      amount: [
        null,
        [Validators.required, greaterThan(0), Validators.max(1_000_000)],
      ],
      type: [this.transactionTypes.INCOME, [Validators.required]],
    });
  }

  onSubmit(): void {
    if (this.transactionForm.valid) {
      const typeTransaction = this.transactionForm.get('type')?.value;

      this.backendErrors = { ...this.initialBackendErrors };

      this.budgetService.addTransaction(this.transactionForm.value).subscribe({
        next: () => {
          this.transactionForm.reset({
            description: '',
            amount: null,
            type: this.transactionTypes.INCOME,
          });

          Swal.fire({
            title: 'Éxito',
            text: `${
              typeTransaction === this.transactionTypes.INCOME
                ? 'Ingreso'
                : typeTransaction === this.transactionTypes.EXPENSE
                  ? 'Egreso'
                  : 'Transacción'
            } ${
              typeTransaction === this.transactionTypes.INCOME ||
              typeTransaction === this.transactionTypes.EXPENSE
                ? 'registrado'
                : 'registrada'
            } correctamente`,
            icon: 'success',
            confirmButtonText: 'Aceptar',
            confirmButtonColor: 'green',
          });
        },
        error: (err) => {
          const error = err.error as TApiErrorResponse;
          const lstErrors = error.errors;

          if (lstErrors) {
            Object.keys(lstErrors).forEach((field) => {
              const control = this.transactionForm.get(field);
              const key = field as keyof TBackendErrors;

              if (control) {
                this.backendErrors[key] = lstErrors[field][0];
              }
            });
          }

          Swal.fire({
            title: error.title || 'Error',
            text: error.detail || 'Ocurrió un error inesperado',
            icon: 'error',
            confirmButtonText: 'Aceptar',
          });
        },
      });
    }
  }

  getErrorMessage(fieldname: string): string {
    const control = this.transactionForm.get(fieldname);

    if (control?.hasError('required')) {
      return `${fieldname} es requerido`;
    }

    if (control?.hasError('minlength')) {
      const minLength = control.errors?.['minlength'].requiredLength;
      return `${fieldname} debe tener al menos ${minLength} caracteres`;
    }

    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `${fieldname} debe tener un máximo de ${maxLength} caracteres`;
    }

    if (control?.hasError('greaterThan')) {
      const requiredValue = control.errors?.['greaterThan'].requiredValue;
      return `${fieldname} debe ser mayor a ${requiredValue}`;
    }

    if (control?.hasError('max')) {
      const max = control.errors?.['max'].max;
      return `${fieldname} debe ser mayor a ${max}`;
    }

    return '';
  }

  isFieldInvalid(fieldname: string): boolean {
    const control = this.transactionForm.get(fieldname);
    return !!control?.invalid && control?.touched;
  }
}
