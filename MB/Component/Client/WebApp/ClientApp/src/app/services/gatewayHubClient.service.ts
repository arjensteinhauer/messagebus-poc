import { Injectable, Inject, Optional } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { EventAggregator } from './eventAggregator'
import { CustomXMLHttpClient } from './custom-xml-http.service'
import { API_BASE_URL } from './gatewayClient.service';


@Injectable()
export class MessageHubConnectionManager {
  public static connectionIds: MessageHubConnection[];

  init() {
    MessageHubConnectionManager.connectionIds = [];
  }
}

export class SignalRHubClient {
  protected readonly hubConnection: signalR.HubConnection;
  protected readonly eventAggregator: EventAggregator;
  private readonly httpClient: CustomXMLHttpClient;
  private readonly baseUrl: string;
  private readonly hubName: string;

  public ConnectionId: string;

  constructor(
    eventAggregator: EventAggregator,
    httpClient: CustomXMLHttpClient,
    hubName: string,
    baseUrl?: string) {

    // init members
    this.eventAggregator = eventAggregator;
    this.httpClient = httpClient;
    this.baseUrl = (baseUrl ? baseUrl : '') + `/${hubName.toLowerCase()}hub`;
    this.hubName = hubName;
    this.ConnectionId = null;

    // initialize the connection with the hub (do not connect yet)
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl, { httpClient: this.httpClient })
      .withAutomaticReconnect()
      .build();

    // save new connection ID on reconnect
    this.hubConnection.onreconnected((connectionId?: string) => {
      this.storeConnectionId(connectionId);
    })
  }

  /**
   * Starts the connection with the engagement hub.
   */
  public startConnection() {
    this.hubConnection
      .start()
      .then(() => this.storeConnectionId(this.hubConnection.connectionId))
      .catch(error => console.log('Error while starting connection with ' + this.baseUrl + ': ' + error));
  }

  /**
   * Stops the connection with the engagement hub.
   */
  public stopConnection() {
    this.hubConnection.stop();
    this.clearConnectionId();
  }

  /**
   * Store the new connection ID. Needed for adding to the http header.
   * @param connectionId
   */
  private storeConnectionId(connectionId: string) {

    this.clearConnectionId();

    const index = MessageHubConnectionManager.connectionIds.findIndex(c => c.connectionId === connectionId);
    if (index < 0) {
      MessageHubConnectionManager.connectionIds.push({ httpHeaderName: `${this.hubName}-Connection-Id`, connectionId: connectionId });
    }

    this.ConnectionId = connectionId;
  }

  /**
   * Clear the connection ID so it will not be added to the http header anymore.
   */
  private clearConnectionId() {
    if (this.ConnectionId) {
      const index = MessageHubConnectionManager.connectionIds.findIndex(c => c.connectionId === this.ConnectionId);
      if (index > -1) {
        MessageHubConnectionManager.connectionIds.splice(index, 1);
      }

      this.ConnectionId = null;
    }
  }
}

@Injectable({
  providedIn: 'root'
})
export class Message1HubClient extends SignalRHubClient {

  constructor(
    @Inject(EventAggregator) eventAggregator: EventAggregator,
    @Inject(CustomXMLHttpClient) httpClient: CustomXMLHttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string) {

    // init members for message1 hub
    super(eventAggregator, httpClient, 'Message1', baseUrl);

    // configure events received from the message1 hub
    this.hubConnection.on('OnEchoed', (eventData: EchoedEventData) => {
      this.eventAggregator.publish('Message1OnEchoedResult', eventData);
    });

    this.hubConnection.on('OnAmAlive', (eventData: AmAliveEventData) => {
      this.eventAggregator.publish('Message1OnAmAliveResult', eventData);
    });

    this.hubConnection.on('OnProcessedOneWayCommand', (eventData: ProcessedOneWayCommandEventData) => {
      this.eventAggregator.publish('Message1OnProcessedOneWayCommandResult', eventData);
    });
  }
}

@Injectable({
  providedIn: 'root'
})
export class Message2HubClient extends SignalRHubClient {

  constructor(
    @Inject(EventAggregator) eventAggregator: EventAggregator,
    @Inject(CustomXMLHttpClient) httpClient: CustomXMLHttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string) {

    // init members for message2 hub
    super(eventAggregator, httpClient, 'Message2', baseUrl);

    // configure events received from the message1 hub
    this.hubConnection.on('OnEchoed', (eventData: EchoedEventData) => {
      this.eventAggregator.publish('Message2OnEchoedResult', eventData);
    });

    this.hubConnection.on('OnAmAlive', (eventData: AmAliveEventData) => {
      this.eventAggregator.publish('Message2OnAmAliveResult', eventData);
    });
  }
}

export interface MessageHubConnection {
  httpHeaderName: string;
  connectionId: string;
}

export interface EchoedEventData {
  result: string;
}

export interface AmAliveEventData {
  message: string;
}

export interface ProcessedOneWayCommandEventData {
  result: string;
}
