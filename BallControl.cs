using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public List<GameObject> BallsInMotion = new List<GameObject>();

    public GameObject[] BallBag = new GameObject[6];
    public LayerMask ExplosionMask = new LayerMask();

    public float ExplodeRadius = 5.0f;
    public float ExplodeForce = 7.0f;

    private NetworkView mNetworkView;

    private void Awake()
    {
        mNetworkView = GetComponent<NetworkView>();
    }

    public void CreateBall(int BallType, float ThrowForce, Vector3 ThrowDirection, Vector3 ThrownFrom, NetworkPlayer BelongsTo)
    {
        Debug.Log(BelongsTo);

        GameObject Ball = (GameObject)Instantiate(BallBag[--BallType], ThrownFrom, Quaternion.Euler(ThrowDirection));
        if (Network.isServer)
        {
            GameObject target = ServerDetails.GetPlayer(BelongsTo);
            if (target.GetComponent<PlayerControl>().baseEnergy > 0)
            {
                target.GetComponent<PlayerControl>().DecreaseEnergy(Ball.GetComponent<DodgeBall>().BaseEnergyCost);
                GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>().mNetworkview.RPC("SetEnergy", target.GetComponent<PlayerControl>().mNetworkPlayer, target.GetComponent<PlayerControl>().baseEnergy);
            }
            else
            {
                GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>().mNetworkview.RPC("SetEnergy", target.GetComponent<PlayerControl>().mNetworkPlayer, 0);
            }
        }

        Ball.GetComponent<DodgeBall>().Launch(ThrowDirection, ThrowForce, BelongsTo);
        Ball.transform.SetParent(transform.FindChild("BallsInMotion"));

        BallsInMotion.Add(Ball);
    }

    public void CreateBall(int Balltype, Vector3 CurrentVelocity, Vector3 CurrentPosition, Quaternion CurrentRotation, NetworkPlayer owner)
    {
        GameObject Ball = (GameObject)Instantiate(BallBag[--Balltype]);
        Ball.GetComponent<Rigidbody>().velocity = CurrentVelocity;
        Ball.transform.position = CurrentPosition;
        Ball.transform.rotation = CurrentRotation;

        Ball.GetComponent<DodgeBall>().Owner = owner;

        Ball.transform.SetParent(transform.FindChild("BallsInMotion"));

        BallsInMotion.Add(Ball);
    }

    [RPC]
    private void GetBallsInMotion(NetworkMessageInfo info)
    {
        for (int i = 0; i < BallsInMotion.Count; i++)
        {
            GameObject _ball = BallsInMotion[i];
            mNetworkView.RPC("ReceiveBallsInMotion", info.sender, (int)_ball.GetComponent<DodgeBall>().mBallType, _ball.GetComponent<Rigidbody>().velocity, _ball.transform.position, _ball.transform.rotation, _ball.GetComponent<DodgeBall>().Owner);
        }
    }

    public void GameMapLoaded()
    {
        if (Network.isClient)
        {
            mNetworkView.RPC("GetBallsInMotion", RPCMode.Server);
        }
    }

    [RPC]
    public void ReceiveBallsInMotion(int BallType, Vector3 Velocity, Vector3 Position, Quaternion Rotation, NetworkPlayer owner, NetworkMessageInfo info)
    {
        try
        {
            CreateBall(BallType, Velocity, Position, Rotation, owner);
        }
        catch (System.Exception e) { }
    }

    public void DestroyBall(GameObject ToDestroy)
    {
        for (int i = 0; i < BallsInMotion.Count; i++)
        {
            if (BallsInMotion[i] == ToDestroy)
            {
                mNetworkView.RPC("NetworkDestoryBall", RPCMode.Others, i);
            }
        }

        BallsInMotion.Remove(ToDestroy);

        if (ToDestroy != null)
            Destroy(ToDestroy);
    }

    public void ExplodeAtPoint(Vector3 explodePos)
    {
        Collider[] HitColliders = Physics.OverlapSphere(explodePos, ExplodeRadius, ExplosionMask);
        int i = 0;
        while (i < HitColliders.Length)
        {
            if (HitColliders[i].GetComponent<Rigidbody>() != null)
            {
                HitColliders[i].GetComponent<Rigidbody>().AddExplosionForce(ExplodeForce, explodePos, ExplodeRadius, 1.0f, ForceMode.Impulse);
            }
            i++;
        }
    }

    [RPC]
    public void NetworkDestoryBall(int listNum)
    {
        if (Network.isClient)
        {
            Debug.Log("Destroy a ball");
            GameObject ToDestroy = null;
            try
            {
                ToDestroy = BallsInMotion[listNum];
            }
            catch (System.Exception e) { }

            BallsInMotion.RemoveAt(listNum);
            if (ToDestroy != null)
                Destroy(ToDestroy);
        }
    }

    [RPC]
    public void RemoveBalls(NetworkPlayer DisconnectedPlayer)
    {
        for (int i = 0; i < BallsInMotion.Count; i++)
        {
            if (BallsInMotion[i].GetComponent<DodgeBall>().Owner == DisconnectedPlayer)
            {
                GameObject del = BallsInMotion[i].gameObject;
                BallsInMotion.RemoveAt(i);
                Destroy(del);
            }
        }
    }

    public void PlayerCollide(int damage, GameObject target)
    {
        if (target.GetComponent<PlayerControl>().baseHealth > 0)
        {
            target.GetComponent<PlayerControl>().DecreaseHealth(damage);
            GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>().mNetworkview.RPC("SetHealth", target.GetComponent<PlayerControl>().mNetworkPlayer, target.GetComponent<PlayerControl>().baseHealth);
        }
        if (target.GetComponent<PlayerControl>().baseHealth <= 0)
        {
            GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>().mNetworkview.RPC("PlayerDead", RPCMode.All, target.GetComponent<PlayerControl>().mNetworkPlayer);
        }
    }
}

public enum BallType
{
    Dodge = 1,
    Spike,
    Sticky,
    Grenade,
    Rocket,
    Spanner
}