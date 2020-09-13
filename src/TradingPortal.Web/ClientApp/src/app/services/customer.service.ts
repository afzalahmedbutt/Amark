
import { Inject, Injectable } from '@angular/core'
import { CustomerEndPoint } from './customer-endpoint.service'
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
//import { EndpointFactory } from './customer-endpoint.service';
import {RequestForOrderModel} from '../models/order/requestfororder.model'

@Injectable()
export class CustomerService {
  constructor(
    private customerEndpoint: CustomerEndPoint
  ) {

  }
  getProductsToBuy() {
    return this.customerEndpoint.getPortalProductsEndpoint<RequestForOrderModel>().pipe(
      map(response => response));
  }

  

}
