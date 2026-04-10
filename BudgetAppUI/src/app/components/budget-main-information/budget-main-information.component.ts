import { AsyncPipe, CurrencyPipe, PercentPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IBudgetData } from 'src/app/interfaces/budget.interface';
import { BudgetService } from 'src/app/services/budget.service';

@Component({
  selector: 'budget-main-information',
  imports: [AsyncPipe, CurrencyPipe, PercentPipe],
  templateUrl: './budget-main-information.component.html',
})
export class BudgetMainInformationComponent {
  private budgetService = inject(BudgetService);
  budgetData$: Observable<IBudgetData | null> = this.budgetService.budgetData$;
}
