export class UserViewModel {
  public Id: string;
  
  public UserName: string;
  public FullName: string;
  
  public Email: string;
  public Active: boolean;
  public CreatedOnUtc: Date;
  public LastActivityDateUtc: Date;
  public CompanyName: string;
  public Roles: string[];
}
