import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAdvertisementDialogComponent } from './create-advertisement-dialog.component';

describe('CreateAdvertisementDialogComponent', () => {
  let component: CreateAdvertisementDialogComponent;
  let fixture: ComponentFixture<CreateAdvertisementDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateAdvertisementDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateAdvertisementDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
