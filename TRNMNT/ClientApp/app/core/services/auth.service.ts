import { HttpClient } from '@angular/common/http';
import { Injectable, Output, EventEmitter } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService as SocialAuthService, FacebookLoginProvider } from 'angular5-social-login';
import { SocialUser } from 'angular5-social-login';
import { from, Observable, of, throwError } from 'rxjs';
import { catchError, flatMap, map } from 'rxjs/operators';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { RefreshTokenModel } from '../model/auth.models';
import { AuthTokenModel } from '../model/auth.models';
import { CredentialsModel } from '../model/credentials.model';
import { UserModel, UserModelRegistration } from '../model/user.models';
import { RouterService } from './router.service';

/**
 * Authentication service.
 */
@Injectable()
export class AuthService {
  /**
   * Stores the URL so we can redirect after signing in.
   */
  redirectUrl: string;

  @Output()
  getLoggedInStatus: EventEmitter<boolean> = new EventEmitter();
  /**
   * User's data.
   */
  private user: any = {};

  private headers: Headers;

  constructor(
    private http: HttpClient,
    private socialAuthService: SocialAuthService,
    private jwtHelper: JwtHelperService,
    private routerService: RouterService
  ) {
    this.decodeToken();
  }

  /**
   * Tries to sign in the user.
   *
   * @param email
   * @param password
   * @return The user's data
   */
  signin(email: string, password: string): Observable<boolean> {
    const credentials: CredentialsModel = {
      email,
      password
    };

    return this.http.post<AuthTokenModel>(ApiMethods.auth.getToken, credentials).pipe(
      map(tokenModel => {
        this.getLoggedInStatus.emit(true);
        return this.processTokensResponse(tokenModel);
      }),
      catchError((error: any) => {
        // Error on post request.
        if (error instanceof Response) {
          if (error.status === 401) {
            return of(false);
          } else {
            return throwError(error);
          }
        } else {
          return throwError(error);
        }
      })
    );
  }

  /**
   * Tries to get a new token using refresh token.
   */
  getNewToken(): Observable<boolean> {
    const refreshToken: string = localStorage.getItem('refreshToken');

    if (refreshToken != null) {
      return this.http.post<AuthTokenModel>(ApiMethods.auth.refreshToken, { refreshToken }).pipe(
        map(r => {
          return this.processTokensResponse(r);
        })
      );
    }
  }

  /**
   * Indicates if token is not expired
   */
  isLoggedIn(): boolean {
    const token = this.jwtHelper.tokenGetter();
    if (token) {
      return !this.jwtHelper.isTokenExpired(token);
    }
    return false;
  }

  /**
   * Revokes token.
   */
  revokeToken(): void {
    const token = this.jwtHelper.tokenGetter();
    if (token) {
      // Revocation endpoint & params.
      const revocationEndpoint: string = ApiMethods.auth.refreshToken;

      const params: any = {
        refreshToken: token,
        token_type_hint: 'idToken'
      };

      // Encodes the parameters.
      const body = JSON.stringify(params);

      this.http.post<AuthTokenModel>(revocationEndpoint, body).subscribe(() => {
        localStorage.removeItem('idToken');
      });
    }
  }

  /**
   * Revokes refresh token.
   */
  revokeRefreshToken(): Observable<boolean> {
    const refreshToken: string = localStorage.getItem('refreshToken');

    if (refreshToken == null) {
      return of(false);
    }

    // Revocation endpoint & params.
    const revocationEndpoint: string = ApiMethods.auth.refreshToken;

    const params: RefreshTokenModel = {
      refreshToken
    };

    // Encodes the parameters.
    const body = JSON.stringify(params);

    return this.http.post<AuthTokenModel>(revocationEndpoint, body).pipe(
      map((res: AuthTokenModel) => {
        // Successful if there's an access token in the response.
        if (res.idToken) {
          // Stores access token & refresh token.
          this.store(res);
          return true;
        }
        return false;
      })
    );
  }

  /**
   * Removes user and revokes tokens.
   */
  signout(): void {
    this.redirectUrl = null;
    this.user = {};
    this.removeTokens();
    this.getLoggedInStatus.emit(false);
    this.routerService.goHome();
  }

  /**
   * Gets user's data.
   *
   * @return The user's data
   */
  getUser(): UserModel {
    if (this.isLoggedIn()) {
      var userModel = new UserModel();
      userModel.userId = this.user.userId;
      userModel.firstName = this.user.firstName;
      userModel.lastName = this.user.lastName;
      userModel.email = this.user.email;
      userModel.roles = this.user.role;
      userModel.dateOfBirth = new Date(this.user.dateOfBirth);
      return userModel;
    }
  }

  getRoles(): string[] {
    if (this.isLoggedIn()) {
      var roles = this.getUser().roles;
      if (Array.isArray(roles)) {
        return roles;
      }
      return [roles];
    }
    return [];
  }

  ifRolesMatch(roles: string[]): boolean {
    if (this.isLoggedIn()) {
      return this.getRoles().filter(r => roles.indexOf(r) !== -1).length > 0;
    }
    return false;
  }

  facebookLogin(): Observable<boolean> {
    console.log(`${this.routerService.getMainDomainUrl()}/${ApiMethods.auth.facebookLogin}`);
    const socialPlatformProvider = FacebookLoginProvider.PROVIDER_ID;
    return from(this.socialAuthService.signIn(socialPlatformProvider)).pipe(
      flatMap((userData: SocialUser) => {
        return this.http.post<AuthTokenModel>(
          `${this.routerService.getMainDomainUrl()}/${ApiMethods.auth.facebookLogin}`,
          { token: userData.token }
        );
      }),
      map(r => {
        return this.processTokensResponse(r);
      }),
      catchError(e => {
        return throwError(e);
      })
    );
  }

  /**
   * Decodes token through JwtHelper.
   */
  private decodeToken(): void {
    if (this.isLoggedIn()) {
      const token: string = localStorage.getItem('idToken');
      this.user = this.jwtHelper.decodeToken(token);
    }
  }

  /**
   * // Encodes the parameters.
   *
   * @param params The parameters to be encoded
   * @return The encoded parameters
   */
  private encodeParams(params: any): string {
    let body: string = '';
    for (const key in params) {
      if (body.length) {
        body += '&';
      }
      body += key + '=';
      body += encodeURIComponent(params[key]);
    }
    return body;
  }

  /**
   * Stores access token & refresh token.
   *
   * @param tokenModel The response of the request to the token endpoint
   */
  private store(tokenModel: AuthTokenModel): void {
    // Stores access token in local storage to keep user signed in.
    localStorage.setItem('idToken', tokenModel.idToken);
    // Stores refresh token in local storage.
    localStorage.setItem('refreshToken', tokenModel.refreshToken);

    // Decodes the token.
    this.decodeToken();
  }

  private processTokensResponse(body): boolean {
    if (typeof body.idToken !== 'undefined') {
      // Stores access token & refresh token.
      this.store(body);
      this.getLoggedInStatus.emit(true);
      return true;
    }
    return false;
  }

  private removeTokens() {
    localStorage.removeItem('idToken');
    localStorage.removeItem('refreshToken');
  }
}
