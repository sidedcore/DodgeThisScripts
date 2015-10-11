using UnityEngine;

public class PlayerWeapon : Player
{
    public int Current = (int)BallType.Dodge;
    public int Last = (int)BallType.Dodge;

    public float ChargingForce = 0f;

    public Transform PlayerHand;
    public WeaponState wState = WeaponState.idle;

    public void ChargeThrow()
    {
        wState = WeaponState.charging;
    }

    public void ReleaseThrow()
    {
        ChargingForce = Mathf.Round(ChargingForce * 100f) / 100f;
        wState = WeaponState.released;
    }

    public void ReleaseThrow(Vector3 LookDir)
    {
        wState = WeaponState.released;
        ReleaseBall(LookDir);
    }

    private void Update()
    {
        switch ((int)wState)
        {
            case 0:
                ChargingForce = 0f;
                break;

            case 1:
                GetComponent<PlayerControl>().mAnimator.SetBool("Charging", true);
                ChargingForce += Chargerate * Time.unscaledDeltaTime;
                break;

            case 2:
                GetComponent<PlayerControl>().mAnimator.SetBool("Charging", false);
                GetComponent<PlayerControl>().mAnimator.SetTrigger("Release");
                wState = WeaponState.cooldown;
                break;

            case 3:
                ChargingForce -= DischargeRate * Time.unscaledDeltaTime;
                if (ChargingForce <= 0f)
                    wState = WeaponState.idle;
                break;

            default:
                Debug.Log("What is this");
                break;
        }

        ChargingForce = Mathf.Clamp(ChargingForce, 0f, 100f);
        Debug.DrawRay(transform.GetChild(0).position, transform.GetChild(0).forward, Color.blue);
    }

    public void ReleaseBall(Vector3 LookDir)
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<BallControl>().CreateBall(Current, ChargingForce, LookDir, GetPosition() + Vector3.up * 2.0f + transform.forward, GetComponent<PlayerControl>().mNetworkPlayer);
    }

    public void NextWeapon()
    {
        Last = Current;
        Current = Current >= 6 ? 1 : ++Current;
        SetWeapon(Current);
    }

    public void PreviousWeapon()
    {
        Last = Current;
        Current = Current <= 1 ? 6 : --Current;
        SetWeapon(Current);
    }

    public void SetWeapon(int ballType)
    {
        if (Current != ballType)
        {
            Last = Current;
            Current = ballType;
        }
        PlayerHand.GetComponent<HandReference>().ShowWeapon(Current);
    }

    public int GetCurrentBall()
    { return Current; }
}