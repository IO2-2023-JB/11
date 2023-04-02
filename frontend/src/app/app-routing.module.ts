import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistrationComponent } from '../app/pages/registration/registration.component';
import { VideoComponent } from './pages/video/video.component';

const routes: Routes = [
  { path: 'register', component: RegistrationComponent },
  { path: 'videos/:videoId', component: VideoComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
