import { AsyncPipe, CurrencyPipe, NgClass } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IBudgetData } from 'src/app/interfaces/budget.interface';
import { BudgetService } from 'src/app/services/budget.service';

@Component({
  selector: 'incomes-list',
  imports: [AsyncPipe, CurrencyPipe, NgClass],
  templateUrl: './incomes-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IncomesListComponent {
  private budgetService = inject(BudgetService);
  budgetData$: Observable<IBudgetData | null> = this.budgetService.budgetData$;
}
