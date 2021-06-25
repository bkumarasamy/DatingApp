import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl=environment.apiUrl;
 
  constructor(private http:HttpClient) { }

getMembers() {
  // console.log(httpOptions.headers.get("Authorization"));
  return this.http.get<Member[]>(this.baseUrl+'users');
}

getMember(username: string) {
  return this.http.get<Member>(this.baseUrl+'users/'+username);
}

}
