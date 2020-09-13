import { Component, Input, OnChanges, TemplateRef, ViewChild, AfterViewInit, HostListener } from '@angular/core';
import { fadeInOut } from '../../../services/animations';
import { ConfigurationService } from '../../../services/configuration.service';
import { RequestForOrderModel, RequestForOrderItemModel, ConfirmOrderResponse } from '../../../models/order/requestfororder.model';
import { ActivatedRoute, Params, Router } from '@angular/router'
import { debug } from 'util';
import { CustomerService } from '../../../services/customer.service'
import { AlertService, MessageSeverity, DialogType } from '../../../services/alert.service';
import { DataService } from '../../../services/data.service';
import { interval } from 'rxjs';
import { map } from 'rxjs/operators';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { Route } from '@angular/compiler/src/core';
//import { Confirmation } from '../../../primeng/common/confirmation';
import { ConfirmationService, Confirmation } from '../../../primeng/common/api';
import { OrderfooterComponent } from '../orderfooter/orderfooter.component'
import { NgForm } from '@angular/forms';
import { LocalStoreManager } from '../../../services/local-store-manager.service';
import { SessionKeys } from '../../../services/db-keys';



@Component({
  selector: 'app-request-for-order',
  templateUrl: './request-for-order.component.html',
  styleUrls: ['./request-for-order.component.css'],
  animations: [fadeInOut]
})
export class RequestForOrderComponent implements AfterViewInit {
  constructor(public configurations: ConfigurationService,
    private activatedRoute: ActivatedRoute,
    private customerService: CustomerService,
    private alertService: AlertService,
    private dataService: DataService,
    private modalService: BsModalService,
    private router: Router,
    private confirmationSerivce: ConfirmationService,
    private localStoreManager: LocalStoreManager
    
  ) {
    this.activatedRoute.url.subscribe(data => {
      let url = data[data.length - 1].path;
      if (url == 'buy') {
        this.isBuy = true;
      }
      else if (url == 'sell') {
        this.isSell = true;
      }
     if (this.localStoreManager.getData(SessionKeys.IS_REVIEW_PAGE_REFRESH)) {
        this.orderModel = this.localStoreManager.getData(SessionKeys.ORDER_MODEL);
        this.postReviewData(true);
      }
      else {
        this.loadRequestForOrderData();
      }
    });
  }

  @HostListener('window:beforeunload') goToPage($event) {
    if (this.lastAction == 'Review') {
      this.localStoreManager.saveSessionData(true, SessionKeys.IS_REVIEW_PAGE_REFRESH);
      this.localStoreManager.saveSessionData(this.orderModel, SessionKeys.ORDER_MODEL);
    }
  }

  @HostListener('window:keydown')
  @HostListener('window:keypress')
  @HostListener('window:keyup')
  checkPageRefresh(e) {
    
    var ev: any = e || window.event;
    if (ev.keyCode == 116) {
      if (this.lastAction == 'Review') {
        this.localStoreManager.saveSessionData(true, SessionKeys.IS_REVIEW_PAGE_REFRESH);
        this.localStoreManager.saveSessionData(this.orderModel, SessionKeys.ORDER_MODEL);
        console.log("F5 pressed Keydown!");
      }
    }
  }
  
  @ViewChild('orderFooter')
  orderFooter: OrderfooterComponent;

  childForm: NgForm;
  errorMsg: string = "Invalid Quantity";
  expandedRows: any = {};
  orderModel: RequestForOrderModel;
  rowGroupMetadata: any;
  orderItems: RequestForOrderItemModel[];
  groupState: any;
  removedRows = {};
  visible: true;
  counter: number;
  displayConfirm: boolean = true;
  isSell: boolean = false;
  isBuy: boolean = false;
  lastAction: string;

  cols: any[];
  favoriteProdutsNames: string[];
  productsSnapshot: RequestForOrderItemModel[];
  filterText: string = "";
  modalRef: BsModalRef;




  filterProducts(search: string) {
    this.orderItems = this.orderModel.Items.filter((item) => { return item.ProductDesc.toLowerCase().indexOf(search.toLowerCase()) > -1 });
    this.updateRowGroupMetaData(this.orderItems);
  }


  loadRequestForOrderData() {
    
    this.dataService.isLoading(true);
    this.alertService.startLoadingMessage("Loading Products ......");
    this.dataService.get('api/RequestForOrder/RequestForOrder', { isSell: this.isSell }).subscribe((data: any) => {
      debugger;
      this.processLoadOrderResponse(data);
      
    })
  }

  processLoadOrderResponse(data: RequestForOrderModel) {
    
    if (!data) {
      return;
    }
    this.dataService.isLoadingReview(false);
    this.alertService.stopLoadingMessage();
    this.dataService.isLoading(false);
    this.orderModel = data;//.orderProducts;
    this.productsSnapshot = JSON.parse(JSON.stringify(this.orderModel.Items));
    this.orderItems = this.orderModel.Items.slice();
    this.updateRowGroupMetaData(this.orderModel.Items);
    this.groupState = {};
    for (let prop in this.rowGroupMetadata) {

      this.groupState[prop] = { expanded: true };
    }

    this.orderModel.Items.forEach((item) => {
      this.expandedRows[item.Commodity] = 1;
    })

  }

  initiliazeProductsData(data: RequestForOrderModel) {
    this.orderModel = data;//.orderProducts;
    this.productsSnapshot = JSON.parse(JSON.stringify(this.orderModel.Items));
    //this.favoriteProdutsNames = this.orderModel.Items.filter((item) => { return item.IsFavorite }).map(item => {item.ProductName,item.IsFavorite }).;
    this.orderItems = this.orderModel.Items.slice();
    console.log(this.orderModel.Items);
    this.updateRowGroupMetaData(this.orderModel.Items);
    this.groupState = {};
    for (let prop in this.rowGroupMetadata) {

      this.groupState[prop] = { expanded: true };
    }

    this.orderModel.Items.forEach((item) => {
      this.expandedRows[item.Commodity] = 1;
    })
  }


  differenceInFirstArray(array1: RequestForOrderItemModel[], array2: RequestForOrderItemModel[], compareField: string[]): RequestForOrderItemModel[] {
    return array1.filter(function (current) {
      return array2.filter(function (current_b) {
        return current_b[compareField[0]] === current[compareField[0]] && current_b[compareField[1]] === current[compareField[1]]
      }).length == 0;
    });
  }

  expandAllRows() {
    for (let prop in this.rowGroupMetadata) {

      this.groupState[prop] = { expanded: true };
    }

    this.orderModel.Items.forEach((item) => {
      this.expandedRows[item.Commodity] = 1;
    })
  }

  onCommodityClicked(index, commodity, event) {
    event.stopPropagation();
    this.groupState[commodity].expanded = !this.groupState[commodity].expanded;
    this.orderItems = this.orderModel.Items.slice();
    return false;
  }

  onCommodityClicked1(index, commodity, event) {
    event.stopPropagation();
    if (!this.groupState[commodity].expanded) {
      let startingChunk = this.orderItems.splice(0, index);
      let groupSize = this.rowGroupMetadata[commodity].size;
      this.orderItems.splice(0, groupSize);
      var allGroupItems = this.orderModel.Items.filter((item) => item.Commodity == commodity);
      this.orderItems = startingChunk.concat(allGroupItems).concat(this.orderItems);
    }
    else {
      //this.orderItems = this.orderItems.filter((item) => {
      //  return (item.Commodity != commodity || item.Quantity > 0);
      //});

      let startingChunk = this.orderItems.splice(0, index);
      let groupSize = this.rowGroupMetadata[commodity].size;
      let groupItems = this.orderItems.splice(0, groupSize);
      let selectedItems = groupItems.filter((item) => {
        return (item.Commodity != commodity || item.Quantity > 0);
      });

      if (selectedItems.length == 0) {
        var item = new RequestForOrderItemModel();
        item.Commodity = commodity;
        item.IsEmpty = true;
        selectedItems.push(item);
      }
      this.orderItems = startingChunk.concat(selectedItems).concat(this.orderItems);
    }
    this.updateRowGroupMetaData(this.orderItems);
    this.groupState[commodity].expanded = !this.groupState[commodity].expanded;
    return false;
  }

  filterRows() {
    if (!this.filterText) {
      this.orderItems = this.orderModel.Items.slice();
    }
    else if (this.filterText.length < 3) {
      return;
    }
    else {
      this.orderItems = this.orderModel.Items.filter((item) => item.ProductDesc.toLowerCase().indexOf(this.filterText.toLowerCase()) > -1);
      this.updateRowGroupMetaData(this.orderItems);
      this.expandAllRows();
    }
  }

  getRandomArbitrary(min, max): number {
    return parseFloat(parseFloat(Math.random() * (max - min) + min).toFixed(5));
  }

  updateRowGroupMetaData(items) {
    if (items) {
      this.rowGroupMetadata = {};
      for (var i = 0; i < items.length; i++) {
        let rowData = items[i];
        let commodity = rowData.Commodity;
        if (i == 0) {
          this.rowGroupMetadata[commodity] = { index: 0, size: 1 };
        }
        else {
          let previousRowData = items[i - 1];
          let previousRowGroup = previousRowData.Commodity;
          if (commodity === previousRowGroup)
            this.rowGroupMetadata[commodity].size++;
          else {
            this.rowGroupMetadata[commodity] = { index: i, size: 1 };
          }
        }
      }
    }
  }

  validateQuantity(index) {

    //var item = this.orderModel.Items[index];
    var item = this.orderItems[index];

    //remove spaces
    let quantity = item.Quantity.toString().replace(/^\s+|\s+$/g, "");

    if (quantity == "") {
      item.Quantity = 0;
    }
    //make sure there is no more than 5 decimal places
    item.Quantity = parseFloat(parseFloat(item.Quantity.toString()).toFixed(5));
    item.ErrorMsg = "";
    if (isNaN(item.Quantity)) {
      item.ErrorMsg = this.errorMsg;
      return;
    }
    //make sure we get the whole number for Qty
    if (item.Quantity != 0) {
      //check to see if we are good with min qty req
      if (item.MinPurchase > item.Quantity) {
        item.ErrorMsg = this.errorMsg;
        return;
      }
      //see if the increment is correct
      if (((Math.ceil(item.Quantity * 100000) - Math.ceil(item.MinPurchase) * 100000) % Math.ceil(item.PurchaseIncrement * 100000)) != 0) {
        item.ErrorMsg = this.errorMsg;
        return;
      }
    }
    //txtOz.html(roundNumber(parseFloat(weight.html()) * parseFloat(txtQty.val()), 5));
    item.WeightSubTotal = item.ProductWeight * item.Quantity;
    //txtTotal.html(formatCurrency(roundNumber(parseFloat(unitprice.html().replace("$", "").replace(",", "")) * parseFloat(txtQty.val()), 2)));
    item.SubTotal = item.UnitPrice * item.Quantity;
  }

  reviewOrderRequest(isRequote) {
    //if (!isRequote) {
    //  this.displayConfirm = true;
    //}
    //this.displayConfirm = true;
    var form = this.childForm;
    var child = this.orderFooter;
    if (child) {
      var form = this.orderFooter.footerForm;
      if (form) {
        this.orderFooter.isSubmitted = true;
        if (!form.valid) {
          return;
        }
      }
    }
    
    if (!this.orderItems.some(item => item.Quantity > 0)) {
      this.alertService.showMessage("Error", "There are no Products selected", MessageSeverity.error);
      return;
    }
    
    if (this.orderItems.some(item => item.ErrorMsg == 'Invalid Quantity')) {
      this.alertService.showMessage("Error", "Please enter valid quantities for products", MessageSeverity.error);
      return;
    }

    this.orderModel.IsSell = this.isSell;
    this.orderModel.IsRequote = isRequote
    this.alertService.stopLoadingMessage();
    var msg = "";
    if (isRequote) {
      msg = "Posting products for requote .......";
    }
    else {
      msg = "Posting Products for review ......";
    }
    if (!this.localStoreManager.getData(SessionKeys.IS_REVIEW_PAGE_REFRESH)) {
      this.alertService.startLoadingMessage(msg);
    }
    this.postReviewData(false);
    
  }

  postReviewData(isRefresh: boolean) {
    debugger;
    if (isRefresh) {
      this.dataService.isLoading(true);
    }
    else {
      this.dataService.isLoadingReview(true);
    }
    this.dataService.post<RequestForOrderModel>('api/RequestForOrder/ReviewOrderRequest', this.orderModel)
      .subscribe((data) => {
        if (data.Warnings.length > 0) {
          this.alertService.stopLoadingMessage();
          this.dataService.isLoadingReview(false);
          for (var i = 0; i < data.Warnings.length; i++) {
            this.alertService.showMessage('', data.Warnings[i], MessageSeverity.error);
          }
        }
        else {
          this.displayConfirm = true;
          this.processReviewRequoteResponse(data);
        }
      });
  }

  processReviewRequoteResponse(data) {
    debugger;
   
    if (!data) {
      return;
    }
    window.scrollTo(0, 0);
    this.dataService.isLoadingReview(false);
    this.localStoreManager.saveSessionData(false, SessionKeys.IS_REVIEW_PAGE_REFRESH);
    
    //this.startTimer(30);
    this.alertService.stopLoadingMessage();
    this.orderModel = data;//.orderProducts;
    this.orderItems = this.orderModel.Items.slice();
    console.log(this.orderModel.Items);
    this.updateRowGroupMetaData(this.orderModel.Items);
    this.groupState = {};
    for (let prop in this.rowGroupMetadata) {

      this.groupState[prop] = { expanded: true };
    }

    this.orderModel.Items.forEach((item) => {
      this.expandedRows[item.Commodity] = 1;
    });
    setTimeout(() => {
      this.startTimer(30);
    }, 10);
    this.lastAction = 'Review';
  }

  


  confirmOrderRequest() {
    var msg = "";
    if (!this.isSell) {
      msg = "Confirming Buy Order Request ......";
    }
    else {
      msg = "Confirming Sell Order Request ......";
    }
    this.alertService.startLoadingMessage(msg);
    this.dataService.post<ConfirmOrderResponse>('api/RequestForOrder/ConfirmOrderRequest/' + this.isSell).subscribe((data) => {
      
      if (!data) {
        return;
      }
      this.alertService.stopLoadingMessage();
      if (!data.IsSuccess) {
        if (data.Warnings.length > 0) {
          //this.alertService.stopLoadingMessage();
          this.dataService.isLoadingReview(false);
          for (var i = 0; i < data.Warnings.length; i++) {
            this.alertService.showStickyMessage('', data.Warnings[i], MessageSeverity.error);
          }
        }
        return;
      }
      
      if(data)
      this.dataService.IsRedirectRequest = true;
      if (!this.isSell) {
        this.router.navigate(['/confirmbuy']);
      }
      else {
        this.router.navigate(['/confirmsell']);
      }
    });
  }

  startTimer(counter) {
    this.counter = counter;
    var countdownTimer = interval(1000).subscribe((t) => {
      this.counter--;
      if (this.counter == 0) {
        this.displayConfirm = false;
        countdownTimer.unsubscribe();
      }
    });
  }

  displayAbs(val) {
    return Math.abs(val);
  }

  openConfirmModal(template: TemplateRef<any>) {
    var confirmation: Confirmation = {
      header: 'Confirm Order Cancellation',
      message: "Are you sure you want to cancel your order?",
      accept: () => { this.router.navigate(['/']); }
    }
    
    this.confirmationSerivce.confirm(confirmation);
    
  }

  editOrderRequest() {
    this.dataService.isLoadingReview(true);
    this.alertService.startLoadingMessage("Loading Products ......");
    this.orderModel.IsSell = this.isSell;

    this.dataService.post('api/RequestForOrder/EditOrderRequest', this.orderModel).subscribe((data: any) => {
      if (!data) {
        return;
      }
      this.processLoadOrderResponse(data);
    });
  }

  scrollToTop() {
    window.scrollTo(0, 0);
  }

  ngAfterViewInit(): void {
    
  }

  cancelOrder() {
    this.modalRef.hide();
    this.router.navigate(['/']);
  }
}
