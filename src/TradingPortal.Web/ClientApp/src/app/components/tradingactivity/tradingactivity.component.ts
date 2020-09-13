import { Component, OnInit,TemplateRef,ViewChild } from '@angular/core';
import { DataService } from '../../services/data.service';
import { LocalStoreManager } from '../../services/local-store-manager.service';
import { OpenModel, ShippingInfo, UpdateShippingInfoResult,OpenPositionsModel, GridOpenModel } from '../../models/openposition/openposition.model';
import { AlertService,MessageSeverity } from '../../services/alert.service';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { saveAs } from 'file-saver';
import { enGbLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService, defineLocale } from 'ngx-bootstrap';
import * as moment from 'moment';
import { LazyLoadEvent } from '../../primeng/common/lazyloadevent';
import { concat } from 'rxjs';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { SessionKeys } from '../../services/db-keys';
import { DomSanitizer, SafeHtml, SafeStyle, SafeScript, SafeUrl, SafeResourceUrl } from '@angular/platform-browser';
import {Messages } from '../../services/messages';



@Component({
  selector: 'app-tradingactivity',
  templateUrl: './tradingactivity.component.html',
  styleUrls: ['./tradingactivity.component.css']
})
export class TradingactivityComponent implements OnInit {

  constructor(
    private dataService: DataService,
    private localStoreManager: LocalStoreManager,
    private alertService: AlertService,
    private modalService: BsModalService,
    private localeService: BsLocaleService,
    private sanitizer: DomSanitizer) {

    
    this.datePickerConfig = Object.assign({},
      {
        containerClass: 'theme-dark-blue',
        showWeekNumbers: false,
        dateInputFormat: 'MM/DD/YYYY'
      });

    enGbLocale.invalidDate = '';//this.startDate.toString();
    defineLocale('custom locale', enGbLocale);
    this.localeService.use('custom locale');

    
    this.startDate = moment().subtract(1, 'months').format('MM/DD/YYYY');
    this.endDate = moment().format('MM/DD/YYYY');
    
    if (this.localStoreManager.getData(SessionKeys.TRADING_ACTIVITY_OPTION)) {
      this.tradingActivityOption = this.localStoreManager.getData(SessionKeys.TRADING_ACTIVITY_OPTION);
    }
    else {
      this.tradingActivityOption = 0;
      this.localStoreManager.saveSessionData(this.tradingActivityOption, SessionKeys.TRADING_ACTIVITY_OPTION);
    }

    this.openPositionsCols = [
      { field: 'sdtTrade', header: 'Trade Date' },
      { field: 'sdtValue', header: 'Value Date' },
      { field: 'sTicketNo', header: 'Ticket#' },
      { field: 'sTradeType', header: 'You Have' },
      { field: 'sProductDescription', header: 'Product Description' },
      { field: 'iQuantity', header: 'Quantity' },
      { field: 'decProductBalance', header: 'Product Balance' },
      { field: 'decCashBalance', header: 'Cash Balance' },
      { field: 'decCashBalance', header: 'Shipping Status' },
    ];

    this.tradingHistoryCols = [
      { field: 'sdtTrade', header: 'Trade Date' },
      { field: 'sdtValue', header: 'Value Date' },
      { field: 'sTicketNo', header: 'Ticket#' },
      { field: 'sTradeType', header: 'You Have' },
      { field: 'sProductDescription', header: 'Product Description' },
      { field: 'iQuantity', header: 'Quantity' },
      { field: 'decProductBalance', header: 'Price' },
      { field: 'decCashBalance', header: 'Total Amount' },
      { field: 'decCashBalance', header: 'Shipping Status' },
    ];
    
    if (this.tradingActivityOption == 0) {
      this.loadingMessage = Messages.LOADING_OPEN_POSITIONS //'Loading Open Positions.....';
      this.loadOpenPositions();
    }
    //this.modalHtml = this.modalHtml + '<span class="shipping-info" (click)="openUpdateInfoModal(template)">Update Shipping Info</span>';
    
  }

 
  loadOpenPositions() {
    
    //this.alertService.startLoadingMessage("Loading Open Positions....");
    this.alertService.startLoadingMessage(this.loadingMessage);
    this.dataService.get<OpenModel[]>('api/OpenPositions/OpenPositionsSelect')
      .subscribe((data) => {
        //if (data["url"] == 'storeclose') {
        //  return;
        //}
        if (!data) {
          return;
        }
        this.alertService.stopLoadingMessage();
        this.openPositionsTable.reset();
        this.openPositions = data;
        this.openPositionsGridModel.OpenModels = data;
        this.openPositionsGridModel.Total = data.length;

        this.openPositionsGridModelCopy.OpenModels = this.openPositionsGridModel.OpenModels.slice();
        this.openPositionsGridModelCopy.Total = this.openPositionsGridModel.Total;
      });
    
  }

  shippingInfo: ShippingInfo;

  tradingActivityOption: number;
  openPositions: OpenModel[];
  tradingHistory: OpenModel[] = [];

  openPositionsGridModel: GridOpenModel = new GridOpenModel();
  openPositionsGridModelCopy: GridOpenModel = new GridOpenModel();

  tradeHistoryModel: GridOpenModel = new GridOpenModel();
  tradeHistoryModelCopy: GridOpenModel = new GridOpenModel();
  openPositionsCols: any[];
  tradingHistoryCols: any[];
  modalRef: BsModalRef;
  //modalHtml: string = '<span class="ui-column-title">{{cols[8].header}}</span>';
  selectedRow: OpenModel;
  birthDate1: any;
  datepickerModel: any = "";
  openPostitionModel: OpenPositionsModel = new OpenPositionsModel();
  startDate: any;
  endDate: string;
  isSearch: boolean = false;
  isPageRefresh: boolean = false;
  loadingMessage: string = '';

  first: number = 0;
  page: number = 1;
  loading: boolean = false;
  @ViewChild('tradeHisotryTable') tradeHisotryTable: any;

  @ViewChild('openPositionsTable') openPositionsTable: any;

  lazyEvent: LazyLoadEvent;
  datePickerConfig: Partial<BsDatepickerConfig>;

  trackingNumbersHtml: SafeHtml;

  thTicketNoSearchTerm: string;
  thProductNameSearchTerm: string;

  opTicketNoSearchTerm: string;
  opProductNameSearchTerm: string;


  openUpdateInfoModal(updateinfotemplate : TemplateRef<any>) {
    this.shippingInfo = new ShippingInfo();
    this.modalRef = this.modalService.show(updateinfotemplate);
  }

  openAddEditInfoModal(updateinfotemplate: TemplateRef<any>, row: OpenModel) {
    
    this.shippingInfo = new ShippingInfo();
    
    this.selectedRow = row;
    this.shippingInfo.iOrderHdrID = row.iOrder_Hdr_ID;
    if (row.sShipping === 'Edit') {
      this.getOrderShippingItem(updateinfotemplate);
    }
    else {
      this.modalRef = this.modalService.show(updateinfotemplate);
    }
  }

  getOrderShippingItem(updateinfotemplate:TemplateRef<any>) {
    this.alertService.startLoadingMessage("Loading shipping info ...");
    this.dataService.get<ShippingInfo>('api/OpenPositions/GetOrderShippingItem', { iOrderHdrID : this.selectedRow.iOrder_Hdr_ID })
      .subscribe((info) => {
        //if (info["url"] == 'storeclose') {
        //  return;
        //}
        if (!info) {
          return;
        }
        this.alertService.stopLoadingMessage();
        this.shippingInfo = info;
        this.modalRef = this.modalService.show(updateinfotemplate);
        //this.shippingInfo.rqs = '0';
        //this.shippingInfo = this.selectedActivity.r
      });
  }


  confirmShippingInfo() {
    
    var msg = '';
    if (this.selectedRow.sShipping === 'New') {
      this.loadingMessage = 'Adding Shipping Info...';
    }
    else if (this.selectedRow.sShipping === 'Edit'){
      this.loadingMessage = 'Updating Shipping Info...';
    }
    this.alertService.startLoadingMessage(this.loadingMessage);
    this.dataService.get<UpdateShippingInfoResult>('api/OpenPositions/UpdateOrderShippingInfo', this.shippingInfo)
      .subscribe((response) => {
        //if (response["url"] == 'storeclose') {
        //  return;
        //}
        if (!response) {
          return;
        }
        this.alertService.stopLoadingMessage();
        if (response.QValue === '0' && response.IsValid) {
          this.alertService.showMessage("Shipping Info","Shipping Info saved successfully!",MessageSeverity.success);
          this.modalRef.hide();
          this.loadingMessage = 'Refreshing Products ......';
          //this.alertService.startLoadingMessage("Refreshing Products ......");
          this.loadOpenPositions();
        }
        else if (response.QValue === '1' && response.IsValid) {
          this.modalRef.hide();
        }
        else {
          var errorMessage = 'City, State and Zip code combination is not valid.';
          this.alertService.showStickyMessage("Invalid Address", errorMessage, MessageSeverity.error);
        }
      });
  }

  generateTemplate(Value : string[]) {
    var template = "";

    for (var i = 0; i < Value.length; i++) {
      //template = template + "<u><a href='http://www.fedex.com/Tracking?action=track&language=english&cntry_code=us&initial=x&mps=y&tracknumbers=" + Value[i] + "' target='_blank'>" + Value[i] + "</a></u> ";
      template = template + "<u><a href='" + Value[i] + "' target='_blank'>" + Value[i].substring(Value[i].lastIndexOf('=') + 1) + "</a></u> ";
    }

    return template
  }

  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }


  refresh() {
    this.alertService.startLoadingMessage(Messages.REFRESHING_OPEN_POSITIONS);
    this.loadOpenPositions();
  }

  exportToExcel() {
    if (this.tradingActivityOption === 0) {
      this.dataService.isLoading(true);
      this.dataService.isLoadingReview(true);
      this.alertService.startLoadingMessage('Exporting to excel....');
      this.dataService.getBlob('api/OpenPositions/ExportExcelAll').subscribe(blob => {
        this.alertService.stopLoadingMessage();
        this.dataService.isLoading(false);
        this.dataService.isLoadingReview(false);
       saveAs(blob, 'openpositions.xlsx', {
          type: 'text/xlsx;charset=windows-1252'
        })
      });
    }
    else {
      if (!this.isDateValid()) {
        return;
      }
      this.openPostitionModel.StartDate = moment(this.startDate).format('MM/DD/YYYY');
      this.openPostitionModel.EndDate = moment(this.endDate).format('MM/DD/YYYY');
      this.openPostitionModel.Page = 1;
      this.openPostitionModel.PageSize = 20000;
      this.alertService.startLoadingMessage('Exporting to excel....');
      this.dataService.isLoading(true);
      this.dataService.isLoadingReview(true);
      this.dataService.getBlob('api/OpenPositions/ExportExcelSelected', this.openPostitionModel).subscribe(blob => {
        this.alertService.stopLoadingMessage();
        this.dataService.isLoading(false);
        this.dataService.isLoadingReview(false);
        saveAs(blob, 'tradinghistory.xlsx', {
          type: 'text/xlsx;charset=windows-1252'
        })
      });
    }
  }

  loadHistoryLazy(event: LazyLoadEvent) {
    this.lazyEvent = event;
    setTimeout(() => { this.loading = true; },0);
    
    this.openPostitionModel.StartDate = moment(this.startDate).format('MM/DD/YYYY');
    this.openPostitionModel.EndDate = moment(this.endDate).format('MM/DD/YYYY');
    this.openPostitionModel.Page = event.first /event.rows + 1;
    this.openPostitionModel.PageSize = event.rows;
    this.loadingMessage = 'Loading Trade Hisotry.....';
    //this.alertService.startLoadingMessage("Loading Trading Hisotry....");
    this.getTradeHistory();  
  
  }

  isDateValid() {
    if (this.startDate == 'Invalid Date'
      || this.endDate == 'Invalid Date') {
      this.showErrorAlert('Invalid Date', 'Please select valid start and end dates!!');
      return false;
    }
    var difference = moment(this.endDate).diff(this.startDate, 'months', true);
    if (difference <= 0) {
      this.showErrorAlert('Invalid Date Range', 'Start date should be less than end date!!');
      return false;
    }
    else if (difference > 6) {
      this.showErrorAlert('Invalid Date Range', 'Date range cannot be greater than 6 months!!');
      return false;
    }
    this.openPostitionModel.StartDate = moment(this.startDate).format('MM/DD/YYYY');
    this.openPostitionModel.EndDate = moment(this.endDate).format('MM/DD/YYYY');
    return true;
  }

  searchTradingHistory(dpStartDate) {
    this.resetTradeHistorySearchFields();
    if (!this.isDateValid()) {
      return;
    }
    this.isSearch = true;
    this.openPostitionModel.Page = 1;
    this.openPostitionModel.PageSize = 10000;
    this.loadingMessage = 'Loading Trade Hisotry....';
    this.getTradeHistory();
  }

  resetTradeHistorySearchFields() {
    this.thProductNameSearchTerm = '';
    this.thTicketNoSearchTerm = '';
  }

  resetOpenPositionsSearchFields() {
    this.opProductNameSearchTerm = '';
    this.opTicketNoSearchTerm = '';
  }

    

  searchTradingHistory1(dpStartDate) {
    if (!this.isDateValid()) {
      return;
    }

    this.isSearch = true;
    this.tradeHisotryTable.first = 0;
    this.openPostitionModel.Page = 1;
    this.first = 0;
    this.openPostitionModel.PageSize = this.lazyEvent.rows;
    this.loadingMessage = 'Loading Trade Hisotry....';
    this.getTradeHistoryLazy();
  }

  getTradeHistory() {
    this.alertService.startLoadingMessage(this.loadingMessage);
    this.dataService.get<GridOpenModel>('api/OpenPositions/TradingHistorySelect', this.openPostitionModel).subscribe((data) => {
      setTimeout(() => { this.loading = false; }, 0);

      this.alertService.stopLoadingMessage();
      this.tradeHisotryTable.reset();
      this.tradeHistoryModel = data;

      this.tradeHistoryModelCopy = new GridOpenModel();
      this.tradeHistoryModelCopy.OpenModels = this.tradeHistoryModel.OpenModels.slice();
      this.tradeHistoryModelCopy.Total = this.tradeHistoryModel.Total;
    });
   }

  getTradeHistoryLazy() {
    this.alertService.startLoadingMessage(this.loadingMessage);
    this.dataService.get<GridOpenModel>('api/OpenPositions/TradingHistorySelect', this.openPostitionModel).subscribe((data) => {
      setTimeout(() => { this.loading = false; }, 0);
      
      this.alertService.stopLoadingMessage();
      this.tradeHistoryModel = data;
      if (this.isSearch) {
        this.isSearch = false;
        this.first = 0;
      }
    });
  }

  onActivityOptionchange() {
    
    this.localStoreManager.saveSessionData(this.tradingActivityOption, SessionKeys.TRADING_ACTIVITY_OPTION);
    if (this.tradingActivityOption == 0) {
      if (!this.openPositionsGridModel.OpenModels) { 
      this.loadingMessage = 'Loading Open Positions.....';
      this.loadOpenPositions();
      }
    }
    else {
      if (!this.tradeHistoryModel) {
        this.tradeHistoryModel = new GridOpenModel();
      }
    }
  }

  

  filterProducts() {
    //console.log(this.opTicketNoSearchTerm);
    if (this.tradingActivityOption == 0) {
      if (this.openPositionsGridModel && this.openPositionsGridModel.OpenModels.length > 0) {
        this.openPositionsGridModelCopy.OpenModels = this.openPositionsGridModel.OpenModels.filter((item) => {
          return ((!this.opTicketNoSearchTerm || item.sTicketNo.toLowerCase().indexOf(this.opTicketNoSearchTerm.toLowerCase()) > -1)
            && (!this.opProductNameSearchTerm || item.sProductDescription.toLocaleLowerCase().indexOf(this.opProductNameSearchTerm.toLowerCase()) > -1))
        });
        this.openPositionsGridModelCopy.Total = this.openPositionsGridModelCopy.OpenModels.length;
      }
    }
    else {
      if (this.tradeHistoryModel && this.tradeHistoryModel.OpenModels.length > 0) {
        this.tradeHistoryModelCopy.OpenModels = this.tradeHistoryModel.OpenModels.filter((item) => {
          return ((!this.thTicketNoSearchTerm || item.sTicketNo.toLowerCase().indexOf(this.thTicketNoSearchTerm.toLowerCase()) > -1)
            && (!this.thProductNameSearchTerm || item.sProductDescription.toLocaleLowerCase().indexOf(this.thProductNameSearchTerm.toLowerCase()) > -1))
        });
        this.tradeHistoryModelCopy.Total = this.tradeHistoryModelCopy.OpenModels.length;
      }
    }
  }

  getHtml(rowData: OpenModel) {
    var html = '';
    if (rowData.sTrackingNumbers.length == 0) {

    }
    else if (rowData.sReceived) {
      html = rowData.sReceived;
    }
    else if (rowData.sShipping != 'Edit' && rowData.sShipping != 'New') {
      html = rowData.sShipping;
    }
    else if (rowData.sShipping == 'New') {
      html = '<span class="shipping-info" (click)="openAddEditInfoModal(updateinfotemplate,rowData)">Add Shipping Info</span>';
    }
    else if (rowData.sShipping == 'Edit') {
      html = '<span class="shipping-info" (click)="openAddEditInfoModal(updateinfotemplate,rowData)">Edit Shipping Info</span>';
    }
    return this.sanitizer.bypassSecurityTrustHtml(html);
    
  }

  getTrackingLinkText(trackingNumber: string) {
    return trackingNumber.substring(trackingNumber.lastIndexOf('=') + 1);
  }

  reset() {
    this.openPositionsTable.reset();
    //this.tradeHisotryTable.first = 0;
  }

  refreshGrid() {
    
    
    if (this.tradingActivityOption == 0) {
      this.resetOpenPositionsSearchFields();
      this.loadingMessage = 'Rfreshing Open Positions.....';
      this.loadOpenPositions();
    }
    else {
      this.resetTradeHistorySearchFields();
      this.loadingMessage = 'Rfreshing Trade History.....';
      this.getTradeHistory();
    }
  }

displayAbs(val) {
    return Math.abs(val);
  }

  ngOnInit() {
  }

}
