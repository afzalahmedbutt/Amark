import {SelectListItem } from './selectlistitem.model';

export class User {
  constructor(id?: string, userName?: string,firstName?:string,lastName?:string, fullName?: string, email?: string, jobTitle?: string, phoneNumber?: string, roles?: string[]) {
    this.id = id;
    this.userName = userName;
    this.firstName = firstName;
    this.lastName = lastName;
    this.fullName = fullName;
    this.email = email;
    this.jobTitle = jobTitle;
    this.phoneNumber = phoneNumber;
    this.roles = roles;
    
  }

  get friendlyName(): string {
    let name = this.fullName || this.userName;

    if (this.jobTitle)
      name = this.jobTitle + " " + name;

    return name;
  }

  public id: string;
  public userName: string;
  public fullName: string;
  public email: string;
  public jobTitle: string;
  public phoneNumber: string;
  public isEnabled: boolean;
  public isLockedOut: boolean;
  public firstName: string;
  public lastName: string;
  public roles: string[];
  
}


export class UserViewModel {
  
  public Id: number = 0;
  public UserName: string = "";
  public FullName: string = "";
  public Email: string = "";
  public Active: boolean = true;
  public CreatedOnUtc: Date;
  public LastActivityDateUtc: Date;
  public CompanyName: string = "";
  public Password: string = "";
  public AmarkTradingPartnerNumber: string = "";
  public AmarkTPAPIKey: string = "";
  public AdminComment: string = "";
  public IsTaxExempt: boolean = false;
  public VendorId: number;
  public Roles: string[];
  //public Vendors: SelectListItem[];
}

export class UsersGridResponse {
  public Users: UserViewModel[];
  public Count: number;
}

export class UsersGridCommand {
  public PageNumber: number;
  public PageSize: number;
  public FirstRow: number;
  public LastRow: number;
  public Email: string;
  public FirstName: string;
  public LastName: string;
  public Company: string;
  public CustomerRoleIds: number[];
}


export class SearchCustomerViewModel 
{
  public Id: number;
  public UserName: string;
  public FirstName: string;
  public LastName: string;
  public Email: string;
  public Company: string;
  public TotalCustomers: number;
  public IsActive: boolean;
  public CreatedOnUtc: string;
  public LastActivityDateUtc: string;
  public Roles: string;
}

export class SearchCustomerGridDto {
    public TotalCustomers: number;
  public Customers: SearchCustomerViewModel[];
}
