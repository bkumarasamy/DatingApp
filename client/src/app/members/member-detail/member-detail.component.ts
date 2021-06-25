import { Photo } from './../../_models/Photo';
import { MembersService } from './../../_services/members.service';
import { Member } from 'src/app/_models/member';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
//import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery/public-api';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
member: Member;
galleryOptions: NgxGalleryOptions[];
galleryImages: NgxGalleryImage[];

  constructor(private membersService:MembersService,private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMemebr()
    
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
      this.galleryImages=this.getImages();
    })
  }

}
