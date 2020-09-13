
export class UserLogin {
  constructor(email?: string, password?: string, confirmPassword?: string, rememberMe?: boolean, newPassword?: string) {
    this.email = email;
    this.password = password;
    this.confirmPassword = confirmPassword;
    this.newPassword = newPassword
    this.rememberMe = rememberMe;
  }

  email: string;
  password: string;
  confirmPassword: string;
  rememberMe: boolean;
  newPassword: string;
}
