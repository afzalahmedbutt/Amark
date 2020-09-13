import { Injectable } from '@angular/core'

@Injectable()
export class RangeContantsService {
  private moneyMax = 922337203685477.5807;
  private floatMax = 3.4e38;
  private twoDecimalPlacesMax = 9999999999999998.99;
  private fourDecimalPlacesMax = 99999999999998.9999;
  private bigIntMax = 9223372036854775807;
  private intMax = 2147483647;

  getMoneyMax() {
    return this.moneyMax;
  }

  getFloatMax() {
    return this.floatMax;
  }

  getTwoDecimalPlacesMax() {
    return this.twoDecimalPlacesMax;
  }

  getFourDecimalPlacesMax() {
    this.fourDecimalPlacesMax;
  }

  getBigIntMax() {
    this.bigIntMax;
  }

  getIntMax() {
    return this.intMax;
  }

}
