import { Component, OnInit,Input,ViewChild } from '@angular/core';
import { RequestForOrderModel, RequestForOrderItemModel } from '../../../models/order/requestfororder.model';
import { NgForm } from '@angular/forms';
import { fadeInOut, moveInOut, moveInOutDropShip} from '../../../services/animations';

@Component({
  selector: 'app-orderfooter',
  templateUrl: './orderfooter.component.html',
  styleUrls: ['./orderfooter.component.css'],
  animations: [fadeInOut, moveInOutDropShip('dropShipInOut',0.5,0.4)]
})
export class OrderfooterComponent implements OnInit {

  @Input()
  orderModel: RequestForOrderModel;
  @Input()
  isSell: boolean = false;
  @ViewChild('f') footerForm: NgForm;

  isSubmitted: boolean = false;

  constructor() { }

  ngOnInit() {
  }

}
