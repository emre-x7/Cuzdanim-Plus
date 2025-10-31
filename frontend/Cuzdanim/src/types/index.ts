// Enums (Backend'den) - Const Object olarak
export const AccountType = {
  BankAccount: 1,
  CreditCard: 2,
  Cash: 3,
  Wallet: 4,
  Investment: 5,
} as const;

export const TransactionType = {
  Income: 1,
  Expense: 2,
  Transfer: 3,
} as const;

export const Currency = {
  TRY: 1,
  USD: 2,
  EUR: 3,
  GBP: 4,
  GOLD: 5,
} as const;

export const GoalStatus = {
  Active: 1,
  Completed: 2,
  Cancelled: 3,
  Paused: 4,
} as const;

export interface ApiResult<T = void> {
  isSuccess: boolean;
  message: string;
  data?: T;
  errors?: string[];
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber?: string;
  dateOfBirth?: string;
}

export interface LoginResponse {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

// User Types
export interface User {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
}

// Account Types
export interface Account {
  id: string;
  name: string;
  type: string;
  balance: number;
  currency: string;
  bankName?: string;
  iban?: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreateAccountRequest {
  name: string;
  type: AccountType;
  initialBalance: number;
  currency: Currency;
  bankName?: string;
  iban?: string;
  cardLastFourDigits?: string;
  creditLimit?: number;
}

export interface UpdateAccountRequest {
  name: string;
  isActive: boolean;
  includeInTotalBalance: boolean;
}

// Transaction Types
export interface Transaction {
  id: string;
  accountId: string;
  accountName: string;
  categoryId: string;
  categoryName: string;
  type: string;
  amount: number;
  currency: string;
  transactionDate: string;
  description?: string;
  notes?: string;
  receiptUrl?: string;
  tags: string[];
  isAutoCategorized: boolean;
  createdAt: string;
}

// Transaction Request/Response Types
export interface CreateTransactionRequest {
  type: number;
  amount: number;
  accountId: string;
  categoryId: string;
  transactionDate: string;
  currency: number;
  description?: string;
  notes?: string;
}

export interface UpdateTransactionRequest {
  amount: number;
  categoryId: string;
  transactionDate: string;
  currency: number;
  description?: string;
  notes?: string;
}

export interface GetTransactionsQuery {
  startDate: string;
  endDate: string;
  accountId?: string;
  categoryId?: string;
}

// Budget Types
export interface Budget {
  id: string;
  name: string;
  categoryId: string;
  categoryName: string;
  categoryIcon?: string;
  categoryColor?: string;
  amount: number;
  currency: string;
  periodStartDate: string;
  periodEndDate: string;
  alertThresholdPercentage: number;
  alertWhenExceeded: boolean;
  isActive: boolean;
  createdAt: string;
}

export interface CreateBudgetRequest {
  categoryId: string;
  name: string;
  amount: number;
  currency: Currency;
  startDate: string;
  endDate: string;
  alertThresholdPercentage: number;
}

export interface UpdateBudgetRequest {
  name: string;
  amount: number;
  currency: Currency;
  startDate: string;
  endDate: string;
  alertThresholdPercentage: number;
}

// Goal Types
export interface Goal {
  id: string;
  name: string;
  description?: string;
  targetAmount: number;
  currentAmount: number;
  currency: string;
  targetDate: string;
  status: string;
  progressPercentage: number;
  daysRemaining: number;
  remainingAmount: number;
  imageUrl?: string;
  icon?: string;
  isShared: boolean;
  createdAt: string;
}

export interface CreateGoalRequest {
  name: string;
  description?: string;
  targetAmount: number;
  currency: Currency;
  targetDate: string;
  imageUrl?: string;
  icon?: string;
}

export interface UpdateGoalRequest {
  name: string;
  description?: string;
  targetAmount: number;
  currency: Currency;
  targetDate: string;
}

export interface AddContributionRequest {
  amount: number;
  currency: Currency;
}

// Dashboard Types
export interface DashboardData {
  totalBalance: number;
  currency: string;
  currentMonthIncome: number;
  currentMonthExpense: number;
  currentMonthNet: number;
  lastMonthIncome: number;
  lastMonthExpense: number;
  incomeChangePercentage: number;
  expenseChangePercentage: number;
  totalAccounts: number;
  activeAccounts: number;
  totalGoals: number;
  activeGoals: number;
  completedGoalsThisMonth: number;
  budgetAlerts: BudgetAlert[];
  recentTransactions: RecentTransaction[];
}

export interface BudgetAlert {
  budgetId: string;
  budgetName: string;
  categoryName: string;
  budgetAmount: number;
  spentAmount: number;
  spentPercentage: number;
  alertLevel: string;
}

export interface RecentTransaction {
  id: string;
  type: string;
  categoryName: string;
  categoryIcon: string;
  amount: number;
  description: string;
  transactionDate: string;
}

export interface Category {
  id: string;
  name: string;
  transactionType: "Income" | "Expense";
  type: string;
  icon?: string;
  color?: string;
  isActive: boolean;
}

// BUDGET TYPES
export interface Budget {
  id: string;
  userId: string;
  categoryId: string;
  categoryName: string;
  categoryIcon?: string;
  categoryColor?: string;
  name: string;
  amount: number;
  spent: number;
  remaining: number;
  currency: string;
  startDate: string;
  endDate: string;
  alertThresholdPercentage: number;
  status: "Normal" | "Warning" | "Exceeded";
  percentageUsed: number;
  isActive: boolean;
  createdAt: string;
}

export interface CreateBudgetRequest {
  name: string;
  categoryId: string;
  amount: number;
  currency: Currency;
  startDate: string;
  endDate: string;
  alertThresholdPercentage: number;
}

export interface UpdateBudgetRequest {
  name: string;
  amount: number;
  currency: Currency;
  startDate: string;
  endDate: string;
  alertThresholdPercentage: number;
}

// GOAL TYPES
export interface Goal {
  id: string;
  userId: string;
  name: string;
  description?: string;
  targetAmount: number;
  currentAmount: number;
  currency: string;
  targetDate: string;
  status: string;
  icon?: string;
  progressPercentage: number;
  remainingAmount: number;
  daysRemaining: number;
  isActive: boolean;
  createdAt: string;
}

export interface CreateGoalRequest {
  name: string;
  description?: string;
  targetAmount: number;
  currency: Currency;
  targetDate: string;
  icon?: string;
}

export interface UpdateGoalRequest {
  name: string;
  description?: string;
  targetAmount: number;
  currency: Currency;
  targetDate: string;
  icon?: string;
}

export interface AddGoalContributionRequest {
  amount: number;
  note?: string;
  currency: Currency;
}

// REPORT TYPES
export interface ReportSummary {
  totalIncome: number;
  totalExpense: number;
  netAmount: number;
  currency: string;
}

export interface CategoryReport {
  categoryId: string;
  categoryName: string;
  categoryIcon?: string;
  categoryColor?: string;
  totalAmount: number;
  transactionCount: number;
  percentage: number;
}

export interface MonthlyReport {
  month: string; // "2025-01"
  year: number;
  monthName: string; // "Ocak 2025"
  income: number;
  expense: number;
  net: number;
}

export interface IncomeExpenseComparison {
  income: number;
  expense: number;
}

export interface ReportData {
  summary: ReportSummary;
  incomeByCategory: CategoryReport[];
  expenseByCategory: CategoryReport[];
  monthlyTrend: MonthlyReport[];
  comparison: IncomeExpenseComparison;
}

export interface GetReportQuery {
  startDate?: string;
  endDate?: string;
}

export type AccountType = (typeof AccountType)[keyof typeof AccountType];
export type TransactionType =
  (typeof TransactionType)[keyof typeof TransactionType];
export type Currency = (typeof Currency)[keyof typeof Currency];
export type GoalStatus = (typeof GoalStatus)[keyof typeof GoalStatus];
export interface GetBudgetsQuery {}
