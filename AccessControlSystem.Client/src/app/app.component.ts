import { Component, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppLayoutComponent } from './views/app-layout/app-layout.component';
import { HeaderComponent } from './views/app-layout/header/header.component';
import { ContentComponent } from './views/app-layout/content/content.component';
import { FooterComponent } from './views/app-layout/footer/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet, 
    AppLayoutComponent,
    HeaderComponent,
    ContentComponent,
    FooterComponent,
  ],
  
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {

  title = 'WS';
}
