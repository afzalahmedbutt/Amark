import { Component, OnInit,Input } from '@angular/core';
import { RequestForOrderModel, RequestForOrderItemModel } from '../../../models/order/requestfororder.model';

@Component({
  selector: 'app-ordersummary',
  templateUrl: './ordersummary.component.html',
  styleUrls: ['./ordersummary.component.css']
})
export class OrdersummaryComponent implements OnInit {

  constructor() { }

  @Input()
  orderModel: RequestForOrderModel;

  ngOnInit() {
    window.scrollTo(0, 0);
  }

}
