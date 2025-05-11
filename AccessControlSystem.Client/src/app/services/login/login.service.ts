import { HttpClient, HttpEvent, HttpRequest } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';



@Injectable({
  providedIn: 'root'
})
export class LoginService {
  protected baseUrl: string;
  protected http = inject(HttpClient);
  constructor() {
    this.baseUrl = `${environment.apiUrl}`;
  }
  login(credentials: { userName: string; password: string }): Observable<any> {
    return this.http.post(`${this.baseUrl}/Users/Login`, credentials)
      .pipe(
        catchError(err => {
          console.error('Login error:', err);
          return of(null); 
        })
      );
  }

 
}
