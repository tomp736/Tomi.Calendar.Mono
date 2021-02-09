# Tomi.Calendar.Mono
Playground for learning blazor by writing a calendar/scheduler/notetaking app.

Using 
- Blazor Wasm
- Fluxor for Centralized State
- Blazored Components (Modal, LocalStorage, TextEditor)
- SignalR for server push Desktop Notification
- gRPC-Web for unary server-client calls.

![alt text](https://github.com/tomp736/Tomi.Calendar.Mono/blob/master/FooBar%20Calendar.png?raw=true)


## Getting Started

- Install DockerDesktop
- Create volume for postgres 'docker volume create --name pgdata'
- Run docker-compose up
- F5 Launch Tomi.Calendar.Mono.Server
- When prompted in browser - apply migrations
