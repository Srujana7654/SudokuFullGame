using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace SudokuFullGame.Services
{
    public class Room
    {
        public string Id { get; set; }
        public string GamePin { get; set; }
        public Dictionary<string, string> Players { get; set; } = new Dictionary<string, string>();
    }

    public class RoomService
    {
        private readonly IMongoCollection<Room> _rooms;

        public RoomService(MongoDBService mongoDBService)
        {
            _rooms = mongoDBService.GetCollection<Room>("Rooms");
        }

        public async Task<(Room room, string playerNumber)> CreateRoomAsync(string gamePin, string playerName)
        {
            var room = new Room { Id = Guid.NewGuid().ToString(), GamePin = gamePin };
            room.Players.Add("Player 1", playerName);

            await _rooms.InsertOneAsync(room);
            Console.WriteLine($"Room created: {gamePin}, First player: {playerName}");
            return (room, "Player 1");
        }

        public async Task<(bool success, string playerNumber)> JoinRoomAsync(string gamePin, string playerId, string playerName)
        {
            var filter = Builders<Room>.Filter.Eq(r => r.GamePin, gamePin);
            var room = await _rooms.Find(filter).FirstOrDefaultAsync();

            if (room != null)
            {
                string playerNumber = $"Player {room.Players.Count + 1}";
                room.Players.Add(playerNumber, playerName);

                var update = Builders<Room>.Update.Set(r => r.Players, room.Players);
                await _rooms.UpdateOneAsync(filter, update);

                Console.WriteLine($"Player joined: {gamePin}, {playerNumber}: {playerName}");
                return (true, playerNumber);
            }

            Console.WriteLine($"Failed to join room: {gamePin}");
            return (false, null);
        }

        public async Task<Room> GetRoomAsync(string gamePin)
        {
            var filter = Builders<Room>.Filter.Eq(r => r.GamePin, gamePin);
            return await _rooms.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> RemovePlayerFromRoomAsync(string gamePin, string playerNumber)
        {
            var filter = Builders<Room>.Filter.Eq(r => r.GamePin, gamePin);
            var room = await _rooms.Find(filter).FirstOrDefaultAsync();

            if (room != null)
            {
                bool removed = room.Players.Remove(playerNumber);
                //////////if (removed)
                {
                    if (room.Players.Count == 0)
                    {
                        await _rooms.DeleteOneAsync(filter);
                        Console.WriteLine($"Room removed: {gamePin}");
                    }
                    else
                    {
                        var update = Builders<Room>.Update.Set(r => r.Players, room.Players);
                        await _rooms.UpdateOneAsync(filter, update);
                    }
                    Console.WriteLine($"Player removed: {gamePin}, {playerNumber}");
                }
                return removed;
            }
            return false;
        }

        private string GenerateGamePin()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}