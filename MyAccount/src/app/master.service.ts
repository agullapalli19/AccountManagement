import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MasterService {

  constructor(private http: HttpClient) {
  }
  apiURL = 'http://localhost:5224/api/';//'https://localhost:7130/api/'; //'http://localhost:5224/api/'//'
  apiAuthURL = this.apiURL + 'Authenticate';
  apiAccountURL = this.apiURL + 'Accounts';
  apiAccountTransferURL = this.apiURL + 'Accounts/Transfer';
  
  header = {
    Type: 'application/json', Accept: '*/*',
    'Content-Type': 'application/json'
  };

  private handleError(err: HttpErrorResponse): Observable<never> {
    // just a test ... more could would go here
    return throwError(() => err);
  }

  UserLogin(user: any) {

    return this.http.post(this.apiAuthURL, user, { headers: this.header });//.pipe(
    //catchError(this.handleError)
    //);
  }

  IsLoggedIn() {
    console.log(sessionStorage.getItem("username"));
    return sessionStorage.getItem("username") != null;
  }

  getAccountDetails(Id: any) {
    return this.http.get(this.apiAccountURL + "/" + Id);
  }

  accountTransfer(payload: any) {
    return this.http.post(this.apiAccountTransferURL, payload, { headers: this.header });
  }

  getPublicKey() {
    return this.http.get<{ publicKey: string }>(this.apiURL + 'RSAKey/public-key')
  }

}
