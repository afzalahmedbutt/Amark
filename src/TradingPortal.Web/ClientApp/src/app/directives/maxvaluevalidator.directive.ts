
import { Directive, Input, forwardRef, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { NG_VALIDATORS, Validator, Validators, ValidatorFn, AbstractControl } from '@angular/forms';



const MAX_VALIDATOR: any = {
  provide: NG_VALIDATORS,
  useExisting: forwardRef(() => MaxValueValidator),
  multi: true
};

@Directive({
  //selector: '[max][formControlName],[max][formControl],[max][ngModel]',
  selector: '[max]',
  providers: [MAX_VALIDATOR]
})
export class MaxValueValidator implements Validator, OnInit, OnChanges {
  @Input() max: number;

  private validator: ValidatorFn;
  private onChange: () => void;

  ngOnInit() {
    this.validator = this.maxValidation(this.max);
  }

  ngOnChanges(changes: SimpleChanges) {
    for (let key in changes) {
      if (key === 'max') {
        this.validator = this.maxValidation(changes[key].currentValue);
        if (this.onChange) this.onChange();
      }
    }
  }

  validate(c: AbstractControl): { [key: string]: any } {

    return this.validator(c);
  }

  registerOnValidatorChange(fn: () => void): void {
    this.onChange = fn;
  }

  maxValidation = (max: number): ValidatorFn => {
    return (control: AbstractControl): { [key: string]: any } => {

      if (!this.isPresent(max)) return null;
      if (this.isPresent(Validators.required(control))) return null;

      let v: number = +control.value;
      return v <= +max ? null : { actualValue: v, requiredValue: +max, max: true };
    };
  };

  isPresent(obj: any): boolean {
    return obj !== undefined && obj !== null;
  }
}
