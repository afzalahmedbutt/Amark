
export class RequestForOrderModel {
  constructor() {
    this.Items = [];
    //this.Warnings = new Array<string>();
  }
  public Items: Array<RequestForOrderItemModel>; //IList<RequestForOrderItemModel>;
  public ModifiedItems : Array<RequestForOrderItemModel>;
  public Warnings: Array<string>;
  public MinOrderSubtotalWarning: string;
  public IsSellMode: boolean;
  public IsEditable: boolean;
  public IsHFI: boolean;
  public IsDropShip: boolean;
  public SpecialInstructions: string;
  public TPConfirmation: string;
  public PriceExpireInterval: number;
  public IsDiscountPricing: boolean;
  public SmallOrderCharge: string;
  public QuoteKey: string;
  public MultipleTransactionGroupsPresent: boolean;
  public sTicketNumber: string;
  public IsHedgedOrder: boolean;
  public Name1: string;
  public Name2: string;
  public Address1: string;
  public Address2: string;
  public City: string;
  public State: string;
  public Zip: string;
  public Country: string;

  public DecGoldTotal: number;
  public DecGoldTotalQuantity: number;
  public DecSilverTotal: number;
  public DecSilverTotalQuantity: number;
  public DecPlatinumTotal: number;
  public DecPlatinumTotalQuantity: number;
  public DecPalladiumTotal: number;
  public DecPalladiumTotalQuantity: number;

  public DecTotal: number;
  public DecTotalQuantity: number;
  public IsSell: boolean;
  public IsRequote: boolean;
}

export class RequestForOrderItemModel {
  constructor() {
    //this.Warnings = new List<string>();
  }
  public Commodity: string;
  public Sku: string;
  public ProductId: number;
  public ProductName: string;
  public ProductDesc: string;
  public ProductWeight: number;
  public WeightSubTotal: number;
  public SpotPrice: number;
  public PremiumIsPercent: boolean;
  public ProductPremium: number;
  public MinPurchase: number;
  public PurchaseIncrement: number;
  public UnitPrice: number;
  public Quantity: any;
  public SubTotal: number;
  public TierPrices: string;
  public Warnings: Array<string>;
  public ErrorMsg: string;
  public IsEmpty: boolean;
  public IsFavorite: boolean;
  public IsNew: boolean;
  public IsDisabled: boolean;
}

export class CustomerFavoriteProduct {
  public CustomerId: number;
  public ProductName: string;
  public IsFavorite: boolean;
}

export class ConfirmOrderResponse {
  public IsSuccess: number;
  public Warnings: string[];
}
