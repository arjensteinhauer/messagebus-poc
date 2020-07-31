import { APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppConfig } from './app.config';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { EventsComponent } from './events/events.component';
import { Message1Component } from './message1/message1.component';
import { Message2Component } from './message2/message2.component';
import { HeaderInterceptor } from './services/headerInterceptor';

import { Message1Client, Message2Client, EventsClient, API_BASE_URL } from './services/gatewayClient.service';
import { MessageHubConnectionManager, Message1HubClient, Message2HubClient } from './services/gatewayHubClient.service';
import { CustomXMLHttpClient } from './services/custom-xml-http.service';

import { EventAggregator } from './services/eventAggregator'

export function initializeApp(appConfig: AppConfig, messageHubConnectionManager: MessageHubConnectionManager) {
  return () => {
    messageHubConnectionManager.init();
    appConfig.load();
  }
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    EventsComponent,
    Message1Component,
    Message2Component
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'message1', component: Message1Component },
      { path: 'message2', component: Message2Component },
      { path: 'events', component: EventsComponent },
    ])
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AppConfig, MessageHubConnectionManager],
      multi: true,
    },
    AppConfig,
    MessageHubConnectionManager,
    EventAggregator,
    CustomXMLHttpClient,
    Message1Client,
    Message1HubClient,
    Message2Client,
    Message2HubClient,
    EventsClient,
    {
      provide: API_BASE_URL,
      useFactory: (appconfig: AppConfig) => AppConfig.settings.API_BASE_URL,
      deps: [APP_INITIALIZER],
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HeaderInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
