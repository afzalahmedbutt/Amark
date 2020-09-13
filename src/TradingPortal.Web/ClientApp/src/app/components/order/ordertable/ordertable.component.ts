import { Component, OnInit, Input } from '@angular/core';
import {RequestForOrderModel,RequestForOrderItemModel, CustomerFavoriteProduct } from '../../../models/order/requestfororder.model';
import { DataService } from '../../../services/data.service';
import { RangeContantsService } from '../../../services/range-constants.service';
import { AppInitService } from '../../../services/appinit.service';

@Component({
  selector: 'app-ordertable',
  templateUrl: './ordertable.component.html',
  styleUrls: ['./ordertable.component.css']
})
export class OrdertableComponent implements OnInit {

  constructor(
    private dataService: DataService,
    private rangeConstants: RangeContantsService,
    private appInitService : AppInitService) {
    this.maxProductQuantity = this.appInitService.getMaxProductQuantity(),
    this.cols = [
      { field: '', header: 'Commodity' },
      { field: '', header: 'Favorite' },
      { field: 'ProductDesc', header: 'Product Name' },
      { field: 'MinPurchase', header: 'Qty Min' },
      { field: 'PurchaseIncrement', header: 'Increment' },
      { field: 'Quantity', header: 'Qty.' },
      { field: 'ProductPremium', header: 'Premium' },
      { field: 'TierPrices', header: 'Tiers' },
      { field: 'WeightSubTotal', header: 'Oz.' },
      { field: 'UnitPrice', header: 'Unit Price' },
      { field: 'SubTotal', header: 'Total' }
      
    ];
  }

  _orderModel: RequestForOrderModel;
  productsSnapshot: RequestForOrderItemModel[];
  modifiedProducts: { ProductName: string, ISFavorite: boolean }[];
  groupState: any;
  rowGroupMetadata: any;
  orderItems: RequestForOrderItemModel[];
  expandedRows: any = {};
  errorMsg: string = "Invalid Quantity";
  @Input()
  isSell: boolean = false;
  cols: any;
  maxProductQuantity: Number;
 
  @Input()
  set orderModel(orderModel: RequestForOrderModel) {
    this._orderModel = orderModel;

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

  get orderModel() {
    return this._orderModel;
  }

  favSelectionChange(row: RequestForOrderItemModel) {
    var prodcut = new CustomerFavoriteProduct();
    prodcut.ProductName = row.ProductName;
    prodcut.IsFavorite = row.IsFavorite;
    row.IsDisabled = true;
    this.dataService.post('api/RequestForOrder/AddRemoveFavoriteProduct', prodcut)
      .subscribe((response) => {
        if (response) {
          row.IsDisabled = false;
        }
      });
  }

  filterProducts(search: string) {
    this.orderItems = this.orderModel.Items.filter((item) => { return item.ProductDesc.toLowerCase().indexOf(search.toLowerCase()) > -1 });
    this.updateRowGroupMetaData(this.orderItems);
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
    var orderItems = this.orderModel.Items;
    //remove spaces
    let quantity = item.Quantity.toString().replace(/^\s+|\s+$/g, "");

    if (quantity == "") {
      item.Quantity = 0;
    }
    //make sure there is no more than 5 decimal places
    item.Quantity = parseFloat(item.Quantity.toString()).toFixed(5);
    //item.Quantity = parseFloat(parseFloat(item.Quantity.toString()).toFixed(5));
    item.ErrorMsg = "";
    if (isNaN(parseFloat(item.Quantity))) {
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
      if ((((Math.ceil(item.Quantity * 100000) - Math.ceil(item.MinPurchase) * 100000) % Math.ceil(item.PurchaseIncrement * 100000)) != 0)
        || item.Quantity > this.maxProductQuantity) {
        item.ErrorMsg = this.errorMsg;
        return;
      }
    }
    //txtOz.html(roundNumber(parseFloat(weight.html()) * parseFloat(txtQty.val()), 5));
    item.WeightSubTotal = item.ProductWeight * item.Quantity;
    //txtTotal.html(formatCurrency(roundNumber(parseFloat(unitprice.html().replace("$", "").replace(",", "")) * parseFloat(txtQty.val()), 2)));
    item.SubTotal = item.UnitPrice * item.Quantity;
  }

  onCommodityClicked(index, commodity, event) {
    event.stopPropagation();
    this.groupState[commodity].expanded = !this.groupState[commodity].expanded;
    this.orderItems = this.orderModel.Items.slice();
    return false;
  }

  expandAllRows() {
    for (let prop in this.rowGroupMetadata) {

      this.groupState[prop] = { expanded: true };
    }

    this.orderModel.Items.forEach((item) => {
      this.expandedRows[item.Commodity] = 1;
    })
  }

  displayAbs(val) {
    return Math.abs(val);
  }

  ngOnInit() {
  }

}
