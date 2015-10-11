using UnityEngine;
using UnityEngine.UI;

public class ServerList : MonoBehaviour
{
    private HostData[] ServerHostList = new HostData[0];

    [SerializeField]
    private GameObject ServerListItem;

    [SerializeField]
    private GameObject ServerListContent;

    public void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
        {
            ServerHostList = MasterServer.PollHostList();
            if (ServerHostList.Length > 0)
                UpdateServerList();
        }
    }

    public void UpdateServerList()
    {
        Debug.Log("Updating server list");
        ServerListContent.transform.ClearChildren();

        for (int i = 0; i < ServerHostList.Length; i++)
        {
            HostData HostGame = ServerHostList[i];

            GameObject SI = (GameObject)Instantiate(ServerListItem, Vector2.zero, Quaternion.identity);
            SI.transform.SetParent(ServerListContent.transform);
            SI.transform.localScale = Vector3.one;
            SI.GetComponentInChildren<Text>().text = HostGame.gameName.ToString();
            SI.GetComponent<Button>().onClick.AddListener(() => { ServerDetails.ConnectToServer(HostGame); });
        }
    }

    public void OnCanvasToggle(bool toggleValue)
    {
        if (toggleValue)
        {
            UpdateServerList();
        }
    }
}