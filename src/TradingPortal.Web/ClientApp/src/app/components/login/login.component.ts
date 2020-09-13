
import { Component, OnInit, OnDestroy, Input } from "@angular/core";
import { AlertService, MessageSeverity, DialogType } from '../../services/alert.service';
import { AuthService } from "../../services/auth.service";
import { ConfigurationService } from '../../services/configuration.service';
import { Utilities } from '../../services/utilities';
import { UserLogin } from '../../models/user-login.model';
import { Router, ActivatedRoute, Params } from "@angular/router";
import { DataService } from '../../services/data.service';
import { debug } from "util";
import { ResetPasswordViewModel, IdentityResultCore } from '../../models/ResetPasswordViewModel'

import { filter } from 'rxjs/operators'
import { HttpHeaders,HttpErrorResponse } from "@angular/common/http";


@Component({
  selector: "app-login",
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit, OnDestroy {

  userLogin = new UserLogin();
  isLoading = false;
  formResetToggle = true;
  modalClosedCallback: () => void;
  loginStatusSubscription: any;


  @Input()
  isModal = false;
  isLogin: boolean = false;
  isForgotPassword: boolean = false;
  isConfirmPassword: boolean = false;
  currentView: string;
  callbackUrl: string;

  userId: any;
  passwordResetCode: any;

  constructor(private alertService: AlertService,
    private authService: AuthService,
    private configurations: ConfigurationService,
    private route: ActivatedRoute,
    private router: Router,
    private dataService: DataService
  ) {

    this.route.params.subscribe((params: Params) => {
      this.userId = params["userId"];
      this.passwordResetCode = params["passwordResetCode"];
    });

    this.route.queryParams.subscribe(params => {
      this.userId = params['userid'];
      this.passwordResetCode = params['passwordresetcode'];
    });



    this.route.url.subscribe(data => {
      
      let url = data[data.length - 1].path;
      if (url == 'forgotpassword') {
        this.currentView = 'forgotpassword';
      }
      else if (url == 'confirmpassword') {
        this.currentView = 'confirmpassword';
      }
      else if (url == 'forgotpasswordconfirmation') {
        this.currentView = 'forgotpasswordconfirmation'
      }
      else if (url == 'changepassword') {
        this.currentView = "changepassword";
      }
      else {
        //if (this.passwordResetCode) {
        //  this.currentView = 'confirmpassword'
        //}
        this.currentView = 'login';
      }
    });
  }


  ngOnInit() {

    this.authService.invalidCredentials = (err : HttpErrorResponse) => this.invalidCredentials(err);

    this.userLogin.rememberMe = this.authService.rememberMe;
    
    if (this.getShouldRedirect()) {
      this.authService.redirectLoginUser();
    }
    else {
      this.loginStatusSubscription = this.authService.getLoginStatusEvent().subscribe(isLoggedIn => {
        if (this.getShouldRedirect()) {
          this.authService.redirectLoginUser();
        }
      });
    }
  }


  ngOnDestroy() {
    if (this.loginStatusSubscription)
      this.loginStatusSubscription.unsubscribe();
  }


  getShouldRedirect() {
    return !this.isModal && this.authService.isLoggedIn && !this.authService.isSessionExpired;
  }


  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }

  closeModal() {
    if (this.modalClosedCallback) {
      this.modalClosedCallback();
    }
  }

  invalidCredentials(err: HttpErrorResponse) {
    this.isLoading = false;
    this.alertService.stopLoadingMessage();
    this.reset();
    this.alertService.showStickyMessage("Invalid UserName or Password", err.error.error_description, MessageSeverity.error);
    //this.alertService.showMessage("Invalid UserName or Password", 'UserName or Password is invalid', MessageSeverity.error);
  }


  login() {
    this.isLoading = true;
    this.alertService.startLoadingMessage("", "Attempting login...");
    this.authService.login(this.userLogin.email, this.userLogin.password, this.userLogin.rememberMe)
      .subscribe(
        user => {
          //setTimeout(() => {
            this.alertService.stopLoadingMessage();
            this.isLoading = false;
          this.reset();
          if (!this.isModal) {
              this.alertService.showMessage("Login", `Welcome ${user.firstName}!`, MessageSeverity.success);
            }
            else {
              this.alertService.showMessage("Login", `Session for ${user.firstName} restored!`, MessageSeverity.success);
              setTimeout(() => {
                this.alertService.showStickyMessage("Session Restored", "Please try your last operation again", MessageSeverity.default);
              }, 500);

              this.closeModal();
          }
          //if (user.configuration == 'storeclosed') {
          //  this.authService.storeCloseDelegate(true);
          //}
          //}, 500);
        },
        error => {

          this.alertService.stopLoadingMessage();

          if (Utilities.checkNoNetwork(error)) {
            this.alertService.showStickyMessage(Utilities.noNetworkMessageCaption, Utilities.noNetworkMessageDetail, MessageSeverity.error, error);
            this.offerAlternateHost();
          }
          else {
            let errorMessage = Utilities.findHttpResponseMessage("error_description", error);

            if (errorMessage)
              this.alertService.showStickyMessage("Unable to login", errorMessage, MessageSeverity.error, error);
            else
              this.alertService.showStickyMessage("Unable to login", "An error occured whilst logging in, please try again later.\nError: " + error.statusText || error.status, MessageSeverity.error, error);
          }

          setTimeout(() => {
            this.isLoading = false;
          }, 500);
        });
  }

  forgotPassword() {
    this.isLoading = true;
    this.alertService.startLoadingMessage("", "Posting Request...");
    //var url = 'api/Account/forgotPassword/'+this.userLogin.email;
    //this.dataService.post(url);

    this.authService.forgotPassword(this.userLogin.email).subscribe((response) => {
      this.alertService.stopLoadingMessage();
      if (response) {
        //this.callbackUrl = response;
        this.router.navigate(['/forgotpasswordconfirmation']);
      }

    });

  }

  resetPassword() {
    
    var resetPasswordViewModel = new ResetPasswordViewModel();
    resetPasswordViewModel.UserId = this.userId;
    resetPasswordViewModel.Email = this.userLogin.email;
    resetPasswordViewModel.Password = this.userLogin.password;
    resetPasswordViewModel.Code = this.passwordResetCode;
    resetPasswordViewModel.ConfirmPassword = this.userLogin.confirmPassword;
    this.alertService.startLoadingMessage("Resetting Password.....");
    this.authService.resetPassword(resetPasswordViewModel).subscribe((response) => {
      this.alertService.stopLoadingMessage();
      if (response.Succeeded) {
        this.alertService.showMessage('Password Reset','Password is reset successfully!',MessageSeverity.success);
        this.router.navigate(['/login']);
      }
      else {
        this.alertService.showMessage("Reset Password error", response.Errors[0], MessageSeverity.error);
      }
    });
  }

  changePassword() {
    //var confirm = this.confirmpassword;
    //return;
    var resetPassword = new ResetPasswordViewModel();
    resetPassword.Password = this.userLogin.password;
    resetPassword.NewPassword = this.userLogin.newPassword;
    resetPassword.ConfirmPassword = this.userLogin.confirmPassword;
    let headers = new HttpHeaders({
      'Authorization': 'Bearer ' + this.authService.accessToken,
      'Content-Type': 'application/json',
      'Accept': `application/json, text/plain, */*`
      //'App-Version': ConfigurationService.appVersion
    });
    this.alertService.startLoadingMessage('Changing password...');
    this.dataService.getHttp().post<IdentityResultCore>('api/Account/ChangePassword', resetPassword, { headers: headers })
      .subscribe((response) => {
        if (response.Succeeded) {
          this.alertService.showMessage('Password Change', 'Password changed successfully!!', MessageSeverity.success);
          this.router.navigate(['/home']);
        }
        else {
          this.alertService.showMessage("Cahnge Password Error", response.Errors[0], MessageSeverity.error);
        }
      });
  }

  //changePassword() {
  //  var resetPassword = new ResetPasswordViewModel();
  //  resetPassword.OldPassword = this.userLogin.oldPassword;
  //  resetPassword.Password = this.userLogin.password;
  //  resetPassword.ConfirmPassword = this.userLogin.confirmPassword;
  //  let headers = new HttpHeaders({
  //    'Authorization': 'Bearer ' + this.authService.accessToken,
  //    'Content-Type': 'application/json',
  //    'Accept': `application/json, text/plain, */*`
  //    //'App-Version': ConfigurationService.appVersion
  //  });

  //  this.dataService.post<boolean>('api/Account/ChangePassword', resetPassword, {headers : headers})
  //    .subscribe((response) => {
  //      if (response) {
  //        this.router.navigate(['/home']);
  //      }
  //    });
  //}


  offerAlternateHost() {

    if (Utilities.checkIsLocalHost(location.origin) && Utilities.checkIsLocalHost(this.configurations.baseUrl)) {
      this.alertService.showDialog("Dear Developer!\nIt appears your backend Web API service is not running...\n" +
        "Would you want to temporarily switch to the online Demo API below?(Or specify another)",
        DialogType.prompt,
        (value: string) => {
          this.configurations.baseUrl = value;
          this.alertService.showStickyMessage("API Changed!", "The target Web API has been changed to: " + value, MessageSeverity.warn);
        },
        null,
        null,
        null,
        this.configurations.fallbackBaseUrl);
    }
  }


  reset() {
    this.formResetToggle = false;

    setTimeout(() => {
      this.formResetToggle = true;
    });
  }
}











