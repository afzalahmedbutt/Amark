import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestForOrderComponent } from './request-for-order.component';

describe('RequestForOrderComponent', () => {
  let component: RequestForOrderComponent;
  let fixture: ComponentFixture<RequestForOrderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequestForOrderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestForOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
