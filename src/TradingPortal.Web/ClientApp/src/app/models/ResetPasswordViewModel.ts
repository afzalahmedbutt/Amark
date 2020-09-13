
export class ResetPasswordViewModel {
  constructor() {
    this.UserId = 0;
    this.Email = "";
    this.Password = "";
    this.NewPassword = "";
    this.ConfirmPassword = "";
    this.Code = "";
  }
  public UserId: number;
  public Email: string;
  public Password: string;
  public NewPassword: string;
  public ConfirmPassword: string;
  public Code: string;
}

export class PasswordRecoveryToken {
  public UserId: number;
  public Token: string;
}

export class IdentityResultCore {
  public Succeeded: boolean;
  public Errors: string[];
}
