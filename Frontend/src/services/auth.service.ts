import { Injectable } from '@angular/core';
import {BehaviorSubject, map, Observable, of, switchMap, tap} from 'rxjs';
import {LoginDto, RegisterDto, TokenData} from '../domain/domain';
import {BackendService} from './backend.service';
import {Hasher} from './security/hasher';
import {TokenParser} from './security/token-parser';
import {environment} from '../environments/environment';
import {KeyMaker} from './security/key-maker';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authState: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
  private jwtData: TokenData | null = null;

  constructor(private backend: BackendService, private router: Router) {
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

        const issuer = decoded.iss === environment.iss; // Check issuer (iss) claim
        const audience = decoded.aud === environment.aud; // Check audience (aud) claim

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
    return KeyMaker.deriveAndStore$(dto.plainPassword, dto.username).pipe(
      // Hash the plain password after deriving and storing the key
      switchMap(() => Hasher.hash(dto.plainPassword, dto.username)),
      tap((hashedPassword) => {
        dto.plainPassword = hashedPassword;
      }),
      // Call the backend login method with the hashed password
      switchMap(() => this.backend.login(dto)),
      // Handle the backend response
      tap((response) => {
        if (response.jwt) {
          sessionStorage.setItem('token', response.jwt);
          this.authState.next(response.jwt);
          this.jwtData = TokenParser.parseToken(response.jwt);
        }
      }),
      // Return success based on the presence of a JWT
      map((response) => !!response.jwt)
    );
  }

  public logout() {
    // Clear the token
    sessionStorage.removeItem('token');
    this.authState.next(null);
    this.router.navigate(['login']);
  }

  register(dto: RegisterDto): Observable<boolean> {
    return Hasher.hash(dto.plainPassword, dto.username).pipe(
      switchMap((hashedPassword) => {
        dto.plainPassword = hashedPassword;
        return this.backend.register(dto);
      }));
  }
}
