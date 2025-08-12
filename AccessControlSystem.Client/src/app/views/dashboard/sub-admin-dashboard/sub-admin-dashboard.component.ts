import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { SidebarService } from '../../../services/sidebar/sidebar.service';
import { LanguageService } from '../../../services/language/language.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { Router } from '@angular/router';
import { UserService } from '../../../services/users/user.service';
import notify from 'devextreme/ui/notify';
import { DomSanitizer } from '@angular/platform-browser';
import {
  DxPopupModule,
  DxButtonModule,
  DxTemplateModule,
  DxToolbarModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxDateBoxModule,
  DxFormModule,
  DxFileUploaderModule,
} from 'devextreme-angular';
@Component({
  selector: 'sub-admin-dashboard',
  standalone: true,
  imports: [CommonModule,
    TranslatePipe,
    DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxDateBoxModule,
    DxFormModule,
    DxFileUploaderModule,],
  templateUrl: './sub-admin-dashboard.component.html',
  styleUrl: './sub-admin-dashboard.component.scss'
})
export class SubAdminDashboardComponent {
  popupVisible: boolean = false;
}
