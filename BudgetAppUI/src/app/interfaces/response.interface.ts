export type TApiErrorResponse = {
  title: string;
  status: number;
  detail: string;
  errors: Record<string, string[]>;
};
