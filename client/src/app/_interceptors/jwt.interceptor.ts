import { AccountService } from './../_services/account.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { User } from '../_models/user';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User;

    // So by using the take and just by taking one thing from this observable, what we're doing here is
    // we're saying that we want to complete after we've received one of these current users.
    // And this way we don't need to subscribe because once an observable has completed,
    // then we are effectively not subscribed to it anymore.
    // So whenever we're not sure if we need to unsubscribe from something, then what we can do is just simply
    // add that pipe and then take one in this case, and then we can go and subscribe and we kind of guarantee unsubscribe
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);

    // Then what we want to do is we want to clone this request and add our authentication header onto it.
    // So what we'll do is we'll say request equals request.clone. And inside here we can say set headers.
    // And inside this object, we can specify our authorization.
    if(currentUser){
      request = request.clone(
        {
          setHeaders: {
            Authorization: `Bearer ${currentUser.token}`
          }
        }
      );
    }

    return next.handle(request);
  }
}
