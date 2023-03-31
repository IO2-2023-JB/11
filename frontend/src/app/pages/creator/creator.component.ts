import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserDTO } from 'src/app/core/models/user-dto';
import { VideoMetadataDto } from 'src/app/core/models/video-metadata-dto';
import { UserService } from 'src/app/core/services/user.service';
import { VideoService } from 'src/app/core/services/video.service';

@Component({
  selector: 'app-creator',
  templateUrl: './creator.component.html',
  styleUrls: ['./creator.component.scss']
})
export class CreatorComponent {
  videosPerRow: number = 6;
  id!: string;
  user!: UserDTO;
  videos!: VideoMetadataDto[];

  constructor(
    private route: ActivatedRoute, 
    private userService: UserService,
    private router: Router,
    private videoService: VideoService
    ) {
    this.id = this.route.snapshot.paramMap.get('id')!;
    userService.getUser(this.id).subscribe(user => this.user = user);

    this.videos = [];

    this.getVideos();
  }

  public getVideos() {
    this.videoService.getUserVideos(this.id).subscribe(videos => {
      this.videos = videos.videos;
    });
  }

  getRows(): number[] {
    const rows = [];
    for (let i = 0; i < this.videos.length; i += this.videosPerRow) {
      rows.push(i / this.videosPerRow);
    }
    return rows;
  }
  
  getListForRow(row: number): VideoMetadataDto[] {
    const startIndex = row * this.videosPerRow;

    return this.videos.slice(startIndex, startIndex + this.videosPerRow);
  }

  public goToUserProfile(id: string): void {
    console.log(id);
    this.router.navigate(['creator/' + id]);
  }
}
