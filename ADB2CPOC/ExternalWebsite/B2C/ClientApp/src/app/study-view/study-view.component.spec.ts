import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudyViewComponent } from './study-view.component';

describe('StudyViewComponent', () => {
  let component: StudyViewComponent;
  let fixture: ComponentFixture<StudyViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudyViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StudyViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
