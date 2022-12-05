import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { HomeComponent } from './home/home.component';
import { VolunteerViewComponent } from './Volunteer-view/Volunteer-view.component';
import { VolunteerEditComponent } from './Volunteer-edit/Volunteer-edit.component';

import { StudyViewComponent } from './study-view/study-view.component';
import { StudyEditComponent } from './study-edit/study-edit.component';

/**
 * MSAL Angular can protect routes in your application
 * using MsalGuard. For more info, visit:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-angular/docs/v2-docs/initialization.md#secure-the-routes-in-your-application
 */
const routes: Routes = [
  {
    path: 'Volunteer-edit/:id',
    component: VolunteerEditComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'Volunteer-view',
    component: VolunteerViewComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'Study-edit/:id',
    component: StudyEditComponent,
  },
  {
    path: 'Study-view',
    component: StudyViewComponent,
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    // Needed for hash routing
    path: 'error',
    component: HomeComponent
  },
  {
    // Needed for hash routing
    path: 'state',
    component: HomeComponent
  },
  {
    // Needed for hash routing
    path: 'code',
    component: HomeComponent
  },
  {
    path: '',
    component: HomeComponent
  }
];

const isIframe = window !== window.parent && !window.opener;

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    useHash: true,
    // Don't perform initial navigation in iframes
    initialNavigation: !isIframe ? 'enabled' : 'disabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
