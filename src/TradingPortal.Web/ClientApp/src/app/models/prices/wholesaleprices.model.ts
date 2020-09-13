
export class WholeSaleResponseViewModel {
  public ComCode: string;
  public WholesalePrices: WholeSalePriceViewModel[];
}

export class WholeSalePriceViewModel {
  public BrochureProductId: string;
  public ProductDescription: string;
  public WholesaleBidPrice: number;
  public WholesaleAskPrice: number;
  public UpdateDate: any;
}
