using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    private bool DisplaySpawnPoints = false;
    public List<Transform> RedSpawnList = new List<Transform>();
    public List<Transform> BlueSpawnList = new List<Transform>();
    public List<Transform> FreeForAllSpawnList = new List<Transform>();

    public List<SpawnPlayer> toSpawnList = new List<SpawnPlayer>();

    [SerializeField]
    private LayerMask SpawnCheck = new LayerMask();

    private void Awake()
    {
        GetAllSpawnPoint();
    }

    public void GetAllSpawnPoint()
    {
        RedSpawnList.Clear();
        BlueSpawnList.Clear();
        FreeForAllSpawnList.Clear();
        GameObject[] SP = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < SP.Length; i++)
        {
            if (SP[i].GetComponent<SpawnPointElement>().SpawnTeamPoint == SpawnTeamID.Red)
            {
                RedSpawnList.Add(SP[i].transform);
                SP[i].transform.SetParent(this.transform.FindChild("SpawnPoints"));
            }
            else if (SP[i].GetComponent<SpawnPointElement>().SpawnTeamPoint == SpawnTeamID.Blue)
            {
                BlueSpawnList.Add(SP[i].transform);
                SP[i].transform.SetParent(this.transform.FindChild("SpawnPoints"));
            }
            else
            {
                FreeForAllSpawnList.Add(SP[i].transform);
                SP[i].transform.SetParent(this.transform.FindChild("SpawnPoints"));
            }
        }
    }

    public void ShowSpawnPoints()
    {
        DisplaySpawnPoints = !DisplaySpawnPoints;

        foreach (Transform SPE in transform.FindChild("SpawnPoints"))
        {
            SPE.GetComponent<SpawnPointElement>().SetEnabled(DisplaySpawnPoints);
        }
    }

    public Transform GetSpawnPoint()
    {
        int point = 0;
        for (int i = 0; i < FreeForAllSpawnList.Count; i++)
        {
            if (!Physics.CheckSphere(FreeForAllSpawnList[i].position, 5.0f, SpawnCheck))
            {
                point = i;
                break;
            }
        }
        return FreeForAllSpawnList[point].transform;
    }

    public Transform GetSpawnPoint(int TeamID, int TeamPosition)
    {
        if (TeamID == (int)SpawnTeamID.Red)
        {
            return RedSpawnList[TeamPosition].transform;
        }
        else
        {
            return BlueSpawnList[TeamPosition].transform;
        }
    }

    public void AddDeath(NetworkPlayer Player)
    {
        if (!Network.isServer) return;
        SpawnPlayer p = new SpawnPlayer();
        p._player = Player;
        p._deathTime = Time.time;

        toSpawnList.Add(p);
    }

    private void Update()
    {
        if (toSpawnList.Count > 0)
        {
            if (Time.time >= (toSpawnList[0]._deathTime + ServerDetails.RespawnTime))
            {
                Transform NewSpawn = GetSpawnPoint();

                GameObject.FindObjectOfType<NetworkController>().mNetworkview.RPC("PlayerRespawn", RPCMode.Server, toSpawnList[0]._player, NewSpawn.position, NewSpawn.rotation);

                Debug.Log("Spawn" + toSpawnList[0]._player.ToString());
                toSpawnList.RemoveAt(0);
            }
        }
    }

    [System.Serializable]
    public struct SpawnPlayer
    {
        public NetworkPlayer _player;
        public float _deathTime;
    }
}