using UnityEngine;

public class InstantiateHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject LocalPlayerPrefab;

    [SerializeField]
    private GameObject NetworkPlayerPrefab;

    [SerializeField]
    private GameObject PlayerHUD;

    [SerializeField]
    private GameObject ServerHUD;

    public GameObject CreateNetworkPlayer(NetworkPlayer Player, NetworkViewID ViewID, Vector3 Position, Quaternion Rotation, int PlayerState, int BallType, int TeamID)
    {
        Debug.Log(Position);
        GameObject N = (GameObject)Instantiate(NetworkPlayerPrefab, Position, Rotation);
        N.GetComponent<PlayerControl>().mNetworkPlayer = Player;
        N.GetComponent<PlayerControl>().mNetworkview.viewID = ViewID;
        N.GetComponent<PlayerControl>().TeamID = TeamID;
        N.GetComponent<PlayerWeapon>().SetWeapon(BallType);

        //N.GetComponent<PlayerMovement>().SetPosition(Position);
        //N.GetComponent<PlayerMovement>().SetRotation(Rotation);
        //N.GetComponent<PlayerMovement>().Movement(0.1f, 0.1f, Position, Vector3.zero);

        Debug.Log(Position);
        return N;

    }

    public GameObject CreateLocalPlayer(NetworkPlayer Player, NetworkViewID ViewID, Vector3 Position, Quaternion Rotation)
    {
        Camera.main.gameObject.SetActive(false);

        GameObject L = (GameObject)Instantiate(LocalPlayerPrefab, Position, Rotation);
        L.GetComponent<PlayerControl>().mNetworkPlayer = Player;
        L.GetComponent<PlayerControl>().mNetworkview.viewID = ViewID;
        L.GetComponent<PlayerControl>().TeamID = 0;
        L.GetComponent<PlayerWeapon>().SetWeapon(1);

        L.GetComponent<PlayerMovement>().SetPosition(Position);
        L.GetComponent<PlayerMovement>().SetRotation(Rotation);
        CreatePlayerHUD(L);

        return L;
    }

    public void CreatePlayerHUD(GameObject Target)
    {
        GameObject HUD = (GameObject)Instantiate(PlayerHUD, Vector2.zero, Quaternion.identity);
        HUD.GetComponent<HUDCanvas>().TargetPlayer = Target;
    }

    public void CreateServerHUD()
    {
        GameObject HUD = (GameObject)Instantiate(ServerHUD, Vector2.zero, Quaternion.identity);
    }
}