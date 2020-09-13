import { Pipe, PipeTransform } from '@angular/core';
import {DecimalPipe} from '@angular/common'

@Pipe({
  name: 'decimalCurrency'
})

export class DecimalCurrencyPipe extends DecimalPipe {
  transform(value: any, digitsInfo?: string, locale?: string): string {
    return '$'+super.transform(value, digitsInfo);
  }

  numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
  }
}



//export class DecimalCurrencyPipe extends DecimalPipe {
//  transform(value: any, digitsInfo?: string, locale?: string): string {
//    var returnVal = "";
//    var floatVal = parseFloat(value.toString());
//    if (decimalPoints) {
//      returnVal = floatVal.toFixed(decimalPoints);
//    }
//    return '$' + this.numberWithCommas(returnVal);
//  }

//  numberWithCommas(x){
//    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//  }
//}
