using System.Collections;
using UnityEngine;

public class ServerDetails : MonoBehaviour
{
    public static string GameTypeName = "DodgeThis";
    public static string GameName = "MyGame";
    public static int Port = 25000;
    public static int MaxConnections = 16;
    public static int MapID = (int)ServerMaps.Map0;
    public static ServerMapMode MapMode = ServerMapMode.TeamDeathMatch;
    private static Hashtable ConnectionsTable = new Hashtable();
    private static int _RespawnTime = 3;
    public static int RespawnTime
    {
        get
        {
            return _RespawnTime;
        }
    }
    public static float MaxServerTime = 1800.0f;
    public static void Initialize()
    {
        MasterServer.ipAddress = "54.187.203.128";
        MasterServer.port = 23466;
        Network.natFacilitatorIP = "54.187.203.128";
        Network.natFacilitatorPort = 50005;
    }
    public static void CreateServer()
    {
        Network.InitializeServer(MaxConnections, Port, !Network.HavePublicAddress());
        MasterServer.updateRate = 0;
        MasterServer.RegisterHost(GameTypeName, GameName);
    }
    public static void ConnectToServer(HostData Host)
    {
        Network.Connect(Host);
    }
    public static void ConnectToServer(string IPAddress, int Port)
    {
        Network.Connect(IPAddress, Port);
    }
    public static void AddConnection(NetworkPlayer player, GameObject playerObject)
    {
        ConnectionsTable.Add(player, playerObject);
    }
    public static int TotalConnections()
    {
        return ConnectionsTable.Count;
    }
    public static void RemoveConnection(NetworkPlayer player)
    {
        if (ConnectionsTable.ContainsKey(player))
        {
            Destroy((GameObject)ConnectionsTable[player]);
            ConnectionsTable.Remove(player);
        }
    }
    public static bool HasPlayer(NetworkPlayer Player)
    {
        return ConnectionsTable.Contains(Player);
    }
    public static GameObject GetPlayer(NetworkPlayer Player)
    {
        return (GameObject)ConnectionsTable[Player];
    }
    public static void ClearConnections()
    {
        ConnectionsTable.Clear();
    }
    public static void Disconnect()
    {
        LevelLoader.Instance.LoadMain();

        if (Network.isServer)
        {
            MasterServer.UnregisterHost();
            Network.Disconnect();
        }
        else
        {
            Network.Disconnect();
        }
        ClearConnections();
    }
    public void SetMaxConnections(int ConnectionsValue)
    {
        if (ConnectionsValue > 16)
        {
            Debug.LogError("Cannot have more than 16 connections");
        }
        else
        {
            MaxConnections = ConnectionsValue;
        }
    }
}
public enum ServerMaps {Map0 = 2,Map1}

public enum ServerMapMode {FreeforAll = 0,TeamDeathMatch = 1}