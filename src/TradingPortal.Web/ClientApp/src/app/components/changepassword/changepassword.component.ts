import { Component, OnInit, OnDestroy, Input,ViewChild } from "@angular/core";
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
//import { Header } from "primeng/components/common/shared";
import { HttpHeaders,HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-changepassword',
  templateUrl: './changepassword.component.html',
  styleUrls: ['./changepassword.component.css']
})
export class ChangepasswordComponent implements OnInit {

  constructor(
    private alertService: AlertService,
    private authService: AuthService,
    private configurations: ConfigurationService,
    private route: ActivatedRoute,
    private router: Router,
    private dataService: DataService,
    private http : HttpClient
  ) {
    this.formResetToggle = false;
    setTimeout(() => {
    this.formResetToggle = true;
    });
  }

  ngOnInit() {
  }

  userLogin = new UserLogin();
  isLoading = false;
  formResetToggle = true;
  @ViewChild('confirmpassword') confirmpassword: any;
  isModal: boolean = false;

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
    this.http.post<IdentityResultCore>('api/Account/ChangePassword', resetPassword, { headers: headers })
      .subscribe((response) => {
        //if (response["url"] == "storeclose") {
        //  return;
        //}
        if (!response) {
          return;
        }
        if (response.Succeeded) {
          this.alertService.showMessage('Password Change','Password changed successfully!!',MessageSeverity.success);
          this.router.navigate(['/home']);
        }
        else {
          this.alertService.showMessage("Cahnge Password Error", response.Errors[0], MessageSeverity.error);
        }
      });
  }

  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }

}
