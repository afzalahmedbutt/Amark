import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpotpriceComponent } from './spotprice.component';

describe('SpotpriceComponent', () => {
  let component: SpotpriceComponent;
  let fixture: ComponentFixture<SpotpriceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpotpriceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpotpriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
