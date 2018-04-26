import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { JwtHelper, tokenNotExpired } from 'angular2-jwt';

import { UserModel } from './../model/user.model'
import { RouterService } from './router.service'

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
    private options: RequestOptions;
    private jwtHelper: JwtHelper = new JwtHelper();

    constructor(private http: Http, private routerService: RouterService) {

        // On bootstrap or refresh, tries to get the user's data.  
        this.decodeToken();

        // Creates header for post requests.  
        this.headers = new Headers({ 'Content-Type': 'application/json' });
        this.options = new RequestOptions({ headers: this.headers });
    }

    login(username: string, password: string) {

        let params: any = {
            username: username,
            password: password
        };
        this.http.post(ApiMethods.auth.getToken, params)
            .map(res => res.json())
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
     * @param username 
     * @param password 
     * @return The user's data 
     */
    signin(username: string, password: string): Observable<boolean> {

        // Token endpoint & params.  
        let tokenEndpoint: string = ApiMethods.auth.getToken;

        let params: any = {
            username: username,
            password: password
        };

        // Encodes the parameters.  
        let body = JSON.stringify(params);

        return this.http.post(tokenEndpoint, body, this.options)
            .map((res: Response) => {

                let body: any = res.json();

                // Sign in successful if there's an access token in the response.  
                if (typeof body.id_token !== 'undefined') {

                    // Stores access token & refresh token.  
                    this.store(body);
                    return true;
                }
                return false;

            }).catch((data: any) => {

                // Error on post request.  
                if (data instanceof Response) {
                    if (data.status != 401) {
                        return Observable.throw(data);
                    }
                } else {
                    return Observable.throw(data);
                }
            });
    }


    register(email, password): Observable<string> {

        let params: any = {
            username: email,
            password: password
        };

        let body = JSON.stringify(params);

       return this.http.post(ApiMethods.auth.register, body, this.options)
            .map((res: Response) => {

                return res.json();

            }).catch((data: any) => {

                // Error on post request.  
                if (data instanceof Response) {
                    if (data.status != 401) {
                        return Observable.throw(data);
                    }
                } else {
                    return Observable.throw(data);
                }

            });
    }

    /** 
     * Tries to get a new token using refresh token. 
     */
    getNewToken(): Observable<boolean> {

        let refreshToken: string = localStorage.getItem('refresh_token');

        if (refreshToken != null) {

            // Token endpoint & params.  
            let tokenEndpoint: string = ApiMethods.auth.getToken;

            let params: any = {
                grant_type: "refresh_token",
                refresh_token: refreshToken
            };

            // Encodes the parameters.  
            let body: string = this.encodeParams(params); 
            return this.http.post(tokenEndpoint, body, this.options).map(
                (res: Response) => {

                    let body: any = res.json();

                    // Successful if there's an access token in the response.  
                    if (typeof body.id_token !== 'undefined') {

                        // Stores access token & refresh token.  
                        this.store(body);
                        return true;
                    }
                    return false;
                });
        }
    }

    /** 
    * Indicates if token is not expired
    */
    isLoggedIn(): boolean {

        return tokenNotExpired("id_token");

    }

    /** 
     * Revokes token. 
     */
    revokeToken(): void {

        let token: string = localStorage.getItem('id_token');

        if (token != null) {

            // Revocation endpoint & params.  
            let revocationEndpoint: string = ApiMethods.auth.refreshToken;

            let params: any = {
                token_type_hint: "id_token",
                refreshToken: token
            };

            // Encodes the parameters.  
            let body = JSON.stringify(params);

            this.http.post(revocationEndpoint, body, this.options)
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

        let refreshToken: string = localStorage.getItem('refresh_token');

        if (refreshToken == null) {
            return Observable.of(false);
        }

        // Revocation endpoint & params.  
        let revocationEndpoint: string = ApiMethods.auth.refreshToken;

        let params: any = {
            RefreshToken: refreshToken
        };

        // Encodes the parameters.  
        let body = JSON.stringify(params);

        return this.http.post(revocationEndpoint, body, this.options).map(
            (res: Response) => {
                let body: any = this.getResponseBody(res);
                // Successful if there's an access token in the response.  
                if (typeof body.id_token !== 'undefined') {
                    // Stores access token & refresh token.  
                    this.store(body);
                    return true;
                }
                return false;
            });
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

            let token: string = localStorage.getItem('id_token');

            let jwtHelper: JwtHelper = new JwtHelper();
            this.user = jwtHelper.decodeToken(token);

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

        let body: string = "";
        for (let key in params) {
            if (body.length) {
                body += "&";
            }
            body += key + "=";
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

}  