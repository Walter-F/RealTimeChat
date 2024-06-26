﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using RealTimeChat.Models;
using System.Text.Json;

public interface IChatClient
{
    public Task ReceiveMessage(string userName, string message);
}

namespace RealTimeChat.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IDistributedCache _cache;
        public ChatHub(IDistributedCache cache) 
        {
            _cache = cache;
        }
        

        public IDistributedCache Cache { get; }

        public async Task JoinChat(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.chatRoom);

            // Кэш хранится в виде Json-формата
            var stringConnection = JsonSerializer.Serialize(connection);

            // Добавление данных о соединении в кэш по ключу ConnectionId из контекста Hub
            await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

            await Clients
                .Group(connection.chatRoom)
                .ReceiveMessage("Система", $"{connection.userName} присоединился к чату");
        }

        public async Task SendMessage(string message)
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);

            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
            
            if(connection is not null)
            {
                await Clients
                    .Group(connection.chatRoom)
                    .ReceiveMessage(connection.userName, message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (connection is not null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.chatRoom);

                await Clients
                    .Group(connection.chatRoom)
                    .ReceiveMessage("Система", $"{connection.userName} отсоединяется от чата");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
