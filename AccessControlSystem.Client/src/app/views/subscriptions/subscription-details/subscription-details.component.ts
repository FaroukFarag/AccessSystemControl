import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-subscription-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './subscription-details.component.html',
  styleUrl: './subscription-details.component.scss'
})
export class SubscriptionDetailsComponent implements OnInit {
  subscription = {
    plan: 'Standard',
    payment: 240,
    startDate: '31-03-2024',
    endDate: '31-03-2026',
    usage: {
      admins: { used: 3, total: 5 },
      devices: { used: 12, total: 20 },
      cards: { used: 2, total: 3 }
    }
  };
  devices = Array.from({ length: 12 }).map((_, i) => ({
    name: `Device ${i + 1}`,
    status: 'Active',
    mac: `50:80:D0:63:XX:${(i + 10).toString(16).toUpperCase()}`,
    type: 'Type name',
    image: '/assets/images/device.png'
  }));

  constructor(
    private route: ActivatedRoute,
    private subscriptionsService: SubscriptionService) {
  }

  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id')!;

    this.subscriptionsService.getById('Subscriptions/Get', id).subscribe(data => {
      console.log(data)
    })
  }

  getProgress(used: number, total: number): number {
    return (used / total) * 100;
  }
}
