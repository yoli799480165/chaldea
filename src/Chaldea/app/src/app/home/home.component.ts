import { Component, OnInit } from '@angular/core';
import { LocationStrategy, PlatformLocation, Location } from '@angular/common';
import { LegendItem, ChartType } from '../lbd/lbd-chart/lbd-chart.component';
import * as Chartist from 'chartist';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public emailChartType: ChartType;
  public emailChartData: any;
  public emailChartLegendItems: LegendItem[];

  public hoursChartType: ChartType;
  public hoursChartData: any;
  public hoursChartOptions: any;
  public hoursChartResponsive: any[];
  public hoursChartLegendItems: LegendItem[];

  public activityChartType: ChartType;
  public activityChartData: any;
  public activityChartOptions: any;
  public activityChartResponsive: any[];
  public activityChartLegendItems: LegendItem[];
  constructor() { }

  ngOnInit() {
    this.emailChartType = ChartType.Pie;
    this.emailChartData = {
      labels: ['62%', '32%', '6%'],
      series: [62, 32, 6]
    };
    this.emailChartLegendItems = [
      { title: '18岁以下', imageClass: 'fa fa-circle text-info' },
      { title: '18-30岁', imageClass: 'fa fa-circle text-danger' },
      { title: '30岁以上', imageClass: 'fa fa-circle text-warning' }
    ];

    this.hoursChartType = ChartType.Line;
    this.hoursChartData = {
      labels: ['00:00AM', '03:00AM', '06:00AM', '09:00AM', '12:00AM', '15:00PM', '18:00PM', '23:00PM'],
      series: [
        [287, 385, 490, 492, 554, 586, 698, 695]
      ]
    };
    this.hoursChartOptions = {
      low: 0,
      high: 800,
      showArea: true,
      height: '245px',
      axisX: {
        showGrid: false,
      },
      lineSmooth: Chartist.Interpolation.simple({
        divisor: 3
      }),
      showLine: false,
      showPoint: false,
    };
    this.hoursChartResponsive = [
      ['screen and (max-width: 640px)', {
        axisX: {
          labelInterpolationFnc: function (value) {
            return value[0];
          }
        }
      }]
    ];
    this.hoursChartLegendItems = [
      { title: '流量', imageClass: 'fa fa-circle text-info' }
    ];

    this.activityChartType = ChartType.Bar;
    this.activityChartData = {
      labels: ['Jan', 'Feb', 'Mar', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
      series: [
        [542, 443, 320, 780, 553, 453, 326, 434, 568, 610, 756, 895],
        [412, 243, 280, 580, 453, 353, 300, 364, 368, 410, 636, 695]
      ]
    };
    this.activityChartOptions = {
      seriesBarDistance: 10,
      axisX: {
        showGrid: false
      },
      height: '245px'
    };
    this.activityChartResponsive = [
      ['screen and (max-width: 640px)', {
        seriesBarDistance: 5,
        axisX: {
          labelInterpolationFnc: function (value) {
            return value[0];
          }
        }
      }]
    ];
    this.activityChartLegendItems = [
      { title: 'Tesla Model S', imageClass: 'fa fa-circle text-info' },
      { title: 'BMW 5 Series', imageClass: 'fa fa-circle text-danger' }
    ];


  }

}
