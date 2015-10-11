using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderUpdate : MonoBehaviour
{
    [SerializeField]
    GameObject Target;
    public void OnValueChanged( float value)
    {
        Target.GetComponent<Text>().text = value.ToString();
    }
}
