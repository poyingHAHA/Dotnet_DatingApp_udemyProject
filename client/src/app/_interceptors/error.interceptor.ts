import { NavigationExtras, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

// what we can do here is we can either intercept the request that goes out
// or the response that comes back in the next where we handle the response.
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  // And what we'll do inside here is we'll add a couple of things in our constructor to inject in here.
  // We're going to add the router. And the reason for this is that for certain types of errors, we're going to want to redirect the user to an error page.
  // And we're also going to bring in the toaster service because for certain types of errors, we might ant to just display a toast notification
  constructor(private router: Router, private toastr: ToastrService) {}

  // what we do inside here we return next handle and then want to just display a toast notification and what we do inside here we return next handle and then the request itself.
  // But what we want to do is catch any errors from this now our interceptor.
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // So in order to do something with this, like any other observable, we're going to need to use the pipe
    return next.handle(request).pipe(
      // So the idea being is that for the majority of cases,
      // when we're going to catch them inside this switch statement, if we don't catch it,
      // we're going to return the error to whatever was calling the HTTP request in the first place.
      catchError(error =>{
        if(error){
          switch(error.status){
            case 400:
              if(error.error.errors){
                const modelStateErrors = [];
                for(const key in error.error.errors){
                  if(error.error.errors[key]){
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modelStateErrors;
              }else{
                this.toastr.error(error.statusText, error.status);
              }
              break;
            case 401:
              this.toastr.error(error.statusText, error.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationiExtras: NavigationExtras = {state: {error: error.error}};
              this.router.navigateByUrl('/server-error', navigationiExtras);
              break;
            default:
              this.toastr.error("Something unexpected went wrong");
              console.log(error);
              break;
          }
        }
        return throwError(error);
      })

    )
  }
  // Now, what we need to do, because this is an interceptor, we need to provide this in our app module
}
