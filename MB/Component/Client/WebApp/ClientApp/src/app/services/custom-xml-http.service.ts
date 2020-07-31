import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { throwError as _observableThrow, of as _observableOf } from 'rxjs';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class CustomXMLHttpClient extends signalR.HttpClient {
  private readonly logger: signalR.ILogger;

  public constructor() {
    super();
    this.logger = signalR.NullLogger.instance;
  }

  /** Issues an HTTP request to the specified URL, returning a {@link Promise} that resolves with an {@link @microsoft/signalr.HttpResponse} representing the result.
   *
   * @param {HttpRequest} request An {@link @microsoft/signalr.HttpRequest} describing the request to send.
   * @returns {Promise<HttpResponse>} A Promise that resolves with an HttpResponse describing the response, or rejects with an Error indicating a failure.
   */
  public send(request: signalR.HttpRequest): Promise<signalR.HttpResponse> {
    // Check that abort was not signaled before calling send
    if (request.abortSignal && request.abortSignal.aborted) {
      return Promise.reject(new signalR.AbortError());
    }

    if (!request.method) {
      return Promise.reject(new Error("No method defined."));
    }
    if (!request.url) {
      return Promise.reject(new Error("No url defined."));
    }

    return new Promise<signalR.HttpResponse>((resolve, reject) => {
      const xhr = new XMLHttpRequest();

      xhr.open(request.method!, request.url!, true);

      //TODO: when we can support 'allow CORS specific origins' (instead of allow '*') in Azure API Management
      //      we cannot send the negotiate request with credentials
      // xhr.withCredentials = true;
      // xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");

      // Explicitly setting the Content-Type header for React Native on Android platform.
      xhr.setRequestHeader("Content-Type", "text/plain;charset=UTF-8");

      const headers = request.headers;
      if (headers) {
        Object.keys(headers)
          .forEach((header) => {
            xhr.setRequestHeader(header, headers[header]);
          });
      }

      if (request.responseType) {
        xhr.responseType = request.responseType;
      }

      if (request.abortSignal) {
        request.abortSignal.onabort = () => {
          xhr.abort();
          reject(new signalR.AbortError());
        };
      }

      if (request.timeout) {
        xhr.timeout = request.timeout;
      }

      xhr.onload = () => {
        if (request.abortSignal) {
          request.abortSignal.onabort = null;
        }

        if (xhr.status >= 200 && xhr.status < 300) {
          resolve(new signalR.HttpResponse(xhr.status, xhr.statusText, xhr.response || xhr.responseText));
        } else {
          reject(new signalR.HttpError(xhr.statusText, xhr.status));
        }
      };

      xhr.onerror = () => {
        this.logger.log(signalR.LogLevel.Warning, `Error from HTTP request. ${xhr.status}: ${xhr.statusText}.`);
        reject(new signalR.HttpError(xhr.statusText, xhr.status));
      };

      xhr.ontimeout = () => {
        this.logger.log(signalR.LogLevel.Warning, `Timeout from HTTP request.`);
        reject(new signalR.TimeoutError());
      };

      xhr.send(request.content || "");
    });
  }
}
