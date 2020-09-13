
import { Component, ViewEncapsulation, OnInit, OnDestroy, ViewChildren, AfterViewInit, QueryList, ElementRef,HostListener,ViewChild } from "@angular/core";
import { Router,ActivatedRoute, NavigationStart,Event, NavigationEnd } from '@angular/router';
import { ToastaService, ToastaConfig, ToastOptions, ToastData } from 'ngx-toasta';
import { ModalDirective } from 'ngx-bootstrap/modal';

import { AlertService, AlertDialog, DialogType, AlertMessage, MessageSeverity } from '../services/alert.service';
import { NotificationService } from "../services/notification.service";
//import { AppTranslationService } from "../services/app-translation.service";
import { AccountService } from '../services/account.service';
import { LocalStoreManager } from '../services/local-store-manager.service';
import { AppTitleService } from '../services/app-title.service';
import { AuthService } from '../services/auth.service';
import { ConfigurationService } from '../services/configuration.service';
import { Permission } from '../models/permission.model';
import { LoginComponent } from "../components/login/login.component";
import { DataService } from '../services/data.service';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { PasswordRecoveryToken } from '../models/ResetPasswordViewModel';
import { WebSpotPrices, UpdateSpotsViewModel, SpotPricePreviewViewModel } from '../models/WebSpotPrice/web-spot-prices.model';
import { Observable, Subject, interval, Subscription } from 'rxjs';
import { fadeInOut, fadeInOutTime, moveInOut, moveIn, fadeOut } from '../services/animations';
import { AppInitService } from '../services/appinit.service';
import { AppRoutingService } from '../services/app-routing.service';
import * as moment from 'moment';
import 'rxjs/add/operator/filter';
import { OwlCarousel } from 'ngx-owl-carousel';
import { CookieService } from 'ngx-cookie-service';
import {ContactFormData,ContactUsMessage} from '../models/appinit.model'



declare var require: any;


var alertify: any = require('../assets/scripts/alertify.js');


@Component({
  selector: "app-root",
  templateUrl: './app.component1.html',
  styleUrls: ['./app.component2.css'],
  animations: [fadeInOut, fadeInOutTime('fadeInOutStickyHeader', 1, 0.2), moveInOut('moveInOutStickyHeader', 0.5, 0.4),
  moveIn('moveInStickyHeader',0.5),fadeOut('fadeOutStickyHeader',0.5)]
  //encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit, AfterViewInit, OnDestroy {

  isAppLoaded: boolean;
  isUserLoggedIn: boolean;
  shouldShowLoginModal: boolean;
  removePrebootScreen: boolean;
  newNotificationCount = 0;
  isPortalClosed: boolean = false;
  appTitle = "A-Mark Portal";
  appLogo = require("../assets/images/logo.png");
  appLogoSmall = require("../assets/images/logo-small.png");
  newsThumb1 = require("../assets/images/news-thumb-1.jpg");
  newsThumb2 = require("../assets/images/news-thumb-2.jpg");
  background = require("../assets/images/bg03.jpg");
  backgroundFooter = require("../assets/images/buildings.png");

  @ViewChild('owlElement') owlElement: OwlCarousel;
  //news-thumb-1

  loading: boolean = false;
  loadingReview: boolean = false;
  isAdminUser: boolean = false;

  isReview: boolean = false;
  displayNavBar: boolean = false;
  displayHeader: boolean = true;

  applyfixedHeader: boolean = false;

  pageTitle: string;

  stickyToasties: number[] = [];

  contactUsMessage: ContactUsMessage = new ContactUsMessage();

  dataLoadingConsecutiveFailurs = 0;
  notificationsLoadingSubscription: any;

  spotPriceViewModel: UpdateSpotsViewModel;
  updateTime: string = "";
  //vsWhich: string = "";
  filterDate: any = "2012-01-01T00:00:00";

  isAdminSection: boolean = false;

  serverTime: any;
  compareDate = new Date();
  private updateSpotSubscription: Subscription;

  @ViewChildren('loginModal,loginControl')
  modalLoginControls: QueryList<any>;

  loginModal: ModalDirective;
  loginControl: LoginComponent;
  announcementContet: string;
  contactFormData: ContactFormData;

  @HostListener('window:scroll', ['$event'])
  onWindowScroll($event) {
    //var windowpos = window.scrollY;
    var windowpos = window.pageYOffset || document.documentElement.scrollTop;
    
    if (windowpos >= 150) {
      //setTimeout(() => { this.applyfixedHeader = true;},100);
      
      this.applyfixedHeader = true;
      //window.scrollTo({ left: 0, top: 0, behavior: 'smooth' });
    }
    else {
      this.applyfixedHeader = false;
    }
    
  }

  mySlideImages: any;
  myCarouselImages: any;
  mySlideOptions: any;
  mySlideOptions1: any;
  myCarouselOptions: any;

  constructor(storageManager: LocalStoreManager, private toastaService: ToastaService, private toastaConfig: ToastaConfig,
    private accountService: AccountService, private alertService: AlertService, private notificationService: NotificationService, private appTitleService: AppTitleService,
    private authService: AuthService,public configurations: ConfigurationService, public router: Router, private dataService: DataService,
    private http: HttpClient,
    private appInitService: AppInitService,
    private localStorage: LocalStoreManager,
    private activatedRoute: ActivatedRoute,
    private approutingService: AppRoutingService,
    private cookieService : CookieService) {

    this.mySlideImages = [1, 2, 3].map((i) => `https://picsum.photos/640/480?image=${i}`);
    this.myCarouselImages = [1, 2, 3, 4, 5, 6].map((i) => `https://picsum.photos/640/480?image=${i}`);
    this.mySlideOptions = {
      items: 1,
      dots: true,
      nav: true,
      autoplay: 5000,
      loop: true,
      smartSpeed: 700,
      margin: 30,
      navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>']
    };
   

    this.myCarouselOptions = { items: 2, dots: true, nav: true };
    this.contactFormData = this.appInitService.getContactFormData();
    //this.setPortalCloseStatus(this.appInitService.isPortalClosed());
    // Spot Price //
    this.spotPriceViewModel = this.appInitService.getWebSpotPrices();
    this.authService.isPortalClosed = this.appInitService.isPortalClosed();
    this.isPortalClosed = this.appInitService.isPortalClosed();
    this.updateTime = " as of " + moment(this.appInitService.getAppLoadDateTime()).format("ddd MMM DD YYYY HH:mm:ss") + " PST";
    
    if (this.spotPriceViewModel.IsAfterHours == 'YES') {
      this.spotPriceViewModel.VsWhich = 'NY';
      this.localStorage.savePermanentData('NY', "VS");
      
    }
    
    else {
      
      let vsWhich = this.localStorage.getData('VS');
      if (vsWhich) {
        this.spotPriceViewModel.VsWhich = vsWhich;
      }
      else {
        this.spotPriceViewModel.VsWhich = "NY"
        this.localStorage.savePermanentData(this.spotPriceViewModel.VsWhich, "VS");
        
      }
   
    }
   

    this.dataService.isLoadingSubject$.subscribe((show: boolean) => {
      this.isReview = false;
      this.loadingReview = show;
    });

    this.dataService.isLoadingReviewSubject$.subscribe((show: boolean) => {
      this.isReview = true;
      setTimeout(() => {
        this.loadingReview = show;
      },1000);
    });

    this.dataService.hidePageHeader$.subscribe((hide: boolean) => {
      this.displayHeader = !hide;
    });

    storageManager.initialiseStorageSyncListener();

    this.toastaConfig.theme = 'bootstrap';
    this.toastaConfig.position = 'top-right';
    this.toastaConfig.limit = 100;
    //this.toastaConfig.showClose = true;

    this.appTitleService.appName = this.appTitle;

  }
  token: string;
  changePassword() {
    this.router.navigate(['/changepassword']);
  }

  sendMessage() {
    debugger;
    this.http.post('api/contactus/sendcustomermessage', this.contactUsMessage)
      .subscribe((response) => {
        if (response) {
          this.alertService.showMessage("contact Us", "Message Sent successfully", MessageSeverity.success);
        }
        else {
          this.alertService.showMessage("contact Us", "Something went wrong!!", MessageSeverity.error);
        }
      });

  }

  getWebSpotPrices() {
    this.dataService.get<UpdateSpotsViewModel>('api/SpotPrice/SpotPricePreview').subscribe((model) => {
      if (model) {
        this.spotPriceViewModel = model;
        //this.spotPriceViewModel.VsWhich = localStorage.getItem("VS") || this.spotPriceViewModel.VsWhich;
        //this.webSpotPrices = model.Spots;
        if (!this.spotPriceViewModel.IsAfterHours) {
          var vs = this.localStorage.getData("VS");
          if (vs) {
            this.spotPriceViewModel.VsWhich = vs;
          }
          else {
            this.localStorage.savePermanentData(this.spotPriceViewModel.VsWhich, "VS");
          }
        }
        else {
          this.localStorage.savePermanentData(this.spotPriceViewModel.VsWhich, "VS");
        }
      }
    });
  }

  vsWhichChanged() {
    var maxDate = new Date("1/1/2000").toISOString();
    this.getUpdatedSpotPrices(maxDate);
  }

  getUpdatedSpotPrices(filterDate) {
    
    var data = {
      LastDate: filterDate,
      VsWhich: this.localStorage.getData("VS"),
      //VsWhich: this.cookieService.get("VS"),
      IsAfterHours: this.spotPriceViewModel ? this.spotPriceViewModel.IsAfterHours : ""
    };

    this.dataService.get<UpdateSpotsViewModel>('api/SpotPrice/UpdateSpots', data).subscribe((data) => {
      
      if (data.IsAfterHours != 'YES' && data.IsClosed != 'YES') {
        this.spotPriceViewModel = data;
        
      }
      else {
        data.Spots = this.spotPriceViewModel.Spots;
        this.spotPriceViewModel = data;
      }
      this.spotPriceViewModel.VsWhich = this.localStorage.getData("VS")
      //this.spotPriceViewModel.VsWhich = this.cookieService.get("VS")
    });
  }


  ngAfterViewInit() {

    this.modalLoginControls.changes.subscribe((controls: QueryList<any>) => {
      controls.forEach(control => {
        if (control) {
          if (control instanceof LoginComponent) {
            this.loginControl = control;
            this.loginControl.modalClosedCallback = () => this.loginModal.hide();
          }
          else {
            this.loginModal = control;
            this.loginModal.show();
          }
        }
      });
    });
  }


  onLoginModalShown() {
    this.alertService.showStickyMessage("Session Expired", "Your Session has expired. Please log in again", MessageSeverity.info);
  }


  onLoginModalHidden() {
    
    this.alertService.resetStickyMessage();
    this.loginControl.reset();
    this.shouldShowLoginModal = false;
    if (this.authService.isSessionExpired)
      this.alertService.showStickyMessage("Session Expired", "Your Session has expired. Please log in again to renew your session", MessageSeverity.warn);
  }


  onLoginModalHide() {
        this.alertService.resetStickyMessage();
  }


  ngOnInit() {

    this.isUserLoggedIn = this.authService.isLoggedIn;
    this.isAdminUser = this.accountService.isAdminUser;

    this.getUpdatedSpotPrices(this.filterDate);
    this.updateSpotSubscription = interval(20000).subscribe(() => {
      this.dataService.get<{ serverTime: string }>("api/Utility/GetServerTime").subscribe((data) => {
        //this.updateTime = " as of " + moment(data.serverTime).format("ddd MMM DD YYYY HH:mm:ss") + " PST";

        this.serverTime = data.serverTime;
        this.compareDate = new Date(parseInt(data.serverTime.substr(6)));

        this.filterDate = new Date(this.compareDate);
        this.filterDate = this.filterDate.toISOString();
        this.getUpdatedSpotPrices(this.filterDate);
        this.updateTime = " as of " + moment(this.serverTime).format("ddd MMM DD YYYY HH:mm:ss") + " PST";
      });
    });

    this.activatedRoute.data.subscribe(data => {
      
    });



    // 1 sec to ensure all the effort to get the css animation working is appreciated :|, Preboot screen is removed .5 sec later
    setTimeout(() => this.isAppLoaded = true, 1000);
    setTimeout(() => this.removePrebootScreen = true, 1500);

    setTimeout(() => {
      if (this.isUserLoggedIn) {
      
      }
    }, 2000);

    this.alertService.showCloseButton$.subscribe((show: boolean) => {
      this.toastaConfig.showClose = show;
    });

    this.alertService.getDialogEvent().subscribe(alert => this.showDialog(alert));
    this.alertService.getMessageEvent().subscribe(message => this.showToast(message, false));
    this.alertService.getStickyMessageEvent().subscribe(message => this.showToast(message, true));

    this.authService.reLoginDelegate = () => this.shouldShowLoginModal = true;
    this.authService.logoutDelegate = () => this.logout();
    this.authService.storeCloseDelegate = (isPortalClosed : boolean) => this.setPortalCloseStatus(isPortalClosed);
    this.authService.getLoginStatusEvent().subscribe(isLoggedIn => {
      this.isUserLoggedIn = isLoggedIn;

      this.isAdminUser = this.accountService.isAdminUser;
      if (this.isUserLoggedIn) {

        this.initNotificationsLoading();
      }
      else {
        this.unsubscribeNotifications();
      }

      setTimeout(() => {
        if (!this.isUserLoggedIn) {
          this.alertService.showMessage("Session Ended!", "", MessageSeverity.default);
        }
      }, 500);
    });

    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        if (this.isUserLoggedIn && event.url.toLowerCase() != "/login") {
          this.dataService.get<string>('api/Content/GetContentById/17')
            .subscribe((content) => {
              this.announcementContet = content;
            });
        }
        if (!this.displayHeader) {
          this.displayHeader = true;
        }
        if (!this.dataService.IsRedirectRequest) {
          this.alertService.stopLoadingMessage();
          this.dataService.isLoading(false);
        }

        let url = (<NavigationStart>event).url;
        if (url !== url.toLowerCase()) {
          this.router.navigateByUrl((<NavigationStart>event).url.toLowerCase());
        }
      }
      else if (event instanceof NavigationEnd) {
        var e = event;
        var title = this.approutingService.getRouteTitle();
       
        if (title && title.toLowerCase() == 'admin') {
          this.isAdminSection = true;
        }
        else {
          this.isAdminSection = false;
        }
        window.scrollTo(0, 0);

      }
    });

    //this.router.events
    //  .filter(event => event instanceof NavigationEnd)
    //  .map(() => this.activatedRoute)
    //  .map(route => {
    //    while (route.firstChild) route = route.firstChild;
    //    return route;
    //  })
    //  .filter(route => route.outlet === 'primary')
    //  .mergeMap(route => route.data)
    //  .subscribe((event) => {
    //    debugger;
    //    var title = event['title'];
    //  });
  
  }
  
  private unsubscribeNotifications() {
    if (this.notificationsLoadingSubscription)
      this.notificationsLoadingSubscription.unsubscribe();
  }

  setPortalCloseStatus(isPortalClosed : boolean) {
    this.isPortalClosed = isPortalClosed;
    //if (this.isPortalClosed) {
    //  this.router.navigate(['/storeclose']);
    //}
  }

  initNotificationsLoading() {

    this.notificationsLoadingSubscription = this.notificationService.getNewNotificationsPeriodically()
      .subscribe(notifications => {
        this.dataLoadingConsecutiveFailurs = 0;
        this.newNotificationCount = notifications.filter(n => !n.isRead).length;
      },
        error => {
          this.alertService.logError(error);

          if (this.dataLoadingConsecutiveFailurs++ < 20)
            setTimeout(() => this.initNotificationsLoading(), 5000);
          else
            this.alertService.showStickyMessage("Load Error", "Loading new notifications from the server failed!", MessageSeverity.error);
        });
  }


  //markNotificationsAsRead() {

  //  let recentNotifications = this.notificationService.recentNotifications;

  //  if (recentNotifications.length) {
  //    this.notificationService.readUnreadNotification(recentNotifications.map(n => n.id), true)
  //      .subscribe(response => {
  //        for (let n of recentNotifications) {
  //          n.isRead = true;
  //        }

  //        this.newNotificationCount = recentNotifications.filter(n => !n.isRead).length;
  //      },
  //        error => {
  //          this.alertService.logError(error);
  //          this.alertService.showMessage("Notification Error", "Marking read notifications failed", MessageSeverity.error);

  //        });
  //  }
  //}



  showDialog(dialog: AlertDialog) {

    alertify.set({
      labels: {
        ok: dialog.okLabel || "OK",
        cancel: dialog.cancelLabel || "Cancel"
      }
    });

    switch (dialog.type) {
      case DialogType.alert:
        alertify.alert(dialog.message);

        break
      case DialogType.confirm:
        alertify
          .confirm(dialog.message, (e) => {
            if (e) {
              dialog.okCallback();
            }
            else {
              if (dialog.cancelCallback)
                dialog.cancelCallback();
            }
          });

        break;
      case DialogType.prompt:
        alertify
          .prompt(dialog.message, (e, val) => {
            if (e) {
              dialog.okCallback(val);
            }
            else {
              if (dialog.cancelCallback)
                dialog.cancelCallback();
            }
          }, dialog.defaultValue);

        break;
    }
  }

  getMainMenuPadding() {
    if (this.isAdminSection) {
      return '10px';
    }
    else {
      return '22px';
    }
  }



  showToast(message: AlertMessage, isSticky: boolean) {
    if (message == null) {
      for (let id of this.stickyToasties.slice(0)) {
        this.toastaService.clear(id);
      }

      return;
    }
    
    let toastOptions: ToastOptions = {
      title: message.summary,
      msg: message.detail,
      timeout: isSticky ? 0 : 4000
      //showClose:false
    };


    if (isSticky) {
      toastOptions.onAdd = (toast: ToastData) => this.stickyToasties.push(toast.id);
      //toastOptions.showClose = false;
      toastOptions.onRemove = (toast: ToastData) => {
        let index = this.stickyToasties.indexOf(toast.id, 0);

        if (index > -1) {
          this.stickyToasties.splice(index, 1);
        }

        toast.onAdd = null;
        toast.onRemove = null;
      };
    }


    switch (message.severity) {
      case MessageSeverity.default: this.toastaService.default(toastOptions); break;
      case MessageSeverity.info: this.toastaService.info(toastOptions); break;
      case MessageSeverity.success: this.toastaService.success(toastOptions); break;
      case MessageSeverity.error: this.toastaService.error(toastOptions); break;
      case MessageSeverity.warn: this.toastaService.warning(toastOptions); break;
      case MessageSeverity.wait: this.toastaService.wait(toastOptions); break;
    }
  }

 



  logout() {
    this.authService.logout();
    this.authService.redirectLogoutUser();
  }


  getYear() {
    return new Date().getUTCFullYear();
  }


  get userName(): string {
    return this.authService.currentUser ? this.authService.currentUser.userName : "";
  }


  get fullName(): string {
    return this.authService.currentUser ? this.authService.currentUser.fullName : "";
  }

  get userFirstName(): string {
    return this.authService.currentUser ? this.authService.currentUser.firstName : "";
  }



  get canViewCustomers() {
    return true;
    //return this.accountService.userHasPermission(Permission.viewUsersPermission); //eg. viewCustomersPermission
  }

  get canViewProducts() {
    return true;
    //return this.accountService.userHasPermission(Permission.viewUsersPermission); //eg. viewProductsPermission
  }

  get canViewOrders() {
    return true; //eg. viewOrdersPermission
  }

  ngOnDestroy() {
    this.unsubscribeNotifications();
    this.updateSpotSubscription.unsubscribe();
  }
}
