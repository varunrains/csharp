import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Volunteer, Study } from './Volunteer';

import { protectedResources } from './auth-config';

@Injectable({
  providedIn: 'root'
})
export class VolunteerService {
  url = protectedResources.VolunteerListApi.endpoint;

  constructor(private http: HttpClient) { }

  getVolunteers() {
    return this.http.get<Volunteer[]>(this.url);
  }

  getVolunteer(id: number) {
    return this.http.get<Volunteer>(this.url + '/' + id);
  }

  postVolunteer(Volunteer: Volunteer) {
    return this.http.post<Volunteer>(this.url, Volunteer);
  }

  deleteVolunteer(id: number) {
    return this.http.delete(this.url + '/' + id);
  }

  editVolunteer(Volunteer: Volunteer) {
    return this.http.put<Volunteer>(this.url + '/' + Volunteer.id, Volunteer);
  }
}

@Injectable({
  providedIn: 'root'
})
export class StudyService {
  url = protectedResources.StudyListApi.endpoint;

  constructor(private http: HttpClient) { }

  getStudys() {
    return this.http.get<Study[]>(this.url);
  }

  getStudy(id: number) {
    return this.http.get<Study>(this.url + '/' + id);
  }

  postStudy(Study: Study) {
    return this.http.post<Study>(this.url, Study);
  }

  deleteStudy(id: number) {
    return this.http.delete(this.url + '/' + id);
  }

  editStudy(Study: Study) {
    return this.http.put<Study>(this.url + '/' + Study.id, Study);
  }
}
