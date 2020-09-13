import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StorecloseComponent } from './storeclose.component';

describe('StorecloseComponent', () => {
  let component: StorecloseComponent;
  let fixture: ComponentFixture<StorecloseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StorecloseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StorecloseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
