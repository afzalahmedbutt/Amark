import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderfooterComponent } from './orderfooter.component';

describe('OrderfooterComponent', () => {
  let component: OrderfooterComponent;
  let fixture: ComponentFixture<OrderfooterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderfooterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderfooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
