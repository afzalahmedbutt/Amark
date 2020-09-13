
import { Injectable, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';

@Injectable()
export class CustomerEndPoint extends EndpointFactory {
  private readonly _getPortalProductsUrl: string = "/api/Customer/GetPortalProducts";


  get portalProductsUrl() {
    return this.configurations.baseUrl + this._getPortalProductsUrl;
  }

  //getPortalProductsEndpoint<T>()

  getPortalProductsEndpoint<T>(userId?: string): Observable<T> {
    let endpointUrl = userId ? `${this.portalProductsUrl}/${userId}` : this.portalProductsUrl;

    return this.http.get<T>(endpointUrl, this.getRequestHeaders()).pipe<T>(
      catchError(error => {
        return this.handleError(error, () => this.getPortalProductsEndpoint(userId));
      }));
  }


}
