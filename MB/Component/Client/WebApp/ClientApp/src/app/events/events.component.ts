import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { EventsClient, IAmAliveRequest } from '../services/gatewayClient.service';
import { Message1HubClient, Message2HubClient, AmAliveEventData } from '../services/gatewayHubClient.service';
import { EventAggregator } from '../services/eventAggregator'

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html'
})
export class EventsComponent implements OnInit, OnDestroy {
  private onAmAliveMessage1Event: Subscription;
  private onAmAliveMessage2Event: Subscription;
  public eventMessageText: string;
  public publishResult: string;
  public asyncResultMessage1: string;
  public asyncResultMessage2: string;
  public errorMessage: string;
  public hasErrors: boolean;

  constructor(
    @Inject(EventsClient) private eventsClient: EventsClient,
    @Inject(Message1HubClient) private message1HubClient: Message1HubClient,
    @Inject(Message2HubClient) private message2HubClient: Message2HubClient,
    @Inject(EventAggregator) private eventAggregator: EventAggregator) {
    // subscribe on events
    this.onAmAliveMessage1Event = this.eventAggregator.listen('Message1OnAmAliveResult').subscribe((eventData: AmAliveEventData) => {
      this.asyncResultMessage1 = eventData.message;
    });
    this.onAmAliveMessage2Event = this.eventAggregator.listen('Message2OnAmAliveResult').subscribe((eventData: AmAliveEventData) => {
      this.asyncResultMessage2 = eventData.message;
    });
  }

  ngOnInit(): void {
    this.Clear();
    this.message1HubClient.startConnection();
    this.message2HubClient.startConnection();
  }

  ngOnDestroy(): void {
    this.onAmAliveMessage2Event.unsubscribe();
    this.onAmAliveMessage1Event.unsubscribe();
    this.message2HubClient.stopConnection();
    this.message1HubClient.stopConnection();
  }

  public onSubmit() {
    this.Clear();

    let request = new IAmAliveRequest({ input: this.eventMessageText });

    this.eventsClient
      .iAmAlive(request)
      .subscribe((response: string) => {
        this.publishResult = response;
      }, error => {
        this.errorMessage = error;
        this.hasErrors = true;
      });
  }

  private Clear() {
    this.hasErrors = false;
    this.publishResult = "";
    this.asyncResultMessage1 = "";
    this.asyncResultMessage2 = "";
  }
}
