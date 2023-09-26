import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'numberToCurrency'
})
export class NumberToCurrencyPipe implements PipeTransform {

  transform(entryNumber: number, currency: string ="COP"): string {
    let entryString = String(entryNumber);
    let output="";
    if (currency==="COP") {
      let dot=3;
      for(var i=entryString.length-1; i>=0;i--){
        if(dot>1){
          output=entryString.substring(i,i+1)+output;
          dot--;
        }
        else {
          output=entryString.substring(i,i+1)+output;
          if(i>0) output="."+ output;
          dot--;
          dot=3;
        }
      }
      output = "$"+output+" COP";
    }
    return output
  }
}
