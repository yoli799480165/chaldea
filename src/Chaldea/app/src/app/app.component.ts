import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { LoadingService } from './shared/loading-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  loading = false;

  constructor(
    public location: Location,
    private loadingService: LoadingService
  ) {
    this.loadingService.subject.subscribe((rep: boolean) => {
      this.loading = rep;
    });
  }

  ngOnInit() {
  }
}
