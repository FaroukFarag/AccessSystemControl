import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { routes } from './app/app.routes';
import { AppComponent } from './app/app.component';
import { ClientInterceptor } from './app/interceptors/client-interceptor.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    { provide: HTTP_INTERCEPTORS, useClass: ClientInterceptor, multi: true },
    provideHttpClient(withInterceptorsFromDi())
  ]
});
