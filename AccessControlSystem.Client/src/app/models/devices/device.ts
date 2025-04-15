import { BaseModel } from "../shared/base-model.model";

export class Device extends BaseModel<number> {
  name!: string;
  macAddress!: string;
}
