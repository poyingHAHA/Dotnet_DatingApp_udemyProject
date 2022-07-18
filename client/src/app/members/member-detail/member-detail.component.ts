import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  // So what we also need in here, because what we want to access, when a user clicks on any of these routes,
  // then we're going to send up or they're going to use their username.
  // Or we going to use the username to decide which user this is and we need to access that particular user's profile
  // So what we need to do to get that is the activated route and what we'll say is private route and then we'll bring in activated route from Angular router.
  constructor(private memberService: MembersService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMember();
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

  }

  getImages():NgxGalleryImage[]{
    const imageUrls = [];
    for(const photo of this.member.photos){
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      })
    }
    return imageUrls;
  }

  loadMember(){
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
      this.member = member;
      // That way we guarantee that we have the photos before we attempt to load or initialize the gallery images there
      this.galleryImages = this.getImages();
    })
  }

}
