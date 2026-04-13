import { Types } from '@constants/types';

export type TBackendErrors = {
  description: string | null;
  amount: string | null;
  type: string | null;
};

export interface IAddBudgetTransaction {
  description: string;
  amount: number;
  type: Types;
}

export interface IBudgetDataTransaction {
  id: string;
  description: string;
  amount: number;
  type: string;
}

export interface IIncomes extends IBudgetDataTransaction {}

export interface IExpenses extends IBudgetDataTransaction {
  percentage: number;
}

export interface IBudgetData {
  totalIncomes: number;
  totalExpenses: number;
  totalExpensesPercentage: number;
  availableBudget: number;
  incomes: IIncomes[];
  expenses: IExpenses[];
}
