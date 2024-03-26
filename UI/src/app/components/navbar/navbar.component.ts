import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateAdvertisementDialogComponent } from 'src/app/dialogs/create-advertisement-dialog/create-advertisement-dialog.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  isAuthenticated = true;

  constructor(public dialog: MatDialog) {}

  openCreateAdvertisementDialog() {
    this.dialog.open(CreateAdvertisementDialogComponent, {
      disableClose: true
    });
  }
}
