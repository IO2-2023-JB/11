import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, forkJoin, tap } from 'rxjs';
import { getTimeAgo } from 'src/app/core/functions/get-time-ago';
import { UserDTO } from 'src/app/core/models/user-dto';
import { VideoMetadataDto } from 'src/app/core/models/video-metadata-dto';
import { UserService } from 'src/app/core/services/user.service';
import { VideoService } from 'src/app/core/services/video.service';
import { SubscriptionService } from 'src/app/core/services/subscription.service';
import { userSubscriptionListDto } from 'src/app/core/models/user-subscription-list-dto';
import { PlaylistService } from 'src/app/core/services/playlist.service';
import { UserPlaylistsDto } from 'src/app/core/models/user-playlists-dto';
import { getUserId } from 'src/app/core/functions/get-user-id';
import { getRole } from 'src/app/core/functions/get-role';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-creator',
  templateUrl: './creator.component.html',
  styleUrls: ['./creator.component.scss']
})
export class CreatorComponent implements OnInit, OnDestroy {
  id!: string;
  user!: UserDTO;
  videos!: VideoMetadataDto[];
  isCreatorSubscribed!: boolean;
  subscriptions: Subscription[] = [];
  playlists!: UserPlaylistsDto[];
  userId: string;
  isOwnProfile!: boolean;
  role!: string;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router,
    private videoService: VideoService,
    private subscriptionService: SubscriptionService,
    private playlistService: PlaylistService,
    private messageService: MessageService,
    ) {
    this.id = this.route.snapshot.paramMap.get('id')!;
    this.userId = getUserId();
    this.role = getRole();
  }

  ngOnInit(): void {
    const getUser$ = this.userService.getUser(this.id);
    const getVideos$ = this.videoService.getUserVideos(this.id);
    const getSubList$ = this.subscriptionService.getSubscriptions(this.userId);
    const getPlaylists$ = this.playlistService.getUserPlaylists(this.id);

    this.subscriptions.push(
      forkJoin([getUser$, getVideos$, getSubList$, getPlaylists$])
      .subscribe(([user, videosList, subscriptionList, playlists]) => {
      this.user = user;
      this.videos = videosList.videos;
      this.isCreatorSubscribed = this.isThisCreatorSubscribed(subscriptionList);
      this.playlists = playlists;
      this.isOwnProfile = (user.id == this.userId);
    }));
  }

  private checkIfCreatorIsSubscribed(): void {
    this.subscriptions.push(this.subscriptionService.getSubscriptions(this.userId).subscribe(subscriptions =>
        this.isCreatorSubscribed = this.isThisCreatorSubscribed(subscriptions)
      ));
  }

  private isThisCreatorSubscribed(subscriptionList: userSubscriptionListDto): boolean {
    for (let subscription of subscriptionList.subscriptions) {
      if (subscription.id === this.user.id) {
        return true;
      }
    }
    return false;
  }

  private updateSubCount(): void {
    this.subscriptions.push(this.userService.getUser(this.user.id).subscribe(user => {
      this.user.subscriptionsCount = user.subscriptionsCount;
    }));
  }

  public handleSubscribtionOnClick(): void {
    if (this.isCreatorSubscribed) {
      const checkIfCreatorIsSubscribed$ = this.subscriptionService.deleteSubscription(this.user.id).pipe(
        tap(() => {
          this.checkIfCreatorIsSubscribed();
        }),
      );

      this.subscriptions.push(checkIfCreatorIsSubscribed$.subscribe());
    } else {
      const checkIfCreatorIsSubscribed$ = this.subscriptionService.postSubscription(this.user.id).pipe(
        tap(() => {
          this.checkIfCreatorIsSubscribed();
        }),
      );

      this.subscriptions.push(checkIfCreatorIsSubscribed$.subscribe());
    }

    this.updateSubCount();
  }

  public handleDeleteOnClick(): void {
    const delete$ = this.userService.deleteUser(this.id).pipe(
      tap(() => {
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'The user has been deleted'
        })
      })
    );
    this.subscriptions.push(delete$.subscribe({
       complete: () => {
         this.router.navigate(['']);
       }
    }));
  }

  public goToVideo(id: string): void {
    this.router.navigate(['videos/' + id]);
  }

  public goToUserProfile(id: string): void {
    this.router.navigate(['creator/' + id]);
  }

  public goToPlaylist(id: string): void {
    this.router.navigate(['playlist', id]);
  }

  public getTimeAgo(video: VideoMetadataDto): string {
    return getTimeAgo(video.uploadDate);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => {
      subscription.unsubscribe();
    });
  }
}
