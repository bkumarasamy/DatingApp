import { User } from './../_models/user';
import { Observable } from 'rxjs/internal/Observable';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  title = 'Sam App'; 
  model:any={};
  // LoggedIn:boolean=false;
  // currentUser$=new Observable<User>();

  constructor(public accountService:AccountService,private router:Router,
    private toastr:ToastrService) { }

  ngOnInit(): void {
    // this.getCurrentUser();
    // this.currentUser$=this.accountService.currentUser$;
  }

  login()
  { 
    console.log("login method calling in NAV");
    this.accountService.login(this.model).subscribe(response=>{
      this.router.navigateByUrl('/members');
        // console.log(response);
        // this.LoggedIn=true;
     })
     //,error=>{
    //   console.log(error);
    //   this.toastr.error(error.error);
      
    //   // this.LoggedIn=false;
    // })
  }

  logout()
  {
    // this.LoggedIn=false;
    this.accountService.logout();
    this.router.navigateByUrl('/');

  }

// getCurrentUser()
// {
//   this.accountService.currentUser$.subscribe(user=>{
//     this.LoggedIn=!!user;
//   },error=>{
//     console.log(error);
//   })
// }

    // this.accountService.login(this.model).subscribe(response=>{
    //   console.log("Success");
    //   console.log(response);
    //   this.loggedIn=true;
    // },error=>{
    //   // console.log(Error);
    // });
  // }



}
