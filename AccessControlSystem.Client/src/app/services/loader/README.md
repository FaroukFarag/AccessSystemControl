# Global Loader Service

This service provides a global loading indicator that can be used throughout the application to show loading states during navigation and data operations.

## Features

- **Global Loading State**: Shows a loading overlay across the entire application
- **Navigation Interceptor**: Automatically shows loader during route changes
- **Manual Control**: Can be manually controlled for custom loading scenarios
- **Localized Text**: Supports multiple languages
- **High Z-Index**: Appears above all other content

## Usage

### Automatic Navigation Loading

The loader automatically shows during route changes. No additional code needed.

### Manual Loading Control

```typescript
import { LoaderService } from '../services/loader/loader.service';

export class YourComponent {
  constructor(private loaderService: LoaderService) {}

  // Show loader
  showLoader() {
    this.loaderService.show();
  }

  // Hide loader
  hideLoader() {
    this.loaderService.hide();
  }

  // Show loader for specific duration
  showLoaderForDuration(duration: number = 1000) {
    this.loaderService.showForDuration(duration);
  }

  // Example: Show loader during API call
  async loadData() {
    this.loaderService.show();
    try {
      await this.apiService.getData();
    } finally {
      this.loaderService.hide();
    }
  }
}
```

### Example: Subscription Details Component

```typescript
ngOnInit(): void {
  this.loaderService.show(); // Show loader when component initializes
  
  this.subscriptionsService.getAll('Subscriptions/GetAll').subscribe({
    next: (data: any) => {
      // Process data
    },
    error: (error) => {
      // Handle error
    },
    complete: () => {
      this.loaderService.hide(); // Hide loader when done
    }
  });
}
```

## Components

### GlobalLoaderComponent

The global loader component is automatically included in the main app component and will show/hide based on the LoaderService state.

### NavigationInterceptor

Automatically manages loading state during route navigation:
- Shows loader on `NavigationStart`
- Hides loader on `NavigationEnd`, `NavigationCancel`, or `NavigationError`

## Styling

The global loader uses:
- Fixed positioning with full viewport coverage
- Semi-transparent background with blur effect
- Centered spinner with localized text
- High z-index (99999) to appear above all content
- Smooth animations

## Localization

The loader text is localized using the `common.loading` translation key:
- English: "Loading..."
- Arabic: "جاري التحميل..."
