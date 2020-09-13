import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TradingactivityComponent } from './tradingactivity.component';

describe('TradingactivityComponent', () => {
  let component: TradingactivityComponent;
  let fixture: ComponentFixture<TradingactivityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TradingactivityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TradingactivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
