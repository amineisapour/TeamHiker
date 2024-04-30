import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoaderService } from '../services/common/loader.service';
import { catchError, finalize, map } from 'rxjs/operators';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {

  constructor(private loaderService: LoaderService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // this.loaderService.isLoading.next(true);
    // return next.handle(request).pipe(
    //   finalize(
    //     () => {
    //       this.loaderService.isLoading.next(false);
    //     }
    //   )
    // );

    this.loaderService.setLoading(true, request.url);
    return next.handle(request)
      .pipe(
        finalize(
          () => {
            this.loaderService.setLoading(false, request.url);
          }
        )
      )
      .pipe(
        map<HttpEvent<any> | any, any>(
          (evt: HttpEvent<any> | any) => {
            if (evt instanceof HttpResponse) {
              this.loaderService.setLoading(false, request.url);
            }
            return evt;
          }
        )
      );

    // return next.handle(request)
    //   .pipe(
    //     catchError((err) => {
    //     this.loaderService.setLoading(false, request.url);
    //     return err;
    //   }))
    //   .pipe(map<HttpEvent<any> | any, any>((evt: HttpEvent<any> | any) => {
    //     if (evt instanceof HttpResponse) {
    //       this.loaderService.setLoading(false, request.url);
    //     }
    //     console.log(1);
    //     return evt;
    //   }));
  }
}
