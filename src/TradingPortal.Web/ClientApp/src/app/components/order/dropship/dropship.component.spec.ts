import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DropshipComponent } from './dropship.component';

describe('DropshipComponent', () => {
  let component: DropshipComponent;
  let fixture: ComponentFixture<DropshipComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DropshipComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DropshipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
