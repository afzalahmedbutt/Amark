import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Observable, Subject, forkJoin } from 'rxjs';
import { AccountService } from "../../../services/account.service";
import { AlertService,MessageSeverity } from '../../../services/alert.service';
import { User, UsersGridResponse, UserViewModel, UsersGridCommand, SearchCustomerGridDto, SearchCustomerViewModel } from "../../../models/user.model";
import { Role } from "../../../models/role.model";
import { Utilities } from '../../../services/utilities';
import { DataService } from '../../../services/data.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { LazyLoadEvent } from '../../../primeng/common/lazyloadevent';
import {SelectListItem } from '../../../models/selectlistitem.model';
//import {  } from 'primeng/table';

@Component({
  selector: 'users-management',
  templateUrl: './usermanagement.component.html',
  styleUrls: ['./usersmanagement.component.css']
})
export class UsersmanagementComponent implements OnInit {

  private readonly _usersUrl: string = "/api/account/searchcustomers";
  private readonly _rolesUrl: string = "/api/account/roles";
  private readonly _userUrl : string = "/api/account/users"

  loadingIndicator: boolean;
  allRoles: Role[] = [];
  rows: SearchCustomerViewModel[];
  columns: any[] = [];
  total: number = 0;
  command: UsersGridCommand = new UsersGridCommand();
  shippingInfo: any = {};
  tableSummary: string;
  vendors: SelectListItem[];

  @ViewChild('indexTemplate')
  indexTemplate: TemplateRef<any>;

  @ViewChild('userNameTemplate')
  userNameTemplate: TemplateRef<any>;

  @ViewChild('rolesTemplate')
  rolesTemplate: TemplateRef<any>;

  @ViewChild('actionsTemplate')
  actionsTemplate: TemplateRef<any>;

  @ViewChild('editorModal')
  editorModal: ModalDirective;

  @ViewChild('addEditUser') addEditUser: any;
  modalRef: BsModalRef;

  @ViewChild('customersTable') customersTable: any;

  selectedUser: UserViewModel;
  allRoleNames: string[];

  dropdownSettings = {};
  isSaveDisabled: boolean = false;
  loading: boolean = false;

  //@ViewChild('userEditor')
  //userEditor: UserInfoComponent;

  constructor(
    private accountService: AccountService,
    private alertService: AlertService,
    private modalService: BsModalService,
    private dataService : DataService
  ) {
    //this.dataService.get<SelectListItem[]>('api/Customer/GetVendors')
    //  .subscribe((vendors : SelectListItem[]) => {
    //    this.vendors = vendors;
    //  });
    this.getRoles();
  }

  ngOnInit() {
  this.columns = [
    { field: 'Id', header: 'Id' },
    { field: 'Email', header: 'Email' },
    { field: 'Name', header: 'Name' },
    { field: 'Roles', header: 'Customer Roles' },
    { field: 'CompanyName', header: 'Company Name' },
    { field: 'Active', header: 'Active' },
    { field: 'CreatedOnUtc', header: 'Created on' },
    { field: 'LastActivityDateUtc', header: 'Last Activity' },
    { field: '', header: 'Edit' },
    ];

    this.dropdownSettings = {
      singleSelection: true,
      //idField: 'Id',
      //textField: 'Name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      //itemsShowLimit: 3,
      allowSearchFilter: true
    };

    
    //this.loadData();
  }

  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }
  
  loadUsersLazy(event : LazyLoadEvent) {
    //var pageNumber: number = (event.first / event.rows) + 1;
    //var pageSize: number = event.rows;
    
    var command = new UsersGridCommand();
    this.command.FirstRow = event.first + 1;
    this.command.LastRow = event.first + event.rows;
    
    this.loadData();
  }

  searchUsers() {
    this.customersTable.first = 1;
    this.command.FirstRow = 1;
    this.command.LastRow = 15;
    this.command.CustomerRoleIds = this.allRoles.filter(r => r.checked).map(r => r.Id);
    this.loadData();
  }

  loadData() {
    //this.alertService.startLoadingMessage("Loading Customers....");
    setTimeout(() => {
      this.loading = true;
    });
    
    this.getUsers();
    
  }

  //onDataLoadSuccessful(response: UsersGridResponse, roles: Role[]) {
  //  debugger;
  //  this.alertService.stopLoadingMessage();
  //  debugger;
  //  //this.rowsCache = [...users];
  //  this.rows = response.Users;
  //  this.total = response.Count;
  //  this.allRoles = roles;
  //  this.allRoleNames = this.allRoles.map(r => r.Name);
  //}

 
  onDataLoadFailed(error: any) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    this.alertService.showStickyMessage("Load Error", `Unable to retrieve users from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
      MessageSeverity.error, error);
  }

  addUser(addUserTemplate: TemplateRef<any>) {
    this.selectedUser = new UserViewModel();
    this.modalRef = this.modalService.show(addUserTemplate, { class: 'modal-lg' });
  }

  editUser(id: number,addUserTemplate: TemplateRef<any>) {
    this.dataService.get<UserViewModel>(this._userUrl + '/' + id)
      .subscribe((user) => {
        
        this.selectedUser = user;
        this.modalRef = this.modalService.show(addUserTemplate, { class: 'modal-lg' });
      });
  }

  getUsers() {
    this.alertService.startLoadingMessage("Loading Customers....");
    this.dataService.post<SearchCustomerGridDto>(this._usersUrl, this.command)
      .subscribe((response) => {
        setTimeout(() => {
          this.loading = false;
        });
        this.alertService.stopLoadingMessage();
        this.rows = response.Customers;
        this.total = response.TotalCustomers
        if(this.command.LastRow)
        this.tableSummary = this.command.FirstRow + ' - ' + (this.command.LastRow > this.total ? this.total : this.command.LastRow) + ' of ' + this.total;
      });
  }

  getRoles() {
    this.dataService.get<Role[]>(this._rolesUrl)
      .subscribe((roles) => {
        this.allRoles = roles;
        this.allRoleNames = this.allRoles.map(r => r.Name);
      });
    
  }

  getUsersAndRoles(command : UsersGridCommand) {

    return forkJoin(
      this.dataService.post<UsersGridResponse>(this._usersUrl, command),
      this.dataService.get<Role[]>(this._rolesUrl));
  }

  saveUser(f : any) {
    var form = f;
    this.isSaveDisabled = true;
    this.alertService.startLoadingMessage('Saving user.....');
    //return;
    if (this.selectedUser.Id == 0) {
      this.dataService.post<UserViewModel>('api/account/register', this.selectedUser)
        .subscribe(user => this.saveSuccessHelper(user), error => this.saveFailedHelper(error));
    }
    else {
      this.dataService.put<UserViewModel>('api/account/users/' + this.selectedUser.Id, this.selectedUser)
        .subscribe(user => this.saveSuccessHelper(user), error => this.saveFailedHelper(error));
    }

  }
    
  private saveSuccessHelper(user?: UserViewModel) {
    debugger
    this.alertService.stopLoadingMessage();
    this.alertService.showMessage('','User Saved Successfully',MessageSeverity.success);
    this.isSaveDisabled = false;
    //this.user = user;
    this.modalRef.hide();
    this.loadData();
  }

  private saveFailedHelper(error: any) {
    //this.isSaving = false;
    this.alertService.stopLoadingMessage();
    this.alertService.showStickyMessage("Save Error", "The below errors occured whilst saving your changes:", MessageSeverity.error, error);
    this.alertService.showStickyMessage(error, null, MessageSeverity.error);
    this.isSaveDisabled = false;
    //if (this.changesFailedCallback)
    //  this.changesFailedCallback();
  }

}
