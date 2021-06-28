import { Observable } from 'rxjs/internal/Observable';
import { MembersService } from './../../_services/members.service';
import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

// members: Member[];
members$:Observable<Member[]>;

  constructor(private memberService:MembersService) { }

  ngOnInit(): void {
    // this.loadMemebers();
    this.members$=this.memberService.getMembers();
  }

  // loadMemebers(){
  //   this.memberService.getMembers().subscribe(members=> {
  //     this.members=members;
  //    console.log(members);
  //   })
  // }
}
