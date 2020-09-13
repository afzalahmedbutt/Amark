import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
//import { Observable } from 'rxjs'
import { DataService } from '../services/data.service';
import { TopicViewModel } from '../models/topic/topic-view.model';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class TermsOfServiceResolver implements Resolve<any> {
  constructor(private dataService: DataService) {

  }
  resolve(route: ActivatedRouteSnapshot, rstate: RouterStateSnapshot) {
    return this.dataService.get<TopicViewModel>('api/Utility/GetContractDetails').pipe(
      map(response => response));
  }
}
