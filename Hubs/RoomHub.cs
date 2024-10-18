using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SudokuFullGame.Hubs
{
    public class RoomHub : Hub
    {
        private static Dictionary<string, RoomState> Rooms = new Dictionary<string, RoomState>();

        public async Task JoinRoom(string gamePin, string playerName, string playerNumber)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gamePin);
            if (!Rooms.ContainsKey(gamePin))
            {
                Rooms[gamePin] = new RoomState();
            }
            Rooms[gamePin].Players[playerNumber] = new PlayerState { Name = playerName };
            await Clients.Group(gamePin).SendAsync("PlayerJoined", playerName, playerNumber);
            await Clients.Group(gamePin).SendAsync("ReceiveMembersUpdate", Rooms[gamePin].Players);
        }

        public async Task LeaveRoom(string gamePin, string playerNumber)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gamePin);
            if (Rooms.ContainsKey(gamePin))
            {
                Rooms[gamePin].Players.Remove(playerNumber);
                if (Rooms[gamePin].Players.Count == 0)
                {
                    Rooms.Remove(gamePin);
                }
                else
                {
                    await Clients.Group(gamePin).SendAsync("PlayerLeft", playerNumber);
                    await Clients.Group(gamePin).SendAsync("ReceiveMembersUpdate", Rooms[gamePin].Players);
                }
            }
        }

        public async Task StartGame(string gamePin)
        {
            if (Rooms.ContainsKey(gamePin) && !Rooms[gamePin].IsGameStarted)
            {
                Rooms[gamePin].IsGameStarted = true;
                foreach (var player in Rooms[gamePin].Players.Values)
                {
                    player.Board = GenerateRandomSudoku();
                }
                await Clients.Group(gamePin).SendAsync("GameStarted");
            }
            else
            {
                await Clients.Caller.SendAsync("ErrorMessage", "Unable to start the game. It may have already started or the room doesn't exist.");
            }
        }

        public async Task UpdateCell(string gamePin, string playerNumber, int row, int col, int value)
        {
            if (Rooms.ContainsKey(gamePin) && Rooms[gamePin].Players.ContainsKey(playerNumber))
            {
                Rooms[gamePin].Players[playerNumber].Board[row, col] = value;
                await Clients.Caller.SendAsync("CellUpdated", row, col, value);
            }
        }

        public async Task UpdateScore(string gamePin, string playerNumber, int score)
        {
            if (Rooms.ContainsKey(gamePin) && Rooms[gamePin].Players.ContainsKey(playerNumber))
            {
                Rooms[gamePin].Players[playerNumber].Score = score;
                await Clients.Group(gamePin).SendAsync("ScoreUpdated", playerNumber, score);
            }
        }

        private int[,] GenerateRandomSudoku()
        {
           
            return new int[9, 9];
        }

        public bool CheckGameStatus(string gamePin)
        {
            return Rooms.ContainsKey(gamePin) && Rooms[gamePin].IsGameStarted;
        }
    }

    public class RoomState
    {
        public Dictionary<string, PlayerState> Players { get; set; } = new Dictionary<string, PlayerState>();
        public bool IsGameStarted { get; set; } = false;
    }

    public class PlayerState
    {
        public string Name { get; set; }
        public int[,] Board { get; set; }
        public int Score { get; set; }
    }
}