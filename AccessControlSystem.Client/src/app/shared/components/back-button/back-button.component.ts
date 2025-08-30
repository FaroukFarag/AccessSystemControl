import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-back-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    <svg 
      (click)="onBackClick()"
      width="10" 
      height="15" 
      viewBox="0 0 10 15" 
      fill="none" 
      xmlns="http://www.w3.org/2000/svg" 
      [class.rtl-icon]="direction === 'rtl'" 
      [style.margin-left.px]="marginLeft"
      [style.margin-right.px]="marginRight"
      [style.cursor]="'pointer'"
      tabindex="0" 
      role="button"
      (keydown.enter)="onBackClick()"
      (keydown.space)="onBackClick()">
      <path d="M8.5 1L1.5 7.5L8.5 14" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
    </svg>
  `,
  styles: [`
    svg {
      transition: opacity 0.2s ease;
    }
    
    svg:hover {
      opacity: 0.7;
    }
    
    svg:focus {
      outline: 2px solid #007bff;
      outline-offset: 2px;
      border-radius: 2px;
    }
    
    .rtl-icon {
      transform: scaleX(-1);
    }
  `]
})
export class BackButtonComponent {
  @Input() direction: 'ltr' | 'rtl' = 'ltr';
  @Input() marginLeft: number = 14;
  @Input() marginRight: number = 0;
  @Output() backClick = new EventEmitter<void>();

  onBackClick(): void {
    this.backClick.emit();
  }
}
