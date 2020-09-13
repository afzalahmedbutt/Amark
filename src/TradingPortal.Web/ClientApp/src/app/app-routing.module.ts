
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from "./components/login/login.component";
import { ChangepasswordComponent } from "./components/changepassword/changepassword.component";
import { AuthService } from './services/auth.service';
import { AuthGuard } from './services/auth-guard.service';
import { ProductsResolver } from './resolvers/productstobuy.resolver';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { TermsOfServiceResolver } from './resolvers/termsofservice.resolver';
import { RequestForOrderComponent } from './components/order/request-for-order/request-for-order.component';
import { ConfirmOrderComponent } from './components/order/confirmorder/confirmorder.component';
import { TradingactivityComponent } from './components/tradingactivity/tradingactivity.component';
import {AdminsectionComponent } from './components/adminsection/adminsection.component';
import { ErrorpageComponent } from './components/errorpage/errorpage.component';

import { SettingsComponent } from './components/adminsection/settings/settings.component';
import {StorecloseComponent} from './components/storeclose/storeclose.component'

const routes: Routes = [
 
  { path: "login", component: LoginComponent, data: { title: "Login" } },
  
  { path: "forgotpassword", component: LoginComponent, data: { title: "Forgot Password" } },
  { path: "confirmpassword", component: LoginComponent, data: { title: "Confirm Password" } },
  { path: "forgotpasswordconfirmation", component: LoginComponent, data: { title: "Forgot Password Confirmation" } },
  { path: "changepassword", component: ChangepasswordComponent, data: { title: "Change Password" } },
  //{ path: "home", redirectTo: "/", pathMatch: "full" },
  {
    path: "adminsection",
    component: AdminsectionComponent,
    canActivate: [AuthGuard],
    data: { title: "Admin" }
    //resolve: {
    //  orderProducts : ProductsResolver
    //}
  },
  {
    path: "buy",
    component: RequestForOrderComponent,
    canActivate: [AuthGuard],
    data: { title: "Buy" }
    //resolve: {
    //  orderProducts : ProductsResolver
    //}
  },
  {
    path: "sell",
    component: RequestForOrderComponent,
    canActivate: [AuthGuard],
    data: { title: "Sell" }
  },
  {
    path: "confirmbuy",
    component: ConfirmOrderComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "confirmsell",
    component: ConfirmOrderComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'tradingactivity',
    component: TradingactivityComponent,
    canActivate:[AuthGuard]
  },
  {
    path: 'errorpage',
    component: ErrorpageComponent
  },
  {
    path: "", component: WelcomeComponent,
    canActivate: [AuthGuard],
    data: { title: "Welcome" }
    
  },
  {
    path: "settings",
    component: SettingsComponent,
    data: {title : "Settings"}
  },
  {
    path: "storeclose",
    component: StorecloseComponent,
    canActivate: [AuthGuard],
    data: { title: "Store Close" }
  }
  //{ path: "**", component: NotFoundComponent, data: { title: "Page Not Found" } }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthService, AuthGuard]
})
export class AppRoutingModule { }
