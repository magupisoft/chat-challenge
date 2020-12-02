import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { MessageDto } from '../dto/MessageDto';
import { Observable, Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatService {

  private connSignalR: any = new signalR
    .HubConnectionBuilder()
    .withUrl("https://localhost:44389/chat-socket")
    .configureLogging(signalR.LogLevel.Information)
    .build();

  private SEND_URL = "https://localhost:44389/api/chat/send";
  private LOAD_URL = "https://localhost:44389/api/chat/load?numMsgs=50";

  private receivedMessageObject: MessageDto = new MessageDto();
  private sharedObservableSubject = new Subject<MessageDto>();

  constructor(private http: HttpClient) {
    this.connSignalR.onclose(async () => {
      await this.start();
    });

    this.connSignalR.on("ReceiveChatMessage", (user, message) => { this.loadReceivedMessage(user, message) });
    this.start();
  }

  private loadReceivedMessage(user: string, message: string) {
    this.receivedMessageObject.user = user;
    this.receivedMessageObject.message = message;
    console.log(this.receivedMessageObject);
    this.sharedObservableSubject.next(this.receivedMessageObject);
  }

  public sendMessage(msg: any) {
    console.log(msg);
    this.http.post(this.SEND_URL, msg).subscribe(data => console.log(data));
  }

  public loadMessages() {
    this.http.get(this.LOAD_URL).subscribe(data => console.log(data));
  }

  public retrieveMessage(): Observable<MessageDto> {
    return this.sharedObservableSubject.asObservable();
  }

  public async start() {
    try {
      await this.connSignalR.start();
    } catch (e) {
      console.log(e);
      setTimeout(() => this.start(), 8000);
    }
  }
}
