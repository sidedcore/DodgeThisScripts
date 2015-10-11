using UnityEngine;

public class HUDCanvas : MonoBehaviour
{
    public bool pauseScreen = false;

    public GameObject TargetPlayer;
    public SliderBar mHealthBar;
    public SliderBar mEnergyBar;
    public SliderBar mChargeBar;

    public GameObject ButtonBar;
    public GameObject Windows;

    public void SetHealth(int value)
    {
        mHealthBar.SetBarValue(value);
    }

    public void SetEnergy(int value)
    {
        mEnergyBar.SetBarValue(value);
    }

    public void SetCharge()
    {
        mChargeBar.SetBarValue(Mathf.CeilToInt(TargetPlayer.GetComponent<PlayerWeapon>().ChargingForce));
    }

    private void Awake()
    {
        SetPaused(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SetPaused();
        }
    }

    public void SetPaused()
    {
        pauseScreen = !pauseScreen;
        if (pauseScreen == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ButtonBar.SetActive(true);
            Windows.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ButtonBar.SetActive(false);
            Windows.SetActive(false);
        }
    }

    public void SetPaused(bool val)
    {
        pauseScreen = val;
        if (pauseScreen == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ButtonBar.SetActive(true);
            Windows.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ButtonBar.SetActive(false);
            Windows.SetActive(false);
        }
    }

    public void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}