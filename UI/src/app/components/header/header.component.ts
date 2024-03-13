import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth/auth.service';
import { LoginDialogComponent } from '../dialogs/login-dialog/login-dialog.component';

@Component({
  selector: 'header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  constructor(public authService: AuthService, public dialog: MatDialog) {}

  openLoginDialog() {
    this.dialog.open(LoginDialogComponent);
  }
}