using UnityEngine;

[RequireComponent(typeof(NetworkView))]
public class NetworkController : MonoBehaviour
{
    public NetworkView mNetworkview;
    public GameObject LocalPlayer;

    public Vector3 LastPosition = Vector3.zero;
    public Quaternion LastRotation = new Quaternion();

    private InstantiateHelper mSpawnHelper;

    public GameObject mLevelController;
    public HUDCanvas pHUD;

    public bool Respawn = false;
    public float RespawnTimer = 0f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        mNetworkview = this.GetComponent<NetworkView>();
        Network.sendRate = 25f;
        mSpawnHelper = GetComponent<InstantiateHelper>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        switch (msEvent)
        {
            case MasterServerEvent.HostListReceived:
                break;

            case MasterServerEvent.RegistrationFailedGameName:
                break;

            case MasterServerEvent.RegistrationFailedGameType:
                break;

            case MasterServerEvent.RegistrationFailedNoServer:
                break;

            case MasterServerEvent.RegistrationSucceeded:
                LevelLoader.Instance.LoadMap(ServerDetails.MapID);
                break;
        }
    }

    public void OnPlayerDisconnected(NetworkPlayer player)
    {
        mNetworkview.RPC("PlayerDisconnect", RPCMode.All, player);
    }

    public void GameMapLoaded()
    {
        if (Network.isServer)
            mSpawnHelper.CreateServerHUD();

        mLevelController = GameObject.FindGameObjectWithTag("LevelController");
    }

    public void OnApplicationQuit()
    {
    }

    private void Update()
    {
        if (LocalPlayer)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                LocalPlayer.GetComponent<PlayerControl>().CameraControl = !LocalPlayer.GetComponent<PlayerControl>().CameraControl;
            }
            if (LocalPlayer.GetComponent<PlayerControl>().pState == PlayerState.Alive)
            {
                if (Network.peerType == NetworkPeerType.Client)
                {
                    LocalWeaponThrowControl();

                    LocalMovementControl();

                    LocalWeaponScrollControl();
                }
            }
        }
        //if (Input.GetKeyUp(KeyCode.Keypad0) && LocalPlayer)
        //{
        //    mNetworkview.RPC("PlayerDead", RPCMode.Server, LocalPlayer.GetComponent<PlayerControl>().mNetworkPlayer);
        //}
    }

    private void LocalWeaponThrowControl()
    {
        if (LocalPlayer.GetComponent<PlayerWeapon>().wState == WeaponState.idle && Input.GetButtonDown("Fire1"))
        {
            if (checkEnergyCost)
            {
                mNetworkview.RPC("UpdateServerCharge", RPCMode.Server);
            }
        }
        if (LocalPlayer.GetComponent<PlayerWeapon>().wState == WeaponState.charging && Input.GetButtonUp("Fire1"))
        {
            LocalPlayer.GetComponent<PlayerWeapon>().ReleaseThrow();
            mNetworkview.RPC("UpdateServerRelease", RPCMode.Server, LocalPlayer.GetComponent<PlayerControl>().LookDirection(), LocalPlayer.GetComponent<PlayerWeapon>().ChargingForce);
        }

        pHUD.SetCharge();
    }

    private bool checkEnergyCost
    {
        get
        {
            int amount = LocalPlayer.GetComponent<PlayerControl>().baseEnergy;
            bool Allowed = false;

            switch (LocalPlayer.GetComponent<PlayerWeapon>().Current)
            {
                case (int)BallType.Dodge:
                    Allowed = amount > 10 ? true : false;
                    break;

                case (int)BallType.Spike:
                    Allowed = amount > 15 ? true : false;
                    break;

                case (int)BallType.Sticky:
                    Allowed = amount > 20 ? true : false;
                    break;

                case (int)BallType.Grenade:
                    Allowed = amount > 15 ? true : false;
                    break;

                case (int)BallType.Rocket:
                    Allowed = amount > 30 ? true : false;
                    break;

                case (int)BallType.Spanner:
                    Allowed = amount > 40 ? true : false;
                    break;
            }
            return Allowed;
        }
    }

    private void LocalWeaponScrollControl()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            LocalPlayer.GetComponent<PlayerWeapon>().NextWeapon();
            mNetworkview.RPC("UpdateServerWeapon", RPCMode.Server, LocalPlayer.GetComponent<PlayerWeapon>().GetCurrentBall());
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            LocalPlayer.GetComponent<PlayerWeapon>().PreviousWeapon();
            mNetworkview.RPC("UpdateServerWeapon", RPCMode.Server, LocalPlayer.GetComponent<PlayerWeapon>().GetCurrentBall());
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LocalPlayer.GetComponent<PlayerWeapon>().SetWeapon((int)BallType.Dodge);
            mNetworkview.RPC("UpdateServerWeapon", RPCMode.Server, LocalPlayer.GetComponent<PlayerWeapon>().GetCurrentBall());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LocalPlayer.GetComponent<PlayerWeapon>().SetWeapon((int)BallType.Spike);
            mNetworkview.RPC("UpdateServerWeapon", RPCMode.Server, LocalPlayer.GetComponent<PlayerWeapon>().GetCurrentBall());
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LocalPlayer.GetComponent<PlayerWeapon>().SetWeapon((int)BallType.Sticky);
            mNetworkview.RPC("UpdateServerWeapon", RPCMode.Server, LocalPlayer.GetComponent<PlayerWeapon>().GetCurrentBall());
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LocalPlayer.GetComponent<PlayerWeapon>().SetWeapon((int)BallType.Grenade);
            mNetworkview.RPC("UpdateServerWeapon", RPCMode.Server, LocalPlayer.GetComponent<PlayerWeapon>().GetCurrentBall());
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            LocalPlayer.GetComponent<PlayerWeapon>().SetWeapon((int)BallType.Rocket);
            mNetworkview.RPC("UpdateServerWeapon", RPCMode.Server, LocalPlayer.GetComponent<PlayerWeapon>().GetCurrentBall());
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            LocalPlayer.GetComponent<PlayerWeapon>().SetWeapon((int)BallType.Spanner);
            mNetworkview.RPC("UpdateServerWeapon", RPCMode.Server, LocalPlayer.GetComponent<PlayerWeapon>().GetCurrentBall());
        }
    }

    private void LocalMovementControl()
    {
        if (LocalPlayer.transform.rotation != LastRotation)
        {
            mNetworkview.RPC("UpdateServerRotation", RPCMode.Server, LocalPlayer.transform.rotation);
            LastRotation = LocalPlayer.transform.rotation;
        }

        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        LocalPlayer.GetComponent<PlayerMovement>().Movement(Horizontal, Vertical);
        if (LastPosition != LocalPlayer.GetComponent<Rigidbody>().position)
        {
            mNetworkview.RPC("UpdateServerMovement", RPCMode.Server, Horizontal, Vertical, LastPosition, LocalPlayer.GetComponent<Rigidbody>().velocity);

            LastPosition = LocalPlayer.GetComponent<Rigidbody>().position;
        }

        if (Input.GetButton("Jump"))
        {
            LocalPlayer.GetComponent<PlayerMovement>().Jump();
            mNetworkview.RPC("UpdateServerJump", RPCMode.Server);
        }
    }

    [RPC]
    public void UpdateServerCharge(NetworkMessageInfo info)
    {
        if (ServerDetails.HasPlayer(info.sender))
        {
            GameObject P = ServerDetails.GetPlayer(info.sender);
            P.GetComponent<PlayerWeapon>().ChargeThrow();
            mNetworkview.RPC("UpdateClientCharge", RPCMode.Others, info.sender);
        }
    }

    [RPC]
    public void UpdateClientCharge(NetworkPlayer Player, NetworkMessageInfo info)
    {
        if (Network.peerType == NetworkPeerType.Client)
            //if (Player != LocalPlayer.GetComponent<PlayerControl>().mNetworkPlayer)
            if (ServerDetails.HasPlayer(Player))
            {
                GameObject P = ServerDetails.GetPlayer(Player);
                P.GetComponent<PlayerWeapon>().ChargeThrow();
            }
    }

    [RPC]
    public void UpdateServerRelease(Vector3 LookDir, float ChargeForce, NetworkMessageInfo info)
    {
        if (ServerDetails.HasPlayer(info.sender))
        {
            GameObject P = ServerDetails.GetPlayer(info.sender);
            //P.GetComponent<PlayerWeapon>().ChargingForce = ChargeForce;
            //P.GetComponent<PlayerWeapon>().ReleaseThrow(LookDir);
            if (info.sender != null)
            {
                mNetworkview.RPC("UpdateClientRelease", RPCMode.All, info.sender, LookDir, ChargeForce);
            }
        }
    }

    [RPC]
    public void UpdateClientRelease(NetworkPlayer Player, Vector3 LookDir, float ChargeForce, NetworkMessageInfo info)
    {
        if (Network.peerType == NetworkPeerType.Client)
            //if (Player != LocalPlayer.GetComponent<PlayerControl>().mNetworkPlayer)
            if (ServerDetails.HasPlayer(Player))
            {
                GameObject P = ServerDetails.GetPlayer(Player);
                P.GetComponent<PlayerWeapon>().ChargingForce = ChargeForce;
                P.GetComponent<PlayerWeapon>().ReleaseThrow(LookDir);
            }
        if (Network.peerType == NetworkPeerType.Server)
            if (ServerDetails.HasPlayer(Player))
            {
                GameObject P = ServerDetails.GetPlayer(Player);
                P.GetComponent<PlayerWeapon>().ChargingForce = ChargeForce;
                P.GetComponent<PlayerWeapon>().ReleaseThrow(LookDir);
            }
    }

    [RPC]
    public void UpdateServerJump(NetworkMessageInfo info)
    {
        if (ServerDetails.HasPlayer(info.sender))
        {
            GameObject P = ServerDetails.GetPlayer(info.sender);
            P.GetComponent<Animator>().SetBool("Jump", true);
            if (info.sender != null)
            {
                mNetworkview.RPC("UpdateClientJump", RPCMode.Others, info.sender);
            }
        }
    }

    [RPC]
    public void UpdateClientJump(NetworkPlayer Player, NetworkMessageInfo info)
    {
        if (Network.peerType == NetworkPeerType.Client)
            if (Player != LocalPlayer.GetComponent<PlayerControl>().mNetworkPlayer)
                if (ServerDetails.HasPlayer(Player))
                {
                    GameObject P = ServerDetails.GetPlayer(Player);
                    P.GetComponent<Animator>().SetBool("Jump", true);
                }
    }

    [RPC]
    public void UpdateServerRotation(Quaternion RotationX, NetworkMessageInfo info)
    {
        if (ServerDetails.HasPlayer(info.sender))
        {
            GameObject P = ServerDetails.GetPlayer(info.sender);
            P.GetComponent<PlayerMovement>().SetRotation(RotationX);
            if (info.sender != null)
            {
                mNetworkview.RPC("UpdateClientRotation", RPCMode.Others, info.sender, RotationX);
            }
        }
    }

    [RPC]
    public void UpdateClientRotation(NetworkPlayer Player, Quaternion RotationX, NetworkMessageInfo info)
    {
        if (Network.peerType == NetworkPeerType.Client)
            if (Player != LocalPlayer.GetComponent<PlayerControl>().mNetworkPlayer)
                if (ServerDetails.HasPlayer(Player))
                {
                    GameObject P = ServerDetails.GetPlayer(Player);
                    P.GetComponent<PlayerMovement>().SetRotation(RotationX);
                }
    }

    [RPC]
    public void UpdateServerMovement(float Horizontal, float Vertical, Vector3 LastPosition, Vector3 LastVelocity, NetworkMessageInfo info)
    {
        if (ServerDetails.HasPlayer(info.sender))
        {
            GameObject P = ServerDetails.GetPlayer(info.sender);
            P.GetComponent<PlayerMovement>().Movement(Horizontal, Vertical, LastPosition, LastVelocity);
            if (info.sender != null)
            {
                mNetworkview.RPC("UpdateClientMovement", RPCMode.Others, info.sender, Horizontal, Vertical, LastPosition, LastVelocity);
            }
        }
    }

    [RPC]
    public void UpdateClientMovement(NetworkPlayer Player, float Horizontal, float Vertical, Vector3 LastPosition, Vector3 LastVelocity, NetworkMessageInfo info)
    {
        if (Network.peerType == NetworkPeerType.Client)
            if (Player != LocalPlayer.GetComponent<PlayerControl>().mNetworkPlayer)
                if (ServerDetails.HasPlayer(Player))
                {
                    GameObject P = ServerDetails.GetPlayer(Player);
                    P.GetComponent<PlayerMovement>().Movement(Horizontal, Vertical, LastPosition, LastVelocity);
                }
    }

    [RPC]
    public void UpdateServerWeapon(int ballType, NetworkMessageInfo info)
    {
        if (ServerDetails.HasPlayer(info.sender))
        {
            GameObject P = ServerDetails.GetPlayer(info.sender);
            P.GetComponent<PlayerWeapon>().SetWeapon(ballType);
            if (info.sender != null)
            {
                mNetworkview.RPC("UpdateClientWeapon", RPCMode.Others, info.sender, ballType);
            }
        }
    }

    [RPC]
    public void UpdateClientWeapon(NetworkPlayer Player, int ballType, NetworkMessageInfo info)
    {
        if (Network.peerType == NetworkPeerType.Client)
            if (Player != LocalPlayer.GetComponent<PlayerControl>().mNetworkPlayer)
                if (ServerDetails.HasPlayer(Player))
                {
                    GameObject P = ServerDetails.GetPlayer(Player);
                    P.GetComponent<PlayerWeapon>().SetWeapon(ballType);
                }
    }

    [RPC]
    public void RequestConnection(NetworkMessageInfo info)
    {
        if (!Network.isServer) return;

        NetworkViewID ViewID = Network.AllocateViewID();

        Transform SP = mLevelController.GetComponent<SpawnControl>().GetSpawnPoint();

        GameObject NetworkPlayer = mSpawnHelper.CreateNetworkPlayer(info.sender, ViewID, SP.position, SP.rotation, (int)PlayerState.Creating, 1, 0);
        Debug.Log(NetworkPlayer.transform.position);
        ServerDetails.AddConnection(info.sender, NetworkPlayer);
        if (info.sender != null)
        {
            mNetworkview.RPC("ReceiveConnectionDetails", info.sender, info.sender, ViewID, SP.position, SP.rotation);
        }
    }

    [RPC]
    public void ReceiveConnectionDetails(NetworkPlayer Player, NetworkViewID ViewID, Vector3 Position, Quaternion Rotation)
    {
        if (!Network.isClient) return;

        LocalPlayer = mSpawnHelper.CreateLocalPlayer(Player, ViewID, Position, Rotation);
        mNetworkview.RPC("ReceiveNewPlayer", RPCMode.Others, Player, ViewID, Position, Rotation);

        pHUD = GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<HUDCanvas>();
        ServerDetails.AddConnection(Player, LocalPlayer);
        mNetworkview.RPC("GetOtherPlayers", RPCMode.Server);
        LastPosition = LocalPlayer.GetComponent<PlayerMovement>().GetPosition();
        mNetworkview.RPC("PlayerAlive", RPCMode.Server, Player);
    }

    [RPC]
    public void ReceiveNewPlayer(NetworkPlayer Player, NetworkViewID ViewID, Vector3 Position, Quaternion Rotation)
    {
        if (Network.isServer) return;

        GameObject NetworkPlayer = mSpawnHelper.CreateNetworkPlayer(Player, ViewID, Position, Rotation, 0, 1, 0);
        ServerDetails.AddConnection(Player, NetworkPlayer);
    }

    [RPC]
    public void ReceiveExistingPlayer(NetworkPlayer Player, NetworkViewID ViewID, Vector3 Position, Quaternion Rotation, int State, int teamID, int ballType)
    {
        if (Network.isServer) return;

        Debug.Log("Existing player at position: " + Position);
        GameObject NetworkPlayer = mSpawnHelper.CreateNetworkPlayer(Player, ViewID, Position, Rotation, State, ballType, teamID);

        Debug.Log(" Existing Player: " + Player);
        ServerDetails.AddConnection(Player, NetworkPlayer);
    }

    [RPC]
    public void GetOtherPlayers(NetworkMessageInfo info)
    {
        if (!Network.isServer) return;

        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < Players.Length; i++)
        {
            NetworkPlayer player = Players[i].GetComponent<PlayerControl>().mNetworkPlayer;
            NetworkViewID ViewID = Players[i].GetComponent<PlayerControl>().mNetworkview.viewID;
            Vector3 Position = Players[i].GetComponent<PlayerMovement>().GetPosition();
            Quaternion Rotation = Players[i].GetComponent<PlayerMovement>().GetRotation();

            int state = Players[i].GetComponent<PlayerControl>().GetState();
            int teamID = Players[i].GetComponent<PlayerControl>().TeamID;
            int ballType = Players[i].GetComponent<PlayerWeapon>().GetCurrentBall();

            if (player.ToString() != info.sender.ToString())
            {
                if (info.sender != null)
                {
                    mNetworkview.RPC("ReceiveExistingPlayer", info.sender, player, ViewID, Position, Rotation, state, teamID, ballType);
                }
            }
        }
    }

    [RPC]
    public void PlayerDisconnect(NetworkPlayer player, NetworkMessageInfo info)
    {
        mLevelController.GetComponent<NetworkView>().RPC("RemoveBalls", RPCMode.All, player);

        ServerDetails.RemoveConnection(player);
    }

    [RPC]
    public void PlayerAlive(NetworkPlayer Player, NetworkMessageInfo info)
    {
        if (Network.isServer)
            if (ServerDetails.HasPlayer(info.sender))
            {
                GameObject P = ServerDetails.GetPlayer(info.sender);
                P.GetComponent<PlayerControl>().pState = PlayerState.Alive;
                if (info.sender != null)
                {
                    mNetworkview.RPC("PlayerAlive", RPCMode.Others, info.sender);
                }
            }

        if (Network.isClient)
            if (ServerDetails.HasPlayer(Player))
            {
                GameObject P = ServerDetails.GetPlayer(Player);
                P.GetComponent<PlayerControl>().pState = PlayerState.Alive;
            }
    }

    [RPC]
    public void PlayerDead(NetworkPlayer Player, NetworkMessageInfo info)
    {
        if (Network.isServer)
            if (ServerDetails.HasPlayer(Player))
            {
                GameObject P = ServerDetails.GetPlayer(Player);
                P.GetComponent<PlayerControl>().pState = PlayerState.Dead;
                mLevelController.GetComponent<SpawnControl>().AddDeath(Player);
                mNetworkview.RPC("PlayerDead", RPCMode.Others, Player);
            }

        if (Network.isClient)
            if (ServerDetails.HasPlayer(Player))
            {
                GameObject P = ServerDetails.GetPlayer(Player);
                P.GetComponent<PlayerControl>().pState = PlayerState.Dead;
            }
    }

    [RPC]
    public void PlayerRespawn(NetworkPlayer Player, Vector3 Position, Quaternion Rotation, NetworkMessageInfo info)
    {
        if (Network.isServer)
            if (ServerDetails.HasPlayer(info.sender))
            {
                Debug.Log("Server Has player");
                GameObject P = ServerDetails.GetPlayer(info.sender);
                P.GetComponent<PlayerMovement>().SetPosition(Position);
                P.GetComponent<PlayerMovement>().SetRotation(Rotation);
                P.GetComponent<PlayerMovement>().Movement(0.1f, 0.1f, Position, Vector3.zero);
                P.GetComponent<PlayerControl>().pState = PlayerState.Alive;
                mNetworkview.RPC("PlayerRespawn", RPCMode.Others, info.sender, Position, Rotation);
            }

        if (Network.isClient)
            if (ServerDetails.HasPlayer(Player))
            {
                LastPosition = Position;
                LastRotation = Rotation;
                GameObject P = ServerDetails.GetPlayer(Player);
                P.GetComponent<PlayerMovement>().SetPosition(Position);
                P.GetComponent<PlayerMovement>().SetRotation(Rotation);
                P.GetComponent<PlayerControl>().pState = PlayerState.Alive;
            }
    }

    [RPC]
    public void SetHealth(int amount, NetworkMessageInfo info)
    {
        pHUD.SetHealth(amount);
        LocalPlayer.GetComponent<PlayerControl>().baseHealth = amount;
    }

    [RPC]
    public void SetEnergy(int amount, NetworkMessageInfo info)
    {
        pHUD.SetEnergy(amount);
        LocalPlayer.GetComponent<PlayerControl>().baseEnergy = amount;
        //LocalPlayer.GetComponent<PlayerControl>().baseEnergy -= amount;
        //pHUD.SetEnergy(LocalPlayer.GetComponent<PlayerControl>().baseEnergy);
    }

    [RPC]
    public void MaxEnergy()
    {
        pHUD.SetEnergy(100);
        LocalPlayer.GetComponent<PlayerControl>().MaxEnergy();
    }

    [RPC]
    public void MaxHealth()
    {
        pHUD.SetHealth(100);
        LocalPlayer.GetComponent<PlayerControl>().MaxHealth();
    }
}