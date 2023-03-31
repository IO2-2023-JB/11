import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { environment } from "../../../environments/environment";
import { PlaylistDto } from "../models/playlist-dto";

@Injectable({
  providedIn: 'root'
})
export class PlaylistService {
  private authenticationStateChangeSubject = new Subject<boolean>();
  public authenticationStateChanged = this.authenticationStateChangeSubject.asObservable();

  private readonly videoPageWebAPIUrl: string = `${environment.webApiUrl}`;

  constructor(private httpClient: HttpClient) {}

  getUserPlaylists(id: string): Observable<PlaylistDto[]> {
    let params = new HttpParams().set('id', id);
    
    return this.httpClient.get<PlaylistDto[]>(`${this.videoPageWebAPIUrl}/playlist/user`, { params: params });
  }
}
