import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AccountDetailsComponent } from './account-details/account-details.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CommonHeader } from './_helpers/commonHeaders.interceptors';


@NgModule({
  declarations: [
    AppComponent,
    AccountDetailsComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    CommonModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: CommonHeader,
    multi: true,
  },],
  bootstrap: [AppComponent],
  entryComponents: [LoginComponent]
})
export class AppModule { }
