import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { CreateAdvertisementDialogComponent } from 'src/app/dialogs/create-advertisement-dialog/create-advertisement-dialog.component';
import { LoginDialogComponent } from 'src/app/dialogs/login-dialog/login-dialog.component';
import { RegisterDialogComponent } from 'src/app/dialogs/register-dialog/register-dialog.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(public dialog: MatDialog, public auth: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.auth.checkAuth();
  }

  openLoginDialog() {
    this.dialog.open(LoginDialogComponent, {
      disableClose: true
    });
  }

  openRegisterDialog() {
    this.dialog.open(RegisterDialogComponent, {
      disableClose: true
    });
  }

  logout() {
    this.auth.logout();
  }

  navigateToDashboard() {
    this.router.navigate(['']);
  }

  navigateToSysAdminPanel() {
    this.router.navigate(['sysadmin']);
  }

  navigateToAdminPanel() {
    this.router.navigate(['admin']);
  }

  navigateToModeratorPanel() {
    this.router.navigate(['moderator']);
  }

  navigateToProfilePanel() {
    this.router.navigate(['profile']);
  }

  navigateToAdvertisementsPanel() {
    this.router.navigate(['advertisements']);
  }
}
