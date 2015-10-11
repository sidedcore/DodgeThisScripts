using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PowerUpControl : MonoBehaviour
{
    public Transform PickupTF;
    public List<PickUpSpawn> toSpawnList = new List<PickUpSpawn>();

    public int RespawnTimer = 5;

    private void Awake()
    {
    }

    private void Update()
    {
        if (Network.isServer)
        {
            if (toSpawnList.Count > 0)
            {
                if (Time.time >= (toSpawnList[0].PickupTime + RespawnTimer))
                {
                    for (int i = 0; i < PickupTF.childCount; i++)
                    {
                        if (PickupTF.GetChild(i).gameObject == toSpawnList[0].TargetPickup)
                        {
                            GetComponent<NetworkView>().RPC("EnablePickup", RPCMode.Others, i);
                        }
                    }

                    toSpawnList[0].TargetPickup.GetComponent<PickUpElement>().EnablePickup();
                    toSpawnList.RemoveAt(0);
                }
            }
        }
    }

    public void AddtoList(GameObject picked, GameObject target)
    {
        for (int i = 0; i < PickupTF.childCount; i++)
        {
            if (PickupTF.GetChild(i).gameObject == picked)
            {
                GetComponent<NetworkView>().RPC("HidePickUp", RPCMode.Others, i);
                if (picked.GetComponent<PickUpElement>().PT == PickUpType.Energy)
                {
                    target.GetComponent<PlayerControl>().MaxEnergy();
                    GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>().mNetworkview.RPC("MaxEnergy", target.GetComponent<PlayerControl>().mNetworkPlayer);
                }
                else
                {
                    target.GetComponent<PlayerControl>().MaxHealth();
                    GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>().mNetworkview.RPC("MaxHealth", target.GetComponent<PlayerControl>().mNetworkPlayer);
                }
            }
        }

        PickUpSpawn ps = new PickUpSpawn();
        ps.TargetPickup = picked;
        ps.PickupTime = Time.time;
        toSpawnList.Add(ps);
    }

    public void AddtoList(GameObject picked, float timed)
    {
        PickUpSpawn ps = new PickUpSpawn();
        ps.TargetPickup = picked;
        ps.PickupTime = timed;
        toSpawnList.Add(ps);
    }

    [System.Serializable]
    public struct PickUpSpawn
    {
        public GameObject TargetPickup;
        public float PickupTime;
    }

    [RPC]
    public void HidePickUp(int number)
    {
        Debug.Log("Hide child pick up: " + number);
        Debug.Log(PickupTF.GetChild(number).name);
        PickupTF.GetChild(number).GetComponent<PickUpElement>().DisablePickup();
    }

    [RPC]
    public void EnablePickup(int number)
    {
        PickupTF.GetChild(number).GetComponent<PickUpElement>().EnablePickup();
    }
}