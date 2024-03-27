import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    if (this.authService.isLoggedIn() && this.authService.hasPermission((next.data as any).requiredPermissions)) {
      return true;
    }
    else if (this.authService.isLoggedIn() && !this.authService.hasPermission((next.data as any).requiredPermissions)) {
      this.router.navigate(['']);
      this.toastr.error("You don't have permission to view this content");
      return false;
    } 
    else {
      this.router.navigate(['']);
      this.toastr.error("You are not authorized to view this content");
      return false;
    }
  }
}
