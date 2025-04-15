import { BaseModel } from "../shared/base-model.model";

export class Subscription extends BaseModel<number> {
  customerName!: string;
  subscriptionType!: number;
  deviceNumber!: number;
  paymentPerMonth!: number;
  startDate!: Date;
  endDate!: Date;
  note!: string;
}
