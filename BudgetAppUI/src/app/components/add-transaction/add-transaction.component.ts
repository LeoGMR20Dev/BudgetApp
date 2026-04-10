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
import { IBudgetData } from 'src/app/interfaces/budget.interface';
import { BudgetService } from 'src/app/services/budget.service';
import { greaterThan } from 'src/app/validators/greaterThan';

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
      this.budgetService.addTransaction(this.transactionForm.value).subscribe({
        next: () => {
          this.transactionForm.reset({
            description: '',
            amount: null,
            type: this.transactionTypes.INCOME,
          });
        },
        error: (err) => {
          console.log('Error al agregar la transacción', err);
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
