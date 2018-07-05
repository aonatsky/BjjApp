﻿import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService as SocialAuthService, FacebookLoginProvider } from 'angular5-social-login';
import { SocialUser } from 'angular5-social-login';
import { from, Observable, of, throwError } from 'rxjs';
import { catchError, flatMap, map } from 'rxjs/operators';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { RefreshTokenModel } from '../model/auth.models';
import { AuthTokenModel } from '../model/auth.models';
import { CredentialsModel } from '../model/credentials.model';
import { UserModel } from '../model/user.models';

/**
 * Authentication service.
 */
@Injectable()
export class AuthService {
  /**
   * Stores the URL so we can redirect after signing in.
   */
  redirectUrl: string;

  /**
   * User's data.
   */
  private user: any = {};

  private headers: Headers;

  constructor(
    private http: HttpClient,
    private socialAuthService: SocialAuthService,
    private jwtHelper: JwtHelperService
  ) {
    // On bootstrap or refresh, tries to get the user's data.
    this.decodeToken();

    // Creates header for post requests.
  }

  login(email: string, password: string) {
    const credentials: CredentialsModel = {
      email,
      password
    };
    this.http.post<any>(ApiMethods.auth.getToken, credentials).subscribe(
      // We're assuming the response will be an object
      // with the JWT on an idToken key
      data => localStorage.setItem('idToken', data.idToken),
      error => console.log(error)
    );
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

  register(email, password): Observable<string> {
    const credentials: CredentialsModel = {
      email,
      password
    };
    return this.http.post<string>(ApiMethods.auth.register, credentials).pipe(
      catchError(e => {
        if (e instanceof Response) {
          if (e.status !== 401) {
            return Observable.throw(e);
          }
        } else {
          return Observable.throw(e);
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
      return this.http.post<AuthTokenModel>(ApiMethods.auth.refreshToken, { refreshToken })
        .pipe(map(r => {
          return this.processTokensResponse(r);
        }));
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

    // Revokes token.
    this.revokeToken();

    // Revokes refresh token.
    this.revokeRefreshToken();
  }

  /**
   * Gets user's data.
   *
   * @return The user's data
   */
  getUser(): UserModel {
    return new UserModel(this.user.UserId, this.user.first_name, this.user.last_name, this.user.email, this.user.role);
  }

  facebookLogin(): Observable<boolean> {
    const socialPlatformProvider = FacebookLoginProvider.PROVIDER_ID;
    return from(this.socialAuthService.signIn(socialPlatformProvider)).pipe(
      flatMap((userData: SocialUser) => {
        return this.http.post<AuthTokenModel>(ApiMethods.auth.facebookLogin, { token: userData.token });
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
      return true;
    }
    return false;
  }
}
