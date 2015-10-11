using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WindowToggle : MonoBehaviour
{
    [SerializeField]
    CanvasGroup Target;

    public void ToggleWindow( bool toggle)
    {
        Target.ToggleCanvasGroup(toggle);
        Target.SendMessage("OnCanvasToggle", toggle,SendMessageOptions.DontRequireReceiver);
        //GameObject.FindObjectOfType<WindowController>().ToggleWindow(Target, toggle);
    }

}
