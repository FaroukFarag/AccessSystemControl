import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoginService } from '../../services/login/login.service';
import notify from 'devextreme/ui/notify';

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
        if (response.succeeded) {
          localStorage.setItem('authToken', response.resultData.token);
          localStorage.setItem('userRole', response.resultData.roleId);
          localStorage.setItem('subscriptionId', response.resultData.subscriptionId);
          
          this.router.navigate(['/dashboard']);
        }

        else {
          notify('Login failed: Invalid username or password', 'error', 2000);
        }

      } else {
        notify('Login failed: No response', 'error', 2000);
        console.error('Login failed: No response');
      }
    });
  }
}
