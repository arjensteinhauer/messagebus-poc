import { Injectable, Inject } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MessageHubConnectionManager } from './gatewayHubClient.service';

@Injectable({
  providedIn: 'root'
})
export class HeaderInterceptor {

  constructor(
    @Inject(MessageHubConnectionManager) private messageHubConnectionManager: MessageHubConnectionManager) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let headers = request.headers;
    headers = headers.set("TenantName", "tenant1");

    if (MessageHubConnectionManager.connectionIds) {

      MessageHubConnectionManager.connectionIds.forEach(connection => {
        headers = headers.set(connection.httpHeaderName, connection.connectionId);
      });
    }

    request = request.clone({ headers });

    return next.handle(request);
  }
}
