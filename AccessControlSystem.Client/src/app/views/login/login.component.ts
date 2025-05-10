import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http'; 
import { Router } from '@angular/router';
import { LoginService } from '../../services/login/login.service';

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

  constructor(private http: HttpClient, private router: Router, private loginService: LoginService,) { } 
  onSubmit() {
    const loginData = {
      userName: this.email,
      password: this.password,
    };

    this.loginService.login(loginData).subscribe(response => {
      if (response) {
        localStorage.setItem('authToken', response.token);
        localStorage.setItem('userRole', response.role);
        this.router.navigate(['/dashboard']);
      } else {
        console.error('Login failed: No response');
      }
    });
  }
}
