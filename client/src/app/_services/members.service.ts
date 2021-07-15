import { AccountService } from './account.service';
import { UserParams } from './../_models/userParams';
import { PaginatedResult } from './../_models/pagination';
import { User } from './../_models/user';
import { map, take } from 'rxjs/operators';
import { Member } from './../_models/member';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { of, pipe } from 'rxjs';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';



@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl=environment.apiUrl;
  members:Member[] =[];
  paginatedResult:PaginatedResult<Member[]>=new PaginatedResult<Member[]>();
  memberCache=new Map();
  user: User;
  userParams:UserParams;


  constructor(private http:HttpClient,private accountService:AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user=user;
      this.userParams=new UserParams(user);
    })
   }

   getUserParam(){
     return this.userParams;
   }

   setUserParams(params:UserParams){
    this.userParams=params;
   }

   resetUserParams(){
    this.userParams=new UserParams(this.user);
    return this.userParams;
   }

  getMembers(userParams:UserParams) {
    // console.log(Object.values(userParams).join('_'));

    var response=this.memberCache.get(Object.values(userParams).join('_'));
    if(response){
      return of(response);
    }
    let params=getPaginationHeaders(userParams.pageNumber,userParams.pageSize);

    params=params.append('minAge',userParams.minAge.toString());
    params=params.append('maxAge',userParams.maxAge.toString());
    params=params.append('gender',userParams.gender);
    params=params.append('orderBy',userParams.orderby);


    return getPaginatedResult<Member[]>(this.baseUrl + 'users',params,this.http)
      .pipe(map(response => {
        this.memberCache.set(Object.values(userParams).join('-'),response);
        return response;
      }))
  } 
    // console.log(httpOptions.headers.get("Authorization"));
    // if(this.members.length>0) return of(this.members);
    // return this.http.get<Member[]>(this.baseUrl+'users').pipe(
    //   map(members=>{
    //     this.members=members;
    //     return members;
    //   })
    // )

  getMember(username: string) {
    const member=[...this.memberCache.values()]
      .reduce((arr,elem) => arr.concat(elem.result),[])
      .find((member:Member)=> member.username === username)
    if(member)
    {
      return of(member)
    }
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) 
  };
    // const url = this.baseUrl+'users/';
    // console.log(url)
    // console.log(member)
    // console.log(options)
    return this.http.put(this.baseUrl+'users',member,options).pipe(
      map(()=>{
        console.log("success")
        const index=this.members.indexOf(member);
        this.members[index]=member;
        console.log(this.members[index])
      })
    )
  }

  setMainPhoto(photoId:number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId,{});

  }

  deletePhoto(photoId:number){
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(username:string){
    return this.http.post(this.baseUrl + 'likes/' + username,{});
  }

  getLikes(predicate:string,pageNumber,pageSize){
    let params=getPaginationHeaders(pageNumber,pageSize);
    params=params.append('predicate',predicate);
    return getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes',params,this.http);
    // return this.http.get<Partial<Member[]>>(this.baseUrl + 'likes?predicate=' + predicate);
  }
}
