import { Component, OnInit } from '@angular/core';
import {DataService } from '../../services/data.service';

@Component({
  selector: 'app-errorpage',
  templateUrl: './errorpage.component.html',
  styleUrls: ['./errorpage.component.css']
})
export class ErrorpageComponent implements OnInit {

  constructor(private dataService: DataService) {
    this.dataService.showHidePageHeader(true);
    this.exceptionIdentifier = this.dataService.exceptionIdentifier;
  }

  exceptionIdentifier: string;

  ngOnInit() {
  }

}
