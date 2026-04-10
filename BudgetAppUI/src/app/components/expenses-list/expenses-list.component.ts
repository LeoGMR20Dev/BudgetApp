import { AsyncPipe, CurrencyPipe, NgClass, PercentPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IBudgetData } from 'src/app/interfaces/budget.interface';
import { BudgetService } from 'src/app/services/budget.service';

@Component({
  selector: 'expenses-list',
  imports: [CurrencyPipe, AsyncPipe, NgClass, PercentPipe],
  templateUrl: './expenses-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExpensesListComponent {
  private budgetService = inject(BudgetService);
  budgetData$: Observable<IBudgetData | null> = this.budgetService.budgetData$;
}
