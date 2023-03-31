import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlaylistDto } from 'src/app/core/models/playlist-dto';
import { UserDTO } from 'src/app/core/models/user-dto';
import { VideoListDto } from 'src/app/core/models/video-list-dto';
import { VideoMetadataDto } from 'src/app/core/models/video-metadata-dto';
import { PlaylistService } from 'src/app/core/services/playlist.service';
import { UserService } from 'src/app/core/services/user.service';
import { VideoService } from 'src/app/core/services/video.service';

@Component({
  selector: 'app-creator',
  templateUrl: './creator.component.html',
  styleUrls: ['./creator.component.scss']
})
export class CreatorComponent {
  id!: string;
  user!: UserDTO;
  videos: VideoMetadataDto[];
  playlists: PlaylistDto[];

  constructor(
    private route: ActivatedRoute, 
    private userService: UserService,
    private playlistService: PlaylistService,
    private videoService: VideoService
    ) {
    this.id = this.route.snapshot.paramMap.get('id')!;
    userService.getUser(this.id).subscribe(user => this.user = user);

    this.playlists = [];
    this.videos = [];

    this.getVideos();
    this.getPlaylists();
  }

  public getVideos() {
    console.log('Tab 1 selected!');
    this.videoService.getUserVideos(this.id).subscribe(videos => {
      this.videos = videos.videos;
    });
  }

  public getPlaylists() {
    console.log('Tab 2 selected!');
    this.playlistService.getUserPlaylists(this.id).subscribe(playlists => {
      this.playlists = playlists;
    });
  }
}
