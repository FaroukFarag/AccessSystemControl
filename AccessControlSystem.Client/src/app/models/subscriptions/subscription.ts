import { BaseModel } from "../shared/base-model.model";

export class Subscription extends BaseModel<number> {
  customerName!: string;
  subscriptionType!: number;
  adminNumber!: number;
  deviceNumber!: number;
  cardNumber!: number;
  usedAdmins!: number;
  usedDevices!: number;
  usedCards!: number;
  paymentPerMonth!: number;
  startDate!: Date;
  endDate!: Date;
  note!: string;
}
