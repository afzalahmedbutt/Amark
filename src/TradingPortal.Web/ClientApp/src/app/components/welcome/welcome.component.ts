import { Component, OnInit,TemplateRef } from '@angular/core';
import { LocalStoreManager } from '../../services/local-store-manager.service';
import { User } from '../../models/user.model';
import { DBkeys,SessionKeys } from '../../services/db-keys';
import { Router, ActivatedRoute, Params } from "@angular/router";
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { TopicViewModel } from '../../models/topic/topic-view.model';
import { AppInitService } from '../../services/appinit.service';
import {DataService } from '../../services/data.service';
import { fadeInOut } from '../../services/animations';
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css'],
  animations: [fadeInOut]
})
export class WelcomeComponent implements OnInit {

  modalRef: BsModalRef;
  termsOfService: TopicViewModel;
  displayAlert: boolean = false;
  isContractChecked: boolean = false;
  isContractAlreadyAccepted: boolean = false;
  constructor(
    private localStoreManager: LocalStoreManager,
    private router: Router,
    private modalService: BsModalService,
    private activatedRoute: ActivatedRoute,
    private appInitService: AppInitService,
    private dataService: DataService,
    private authService: AuthService) {
    
    if (this.localStoreManager.getData(SessionKeys.IS_CONTRACT_ACCEPTED)) {
      this.isContractAlreadyAccepted = this.localStoreManager.getData(SessionKeys.IS_CONTRACT_ACCEPTED);
    }
    //this.activatedRoute.data.subscribe((data: { termsOfService : TopicViewModel}) => {
    //   this.termsOfService = data.termsOfService;
      
    //  });
  }

  user: User
  ngOnInit() {
    this.user = this.localStoreManager.getData(DBkeys.CURRENT_USER);
    this.termsOfService = this.appInitService.getServiceContract();
    
        
  }

  navigateToRoute(route) {
    if (!this.isContractChecked && !this.isContractAlreadyAccepted) {
      this.displayAlert = true;
      return;
    }
    this.router.navigate(['/'+route]);
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  contractChecked() {
    if (this.isContractChecked) {
      this.localStoreManager.saveSessionData(true, SessionKeys.IS_CONTRACT_ACCEPTED);
      this.displayAlert = false;
    }
    else {
      this.localStoreManager.saveSessionData(false, SessionKeys.IS_CONTRACT_ACCEPTED);
    }
  }
  

}
