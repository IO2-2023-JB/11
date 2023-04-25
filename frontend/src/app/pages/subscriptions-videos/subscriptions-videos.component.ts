import { Component, OnInit } from '@angular/core';
import { VideoMetadataDto } from 'src/app/core/models/video-metadata-dto';
import { SubscriptionsVideosService } from './services/subscriptions-videos.service';
import { Observable, Subscription, finalize, of, switchMap, tap } from 'rxjs';
import { getTimeAgo } from 'src/app/core/functions/get-time-ago';
import { Router } from '@angular/router';
import { VideoListDto } from 'src/app/core/models/video-list-dto';

@Component({
  selector: 'app-subscriptions',
  templateUrl: './subscriptions-videos.component.html',
  styleUrls: ['./subscriptions-videos.component.scss']
})
export class SubscriptionsVideosComponent implements OnInit {
  videos!: VideoMetadataDto[];
  subscriptions: Subscription[] = [];
  isProgressSpinnerVisible = false;

  constructor(
    private subscriptionsVideosService: SubscriptionsVideosService,
    private router: Router) { }

  ngOnInit(): void {
    const $getSubscriptionsVideos = this.subscriptionsVideosService.getVideosFromSubscriptions().pipe(
      tap((videoListDto: VideoListDto) => {
        this.videos = videoListDto.videos;
      })
    );
    this.subscriptions.push(this.doWithLoading($getSubscriptionsVideos).subscribe());
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => {
      subscription.unsubscribe();
    });
  }

  private doWithLoading(observable$: Observable<any>): Observable<any> {
    return of(this.isProgressSpinnerVisible = true).pipe(
      switchMap(() => observable$),
      finalize(() => this.isProgressSpinnerVisible = false)
    );
  }

  getTimeAgo(video: VideoMetadataDto): string {
    return getTimeAgo(video.uploadDate);
  }

  public goToVideo(id: string): void {
    this.router.navigate(['videos/' + id]);
  }

  public goToCreator(id: string): void {
    this.router.navigate(['creator/' + id]);
  }
}
