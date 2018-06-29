import { Injectable } from '@angular/core';
import { Observable, throwError, of, from } from 'rxjs';
import { map, flatMap, catchError } from 'rxjs/operators';
import { ApiMethods } from '../dal/consts/api-methods.consts';
//import 'rxjs/add/operator/map';
//import 'rxjs/add/operator/catch';
//import 'rxjs/add/observable/throw';

import { JwtHelperService, } from '@auth0/angular-jwt';

import { UserModel } from './../model/user.model'
import { RouterService } from './router.service'
import { CredentialsModel } from '../model/credentials.model';
import { AuthService as SocialAuthService, FacebookLoginProvider } from 'angular5-social-login';
import { SocialUser } from 'angular5-social-login';
import { HttpClient } from '@angular/common/http';

/** 
 * Authentication service. 
 */
@Injectable() export class AuthService {

    /** 
     * Stores the URL so we can redirect after signing in. 
     */
    redirectUrl: string;

    /** 
     * User's data. 
     */
    private user: any = {};

    private headers: Headers;

    constructor(private http: HttpClient, private routerService: RouterService, private socialAuthService: SocialAuthService, private jwtHelper: JwtHelperService) {

        // On bootstrap or refresh, tries to get the user's data.  
        this.decodeToken();

        // Creates header for post requests.  
    }

    login(email: string, password: string) {

        const credentials: CredentialsModel = {
            email: email,
            password: password
        };
        this.http.post<any>(ApiMethods.auth.getToken, credentials)
            .subscribe(
            // We're assuming the response will be an object
            // with the JWT on an id_token key
            data => localStorage.setItem('id_token', data.id_token),
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
            email: email,
            password: password
        };

        return this.http.post(ApiMethods.auth.getToken, credentials)
            .pipe(map((res: Response) => {
                return this.processTokensResponse(res.json());
            }), catchError((error: any) => {
                // Error on post request.  
                if (error instanceof Response) {
                    if (error.status == 401) {
                        return of(false);
                    } else {
                        return throwError(error);
                    };
                } else {
                    return throwError(error);
                }
            }));
    }


    register(email, password): Observable<string> {

        const credentials: CredentialsModel = {
            email: email,
            password: password
        };
        return this.http.post<string>(ApiMethods.auth.register, credentials).pipe(catchError((e) => {
            if (e instanceof Response) {
                if (e.status != 401) {
                    return Observable.throw(e);
                }
            } else {
                return Observable.throw(e);
            }
        }
        ));
    }

    /** 
     * Tries to get a new token using refresh token. 
     */
    getNewToken(): Observable<boolean> {

        const refreshToken: string = localStorage.getItem('refresh_token');

        if (refreshToken != null) {

            // Token endpoint & params.  
            const tokenEndpoint: string = ApiMethods.auth.getToken;

            const params: any = {
                grant_type: 'refresh_token',
                refresh_token: refreshToken
            };

            // Encodes the parameters.  
            const body: string = this.encodeParams(params);
            return this.http.post(tokenEndpoint, body).pipe(map(
                (res: Response) => {
                    return this.processTokensResponse(res.json());
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

        const token: string = localStorage.getItem('id_token');

        if (token != null) {

            // Revocation endpoint & params.  
            const revocationEndpoint: string = ApiMethods.auth.refreshToken;

            const params: any = {
                token_type_hint: 'id_token',
                refreshToken: token
            };

            // Encodes the parameters.  
            const body = JSON.stringify(params);

            this.http.post(revocationEndpoint, body)
                .subscribe(
                () => {
                    localStorage.removeItem('id_token');

                });
        }
    }


    /** 
     * Revokes refresh token. 
     */
    revokeRefreshToken(): Observable<boolean> {

        const refreshToken: string = localStorage.getItem('refresh_token');

        if (refreshToken == null) {
            return of(false);
        }

        // Revocation endpoint & params.  
        const revocationEndpoint: string = ApiMethods.auth.refreshToken;

        const params: any = {
            RefreshToken: refreshToken
        };

        // Encodes the parameters.  
        const body = JSON.stringify(params);

        return this.http.post(revocationEndpoint, body).pipe(map(
            (res: Response) => {
                const body: any = this.getResponseBody(res);
                // Successful if there's an access token in the response.  
                if (typeof body.id_token !== 'undefined') {
                    // Stores access token & refresh token.  
                    this.store(body);
                    return true;
                }
                return false;
            }));
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

    /** 
     * Decodes token through JwtHelper. 
     */
    private decodeToken(): void {

        if (this.isLoggedIn()) {

            const token: string = localStorage.getItem('id_token');
            this.user = this.jwtHelper.decodeToken(token);
        }

    }

    private getResponseBody(res: Response) {
        let body: any;
        try {
            body = res.json();
        } catch (e) {
            return {};
        }
        return body;
    }

    /** 
     * // Encodes the parameters. 
     * 
     * @param params The parameters to be encoded 
     * @return The encoded parameters 
     */
    private encodeParams(params: any): string {

        let body: string = '';
        for (let key in params) {
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
     * @param body The response of the request to the token endpoint 
     */
    private store(body: any): void {

        // Stores access token in local storage to keep user signed in.  
        localStorage.setItem('id_token', body.id_token);
        // Stores refresh token in local storage.  
        localStorage.setItem('refresh_token', body.refresh_token);

        // Decodes the token.  
        this.decodeToken();
    }

    private processTokensResponse(body): boolean {
        if (typeof body.id_token !== 'undefined') {

            // Stores access token & refresh token.  
            this.store(body);
            return true;
        }
        return false;
    }


    facebookLogin(): Observable<boolean> {
        const socialPlatformProvider = FacebookLoginProvider.PROVIDER_ID;
        return from(this.socialAuthService.signIn(socialPlatformProvider)).pipe(flatMap(
            (userData: SocialUser) => {
                return this.http.post(ApiMethods.auth.facebookLogin, { token: userData.token });
            }),
            map(
                (r) => {
                    return this.processTokensResponse(r);
                }), catchError(e => { return throwError(e); }))
    }

}
