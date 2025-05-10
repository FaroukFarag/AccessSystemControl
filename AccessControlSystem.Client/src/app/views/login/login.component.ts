import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http'; 

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule], 
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  rememberMe: boolean = false;

  constructor(private http: HttpClient) { } 

  onSubmit() {
    const loginData = {
      userName: this.email,  
      password: this.password,
    };

    console.log('Sending login data:', loginData);

    this.http.post('http://localhost:5273/api/Users/Login', loginData)
      .subscribe({
        next: (response) => {
          console.log('Login successful:', response);
          // You can redirect to dashboard, store token, etc. here
        },
        error: (error) => {
          console.error('Login failed:', error);
        }
      });
  }
}
