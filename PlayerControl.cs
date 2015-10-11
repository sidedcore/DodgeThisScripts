using UnityEngine;

[RequireComponent(typeof(NetworkView))]
public class PlayerControl : Player
{
    public int baseHealth = 100;
    public int baseEnergy = 100;

    public int TeamID;
    public Vector2 MouseRotation = new Vector2();
    public PlayerState pState = PlayerState.Creating;

    public Animator mAnimator = new Animator();
    public AnimatorStateInfo mAnimState;
    public AnimatorStateInfo mMaskState;

    protected static int IdleState = Animator.StringToHash("Idle");
    protected static int LocoState = Animator.StringToHash("LocoMotion");
    protected static int RunBackward = Animator.StringToHash("RunBackward");
    protected static int JumpState = Animator.StringToHash("Jump");
    protected static int ChargeState = Animator.StringToHash("Charge");
    protected static int ReleaseState = Animator.StringToHash("Release");

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        mAnimator.SetLayerWeight(1, 1);
        mAnimator.speed = 0.8f;
    }

    private void FixedUpdate()
    {
        if (_Camera && CameraControl == true)
            CameraRotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        mAnimState = mAnimator.GetCurrentAnimatorStateInfo(0);
        mMaskState = mAnimator.GetCurrentAnimatorStateInfo(1);

        if (mAnimState.shortNameHash == LocoState)
        {
        }
        else if (mAnimState.shortNameHash == JumpState)
        {
            if (!mAnimator.IsInTransition(0))
            {
                mCollider.height = mAnimator.GetFloat("ColliderHeight");

                mAnimator.SetBool("Jump", false);
            }
        }
        else if (mAnimState.shortNameHash == IdleState)
        {
            if (!mAnimator.IsInTransition(0))
            {
                mAnimator.SetBool("Jump", false);
            }
        }
        else if (mAnimState.shortNameHash == RunBackward)
        {
        }

        if (mMaskState.shortNameHash == ChargeState)
        {
        }
        else if (mMaskState.shortNameHash == ReleaseState)
        {
            if (!mAnimator.IsInTransition(0))
            {
                mAnimator.SetBool("Charging", false);
            }
        }
    }

    private void CameraRotate(float MouseX, float MouseY)
    {
        MouseRotation.x = Input.GetAxis("Mouse X") * 10.0f;
        MouseRotation.y -= Input.GetAxis("Mouse Y") * 10.0f;
        MouseRotation.y = Mathf.Clamp(MouseRotation.y, -60.0f, 60.0f);

        GetComponent<PlayerMovement>().SetRotation(MouseRotation.x);
        _Camera.transform.localRotation = Quaternion.Euler(new Vector3(MouseRotation.y, 0, 0));
    }

    public int GetState()
    {
        return (int)pState;
    }

    public void SetState(int NewState)
    {
        pState = (PlayerState)NewState;
    }

    public void DecreaseHealth(int amount)
    {
        baseHealth -= amount;
    }

    public void DecreaseEnergy(int amount)
    {
        baseEnergy -= amount;
    }

    public void MaxEnergy()
    {
        baseEnergy = 100;
    }

    public void MaxHealth()
    {
        baseHealth = 100;
    }
}