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
  Users:any;
  

  // private http:HttpClient,

  constructor(private accountservice:AccountService)
  {
  }
  
  ngOnInit(): void {
      // this.getuser();
      this.setCurrentUser();
    }

    setCurrentUser(){
      var localStorage: Storage|null;
      const user:User=JSON.parse(localStorage.getItem('user'));
      this.accountservice.setCurrentUser(user)
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
