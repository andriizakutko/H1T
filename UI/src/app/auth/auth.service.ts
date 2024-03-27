import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Login } from '../models/Login';
import { Response } from '../models/Response';
import { BehaviorSubject } from 'rxjs';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7110/api/user/';
  private defaultUser: User = {
    id: '',
    firstName: '',
    lastName: '',
    email: '',
    country: '',
    city: '',
    address: '',
    isActive: false,
    permissions: []
  }
  tokenKey = 'auth_token';
  isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  userSubject = new BehaviorSubject<User>(this.defaultUser);
  user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient) { }

  login(loginModel: Login) {
    return this.http.post<Response>(this.apiUrl + 'login', loginModel);
  }

  authProcess(response: Response) {
    window.localStorage.setItem(this.tokenKey, (response.data as any).token);
    this.updateIsAuthenticatedStatus(true)
    this.setUser();
  }

  private updateIsAuthenticatedStatus(newValue: boolean) {
    this.isAuthenticatedSubject.next(newValue);
  }

  getToken() {
    return window.localStorage.getItem(this.tokenKey);
  }

  setUser() {
    this.http.get<Response>(this.apiUrl + 'get-user').subscribe(response => {
      if(response.statusCode === 200) {
        this.userSubject.next(response.data as any);
      }
    })
  }

  logout() {
    window.localStorage.removeItem(this.tokenKey);
    this.updateIsAuthenticatedStatus(false);
    this.userSubject.next(this.defaultUser);
  }

  checkAuth() {
    if(window.localStorage.getItem(this.tokenKey) !== null) {
      this.isAuthenticatedSubject.next(true);
      this.setUser();
    }
  }

  isLoggedIn() {
    let result: boolean = false;
    this.isAuthenticated$.subscribe(data => {
      result = data;
    })
    return result;
  }

  hasPermission(requiredPermissions: string[]) {
    console.log(requiredPermissions);
    let result: boolean = false;
    this.user$.subscribe(data => {
      if(requiredPermissions.some(permission => data.permissions.includes(permission))) {
        result = true;
      }
    });
    return result;
  }
}
