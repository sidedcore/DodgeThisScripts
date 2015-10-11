using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkView))]
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    private NetworkView _NetworkView;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        _NetworkView = GetComponent<NetworkView>();
        _NetworkView.group = 10;
    }

    private void Start()
    {
        LoadMain();
    }

    public void LoadMain()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StopAllCoroutines();
        StartCoroutine(DoMainLoading());
    }

    private IEnumerator DoMainLoading()
    {
        Network.SetSendingEnabled(0, false);
        Network.isMessageQueueRunning = false;

        Network.SetLevelPrefix(0);
        Application.LoadLevel("Main");

        yield return null;
        yield return null;

        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);
    }

    public void LoadMap(int MapID)
    {
        StopAllCoroutines();
        StartCoroutine(DoMapLoading(MapID));
    }

    private IEnumerator DoMapLoading(int MapID)
    {
        Network.SetSendingEnabled(0, false);
        Network.isMessageQueueRunning = false;

        Network.SetLevelPrefix(MapID);
        Application.LoadLevel(MapID);

        yield return null;
        yield return null;

        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);

        GameLevelLoaded();

        if (Network.isClient)
            GameObject.FindObjectOfType<NetworkController>().GetComponent<NetworkView>().RPC("RequestConnection", RPCMode.Server);
    }

    public void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        ServerDetails.ClearConnections();
        LoadMain();
    }

    public void OnPlayerConnected(NetworkPlayer Player)
    {
        if (Network.isServer)
        {
            _NetworkView.RPC("SendServerDetails", Player, ServerDetails.MapID, (int)ServerDetails.MapMode, ServerDetails.MaxConnections);
        }
    }

    [RPC]
    public void SendServerDetails(int MapID, int MapMode, int MaxConnections, NetworkMessageInfo info)
    {
        ServerDetails.MaxConnections = MaxConnections;
        ServerDetails.MapID = MapID;
        ServerDetails.MapMode = (ServerMapMode)MapMode;

        LoadMap(ServerDetails.MapID);
    }

    private void GameLevelLoaded()
    {
        GameObject.FindObjectOfType<BallControl>().GameMapLoaded();
        GameObject.FindObjectOfType<NetworkController>().GameMapLoaded();
        if (Network.isClient)
            GameObject.FindObjectOfType<NetworkController>().mNetworkview.RPC("GetTrack", RPCMode.Server);
    }
}