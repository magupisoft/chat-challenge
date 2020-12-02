[![Build Status](https://travis-ci.org/magupisoft/jobsity-challenge.svg?branch=develop)](https://travis-ci.org/magupisoft/jobsity-challenge)


## Jobsity Code Challenge
- Authenticated users (.NET Identity) are able to chat in the chatroom. 
- The chat will load the last 50 messages
- Market Quotes can be requested to Chat-Bot using the following command format:
/stock=nio.us
- 

@Author Manuel Gutierrez Pineda
 
## Generate Database from Package Manager Console in Visual Studio  

> Update-Database -Context IdentityDbContext -StartupProject Jobsity.Chat.UI -Project Jobsity.Chat.DataContext
>  Update-Database -Context ChatDbContext -StartupProject Jobsity.Chat.UI -Project Jobsity.Chat.DataContext
Install and run RabbitMq with Docker

  
## Running local instance of RabbitMq using Docker
> docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

Confirm RabbitMq is running going to your browser and opening http://127.0.0.1:15672/

