using UnityEngine;

public class DodgeBall : MonoBehaviour
{
    private float InitialForce = 25.0f;
    private float ExtraForce = 0.0f;
    private Vector3 ThrowDirection = Vector3.zero;

    private Rigidbody mRigidbody;
    public int MaxBounce = 6;
    int CurrentBounce = 0;

    float stickyTimer = 0.0f;
    bool stuck;

    public int BaseDamage = 10;
    public int BaseEnergyCost = 10;

    public NetworkPlayer Owner;

    public BallType mBallType = BallType.Dodge;

    private void Awake()
    {
        if (!mRigidbody)
            mRigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }

    public void Launch(Vector3 throwDirection, float extraForce, NetworkPlayer owner)
    {
        ThrowDirection = throwDirection;
        ExtraForce = extraForce;
        Owner = owner;
        mRigidbody.rotation = Quaternion.LookRotation(throwDirection);
        mRigidbody.AddForce(ThrowDirection * (InitialForce + ExtraForce), ForceMode.Impulse);
    }
    void Update()
    {
        if( mBallType == BallType.Rocket)
        {
            mRigidbody.AddForce(transform.forward * 25f, ForceMode.Acceleration);
        }

        if( mBallType == BallType.Sticky)
        {
            if( stuck)
            {
                if( Time.time >= stickyTimer + 1.5f)
                {
                    Explode();
                    DestroyBall();
                }
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if( Network.isServer)
        {
            switch (mBallType)
            {
                case BallType.Dodge:
                case BallType.Spike:
                case BallType.Grenade:
                case BallType.Spanner:
                    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                        CurrentBounce += 1;
                    if (CurrentBounce > MaxBounce)
                        DestroyBall();
                    break;
                case BallType.Rocket:
                    RocketCollide();
                    break;
                case BallType.Sticky:
                    StickyCollide(collision.transform);
                    break;
            }


            if (collision.gameObject.tag == "Player")
            {
                PlayerCollision( collision.gameObject );
            }
        }        
    }

    public void PlayerCollision( GameObject target)
    {
        if( Owner != target.GetComponent<PlayerControl>().mNetworkPlayer)
        {
            GameObject.FindGameObjectWithTag("LevelController").GetComponent<BallControl>().PlayerCollide(BaseDamage, target);
        }
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<BallControl>().DestroyBall(this.gameObject);
    }


    public void RocketCollide()
    {
        Explode();
        DestroyBall();
    }
    public void StickyCollide( Transform sticky)
    {
        mRigidbody.isKinematic = true;
        //mRigidbody.useGravity = false;
        transform.SetParent(sticky);
        stuck = true;
        stickyTimer = Time.time;

    }
    public void DestroyBall()
    {
        //Destroy(this.gameObject);
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<BallControl>().DestroyBall(this.gameObject);
    }
    public void Explode()
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<BallControl>().ExplodeAtPoint(this.transform.position);
    }

}