import { Component, OnInit, Input } from '@angular/core';
import { RequestForOrderModel, RequestForOrderItemModel } from '../../../models/order/requestfororder.model';

@Component({
  selector: 'app-dropship',
  templateUrl: './dropship.component.html',
  styleUrls: ['./dropship.component.css']
})
export class DropshipComponent implements OnInit {

  constructor() { }

  @Input()
  orderModel: RequestForOrderModel;

  isSubmitted: boolean = false;

  ngOnInit() {
  }

}
