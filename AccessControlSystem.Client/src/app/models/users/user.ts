import { BaseModel } from "../shared/base-model.model";

export class User extends BaseModel<number> {
  userName!: string;
  email!: string;
  phoneNumber!: string;
  roleId!: string;
  password!: string; 
  confirmPassword?: string;
}
