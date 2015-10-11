using System.Collections.Generic;
using UnityEngine;

public class TeamControl : MonoBehaviour
{
    public List<GameObject> RedTeam = new List<GameObject>();
    public List<GameObject> BlueTeam = new List<GameObject>();

    public void AddPlayer(int TeamID, GameObject Player)
    {
        if (TeamID == 1)
            if (RedTeam.Contains(Player)) return;
            else RedTeam.Add(Player);
        else
            if (BlueTeam.Contains(Player)) return;
            else BlueTeam.Add(Player);
    }

    public void RemovePlayer(int TeamID, GameObject Player)
    {
        if (TeamID == 1)
            if (!RedTeam.Contains(Player)) return;
            else RedTeam.Remove(Player);
        else
            if (!BlueTeam.Contains(Player)) return;
            else BlueTeam.Remove(Player);
    }

    public void ChangeTeam(int PreviousTeam, int NewTeamID, GameObject Player)
    {
        RemovePlayer(PreviousTeam, Player);
        AddPlayer(NewTeamID, Player);
    }
}

public enum SpawnTeamID
{
    FreeForAll = 0,
    Red = 1,
    Blue = 2,
}