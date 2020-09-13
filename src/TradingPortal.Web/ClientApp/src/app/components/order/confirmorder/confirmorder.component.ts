import { Component, OnInit } from '@angular/core';
import { RequestForOrderModel, RequestForOrderItemModel } from '../../../models/order/requestfororder.model';
import { DataService } from '../../../services/data.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AlertService, MessageSeverity, DialogType } from '../../../services/alert.service';



@Component({
  selector: 'app-confirmorder',
  templateUrl: './confirmorder.component.html',
  styleUrls: ['./confirmorder.component.css']
})
export class ConfirmOrderComponent implements OnInit {

  orderModel: RequestForOrderModel;
  isSell: boolean = false;
  orderType: string;
  orderTypeNormalized: string;
  

  constructor(
    private dataService: DataService,
    private activatedRoute: ActivatedRoute,
    private alertService : AlertService
  ) {
    this.activatedRoute.url.subscribe(data => {
      var msg = "";
      
      let url = data[data.length - 1].path;
      if (url == 'confirmsell') {
        this.isSell = true;
        this.orderType = "Sell";
        this.orderTypeNormalized = "SELL";
        msg = "Confirming Sell Order Request ......";
      }
      else {
        this.orderType = "Buy";
        this.orderTypeNormalized = "BUY";
        msg = "Confirming Buy Order Request ......";
      }
      if (!this.dataService.IsRedirectRequest) {
        this.alertService.startLoadingMessage(msg);
      }
      
      this.dataService.get<RequestForOrderModel>('api/RequestForOrder/RequestForConfirmation')
        .subscribe((model) => {
          this.alertService.stopLoadingMessage();
          this.dataService.isLoading(false);
          this.dataService.IsRedirectRequest = false;
          
          this.orderModel = model;
        });  
    });
    

  }

  ngOnInit() {
    window.scrollTo(0, 0);
  }

}
