import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { VolunteerService } from './../Volunteer.service';
import { Volunteer } from '../Volunteer';

@Component({
  selector: 'app-Volunteer-view',
  templateUrl: './Volunteer-view.component.html',
  styleUrls: ['./Volunteer-view.component.css']
})
export class VolunteerViewComponent implements OnInit {
  
  Volunteer?: Volunteer;

  Volunteers: Volunteer[] = [];

  displayedColumns = ['status', 'description', 'edit', 'remove'];

  constructor(private service: VolunteerService) { }

  ngOnInit(): void {
    this.getVolunteers();
  }

  getVolunteers(): void {
    this.service.getVolunteers()
      .subscribe((Volunteers: Volunteer[]) => {
        this.Volunteers = Volunteers;
      });
  }

  addVolunteer(add: NgForm): void {
    this.service.postVolunteer(add.value).subscribe(() => {
      this.getVolunteers();
    })
    add.resetForm();
  }

  checkVolunteer(Volunteer: Volunteer): void {
    this.service.editVolunteer(Volunteer).subscribe();
  }

  removeVolunteer(id: string): void {
    this.service.deleteVolunteer(+id).subscribe(() => {
      this.getVolunteers();
    })
  }

}
