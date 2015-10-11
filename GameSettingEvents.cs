using UnityEngine;
using System.Collections;

public class GameSettingEvents : MonoBehaviour 
{
    public void ToggleXInvert()
    {
        GameSettings.ToggleXInvert();
    }
    public void ToggleYInvert()
    {
        GameSettings.ToggleYInvert();
    }
    public void SetFOV(float value)
    {
        GameSettings.SetFOV(value);
    }
}
