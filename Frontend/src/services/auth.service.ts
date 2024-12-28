import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable, of} from 'rxjs';
import {LoginDto} from '../domain/domain';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authState: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);

  constructor() { }

  get isAuthenticated$() {
    return this.authState.asObservable();
  }


  login(dto: LoginDto): Observable<boolean> {
    // Call the backend to authenticate
    // If successful, set the auth state to true
    this.authState.next(true);

    return of(true);
  }
}
