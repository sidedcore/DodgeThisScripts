using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkEvents : MonoBehaviour 
{
    string DirectIP;
    int DirectPort;

    public InputField GameName;
    public InputField GameMaxPlayers;
    public InputField GamePort;

    public void CreateServer()
    {
        if (GameName.text.Length != 0)
            ServerDetails.GameName = GameName.text.ToString();
        else
            ServerDetails.GameName = "MyGame";
        if (GameMaxPlayers.text.Length > 0)
            ServerDetails.MaxConnections = int.Parse(GameMaxPlayers.text.ToString());
        if (GamePort.text.Length > 0)
            ServerDetails.Port = int.Parse(GamePort.text.ToString());        
        ServerDetails.CreateServer();
    }
    public void RefreshServerList()
    {
        MasterServer.RequestHostList(ServerDetails.GameTypeName);
    }
    public void ConnectDirect( )
    {
        ServerDetails.ConnectToServer(DirectIP, DirectPort);
    }
    public void SetDirectIP(string IPAddress )
    {
        DirectIP = IPAddress;
    }
    public void SetDirectPort(string Port)
    {
        DirectPort = int.Parse(Port);
    }
    public void Disconnect()
    {
        ServerDetails.Disconnect();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
