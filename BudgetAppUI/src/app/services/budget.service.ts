import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import {
  IAddBudgetTransaction,
  IBudgetData,
  IBudgetDataTransaction,
} from '@interfaces/budget.interface';
import { Types } from '../constants/types';

@Injectable({
  providedIn: 'root',
})
export class BudgetService {
  public httpRequest = inject(HttpClient);
  public url: string = `${environment.API_URL}`;
  
  private budgetDataSubject = new BehaviorSubject<IBudgetData | null>(null);
  budgetData$ = this.budgetDataSubject.asObservable();

  getBudgetData() {
    this.httpRequest
      .get<IBudgetData>(`${this.url}/api/Budget/getBudgetData`)
      .subscribe((data) => this.budgetDataSubject.next(data));
  }

  addTransaction(
    transaction: IAddBudgetTransaction,
  ): Observable<IBudgetDataTransaction> {
    return this.httpRequest
      .post<IBudgetDataTransaction>(`${this.url}/api/Budget`, transaction)
      .pipe(
        tap((newTransaction) => {
          const current = this.budgetDataSubject.value;

          if (!current) return;

          let updated = { ...current };

          if (newTransaction.type === Types.INCOME) {
            updated.incomes = [...(updated.incomes || []), newTransaction];
          } else if (newTransaction.type === Types.EXPENSE) {
            updated.expenses = [
              ...(updated.expenses || []),
              { ...newTransaction, percentage: 0 },
            ];
          }

          updated = this.updateBudgetData(updated);

          this.budgetDataSubject.next(updated);
        }),
      );
  }

  private updateBudgetData(budgetData: IBudgetData): IBudgetData {
    const totalIncomes = budgetData.incomes.reduce(
      (acc, val) => acc + val.amount,
      0,
    );
    const totalExpenses = budgetData.expenses.reduce(
      (acc, val) => acc + val.amount,
      0,
    );

    const availableBudget = totalIncomes - totalExpenses;

    const expenses = budgetData.expenses.map((exp) => ({
      ...exp,
      percentage: totalIncomes > 0 ? exp.amount / totalIncomes : 0,
    }));

    const totalExpensesPercentage = expenses.reduce(
      (acc, val) => acc + val.percentage,
      0,
    );

    return {
      ...budgetData,
      totalExpenses,
      totalIncomes,
      totalExpensesPercentage,
      availableBudget,
      expenses,
    };
  }
}
