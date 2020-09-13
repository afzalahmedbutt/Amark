import { Injectable, Injector } from '@angular/core';
import { HttpClient, HttpResponse, HttpParams } from '@angular/common/http';
import { Observable,Subject,throwError } from 'rxjs';
import { mergeMap, switchMap, catchError } from 'rxjs/operators';
import { AccountService } from './account.service';
import { AuthService } from './auth.service';
import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';


@Injectable()
export class DataService {

  // Define the internal Subject we'll use to push the command count
  public pendingCommandsSubject = new Subject<number>();
  public pendingCommandCount = 0;

  public exceptionIdentifier: string;
  private isLoadingSubject = new Subject<boolean>();
  public isLoadingSubject$ = this.isLoadingSubject.asObservable();

  private isLoadingReviewSubject = new Subject<boolean>();
  public isLoadingReviewSubject$ = this.isLoadingReviewSubject.asObservable();
  // Provide the *public* Observable that clients can subscribe to
  public pendingCommands$: Observable<number>;

  private hidePageHeader = new Subject<boolean>();
  public hidePageHeader$ = this.hidePageHeader.asObservable();

  private _isEditOrderRequest: boolean = false;
  private _isRedirectRequest: boolean = false;

  private taskPauser: Subject<any>;
  private isRefreshingLogin: boolean;

  private readonly _loginUrl: string = "/connect/token";
  private get loginUrl(){
    return this.configurations.baseUrl + this._loginUrl;
  }

  constructor(
    public http: HttpClient,
    private inj: Injector,
    private authService: AuthService,
    private endPointFactory: EndpointFactory,
    private configurations : ConfigurationService
  ) {
    this.pendingCommands$ = this.pendingCommandsSubject.asObservable();
  }

  public getImage(url: string): Observable<any> {
    return Observable.create((observer: any) => {
      const req = new XMLHttpRequest();
      req.open('get', url);
      req.onreadystatechange = function () {
        if (req.readyState === 4 && req.status === 200) {
          observer.next(req.response);
          observer.complete();
        }
      };

      //req.setRequestHeader('Authorization', `Bearer ${this.inj.get(AccountService).accessToken}`);
      req.send();
    });
  }

  public get<T>(url: string, params?: any): Observable<T> {
    return this.http.get<T>(url, { params: this.buildUrlSearchParams(params) });
  }

  public getBlob(url: string, params?: any): Observable<Blob> {
    return this.http.get(url, { params: this.buildUrlSearchParams(params), responseType: 'blob' });
  }

  getHttp() {
    return this.http;
  }

  public getFull<T>(url: string): Observable<HttpResponse<T>> {
    return this.http.get<T>(url, { observe: 'response' });
  }

  public post<T>(url: string, data?: any, params?: any): Observable<T> {
    return this.http.post<T>(url, data, { params: params });
  }

  

  public put<T>(url: string, data?: any, params?: any): Observable<T> {
    return this.http.put<T>(url, data, { params: params });
  }

  public delete<T>(url: string): Observable<T> {
    return this.http.delete<T>(url);
  }

  protected handleError(error, continuation: () => Observable<any>) {

    if (error.status == 401) {
      if (this.isRefreshingLogin) {
        return this.pauseTask(continuation);
      }

      this.isRefreshingLogin = true;

      return this.authService.refreshLogin().pipe(
        mergeMap(data => {
          this.isRefreshingLogin = false;
          this.resumeTasks(true);

          return continuation();
        }),
        catchError(refreshLoginError => {
          this.isRefreshingLogin = false;
          this.resumeTasks(false);

          if (refreshLoginError.status == 401 || (refreshLoginError.url && refreshLoginError.url.toLowerCase().includes(this.loginUrl.toLowerCase()))) {
            this.authService.reLogin();
            return throwError('session expired');
          }
          else {
            return throwError(refreshLoginError || 'server error');
          }
        }));
    }

    if (error.url && error.url.toLowerCase().includes(this.loginUrl.toLowerCase())) {
      this.authService.reLogin();

      return throwError((error.error && error.error.error_description) ? `session expired (${error.error.error_description})` : 'session expired');
    }
    else {
      return throwError(error);
    }
  }

  private pauseTask(continuation: () => Observable<any>) {
    if (!this.taskPauser)
      this.taskPauser = new Subject();

    return this.taskPauser.pipe(switchMap(continueOp => {
      return continueOp ? continuation() : throwError('session expired');
    }));
  }


  private resumeTasks(continueOp: boolean) {
    setTimeout(() => {
      if (this.taskPauser) {
        this.taskPauser.next(continueOp);
        this.taskPauser.complete();
        this.taskPauser = null;
      }
    });
  }





  //get IsEditOrderRequest() {
  //  return this._isEditOrderRequest;
  //}

  //set IsEditOrderRequest(isEdit) {
  //  this._isEditOrderRequest = isEdit;
  //}

  get IsRedirectRequest() {
    return this._isRedirectRequest;
  }

  set IsRedirectRequest(isRedirect : boolean){
    this._isRedirectRequest = isRedirect;
  }

  private buildUrlSearchParams(params: any): HttpParams {
    let searchParams = new HttpParams();
    for (const key in params) {
      if (params.hasOwnProperty(key)) {
        searchParams = searchParams.append(key, params[key]);
      }
    }
    return searchParams;
  }

  isLoading(show : boolean) {
    this.isLoadingSubject.next(show);
  }

  isLoadingReview(show: boolean) {
    this.isLoadingReviewSubject.next(show);
  }

  showHidePageHeader(hide : boolean) {
    this.hidePageHeader.next(hide);
  }

}
