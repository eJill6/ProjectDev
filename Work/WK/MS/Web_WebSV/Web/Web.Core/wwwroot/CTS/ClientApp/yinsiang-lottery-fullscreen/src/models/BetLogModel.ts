export interface BetsLogDetail {
  category: string;
  count: number;
  values: {
    value: string;
    totalAmount: string;
  }[];
}

export interface BetsLog {
  numberOdds: BetsLogDetail[];
}
