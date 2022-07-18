import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member

  // So what we also need in here, because what we want to access, when a user clicks on any of these routes,
  // then we're going to send up or they're going to use their username.
  // Or we going to use the username to decide which user this is and we need to access that particular user's profile
  // So what we need to do to get that is the activated route and what we'll say is private route and then we'll bring in activated route from Angular router.
  constructor(private memberService: MembersService, private route: ActivatedRoute) { }
  
  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
      this.member = member;
    })
  }

}
