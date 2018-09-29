import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BangumiAnimeComponent } from './bangumi-anime.component';

describe('BangumiAnimeComponent', () => {
  let component: BangumiAnimeComponent;
  let fixture: ComponentFixture<BangumiAnimeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BangumiAnimeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BangumiAnimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
