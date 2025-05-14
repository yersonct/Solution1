import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpInterceptor,
} from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable()
export class OAuthInterceptor implements HttpInterceptor {
  constructor(private oauthService: OAuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const accessToken = this.oauthService.getAccessToken();

    if (accessToken) {
      const authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`,
        },
      });
      return next.handle(authReq);
    }

    return next.handle(req);
  }
}
