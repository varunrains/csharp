import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { StudyService } from './../Volunteer.service';
import { Study } from '../Volunteer';

@Component({
  selector: 'app-study-view',
  templateUrl: './study-view.component.html',
  styleUrls: ['./study-view.component.css']
})
export class StudyViewComponent implements OnInit {

  Study?: Study;

  Studys: Study[] = [];

  displayedColumns = ['status', 'description', 'subscribe'];

  constructor(private service: StudyService) { }

  ngOnInit(): void {
    this.getStudys();
  }

  getStudys(): void {
    this.service.getStudys()
      .subscribe((Studys: Study[]) => {
        this.Studys = Studys;
      });
  }

  addStudy(add: NgForm): void {
    this.service.postStudy(add.value).subscribe(() => {
      this.getStudys();
    })
    add.resetForm();
  }

  checkStudy(Study: Study): void {
    this.service.editStudy(Study).subscribe();
  }

  removeStudy(id: string): void {
    this.service.deleteStudy(+id).subscribe(() => {
      this.getStudys();
    })
  }

}
