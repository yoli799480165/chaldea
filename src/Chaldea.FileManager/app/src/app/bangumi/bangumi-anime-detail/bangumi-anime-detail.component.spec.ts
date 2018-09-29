import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BangumiAnimeDetailComponent } from './bangumi-anime-detail.component';

describe('BangumiAnimeDetailComponent', () => {
  let component: BangumiAnimeDetailComponent;
  let fixture: ComponentFixture<BangumiAnimeDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BangumiAnimeDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BangumiAnimeDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
