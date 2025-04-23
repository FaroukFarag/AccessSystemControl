export class BaseModel<T> {
    id!: T;
    [key: string]: any;
  }
  