import { Component } from '@angular/core';
import { DxDataGridModule, DxDataGridTypes } from 'devextreme-angular/ui/data-grid';

@Component({
  selector: 'app-device-details',
  standalone: true,
  imports: [DxDataGridModule],
  templateUrl: './device-details.component.html',
  styleUrl: './device-details.component.scss'
})
export class DeviceDetailsComponent {
  dataSource: any[] = [

    {

      "Traffic type": "Check In",

      "Time": "12:00 PM",

      "Date": new Date(2023, 9, 1), // Example date (October 1, 2023)

      "DeviceMacAddress": "00:1A:2B:3C:4D:5E",

      "image": "path/to/image1.png" // Path to the image

    },

    {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    },   {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    },   {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    },   {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    },   {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    },   {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    },


  ];
}
