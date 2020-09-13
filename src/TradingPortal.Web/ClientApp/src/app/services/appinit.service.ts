import { Injectable } from '@angular/core'
import { HttpClient, HttpResponse, HttpParams } from '@angular/common/http';
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { map } from 'rxjs/operators';
import {AppInitData } from '../models/appinit.model';
import { UpdateSpotsViewModel } from '../models/WebSpotPrice/web-spot-prices.model';


@Injectable()
export class AppInitService {
  constructor (private httpClient : HttpClient) {

  }
  private initData: AppInitData;
  //firstLoad: boolean = true;
  getWebSpotPrices() {
    return this.initData.UpdateSpotsViewModel;
  }

  getContactFormData() {
    return this.initData.ContactFormData;
  }

  getServiceContract() {
    return this.initData.ServiceContract;
  }

  getAppLoadDateTime() {
    return this.initData.ServerTime;
  }

  getMaxProductQuantity() {
    return this.initData.MaxProductQuantity;
  }

  isPortalClosed() {
    return this.initData.IsPortalClosed;
  }

  loadInitialData(): Promise<AppInitData> {
    const headersConfig = {
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    };

    return this.httpClient.get<AppInitData>('api/Utility/GetAppInitData', { headers: headersConfig })
      .pipe(map(res => this.initData = res ))
      .toPromise<AppInitData>();
  }
}
