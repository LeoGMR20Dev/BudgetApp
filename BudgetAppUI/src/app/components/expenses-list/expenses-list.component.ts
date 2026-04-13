import { AsyncPipe, CurrencyPipe, NgClass, PercentPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  ViewChild,
} from '@angular/core';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { Observable } from 'rxjs';
import {
  IBudgetData,
  IBudgetDataTransaction,
} from 'src/app/interfaces/budget.interface';
import { TApiErrorResponse } from 'src/app/interfaces/response.interface';
import { BudgetService } from 'src/app/services/budget.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'expenses-list',
  imports: [CurrencyPipe, AsyncPipe, NgClass, PercentPipe, SwalComponent],
  templateUrl: './expenses-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExpensesListComponent {
  private budgetService = inject(BudgetService);
  budgetData$: Observable<IBudgetData | null> = this.budgetService.budgetData$;

  selectedTransaction: IBudgetDataTransaction | null = null;

  @ViewChild('deleteExpense')
  deleteExpense!: SwalComponent;

  openDeleteModal(transaction: IBudgetDataTransaction) {
    this.selectedTransaction = transaction;
    this.deleteExpense.fire();
  }

  confirmDelete() {
    if (!this.selectedTransaction) return;

    this.onDelete(this.selectedTransaction.id, this.selectedTransaction.type);

    this.selectedTransaction = null;
  }

  onDelete(id: string, type: string): void {
    this.budgetService.deleteTransaction(id, type).subscribe({
      next: () => {
        Swal.fire({
          title: 'Éxito',
          text: 'Egreso eliminado correctamente',
          icon: 'success',
          confirmButtonText: 'Aceptar',
          confirmButtonColor: 'green',
        });
      },
      error: (err: HttpErrorResponse) => {
        const error = err.error as TApiErrorResponse;

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
