import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BangumiListComponent } from './bangumi-list.component';

describe('BangumiListComponent', () => {
  let component: BangumiListComponent;
  let fixture: ComponentFixture<BangumiListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BangumiListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BangumiListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
