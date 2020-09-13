import { Injectable } from '@angular/core'
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router'
//import { Observable } from 'rxjs'
import {CustomerService} from '../services/customer.service'

@Injectable()
export class ProductsResolver implements Resolve<any> {
  constructor(private customerService : CustomerService) {

  }
  resolve(route: ActivatedRouteSnapshot, rstate: RouterStateSnapshot) {
    return this.customerService.getProductsToBuy();
  }
}
