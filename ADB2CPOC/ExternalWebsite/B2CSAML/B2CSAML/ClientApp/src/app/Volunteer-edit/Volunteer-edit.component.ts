import { VolunteerService } from './../Volunteer.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Volunteer } from '../Volunteer';

@Component({
  selector: 'app-Volunteer-edit',
  templateUrl: './Volunteer-edit.component.html',
  styleUrls: ['./Volunteer-edit.component.css']
})
export class VolunteerEditComponent implements OnInit {

  Volunteer: Volunteer = {
    id: 1,
    description: "undefined",
    status: false,
  };

  constructor(private route: ActivatedRoute, private router: Router, private service: VolunteerService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      let id = +params.get('id')!;
      this.service.getVolunteer(+id).subscribe((response: Volunteer) => {
        this.Volunteer = response;
      })
    })
  }

  editVolunteer(Volunteer: Volunteer): void {
    this.Volunteer.description = Volunteer.description;
    this.service.editVolunteer(this.Volunteer).subscribe(() => {
      this.router.navigate(['/Volunteer-view']);
    })
  }

}
