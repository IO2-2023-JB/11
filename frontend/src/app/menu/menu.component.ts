import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit, OnDestroy {
  isUserAuthenticated!: boolean;
  isUserBankEmployee!: boolean;
  subscriptions: Subscription[] = [];

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  homeButtonOnClick(): void {
    this.router.navigate(['']);
  }

  registerButtonOnClick(): void {
    this.router.navigate(['register']);
  }

  loginButtonOnClick(): void {
    this.router.navigate(['login']);
  }

  logoutButtonOnClick(): void {
    localStorage.removeItem('token');
    this.router.navigate(['login']);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => {
      subscription.unsubscribe();
    });
  }
}