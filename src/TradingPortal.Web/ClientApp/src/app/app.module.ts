
import { NgModule, ErrorHandler, APP_INITIALIZER } from "@angular/core";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastaModule } from 'ngx-toasta';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from "ngx-bootstrap/tooltip";
import { PopoverModule } from "ngx-bootstrap/popover";
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
//import { CarouselModule } from 'ngx-bootstrap/carousel';
//import { ChartsModule } from 'ng2-charts';
import { AlertModule } from 'ngx-bootstrap/alert'
import { LoadingModule, ANIMATION_TYPES } from 'ngx-loading';
import { UiSwitchModule } from 'ngx-ui-switch';

import { AppRoutingModule } from './app-routing.module';
import { AppErrorHandler } from './app-error.handler';
import { AppTitleService } from './services/app-title.service';
import { ConfigurationService } from './services/configuration.service';
import { AlertService } from './services/alert.service';
import { LocalStoreManager } from './services/local-store-manager.service';
import { EndpointFactory } from './services/endpoint-factory.service';
import {AccountEndpoint } from './services/account-endpoint.service';
import { NotificationService } from './services/notification.service';
import { NotificationEndpoint } from './services/notification-endpoint.service';
import { AccountService } from './services/account.service';
import { AppRoutingService } from './services/app-routing.service';



import { AppComponent } from "./components/app.component";
import { LoginComponent } from "./components/login/login.component";
//import { BuyComponent } from "./components/requestfororder/buy/buy.component";
//import { TableModule } from 'primeng/table';
import { TableModule} from './primeng/table/table';
import { CustomerEndPoint } from './services/customer-endpoint.service';
import { ProductsResolver } from './resolvers/productstobuy.resolver';
import { CustomerService } from './services/customer.service';
import { HttpTokenInterceptor } from './services/interceptors/http.token.interceptor';
import {DataService } from './services/data.service';
import { ChangepasswordComponent } from './components/changepassword/changepassword.component';
import {EqualValidator} from './directives/equal-validator.directive';
import { SpotpriceComponent } from './components/spotprice/spotprice.component1';
import {DecimalCurrencyPipe} from './pipes/decimalcurrency.pipe';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { TermsOfServiceResolver } from './resolvers/termsofservice.resolver';
import { AppInitService } from './services/appinit.service';
import { FilterlistPipe } from './pipes/productsfilter.pipe';
import { ConfirmationService } from './primeng/common/confirmationservice';
import { ConfirmDialogModule } from './primeng/confirmdialog/confirmdialog';

import { JwtHelper } from './services/jwt-helper';
import {FilterPipe } from './pipes/listfilter.pipe';
import { ConfirmOrderComponent } from './components/order/confirmorder/confirmorder.component';
import { OrdersummaryComponent } from './components/order/ordersummary/ordersummary.component';
import { OrdertableComponent } from './components/order/ordertable/ordertable.component';
import {RequestForOrderComponent } from './components/order/request-for-order/request-for-order.component';
import { OrderfooterComponent } from './components/order/orderfooter/orderfooter.component';
import { TradingactivityComponent } from './components/tradingactivity/tradingactivity.component';

import { UpperCaseDirective } from './directives/uppercase.directive';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

import { SafePipe } from './pipes/safe.pipe';
import { DropshipComponent } from './components/order/dropship/dropship.component';
import {MaxValueValidator } from './directives/maxvaluevalidator.directive';
import { ErrorpageComponent } from './components/errorpage/errorpage.component';
import { AdminsectionComponent } from './components/adminsection/adminsection.component';
import {RangeContantsService } from './services/range-constants.service';
import { MousePositionDirective } from './directives/mouse-position.directive';
import { SettingsComponent } from './components/adminsection/settings/settings.component';
import { BootstrapTabDirective } from './directives/bootstrap-tab.directive';
import { UsersmanagementComponent } from './components/adminsection/usersmanagement/usersmanagement.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
//import { GoldSilverAskComponent } from './components/gold-silver-ask/gold-silver-ask.component';
import { OwlModule } from 'ngx-owl-carousel';
import { CKEditorModule } from 'ng2-ckeditor';
import { ContentmanagementComponent } from './components/adminsection/contentmanagement/contentmanagement.component';
import { CookieService } from 'ngx-cookie-service';
import { KillswitchComponent } from './components/adminsection/killswitch/killswitch.component';
import { StorecloseComponent } from './components/storeclose/storeclose.component';
//import { BlockUIModule } from 'ng-block-ui';




@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    ToastaModule.forRoot(),
    TooltipModule.forRoot(),
    PopoverModule.forRoot(),
    ModalModule.forRoot(),
    TableModule,
    MDBBootstrapModule.forRoot(),
    AlertModule.forRoot(),
    LoadingModule.forRoot({
      animationType: ANIMATION_TYPES.pulse,
      backdropBackgroundColour: 'rgba(0,0,0,0.1)',
      //backdropBackgroundColour: '#d9d9d9',
      backdropBorderRadius: '4px',
      //primaryColour: '#ffffff',
      primaryColour: '#0002FF',
      secondaryColour: '#008080',
      tertiaryColour: '#0000FF'
    }),
    UiSwitchModule.forRoot({
      size: 'small',
      color: '#2196F3',
      switchColor: 'white',
      defaultBgColor: '#ccc',
      //defaultBoColor: '#476EFF',
      checkedLabel: 'on',
      uncheckedLabel: 'off'
    }),
    ConfirmDialogModule,
    BsDatepickerModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    OwlModule,
    CKEditorModule
    //BlockUIModule.forRoot()
  ],
  declarations: [
    AppComponent,
    LoginComponent,
    EqualValidator,
    ChangepasswordComponent,
    SpotpriceComponent,
    DecimalCurrencyPipe,
    WelcomeComponent,
    //SellComponent,
    RequestForOrderComponent,
    FilterlistPipe,
    FilterPipe,
    ConfirmOrderComponent,
    OrdersummaryComponent,
    OrdertableComponent,
    OrderfooterComponent,
    TradingactivityComponent,
    UpperCaseDirective,
    SafePipe,
    DropshipComponent,
    MaxValueValidator,
    ErrorpageComponent,
    AdminsectionComponent,
    //GraphsComponent,
    //WholesalepricesComponent,
    //MetalpricesComponent,
    //ProductComponent,
    MousePositionDirective,
    SettingsComponent,
    BootstrapTabDirective,
    UsersmanagementComponent,
    //GoldSilverAskComponent,
    ContentmanagementComponent,
    KillswitchComponent,
    StorecloseComponent
    
  ],
  providers: [
    { provide: 'BASE_URL', useFactory: getBaseUrl },
   // { provide: ErrorHandler, useClass: AppErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: HttpTokenInterceptor, multi: true },
    DataService,
    AlertService,
    ConfigurationService,
    AppTitleService,
    //AppTranslationService,
    NotificationService,
    NotificationEndpoint,
    AccountService,
    //AccountEndpoint,
    LocalStoreManager,
    EndpointFactory,
    CustomerEndPoint,
    ProductsResolver,
    CustomerService,
    TermsOfServiceResolver,
    AppInitService,
    ConfirmationService,
    RangeContantsService,
    AccountEndpoint,
    AppRoutingService,
    CookieService,
    { provide: APP_INITIALIZER, useFactory: init_app, deps: [AppInitService], multi: true }
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}




export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

export function init_app(appInitService : AppInitService) {
  return () => appInitService.loadInitialData();
}
