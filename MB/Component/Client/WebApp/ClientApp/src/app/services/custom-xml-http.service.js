"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var signalR = require("@microsoft/signalr");
var CustomXMLHttpClient = /** @class */ (function (_super) {
    __extends(CustomXMLHttpClient, _super);
    function CustomXMLHttpClient(logger, apimKey) {
        var _this = _super.call(this) || this;
        _this.logger = logger;
        _this.apimKey = apimKey;
        return _this;
    }
    /** Issues an HTTP request to the specified URL, returning a {@link Promise} that resolves with an {@link @microsoft/signalr.HttpResponse} representing the result.
     *
     * @param {HttpRequest} request An {@link @microsoft/signalr.HttpRequest} describing the request to send.
     * @returns {Promise<HttpResponse>} A Promise that resolves with an HttpResponse describing the response, or rejects with an Error indicating a failure.
     */
    CustomXMLHttpClient.prototype.send = function (request) {
        var _this = this;
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
        return new Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.open(request.method, request.url, true);
            //TODO: when we can support 'allow CORS specific origins' (instead of allow '*') in Azure API Management
            //      we cannot send the negotiate request with credentials
            // xhr.withCredentials = true;
            // xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            // Explicitly setting the Content-Type header for React Native on Android platform.
            xhr.setRequestHeader("Content-Type", "text/plain;charset=UTF-8");
            if (_this.apimKey || _this.apimKey !== "") {
                xhr.setRequestHeader("Ocp-Apim-Subscription-Key", _this.apimKey);
            }
            var headers = request.headers;
            if (headers) {
                Object.keys(headers)
                    .forEach(function (header) {
                    xhr.setRequestHeader(header, headers[header]);
                });
            }
            if (request.responseType) {
                xhr.responseType = request.responseType;
            }
            if (request.abortSignal) {
                request.abortSignal.onabort = function () {
                    xhr.abort();
                    reject(new signalR.AbortError());
                };
            }
            if (request.timeout) {
                xhr.timeout = request.timeout;
            }
            xhr.onload = function () {
                if (request.abortSignal) {
                    request.abortSignal.onabort = null;
                }
                if (xhr.status >= 200 && xhr.status < 300) {
                    resolve(new signalR.HttpResponse(xhr.status, xhr.statusText, xhr.response || xhr.responseText));
                }
                else {
                    reject(new signalR.HttpError(xhr.statusText, xhr.status));
                }
            };
            xhr.onerror = function () {
                _this.logger.log(signalR.LogLevel.Warning, "Error from HTTP request. " + xhr.status + ": " + xhr.statusText + ".");
                reject(new signalR.HttpError(xhr.statusText, xhr.status));
            };
            xhr.ontimeout = function () {
                _this.logger.log(signalR.LogLevel.Warning, "Timeout from HTTP request.");
                reject(new signalR.TimeoutError());
            };
            xhr.send(request.content || "");
        });
    };
    return CustomXMLHttpClient;
}(signalR.HttpClient));
exports.CustomXMLHttpClient = CustomXMLHttpClient;
//# sourceMappingURL=custom-xml-http.service.js.map