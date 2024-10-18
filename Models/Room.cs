using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace SudokuFullGame.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string GamePin { get; set; }
        public Dictionary<string, string> Players { get; set; } = new Dictionary<string, string>();

    }
}
public class Room
{
    internal Dictionary<string, string> Players;
    public Room()
    {
        Players = new Dictionary<string, string>();
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string GamePin { get; set; }
    public Dictionary<string, string> Members { get; set; } = new Dictionary<string, string>();
    public bool IsFull => Members.Count >= 3;

    public string AddMember(string memberId)
    {
        if (IsFull) return null;
        string playerNumber = $"Player {Members.Count + 1}";
        Members[memberId] = playerNumber;
        return playerNumber;
    }

    public void UpdateMembers(Dictionary<string, string> newMembers)
    {
        Members.Clear();
        foreach (var member in newMembers)
        {
            Members.Add(member.Key, member.Value);
        }
    }
}
