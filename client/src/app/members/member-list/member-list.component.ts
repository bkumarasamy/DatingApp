import { UserParams } from './../../_models/userParams';
import { take } from 'rxjs/operators';
import { AccountService } from './../../_services/account.service';
import { Pagination } from './../../_models/pagination';
import { Observable } from 'rxjs/internal/Observable';
import { MembersService } from './../../_services/members.service';
import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
members:Member[];
pagination:Pagination;
userParams:UserParams;
user:User;
genderList=[
            {value:'male',display:'Males'},
            {value:'female',display:'Females'}
           ]


  constructor(private memberService:MembersService) { 
    this.userParams=this.memberService.getUserParam();
  }

  ngOnInit(): void {
    this.loadMemebers();
    // this.members$=this.memberService.getMembers();
  }

  loadMemebers(){
    this.memberService.setUserParams(this.userParams);
    this.memberService.getMembers(this.userParams).subscribe(response=> {
      this.members=response.result;
      this.pagination=response.pagination;
    })
  }

  resetFilters(){
    // this.userParams=new UserParams(this.user);
    this.userParams=this.memberService.resetUserParams();
    this.loadMemebers();
  }

  pageChanged(event: any){
    this.userParams.pageNumber=event.page;
    this.memberService.setUserParams(this.userParams);
    this.loadMemebers();
  }


}
