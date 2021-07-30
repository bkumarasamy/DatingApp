import { environment } from './../../environments/environment';
import { User } from './../_models/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
// import { constants } from 'buffer';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/operators';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
baseurl=environment.apiUrl;  //'https://localhost:5001/api/';
private currentUserSource=new ReplaySubject<User>(1);
currentUser$=this.currentUserSource.asObservable();
// currentUser$: any;

  constructor(private http:HttpClient) { }

  // login(model:any){
  //   const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' })};
  //   return this.http.post(this.baseurl+"account/login",model,options);
  // }


  login(model: any): Observable<any> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) 
  };
    const url = this.baseurl+"Account/login";
    console.log(url)
    console.log(model)
    console.log(options)

    return this.http.post<any>(url, model, options).pipe(
            map((response:User) => {
                const user=response;
                if(user){

                  this.setCurrentUser(user);

                  // localStorage.setItem('user',JSON.stringify(user));
                  // this.currentUserSource.next(user);

                  // this.currentUser$=user;
                  // console.log(user);
                  // console.log(this.currentUser$);
                } 
                return user;
            })
        );
  }

  setCurrentUser(user:User){
    user.roles=[];
    const roles=this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles= roles:user.roles.push(roles);
    localStorage.setItem('user',JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  getDecodedToken(token){
    return JSON.parse(atob(token.split('.')[1]));
  }

  // login(model:any):Observable<any>
  // {
  //   const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' })};
  //   const postUrl = this.baseurl +'Account/login';
  //   // const headers = new HttpHeaders();
  //   // headers.append('Content-Type', 'application/json');
  //   // headers.append('Accept', 'application/json');
  //   return this.http.post(postUrl,model,options);
  // }

  Register(model:any)
  {
    const postUrl = this.baseurl +'Account/register';
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    headers.append('Accept', 'application/json');
    return this.http.post(postUrl,model,{headers:headers}).pipe(
      map((user:User)=>{
          if(user)
          {
            this.setCurrentUser(user);

              // localStorage.setItem("user",JSON.stringify(user));
              // this.currentUserSource.next(user);
          }
          return user;
        })
    );
  }

}
