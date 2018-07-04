import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class StateService {
  private state: any = new Object();

  setValue(key: string, value: any) {
    this.state[key] = value;
  }

  getValue(key: string) {
    return this.state[key];
  }
}
