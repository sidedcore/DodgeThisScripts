using UnityEngine;

public class Player : MonoBehaviour
{
    private float gravity = -25.0f;
    private float forward = 9.0f;
    private float right = 8.0f;
    private float jump = 5.0f;
    private float chargerate = 24f;
    private float dischargerate = 44f;

    public float Chargerate
    {
        get { return chargerate; }
        set { chargerate = value; }
    }

    public float DischargeRate
    {
        get { return dischargerate; }
        set { dischargerate = value; }
    }

    public float ForwardVel
    {
        get { return forward; }
    }

    public float RightVel
    {
        get { return right; }
    }

    public float JumpVel
    {
        get { return jump; }
    }

    public float Gravity
    {
        get { return gravity; }
    }

    public Camera _Camera;

    [HideInInspector]
    protected Rigidbody mRigidbody;

    [HideInInspector]
    protected CapsuleCollider mCollider;

    [HideInInspector]
    public NetworkView mNetworkview;

    [HideInInspector]
    public NetworkPlayer mNetworkPlayer;

    public bool CameraControl = false;

    public Vector3 LookDirection()
    {
        return _Camera.transform.forward;
    }

    public void GetCamera()
    {
        try
        {
            _Camera = transform.FindChild("Camera").GetComponent<Camera>();
            _Camera.fieldOfView = GameSettings.GetFOV();
            CameraControl = true;
        }
        catch (System.Exception e)
        {
        }
    }

    private void Awake()
    {
        GetCamera();

        mNetworkview = GetComponent<NetworkView>();

        mRigidbody = GetComponent<Rigidbody>();
        mCollider = GetComponent<CapsuleCollider>();
    }

    public Vector3 GetPosition()
    {
        return mRigidbody.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public void SetPosition(Vector3 NewPostion)
    {
        transform.position = NewPostion;
        mRigidbody.position = NewPostion;
    }

    public void SetRotation(float RotationX)
    {
        transform.Rotate(Vector3.up, RotationX, Space.Self);
    }

    public void SetRotation(Quaternion NewRotation)
    {
        transform.rotation = NewRotation;
    }
}

public enum PlayerState
{
    Creating = 0,
    Spectating,
    Alive,
    Dead
}

public enum WeaponState
{
    idle = 0,
    charging,
    released,
    cooldown
}