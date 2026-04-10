import { Component, inject } from '@angular/core';
import { AddTransactionComponent } from './components/add-transaction/add-transaction.component';
import { BudgetMainInformationComponent } from './components/budget-main-information/budget-main-information.component';
import { ExpensesListComponent } from './components/expenses-list/expenses-list.component';
import { IncomesListComponent } from './components/incomes-list/incomes-list.component';
import { BudgetService } from './services/budget.service';
import { LoadingService } from './services/loading.service';
import { LoadingPageComponent } from './components/loading-page/loading-page.component';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [
    AddTransactionComponent,
    BudgetMainInformationComponent,
    ExpensesListComponent,
    IncomesListComponent,
    LoadingPageComponent,
    AsyncPipe,
  ],
  templateUrl: './app.component.html',
})
export class AppComponent {
  private budgetService = inject(BudgetService);
  private loadingService = inject(LoadingService);
  isloading$ = this.loadingService.isLoading$;
  title = 'App Presupuesto';

  ngOnInit() {
    this.budgetService.getBudgetData();
  }
}
