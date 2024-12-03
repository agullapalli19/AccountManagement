import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { MasterService } from './master.service';
import { EncryptionService } from './_services/EncryptionService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnInit{
  showError = false;
  errorMessage = "";
  title = 'MyAccount';
  jwtToken?: string = "";

  constructor(private router:Router, private service:MasterService, private encryptionService:EncryptionService){
  }
  ngOnInit(): void { 
    this.service.getPublicKey().subscribe((response: any) => {
      this.encryptionService.setPublicKey(response.publicKey);
    });
    this.jwtToken = sessionStorage.getItem("jwtToken")??'';
    if (this.jwtToken)
    {
      this.router.navigate(["AccountDetails"]);
      
    }
    else
    {
      this.router.navigate(["login"]);
    }
  }
  
}
