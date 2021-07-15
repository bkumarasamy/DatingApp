import { take } from 'rxjs/operators';
import { MessageService } from './../../_services/message.service';
import { Message } from './../../_models/Message';
import { Photo } from './../../_models/Photo';
import { MembersService } from './../../_services/members.service';
import { Member } from 'src/app/_models/member';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
//import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery/public-api';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs',{static: true}) memberTabs: TabsetComponent;
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab:TabDirective;
  messages:Message[]=[];

  constructor(private membersService:MembersService,private route:ActivatedRoute,
    private messageService:MessageService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.member=data.member;
    })

    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab):this.selectTab(0)
    })
    
    this.galleryOptions=[
      {
        width:'500px',
        height:'500px',
        imagePercent:100,
        thumbnailsColumns:4,
        imageAnimation:NgxGalleryAnimation.Slide,
        preview:false
      }
    ]
    this.galleryImages=this.getImages();
  }

  getImages():NgxGalleryImage[]{
    const imageUrls=[];
    for(const Photo of this.member.photos){
      imageUrls.push({
        small:Photo.url,
        medium:Photo.url,
        big:Photo?.url
      })
    }
    return imageUrls;
  }

  

  loadMemebr(){
    this.membersService.getMember(this.route.snapshot.paramMap.get('username'))
    .subscribe(member=> {
      this.member = member;
      })
  }

  loadMessages(){
    this.messageService.getMessageThread(this.member.username).subscribe(messages => {
      this.messages=messages;
    })
  }

  selectTab(tabId:number){
    this.memberTabs.tabs[tabId].active=true;
  }

  onTabActivated(data:TabDirective){
    this.activeTab=data;
    if(this.activeTab.heading==='Messages' && this.messages.length===0){
      this.loadMessages();
    }
  }

}
