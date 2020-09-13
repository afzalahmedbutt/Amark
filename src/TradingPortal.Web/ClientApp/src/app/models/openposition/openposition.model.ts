export class OpenModel {
 public iOrder_Hdr_ID: number;
  public iTrade_ID: number;
 public sdtTrade: string;
 public sdtValue: string;
 public sTicketNo: string;
 public sTPConfirmNo: string;
 public sTradeType: string;
 public sProductDescription: string;
 public iQuantity: string;
 public decProductBalance: number;
 public decCashBalance: number;
 public sTrackingNumbers: string[];
 public sReceived: string;
 public sShipping: string;
 public decPrice: number;
 public decTotalAmount: number;
 public iTotalRecords: string;
 public sCurrencySymbol: string; 
}

export class ShippingInfo {
  constructor() {
    this.sShippingPhoneNumber = "";
    this.rqs = '0';
  }
  iRequestID: number;
  iOrderHdrID: number;
  sShippingName1: string;
  sShippingName2: string;
  sShippingAddress1: string;
  sShippingAddress2: string;
  sShippingCity: string;
  sShippingState: string;
  sShippingZipCode: string;
  sShippingCountry: string;
  sShippingPhoneNumber: string = "";
  rqs: string;
}

export class UpdateShippingInfoResult {
  public IsValid: boolean;
  public QValue: string;
}

export class OpenPositionsModel {
  constructor() {
    //this.AvailableCategories = new List<SelectListItem>();
    this.StartDate = new Date(),
    this.EndDate = new Date()
  }
  public GridSize: number;
  public SearchCategoryId: number;
  //public AvailableCategories: IList<SelectListItem>;
  public StartDate: any;
  public EndDate: any;
  public Page: number;
  public PageSize: number;
}

export class GridOpenModel {
  public OpenModels: OpenModel[];
  public Total: number;
}
