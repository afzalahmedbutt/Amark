import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KillswitchComponent } from './killswitch.component';

describe('KillswitchComponent', () => {
  let component: KillswitchComponent;
  let fixture: ComponentFixture<KillswitchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KillswitchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KillswitchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
