import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { WebSpotPrices, UpdateSpotsViewModel, SpotPricePreviewViewModel } from '../../models/WebSpotPrice/web-spot-prices.model';
import { DataService } from '../../services/data.service';
import { Observable, Subscription, interval } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { LocalStoreManager } from '../../services/local-store-manager.service';
import { debug } from 'util';
import { AppInitService } from '../../services/appinit.service';
import { CookieService } from 'ngx-cookie-service';
import * as moment from 'moment';

declare var require: any;

@Component({
  selector: 'app-spotprice',
  templateUrl: './spotprice.component1.html',
  styleUrls: ['./spotprice.component.css']
})
export class SpotpriceComponent {

  constructor(
    private dataService: DataService,
    private localStorage: LocalStoreManager,
    private appInitService: AppInitService,
    private cookieService: CookieService) {

  }

  spotPriceLogo = require("../../assets/images/chart-thumb.png");
  downArrow = require("../../assets/images/down-arrow.png");
  upArrow = require("../../assets/images/up-arrow.png");

  private updateSpotSubscription: Subscription;
  isAfterhours: boolean;

  @Input()
  spotPriceViewModel: UpdateSpotsViewModel;

  @Input()
  prefix: string;

  @Output()
  vsWhichChange: EventEmitter<string> = new EventEmitter<string>();

  @Input()
  updateTime: string = "";//" as of Mon Jul 30 2018 20:14:53 PST";

  appLogo = require("../../assets/images/logo-small.png");


  vsWhichClicked() {
    this.localStorage.savePermanentData(this.spotPriceViewModel.VsWhich, "VS");
    var maxDate = new Date("1/1/2000").toISOString();
    this.vsWhichChange.emit(maxDate);
  }


  getMetalNameFromComCode(comCode) {
    var metalName: string = "";
    switch (comCode) {
      case "G":
        metalName = "GOLD";
        break;
      case "S":
        metalName = "SILVER";
        break;
      case "P":
        metalName = "PLATINUM";
        break;
      case "L":
        metalName = "PALLADIUM";
        break;
    }
    return metalName;
  }

  getColor(item: WebSpotPrices) {
    if (item.Ask < item.VsClose) {
      return 'Red';
    }
    else if (item.Ask == item.VsClose) {
      return 'Grey';
    }
    else if (item.Ask > item.VsClose) {
      return '';
    }
  }

  parseInteger(num) {
    return parseInt(num);
  }
}
