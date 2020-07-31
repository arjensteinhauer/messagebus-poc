import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Message2Client, EchoRequest } from '../services/gatewayClient.service';
import { Message2HubClient, EchoedEventData } from '../services/gatewayHubClient.service';
import { EventAggregator } from '../services/eventAggregator'

@Component({
  selector: 'app-message2',
  templateUrl: './message2.component.html'
})
export class Message2Component implements OnInit, OnDestroy {
  private onEchoedEvent: Subscription;
  public echoText: string;
  public syncResult: string;
  public asyncResult: string;
  public errorMessage: string;
  public hasErrors: boolean;
  public selectedCommand: Message2Commands;
  public get selectedCommandText(): string { return Message2Commands[this.selectedCommand]; }

  constructor(
    @Inject(Message2Client) private message2Client: Message2Client,
    @Inject(Message2HubClient) private message2HubClient: Message2HubClient,
    @Inject(EventAggregator) private eventAggregator: EventAggregator) {
    // subscribe on events
    this.onEchoedEvent = this.eventAggregator.listen('Message2OnEchoedResult').subscribe((eventData: EchoedEventData) => {
      this.asyncResult = eventData.result;
    });
  }

  ngOnInit(): void {
    this.selectedCommand = Message2Commands.Echo;
    this.Clear();
    this.message2HubClient.startConnection();
  }

  ngOnDestroy(): void {
    this.onEchoedEvent.unsubscribe();
    this.message2HubClient.stopConnection();
  }

  public selectCommand(newCommand: Message2Commands) {
    this.selectedCommand = newCommand;
    this.Clear();
  }

  public onSubmit() {
    this.Clear();

    switch (this.selectedCommand) {
      case Message2Commands.Echo:
        this.echo();
        break;
    }
  }

  private echo() {
    let request = new EchoRequest({ input: this.echoText });

    this.message2Client
      .echo(request)
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

enum Message2Commands {
  Echo = 0
}
