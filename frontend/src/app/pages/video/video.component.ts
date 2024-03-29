import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { getToken } from 'src/app/core/functions/get-token';
import { UserDTO } from 'src/app/core/models/user-dto';
import { VideoMetadataDto } from 'src/app/core/models/video-metadata-dto';
import { UserService } from 'src/app/core/services/user.service';
import { VideoService } from 'src/app/core/services/video.service';
import { SubscriptionService } from 'src/app/core/services/subscription.service';
import { switchMap, tap } from 'rxjs/operators';
import { Observable, of, Subscription, finalize, forkJoin } from 'rxjs';
import { MenuItem, MessageService } from 'primeng/api';
import { Location } from '@angular/common';
import { ReactionsDTO } from './models/reactions-dto';
import { ReactionsService } from './services/reactions.service';
import { AddReactionDTO } from './models/add-reaction-dto';
import { userSubscriptionListDto } from 'src/app/core/models/user-subscription-list-dto';
import { SubmitTicketDto } from 'src/app/core/models/tickets/submit-ticket-dto';
import { TicketService } from 'src/app/core/services/ticket.service';
import { DonationService } from 'src/app/core/services/donation.service';
import { UserPlaylistsDto } from 'src/app/core/models/user-playlists-dto';
import { PlaylistService } from 'src/app/core/services/playlist.service';
import { getApiUrl } from 'src/app/core/functions/get-api-url';
import { getTimeAgo } from 'src/app/core/functions/get-time-ago';
import { getUserId } from 'src/app/core/functions/get-user-id';
import { getRole } from 'src/app/core/functions/get-role';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.scss']
})
export class VideoComponent implements OnInit, OnDestroy {
  videoId: string;
  videoMetadata!: VideoMetadataDto;
  author!: UserDTO;
  isAuthorSubscribed!: boolean;
  videos: VideoMetadataDto[] = [];
  reactions!: ReactionsDTO;
  subscriptions: Subscription[] = [];
  positiveReaction = 'Positive';
  negativeReaction = 'Negative';
  noneReaction = 'None';
  videoMenuModel: MenuItem[] = [
    {
      label: 'Add to playlist',
      icon: 'pi pi-list',
      command: () => this.addToPlaylist(),
    },
    {
      label: 'Delete',
      icon: 'pi pi-trash',
      command: () => this.deleteVideo(),
      visible: false,
    },
    {
      label: 'Report',
      icon: 'pi pi-flag',
      command: () => this.reportVideo(),
    },
    {
      label: 'Edit metadata',
      icon: 'pi pi-file-edit',
      command: () => this.editMetadata(),
      visible: false,
    },
  ];
  showReportDialog = false;
  showDonateDialog = false;
  isProgressSpinnerVisible = false;
  id = '';
  donateAmount = 0;
  showPlaylistDialog = false;
  userPlaylists!: UserPlaylistsDto[];
  reportReason = '';
  targetId = '';
  userId: string;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router,
    private videoService: VideoService,
    private location: Location,
    private reactionsService: ReactionsService,
    private subscriptionService: SubscriptionService,
    private ticketService: TicketService,
    private donationService: DonationService,
    private playlistService : PlaylistService,
    private messageService : MessageService
  ) {
    this.videoId = this.route.snapshot.params['videoId'];
    this.userId = getUserId();
  }

  ngOnInit(): void {
    this.subscriptions.push(this.videoService
      .getVideoMetadata(this.videoId)
      .pipe(
        switchMap(videoMetadata => {
          this.videoMetadata = videoMetadata;

          return forkJoin({
            user: this.userService.getUser(this.videoMetadata.authorId),
            userVideos: this.videoService.getUserVideos(this.videoMetadata.authorId),
            subscriptionList: this.subscriptionService.getSubscriptions(this.userId),
          });
        })
      )
      .subscribe(({ user, userVideos, subscriptionList }) => {
        this.author = user;
        this.videos = userVideos.videos;
        this.isAuthorSubscribed = this.isThisAuthorSubscribed(subscriptionList);
        if (this.userId == this.author.id) {
          this.videoMenuModel[1].visible = true;
          this.videoMenuModel[3].visible = true;
        }
        if (getRole() == 'Administrator') {
          this.videoMenuModel[1].visible = true;
        }
      }));
    this.getReactions();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => {
      subscription.unsubscribe();
    });
  }

  public get videoUrl(): string {
    return `${getApiUrl()}/video/${this.videoId}?access_token=${getToken()}`;
  }

  private getReactions(): void {
    this.subscriptions.push(this.reactionsService.getReactions(this.videoId).subscribe(reactions => this.reactions = reactions));
  }

  private checkIfAuthorIsSubscribed(): void {
    this.subscriptions.push(this.subscriptionService.getSubscriptions(this.userId).subscribe(subscriptions =>
        this.isAuthorSubscribed = this.isThisAuthorSubscribed(subscriptions)
      ));
  }

  private isThisAuthorSubscribed(subscriptionList: userSubscriptionListDto): boolean {
    for (let subscription of subscriptionList.subscriptions) {
      if (subscription.id === this.videoMetadata.authorId) {
        return true;
      }
    }
    return false;
  }

  private updateSubCount(): void {
    this.subscriptions.push(this.userService.getUser(this.author.id).subscribe(user => {
      this.author.subscriptionsCount = user.subscriptionsCount;
    }));
  }

  private deleteVideo(): void {
    const deleteVideo$ = this.videoService.deleteVideo(this.videoMetadata.id).pipe(
      tap(() => {
        this.location.back();
      }),
    );
    this.subscriptions.push(deleteVideo$.subscribe());
  }

  private addToPlaylist(): void {
    if (this.userPlaylists == null)
    {
      this.getOwnPlaylists();
    }
    this.showPlaylistDialog = true;
  }

  choosePlaylist(id: string)
  {
    const playlist$ = this.playlistService.addToPlaylist(id, this.videoId).pipe(
        tap(() => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Video added to playlist'
          })
          this.getOwnPlaylists();
        })
      );
      this.subscriptions.push(this.doWithLoading(playlist$).subscribe());
    this.showPlaylistDialog = false;
  }

  private doWithLoading(observable$: Observable<any>): Observable<any> {
    return of(this.isProgressSpinnerVisible = true).pipe(
      switchMap(() => observable$),
      finalize(() => this.isProgressSpinnerVisible = false)
    );
  }

  getOwnPlaylists() {
    this.playlistService.getOwnPlaylists().subscribe(
      (userPlaylists) => {
        this.userPlaylists = userPlaylists;
      }
    );
  }

  private reportVideo(): void {
    this.targetId = this.videoId;
    this.showReportDialog = true;
  }

  private editMetadata(): void {
    this.router.navigate(['update-video-metadata/' + this.videoId]);
  }

  public goToUserProfile(id: string): void {
    this.router.navigate(['creator/' + id]);
  }

  public goToVideo(id: string): void {
    this.router.navigate(['videos/' + id]);
  }

  handlePositiveReactionOnClick(): void {
    const addReaction: AddReactionDTO = this.reactions.currentUserReaction === this.positiveReaction
      ? { value: this.noneReaction }
      : { value: this.positiveReaction };

    this.addOrUpdateReaction(addReaction);
  }

  handleNegativeReactionOnClick(): void {
    const addReaction: AddReactionDTO = this.reactions.currentUserReaction === this.negativeReaction
      ? { value: this.noneReaction }
      : { value: this.negativeReaction };

    this.addOrUpdateReaction(addReaction);
  }

  handleSubscribtionOnClick(): void {
    if (this.isAuthorSubscribed) {
      const checkIfAuthorIsSubscribed$ = this.subscriptionService.deleteSubscription(this.author.id).pipe(
        tap(() => {
          this.checkIfAuthorIsSubscribed();
        }),
      );

      this.subscriptions.push(checkIfAuthorIsSubscribed$.subscribe());
    } else {
      const checkIfAuthorIsSubscribed$ = this.subscriptionService.postSubscription(this.author.id).pipe(
        tap(() => {
          this.checkIfAuthorIsSubscribed();
        }),
      );

      this.subscriptions.push(checkIfAuthorIsSubscribed$.subscribe());
    }

    this.updateSubCount();
  }

  private addOrUpdateReaction(addReaction: AddReactionDTO): void {
    const getReactions$ = this.reactionsService.addOrUpdateReaction(this.videoId, addReaction).pipe(
      tap(() => {
        this.getReactions();
      }),
    );

    this.subscriptions.push(getReactions$.subscribe());
  }

  public getTimeAgo(value: Date): string {
    return getTimeAgo(value);
  }

  report() {
    this.showReportDialog = false;  // close the dialog

    if(this.targetId && this.reportReason) {
      const dto: SubmitTicketDto = {
        targetId: this.targetId,
        reason: this.reportReason
      };
      this.ticketService.submitTicket(dto).subscribe(
        response => console.log(response),  // replace with actual response handling
        error => console.error(error)  // replace with actual error handling
      );
    }
    this.reportReason = '';  // reset the reason
  }

  startDonate() {
    this.showDonateDialog = true;
  }

  donate() {
    if (!this.isDonateImpossible())
    {
      const donate$ = this.donationService.sendDonation(this.author.id, this.donateAmount).pipe(
        tap(() => {
          this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Money was donated'
        })
      })
      );
      this.subscriptions.push(this.doWithLoading(donate$).subscribe());
      this.showDonateDialog = false;
    }
  }

  isDonateImpossible() : boolean {
    return this.donateAmount <= 0;
  }
}
