export class WebSpotPrices {
  public ID: number;
  public VsMarket: string;
  public ComCode: string;
  public Bid: number;
  public Ask: number;
  public BidAskDate: Date;
  public VsClose: number;
  public VsChange: number;
  public NyTradeId: number;
  public NyTradeSymbol: string;
  public NyTradeDate: Date;
  public Update_Date: Date;

  public ChangeVal: string;
  public ChangePer: string;
  public RoundedAsk: string;
  public RoundedBid: string;
  public RoundedClose: string;
  public MetalName: string;
  public Digits: number;
}

export class UpdateSpotsViewModel {
  public Spots: WebSpotPrices[];
  public IsAfterHours: string;
  public IsClosed: string;
  public MarketText: string;
  public AmarkText: string;
  public VsWhich: string;
  public ServerTime: any;
}

export class SpotPricePreviewViewModel {
  constructor() {
    this.VsWhich = "NY";
  }
  public Spots: WebSpotPrices[];
  public IsAfterHours: boolean;
  public VsWhich: string;
  public ServerTime: any;
}
