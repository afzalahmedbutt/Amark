import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
//import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { DataService } from '../../../services/data.service';
import { ConfirmationService, Confirmation } from '../../../primeng/common/api';
import { AuthService } from '../../../services/auth.service';
import { AlertService, MessageSeverity, DialogType } from '../../../services/alert.service';

declare var require: any;

@Component({
  selector: 'killswitch-management',
  templateUrl: './killswitch.component.html',
  styleUrls: ['./killswitch.component.css']
})
export class KillswitchComponent implements OnInit {
  statusCircle = require("../../../assets/images/Status_circle.gif");
  isPortalClosed: boolean = false;
  isPortalOpen: boolean = true;
  dislpayStatusCheck: boolean = true;
  displaySwitch: boolean = false;
  @ViewChild('updatePortalStatus') updatePortalStatus;
  showSwitch: boolean = true;


  constructor(
    private dataService: DataService,
    private confirmationSerivce: ConfirmationService,
    private authService: AuthService,
    private alertService : AlertService) {
    this.dataService.get<boolean>('api/switch/getstoreclosestatus')
      .subscribe((isStoreClosed) => {
        debugger;
        this.isPortalOpen = !isStoreClosed;
        this.dislpayStatusCheck = false;
        this.displaySwitch = true;
      });
      
   
  }

  switchToggled() {
    this.displaySwitch = false;
    var prevValue = !this.isPortalOpen;
    var confirmation: Confirmation = {
      header: 'Kill Switch',
      message: "Continue turning "+ (this.isPortalOpen ? 'On' : 'Off')+" trading?",
      accept: () => {
        this.dataService.isLoading(true);
        var msg = "";
        if (this.isPortalOpen) {
          msg = 'Turning portal on!'
        }
        else {
          msg = 'Turning portal Off!'
        }
        this.alertService.startLoadingMessage(msg);
        this.dataService.post('api/switch/setstoreclosestatus/' + !this.isPortalOpen)
          .subscribe(() => {
            //this.isPortalClosed = !this.isPortalClosed;
            this.displaySwitch = true;
            this.alertService.stopLoadingMessage();
            this.dataService.isLoading(false);
            this.authService.setPortalClosedStatus(!this.isPortalOpen);
          });
      }, reject: () => {
        this.isPortalOpen = prevValue;
        this.displaySwitch = true;
      }
    }

    this.confirmationSerivce.confirm(confirmation);
  }

  

  ngOnInit() {

  }

}
