import { Component, OnInit } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';
import { MessageDto } from 'src/app/dto/messageDto';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  public conversationMessages: MessageDto[] = [];
  public msgDto = new MessageDto();

  constructor(private chatService: ChatService) { }

  ngOnInit(): void {
    this.chatService.retrieveMessage().subscribe(
      (receivedMessage: MessageDto) => {
        this.addToDisplayedConversation(receivedMessage);
        this.msgDto.message = '';
      }
    );
  }

  private addToDisplayedConversation(msg: MessageDto) {
    console.log(msg);
    const newMsg = new MessageDto();
    newMsg.user = msg.user;
    newMsg.message = msg.message;
    this.conversationMessages.push(newMsg);
  }

  public sendMessage(): void {
    if (this.msgDto) {
      this.msgDto.user = 'Anonymous';
      if(this.msgDto.message.length === 0) {
        window.alert("Message is required");
        return;
      }
      console.log(this.msgDto);
      this.chatService.sendMessage(this.msgDto);
    }
  }
}
