import { Component, OnInit } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';
import { MessageDto } from 'src/app/dto/MessageDto';

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
      }
    );
  }

  private addToDisplayedConversation(msg: MessageDto) {
    const newMsg = new MessageDto();
    newMsg.user = msg.user;
    newMsg.message = msg.message;
    this.conversationMessages.push(newMsg);
  }

  public sendMessage(): void {
    if (this.msgDto) {
      if(this.msgDto.message.length === 0 || this.msgDto.user.length === 0) {
        window.alert("User name and message are required");
        return;
      }

      this.chatService.sendMessage(this.msgDto);
      this.msgDto.message = '';
    }
  }
}
