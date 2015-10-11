using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour
{
    private static bool XInvert = false;
    private static bool YInvert = true;

    private static float FieldOfView = 90f;

    public static void ToggleXInvert()
    {
        XInvert = !XInvert;
    }
    public static void ToggleYInvert()
    {
        YInvert = !YInvert;
    }
    public static void SetFOV( float _FOV)
    {
        FieldOfView = _FOV;
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fov = FieldOfView;
    }
    public static float GetFOV()
    {
        return FieldOfView;
    }

}
