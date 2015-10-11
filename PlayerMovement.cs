using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : Player
{
    public float FallingVel = 0;
    public bool grounded = false;

    [SerializeField]
    private LayerMask groundingMask = new LayerMask();

    private void FixedUpdate()
    {
        GroundCheck();
        //if (Input.GetButton("Jump")) Jump();
        //Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public void Movement(float Horizontal, float Vertical)
    {
        Vector3 MoveDir = new Vector3(Horizontal, 0, Vertical);

        GetComponent<PlayerControl>().mAnimator.SetFloat("Speed", Vertical);
        GetComponent<PlayerControl>().mAnimator.SetFloat("Direction", Horizontal);

        MoveDir = transform.TransformDirection(MoveDir);
        MoveDir.x *= RightVel;
        MoveDir.z *= ForwardVel;

        MoveDir.y = FallingVel;

        mRigidbody.velocity = MoveDir;
    }

    public void Jump()
    {
        if (grounded)
        {
            GetComponent<PlayerControl>().mAnimator.SetBool("Jump", true);
            grounded = false;
            FallingVel = Mathf.Sqrt(JumpVel * -Gravity);
        }
    }

    public void Movement(float Horizontal, float Vertical, Vector3 LastPosition, Vector3 LastVelocity)
    {
        GetComponent<PlayerControl>().mAnimator.SetFloat("Speed", Vertical);
        GetComponent<PlayerControl>().mAnimator.SetFloat("Direction", Horizontal);

        mRigidbody.position = Vector3.Lerp(mRigidbody.position, LastPosition, 1.0f);
        mRigidbody.velocity = Vector3.Lerp(mRigidbody.velocity, LastVelocity, 1.0f);
    }

    public void GroundCheck()
    {
        if (grounded || mRigidbody.velocity.y < 10.0f * 0.5f)
        {
            grounded = false;

            RaycastHit hitinfo;

            Vector3 hitPoint = new Vector3();
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f + transform.forward * mCollider.radius, Vector3.down, out hitinfo, 1.0f, groundingMask))
            {
                grounded = true;
                hitPoint.y = hitinfo.point.y;
            }
            else if (Physics.Raycast(transform.position + Vector3.up * 0.5f + -transform.forward * mCollider.radius, Vector3.down, out hitinfo, 1.0f, groundingMask))
            {
                grounded = true;
                hitPoint.y = hitinfo.point.y;
            }
            else if (Physics.Raycast(transform.position + Vector3.up * 0.5f + transform.right * mCollider.radius, Vector3.down, out hitinfo, 1.0f, groundingMask))
            {
                grounded = true;
                hitPoint.y = hitinfo.point.y;
            }
            else if (Physics.Raycast(transform.position + Vector3.up * 0.5f + -transform.right * mCollider.radius, Vector3.down, out hitinfo, 1.0f, groundingMask))
            {
                grounded = true;
                hitPoint.y = hitinfo.point.y;
            }
            else if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hitinfo, 1.0f, groundingMask))
            {
                grounded = true;
                hitPoint.y = hitinfo.point.y;
            }

            if (grounded)
            {
                FallingVel = 0f;

                hitPoint.x = mRigidbody.position.x;
                hitPoint.z = mRigidbody.position.z;

                mRigidbody.position = Vector3.MoveTowards(mRigidbody.position, hitPoint + Vector3.up * 0.1f, Time.unscaledDeltaTime * -Gravity * 0.5f);
                mRigidbody.velocity = new Vector3(mRigidbody.velocity.x, 0, mRigidbody.velocity.z);
            }
        }
        if (!grounded)
        {
            FallingVel += Gravity * Time.unscaledDeltaTime;
            mRigidbody.velocity = new Vector3(mRigidbody.velocity.x, FallingVel, mRigidbody.velocity.z);
        }

        Debug.DrawRay(mRigidbody.position + Vector3.up * 0.5f, Vector3.down, Color.red);

        Debug.DrawRay(mRigidbody.position + Vector3.up * 0.5f + transform.forward * (mCollider.radius * transform.localScale.x), Vector3.down, Color.red);
        Debug.DrawRay(mRigidbody.position + Vector3.up * 0.5f + -transform.forward * (mCollider.radius * transform.localScale.x), Vector3.down, Color.red);
        Debug.DrawRay(mRigidbody.position + Vector3.up * 0.5f + transform.right * (mCollider.radius * transform.localScale.x), Vector3.down, Color.red);
        Debug.DrawRay(mRigidbody.position + Vector3.up * 0.5f + -transform.right * (mCollider.radius * transform.localScale.x), Vector3.down, Color.red);
    }
}