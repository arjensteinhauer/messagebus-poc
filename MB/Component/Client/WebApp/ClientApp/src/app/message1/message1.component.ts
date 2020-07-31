import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Message1Client, EchoRequest, RequestResponseRequest, OneWayRequest } from '../services/gatewayClient.service';
import { Message1HubClient, EchoedEventData, ProcessedOneWayCommandEventData } from '../services/gatewayHubClient.service';
import { EventAggregator } from '../services/eventAggregator'

@Component({
  selector: 'app-message1',
  templateUrl: './message1.component.html'
})
export class Message1Component implements OnInit, OnDestroy {
  private onEchoedEvent: Subscription;
  private onProcessedOneWayCommandEvent: Subscription;
  public echoText: string;
  public requestText: string;
  public commandText: string;
  public syncResult: string;
  public asyncResult: string;
  public errorMessage: string;
  public hasErrors: boolean;
  public selectedCommand: Message1Commands;
  public get selectedCommandText(): string { return Message1Commands[this.selectedCommand]; }

  constructor(
    @Inject(Message1Client) private message1Client: Message1Client,
    @Inject(Message1HubClient) private message1HubClient: Message1HubClient,
    @Inject(EventAggregator) private eventAggregator: EventAggregator) {
    // subscribe on events
    this.onEchoedEvent = this.eventAggregator.listen('Message1OnEchoedResult').subscribe((eventData: EchoedEventData) => {
      this.asyncResult = eventData.result;
    });
    this.onProcessedOneWayCommandEvent = this.eventAggregator.listen('Message1OnProcessedOneWayCommandResult').subscribe((eventData: ProcessedOneWayCommandEventData) => {
      this.asyncResult = eventData.result;
    });
  }

  ngOnInit(): void {
    this.selectedCommand = Message1Commands.Echo;
    this.Clear();
    this.message1HubClient.startConnection();
  }

  ngOnDestroy(): void {
    this.onProcessedOneWayCommandEvent.unsubscribe();
    this.onEchoedEvent.unsubscribe();
    this.message1HubClient.stopConnection();
  }

  public selectCommand(newCommand: Message1Commands) {
    this.selectedCommand = newCommand;
    this.Clear();
  }

  public onSubmit() {
    this.Clear();

    switch (this.selectedCommand) {
      case Message1Commands.Echo:
        this.echo();
        break;
      case Message1Commands.RequestResponse:
        this.requestResponse();
        break;
      case Message1Commands.OneWay:
        this.oneWay();
        break;
    }
  }

  private echo() {
    let request = new EchoRequest({ input: this.echoText });

    this.message1Client
      .echo(request)
      .subscribe((response: string) => {
        this.syncResult = response;
      }, error => {
        this.errorMessage = error;
        this.hasErrors = true;
      });
  }

  private requestResponse() {
    let request = new RequestResponseRequest({ input: this.requestText });

    this.message1Client
      .requestResponse(request)
      .subscribe((response: string) => {
        this.syncResult = response;
      }, error => {
        this.errorMessage = error;
        this.hasErrors = true;
      });
  }

  private oneWay() {
    let request = new OneWayRequest({ input: this.requestText });

    this.message1Client
      .oneWay(request)
      .subscribe((response: string) => {
        this.syncResult = response;
      }, error => {
        this.errorMessage = error;
        this.hasErrors = true;
      });
  }

  private Clear() {
    this.hasErrors = false;
    this.syncResult = "";
    this.asyncResult = "";
  }
}

enum Message1Commands {
  Echo = 0,
  RequestResponse = 1,
  OneWay = 2
}
