import { Injectable } from '@angular/core';
import {BehaviorSubject, map, Observable, of, switchMap, tap} from 'rxjs';
import {LoginDto, RegisterDto, TokenData} from '../domain/domain';
import {BackendService} from './backend.service';
import {Hasher} from './security/hasher';
import {TokenParser} from './security/token-parser';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authState: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
  private jwtData: TokenData | null = null;

  constructor(private backend: BackendService) {
    const token = this.getToken();
    this.authState.next(token);
    this.authState.subscribe((token) => this.updateTokenData(token));
  }

  public getToken(): string | null {
    // Retrieve from session storage if needed
    return this.authState.value || sessionStorage.getItem('token');
  }

  public getTokenData(): TokenData | null {
    return this.jwtData;
  }

  get isAuthenticated$() {
    return this.authState.asObservable().pipe(
      map((token) => {

        if (token === null || !this.jwtData) {
          return false;
        }

        const decoded = this.jwtData;

        // Check expiration (exp) claim
        const now = new Date();
        if (decoded?.exp && decoded.exp < now) {
          this.logout();
          return false;
        }

        // Check not before (nbf) claim
        if (decoded?.nbf && decoded.nbf > now) {
          this.logout();
          return false;
        }

        const issuer = decoded.iss === 'Issuer'; // Check issuer (iss) claim
        const audience = decoded.aud === 'Audience'; // Check audience (aud) claim

        return issuer && audience;
      }
    ));
  }

  private updateTokenData(token: string | null) {
    if (!token) {
      this.jwtData = null;
      return;
    }
    try {
      this.jwtData = TokenParser.parseToken(token);
    }
    catch (e) { // Issue with token
      this.logout();
    }
  }

  login(dto: LoginDto): Observable<boolean> {
    return Hasher.hash(dto.plainPassword).pipe(
      switchMap((hashedPassword) => {
        dto.plainPassword = hashedPassword;
        return this.backend.login(dto);
      }),
      tap((response) => {
        if (response.jwt) {
          sessionStorage.setItem('token', response.jwt);
          this.authState.next(response.jwt);
          this.jwtData = TokenParser.parseToken(response.jwt);
        }
      }),
      switchMap((response) => of(response.jwt !== null))
    );
  }

  public logout() {
    // Clear the token
    sessionStorage.removeItem('token');
    this.authState.next(null);
  }

  register(dto: RegisterDto): Observable<boolean> {
    return Hasher.hash(dto.plainPassword).pipe(
      switchMap((hashedPassword) => {
        dto.plainPassword = hashedPassword;
        return this.backend.register(dto);
      }));
  }
}
