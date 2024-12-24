import { Injectable } from '@angular/core';
import {BehaviorSubject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authState: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor() { }

  get isAuthenticated$() {
    return this.authState.asObservable();
  }
}
