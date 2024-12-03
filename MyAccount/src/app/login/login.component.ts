import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { MasterService } from '../master.service';
import { EncryptionService } from '../_services/EncryptionService';
import {jwtDecode} from 'jwt-decode';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  showError = false;
  errorMessage = "";
  title = 'MyAccount';
  decodedToken: any;
  constructor(private router:Router, private service:MasterService, private encryptionService:EncryptionService) { }

  ngOnInit(): void {
  }
  

  proceedLogin(username:any, password: any)
  {
    const key = this.encryptionService.getPublicKey();
    console.log("In Proceed login");
    console.log("publicKey:");
    console.log(key);
    var user = {"username": username, "password": password};
    const encryptedData: string = this.encryptionService.encryptData(user, key);
    const payload = {
      encryptedData: encryptedData
    };
 
    this.service.UserLogin(payload).subscribe((result: any) => {

      if (result != null && result.jwtToken != null)
      {
        this.decodedToken = jwtDecode(result.jwtToken);
       // let userObj = {username: this.decodedToken.unique_name, userId: Number(this.decodedToken.upn), givenName:  this.decodedToken.given_name}

        //console.log(userObj);
        console.log(result.refreshToken);
        sessionStorage.setItem("jwtToken",result.jwtToken); 
        sessionStorage.setItem("refreshToken",result.refreshToken); 
   
        this.router.navigate(["AccountDetails"]);
      }
      else
      {
        this.errorMessage =  "Your credentials are invalid. Please verify your username and password"
        this.showError = true;
      }
    }, (error: any) => {
      console.log(error);
        this.errorMessage =  "Your credentials are invalid. Please verify your username and password"
        this.showError = true;
      
      console.log('Error:', error);

    })
  }


}


