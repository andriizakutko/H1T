import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysadminPanelComponent } from './sysadmin-panel.component';

describe('SysadminPanelComponent', () => {
  let component: SysadminPanelComponent;
  let fixture: ComponentFixture<SysadminPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SysadminPanelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SysadminPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
