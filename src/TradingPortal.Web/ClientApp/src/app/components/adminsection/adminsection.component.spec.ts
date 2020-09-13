import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsectionComponent } from './adminsection.component';

describe('AdminsectionComponent', () => {
  let component: AdminsectionComponent;
  let fixture: ComponentFixture<AdminsectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminsectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminsectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
