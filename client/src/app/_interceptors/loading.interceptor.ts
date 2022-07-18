import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { delay, finalize, Observable } from 'rxjs';
import { BusyService } from '../_services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.busyService.busy();
    // And then what are we going to do once the request comes back?
    // We know it's completed so we can turn off our busy spinner.
    // And the first thing we want to do is add a fake delay. Our application is way too fast.
    //
    return next.handle(request).pipe(
      delay(1000),
      // And this gives us an opportunity to do something when things have completed.
      finalize(() => {
        this.busyService.idle();
      })
    )
  }
}
