import { Injectable, Injector,isDevMode } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest,
  HttpResponse, HttpErrorResponse, HttpSentEvent, HttpProgressEvent,
  HttpHeaderResponse, HttpUserEvent
} from '@angular/common/http';

import { AuthService } from '../auth.service';
import { Router, ActivatedRoute, Params } from "@angular/router";
import { DataService } from '../data.service';
import { AlertService, MessageSeverity } from '../alert.service';
import { switchMap, mergeMap, flatMap, filter, finalize, take, tap, catchError,map } from 'rxjs/operators';
import { Observable, BehaviorSubject, throwError } from 'rxjs';




@Injectable()
export class HttpTokenInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private activatedRoute: ActivatedRoute, private dataService: DataService,private router : Router,private alertService : AlertService) {
    this.activatedRoute.url.subscribe((data) => {
      
      this.route = data[data.length - 1].path;
    });
  }
  route: string = "";
  isRefreshingToken: boolean = false;
  tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpSentEvent | HttpHeaderResponse | HttpProgressEvent | HttpResponse<any> | HttpUserEvent<any> | any> {
    const request = req.clone({ setHeaders: this.getAuthHeaders() });
    var sub =  next.handle(request)
      .pipe(
        map((event: HttpEvent<any>) => {
          
        if (event instanceof HttpResponse) {
            if (event.body["url"] == 'storeclose') {
            this.authService.isPortalClosed = true;
            this.authService.storeCloseDelegate(true);
            this.router.navigate(['/storeclose']);
            return event.clone({ body: null });
          }
          else {
            return event;
          }
          }
        }),
      catchError(err => {
        debugger;
        var st = (<HttpErrorResponse>err).status;
        if (err instanceof HttpErrorResponse) {
          switch ((<HttpErrorResponse>err).status) {
              case 401:
                return this.handle401Error(req, next);
              case 400:
                if (req.url == 'connect/token') {
                  return <any>this.authService.invalidCredentials(err);
                }
                else {
                  //return <any>this.authService.logoutDelegate();
                }
              case 500:
                if (!isDevMode()) {
                  this.dataService.exceptionIdentifier = err.error.ExceptionIdentifier;
                  this.router.navigate(['/errorpage']);
                  return {};
                }
                else {
                  debugger;
                  this.alertService.stopLoadingMessage();
                  this.alertService.showStickyMessage("Internal Server Error","Something went wrong",MessageSeverity.error);
                  console.log(err.error);
                  throwError(err);
                }
            }
          } else {
            return throwError(err);
          }
        }));
      return sub;
 }

  handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshingToken) {
      this.isRefreshingToken = true;
      console.log(request);
      // Reset here so that the following requests wait until the token
      // comes back from the refreshToken call.
      this.tokenSubject.next(null);

      return this.authService.refreshLogin()
        .pipe(
          switchMap((token) => {
            
            if (token) {
              this.tokenSubject.next("token");
              //localStorage.setItem('currentUser', JSON.stringify(user));
              return next.handle(request.clone({ setHeaders: this.getAuthHeaders() }));
            }

            return <any>this.authService.logout();
          }),
          catchError(err => {
            return <any>this.authService.logout();
          }),
          finalize(() => {
            this.isRefreshingToken = false;
          })
        );
    } else {
      this.isRefreshingToken = false;

      return this.tokenSubject
        .pipe(filter(token => token != null),
          take(1),
          switchMap(token => {

            return next.handle(request.clone({ setHeaders: this.getAuthHeaders() }));
          }));
    }
  }

  getAuthHeaders() {
    var headersConfig: {};
    const token = this.authService.accessToken;
    if (token) {
      if (!this.authService.isSessionExpired) {
        headersConfig = {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        };
        var time = this.authService.accessTokenExpiryDate;
        var sessionExpiry = this.authService.isSessionExpired;
        headersConfig['Authorization'] = `Bearer ${token}`;

      }

    }
    return headersConfig;
  }
}
