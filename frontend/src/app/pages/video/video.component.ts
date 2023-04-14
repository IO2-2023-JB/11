import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.scss']
})
export class VideoComponent  {
  videoUrl: string = `${environment.webApiUrl}/video/${this.route.snapshot.params['videoId']}?access_token=${this.userService.getToken()}`;

  constructor(private route: ActivatedRoute, private userService: UserService) {}
}
