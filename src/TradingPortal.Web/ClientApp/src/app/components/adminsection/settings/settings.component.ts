import { Component, OnInit,ViewChild } from '@angular/core';
import { BootstrapTabDirective } from "../../../directives/bootstrap-tab.directive";
import { fadeInOut } from '../../../services/animations';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from "../../../services/account.service";
import { debug } from 'util';

@Component({
  selector: 'settings',
  templateUrl: './settings.component1.html',
  styleUrls: ['./settings.component.css'],
  animations: [fadeInOut]
})
export class SettingsComponent implements OnInit {

  isUsersActivated = false;
  isRolesActivated = false;
  isContentManagementActivated = false;
  isKillSwitchActivated = false;

  fragmentSubscription: any;

  readonly usersTab = "users";
  readonly rolesTab = "roles";
  readonly contentTab = "content";
  readonly killSwitchTab = "killswitch";

  columns: any[];

  @ViewChild("tab")
  tab: BootstrapTabDirective;

  constructor(private route: ActivatedRoute, private accountService: AccountService) {
  }

  ngOnInit() {
    this.fragmentSubscription = this.route.fragment.subscribe(anchor => this.showContent(anchor));
  }

  showContent(anchor: string) {
   
    //if ((this.isFragmentEquals(anchor, this.usersTab)) ||
    //  (this.isFragmentEquals(anchor, this.rolesTab)))
    //  return;

    this.tab.show(`#${anchor}Tab`);
  }

  onShowTab(event) {
    let activeTab = event.target.hash.split("#", 2).pop();
    
    //this.isProfileActivated = activeTab == this.profileTab;
    //this.isPreferencesActivated = activeTab == this.preferencesTab;
    this.isUsersActivated = activeTab == this.usersTab;
    this.isRolesActivated = activeTab == this.rolesTab;
    this.isContentManagementActivated = activeTab == this.contentTab;
    this.isKillSwitchActivated = activeTab == this.killSwitchTab;
    
  }

  isFragmentEquals(fragment1: string, fragment2: string) {

    if (fragment1 == null)
      fragment1 = "";

    if (fragment2 == null)
      fragment2 = "";

    return fragment1.toLowerCase() == fragment2.toLowerCase();
  }

  ngOnDestroy() {
    this.fragmentSubscription.unsubscribe();
  }

}
