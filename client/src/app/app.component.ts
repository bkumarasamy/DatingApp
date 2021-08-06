import { PresenceService } from './_services/presence.service';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Sam App'; 
  User:any;
  

  // private http:HttpClient,

  constructor(private accountservice:AccountService
    ,private presenceservice:PresenceService)
  {
  }
  
  ngOnInit(): void {
      // this.getuser();
      this.setCurrentUser();
    }

    setCurrentUser(){
      var localStorage: Storage|null;
      console.log(localStorage.getItem('user'));
      const user:User=JSON.parse(localStorage.getItem('user'));
      if(user) {
        this.accountservice.setCurrentUser(user);
        this.presenceservice.createHubConnection(user);
      }
    }

    // getuser()
    // {
    //   this.http.get("https://localhost:5001/api/Users")
    //       .subscribe(response=>{
    //         this.Users=response;
    //       },Error=>{
    //         console.log("error");
    //       });
    // }

}
