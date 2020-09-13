import { Injectable } from '@angular/core';
import { Router, NavigationExtras } from "@angular/router";
import { HttpClient } from '@angular/common/http';
import { Observable, Subject, forkJoin } from 'rxjs';
import { mergeMap, tap } from 'rxjs/operators';
import { User, UsersGridCommand } from '../models/user.model';
import { Role } from '../models/role.model';
import { AuthService } from './auth.service';
import { RoleKeys } from './db-keys';
import { AccountEndpoint } from './account-endpoint.service';
import {UsersGridResponse } from '../models/user.model';

@Injectable()
export class AccountService {
  constructor(
    private authService: AuthService,
    private accountEndpoint :AccountEndpoint) {

  }


  isInRole(roleName: string): boolean {
    var roles = this.authService.userRoles;
    if (roles.length > 0 && roles.indexOf(roleName) > -1) {
      return true;
    }
    return false;
  }

  get isAdminUser() {
    return this.isInRole(RoleKeys.ADMIN);
  }

  getUsersAndRoles(page?: number, pageSize?: number) {

    return forkJoin(
      this.accountEndpoint.getUsersEndpoint<User[]>(page, pageSize),
      this.accountEndpoint.getRolesEndpoint<Role[]>());
  }

  getUsersAndRolesForGrid(usersGridCommand : UsersGridCommand) {

    return forkJoin(
      this.accountEndpoint.getUsersEndpointForGrid<UsersGridResponse>(usersGridCommand),
      this.accountEndpoint.getRolesEndpoint<Role[]>());
  }

}
