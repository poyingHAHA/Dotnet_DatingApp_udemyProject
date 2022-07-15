import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  loggedIn: boolean;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }

  login(){
    this.accountService.login(this.model).subscribe(
      {
        next: response => {
          console.log(response);
          this.loggedIn = true;
        },
        error: error => {
          console.log(error);
        }
      }
    )
  }

  logout(){
    this.accountService.logout();
    this.loggedIn = false;
  }

  observer = {
    next: user => {
      // double exclamation marks here. Turn our object into a boolean
      // Now our user is either null or it's a user object.
      // But if we use the double exclamation marks here, we effectively saying if the user is null,
      // then this means it's false. And if the user is something, then then it works out to be true.
      this.loggedIn = !!user;
    },
    error: error => {
      console.log(error)
    }
  }

  getCurrentUser(){
    this.accountService.currentUser$.subscribe(this.observer);
  }

}
