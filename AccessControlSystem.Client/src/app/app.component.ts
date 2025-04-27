import { Component, ViewChild } from '@angular/core';
import { AppLayoutComponent } from './views/app-layout/app-layout.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    AppLayoutComponent
  ],
  
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'WS';
}
