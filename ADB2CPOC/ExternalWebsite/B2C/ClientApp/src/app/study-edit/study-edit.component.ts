import { StudyService } from './../Volunteer.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Study } from '../Volunteer';

@Component({
  selector: 'app-study-edit',
  templateUrl: './study-edit.component.html',
  styleUrls: ['./study-edit.component.css']
})
export class StudyEditComponent implements OnInit {

  Study: Study = {
    id: 1,
    description: "undefined",
    status: false,
  };

  constructor(private route: ActivatedRoute, private router: Router, private service: StudyService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      let id = +params.get('id')!;
      this.service.getStudy(+id).subscribe((response: Study) => {
        this.Study = response;
      })
    })
  }

  editStudy(Study: Study): void {
    this.Study.description = Study.description;
    this.service.editStudy(this.Study).subscribe(() => {
      this.router.navigate(['/Study-view']);
    })
  }

}
