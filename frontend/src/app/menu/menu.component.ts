import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserService } from '../core/services/user.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit, OnDestroy {
  isUserAuthenticated!: boolean;
  isUserBankEmployee!: boolean;
  subscriptions: Subscription[] = [];
  query: string = '';

  constructor(private router: Router, private userService: UserService) {
    this.isUserAuthenticated = this.userService.isUserAuthenticated();
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.userService.authenticationStateChanged.subscribe(isAuthenticated => {
        this.isUserAuthenticated = isAuthenticated;
    }));
  }

  homeButtonOnClick(): void {
    this.router.navigate(['']);
  }

  searchButtonOnClick(): void {
    this.router.navigate(['search'], {state: {query: this.query}});
  }

  playlistsButtonOnClick(): void {
    this.router.navigate(['playlists']);
  }

  subscriptionsVideosButtonOnClick(): void {
    this.router.navigate(['subscriptions-videos']);
  }

  registerButtonOnClick(): void {
    this.router.navigate(['register']);
  }

  loginButtonOnClick(): void {
    this.router.navigate(['login']);
  }

  accountButtonOnClick(): void {
    this.router.navigate(['user']);
  }

  logoutButtonOnClick(): void {
    this.userService.logout();
    this.router.navigate(['login']);
  }

  isQueryEmpty() {
    return (this.query.length === 0);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => {
      subscription.unsubscribe();
    });
  }
}
