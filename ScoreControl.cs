using System.Collections.Generic;
using UnityEngine;

public class ScoreControl : MonoBehaviour
{
    public static ScoreControl Instance;

    private Dictionary<NetworkPlayer, Dictionary<string, int>> Scoreboard = new Dictionary<NetworkPlayer, Dictionary<string, int>>();

    private Dictionary<string, Dictionary<string, int>> ScoreDict = new Dictionary<string, Dictionary<string, int>>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddPlayer(NetworkPlayer Player)
    {
    }

    private void SetScore(NetworkPlayer Player, string Key, int Value)
    {
        if (!Scoreboard.ContainsKey(Player))
        {
            Scoreboard[Player] = new Dictionary<string, int>();
        }
        Scoreboard[Player][Key] = Value;
    }

    public int GetScore(NetworkPlayer Player, string Key)
    {
        if (!Scoreboard.ContainsKey(Player))
        {
            return 0;
        }
        if (!Scoreboard[Player].ContainsKey(Key))
        {
            return 0;
        }
        return Scoreboard[Player][Key];
    }

    //[RPC]
    //public void AddPlayer( string Username )
    //{
    //    SetScore(Username, "Kills", 0);
    //    SetScore(Username, "Deaths", 0);
    //}
    //[RPC]
    //public void AddKill(string Username)
    //{
    //    int CurrentKills = GetScore(Username, "Kills");
    //    SetScore(Username, "Kills", CurrentKills + 1);
    //}
    //[RPC]
    //public void AddDeath(string Username)
    //{
    //    int CurrentDeaths = GetScore(Username, "Deaths");
    //    SetScore(Username, "Deaths", CurrentDeaths + 1);
    //}
    //private void SetScore( string Username, string ScoreType, int Value)
    //{
    //    if( ScoreDict.ContainsKey(Username) == false)
    //    {
    //        ScoreDict[Username] = new Dictionary<string, int>();
    //    }
    //    ScoreDict[Username][ScoreType] = Value;
    //}
    //private int GetScore(string Username, string ScoreType)
    //{
    //    if (ScoreDict.ContainsKey(Username) == false)
    //    {
    //        return 0;
    //    }
    //    if (ScoreDict[Username].ContainsKey(ScoreType) == false)
    //    {
    //        return 0;
    //    }
    //    return ScoreDict[Username][ScoreType];
    //}
}