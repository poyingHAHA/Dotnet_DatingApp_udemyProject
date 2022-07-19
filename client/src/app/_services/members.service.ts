import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  // Now, remember, services are singletons, they're instantiated when a component needs the service.
  // And that it operates as a singleton and it stays alive until the application is closed.
  // So services make a good candidate for storing our application state.
  members: Member[] = [];

  constructor(private http: HttpClient) { }

  getMembers() {
    // So if we have the members, then we're going to return the members from the service as an observable.
    if(this.members.length > 0) return of(this.members)
    // And then what we need to do is set the members once we've gone and made the effort to get them from the API.
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    )
  }

  getMember(username: string) {
    const member = this.members.find(x => x.username === username);
    if(member !== undefined) return of(member);
    // And obviously if we don't have the member, then we're gonna go and make our API call.
    return this.http.get<Member>(this.baseUrl + 'users/' + username)
  }

  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {})
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl+"users/delete-photo/" +photoId)
  }

}
