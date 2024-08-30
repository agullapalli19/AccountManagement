import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { AppComponent } from './app.component';
import { AccountDetailsComponent } from './account-details/account-details.component';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
{path:"",component: AppComponent},//,canActivate:[AuthGuard]},
{path:"login",component:LoginComponent},
{path:"AccountDetails",component:AccountDetailsComponent}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
