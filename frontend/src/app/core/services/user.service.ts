import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { environment } from "../../../environments/environment";
import { UserForRegistrationDTO } from "../../authentication/models/user-for-registration-dto";
import { UserDTO } from "../models/user-dto";
import { UserForLoginDTO } from "src/app/authentication/models/user-login-dto";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly registrationPageWebAPIUrl: string = `${environment.webApiUrl}`;
  private authenticationStateChangeSubject = new Subject<boolean>();

  constructor(private httpClient: HttpClient) {}

  sendAuthenticationStateChangedNotification = (isAuthenticated: boolean): void => {
    this.authenticationStateChangeSubject.next(isAuthenticated);
  }

  registerUser(userForRegistration: UserForRegistrationDTO): Observable<void> {
    return this.httpClient.post<void>(`${this.registrationPageWebAPIUrl}/register`, userForRegistration);
  }

  getUser(id: string): Observable<UserDTO> {
    let params = new HttpParams().set('id', id);
    
    return this.httpClient.get<UserDTO>(`${this.registrationPageWebAPIUrl}/user`, { params: params });
  }

  loginUser(userForLogin: UserForLoginDTO): Observable<string> {
    return this.httpClient.post<string>(`${this.registrationPageWebAPIUrl}/login`, userForLogin);
  }
}
