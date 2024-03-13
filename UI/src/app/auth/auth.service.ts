import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginDto } from '../models/login';
import { BehaviorSubject, Observable, catchError, map, of } from 'rxjs';
import { User } from '../models/user';
import { ApiResponse } from '../models/response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7110/api/';
  private tokenKey = 'auth_token';
  private userSubject = new BehaviorSubject<User | null>(null);
  public $user = this.userSubject.asObservable();

  constructor(private http: HttpClient) { }

  login(LoginDto: LoginDto): Observable<any> {
    return this.http.post(this.apiUrl + 'users/login', LoginDto);
  }

  setToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return this.$user !== null;
  }

  getHeaders(): HttpHeaders { 
    const token = this.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  setUser() {
    const headers = this.getHeaders();
    this.http.get<ApiResponse<User>>(this.apiUrl + 'users/get-user', { headers }).pipe(
      map(response => {
        if (response.statusCode === 200) {
          this.userSubject.next(response.data);
        } else {
          throw new Error('Unable to fetch user data');
        }
      }),
      catchError(error => {
        console.error('Error fetching user data:', error);
        throw error;
      })
    );
  }
}
