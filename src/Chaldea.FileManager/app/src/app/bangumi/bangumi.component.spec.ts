import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BangumiComponent } from './bangumi.component';

describe('BangumiComponent', () => {
  let component: BangumiComponent;
  let fixture: ComponentFixture<BangumiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BangumiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BangumiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
