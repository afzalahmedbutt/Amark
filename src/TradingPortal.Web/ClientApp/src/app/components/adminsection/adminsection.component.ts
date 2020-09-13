import { Component, OnInit } from '@angular/core';
import { DataService } from '../../services/data.service';
import { fadeInOut, fadeInOutTime, moveInOut, moveIn, fadeOut } from '../../services/animations';

declare var require: any;

@Component({
  selector: 'app-adminsection',
  templateUrl: './adminsection.component.html',
  styleUrls: ['./adminsection.component.css'],
  animations: [fadeInOut, fadeInOutTime('fadeInOutStickyHeader', 1, 0.2), moveInOut('moveInOutStickyHeader', 0.5, 0.4),
    moveIn('moveInStickyHeader', 0.5), fadeOut('fadeOutStickyHeader', 0.5)]
})
export class AdminsectionComponent implements OnInit {

  appLogo = require("../../assets/images/logo.png");
  appLogoSmall = require("../../assets/images/logo-small.png");

  constructor(private dataService: DataService) {
    //this.dataService.showHidePageHeader(true);
    //his.exceptionIdentifier = this.dataService.exceptionIdentifier;
  }

  ngOnInit() {
  }

}
