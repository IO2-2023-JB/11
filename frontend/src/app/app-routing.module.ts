import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistrationComponent } from '../app/pages/registration/registration.component';
import { CreatorComponent } from './pages/creator/creator.component';
import { LoginComponent } from '../app/pages/login/login.component';

const routes: Routes = [
  { path: 'register', component: RegistrationComponent },
  { path: 'creator/:id', component: CreatorComponent },
  { path: 'login', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
