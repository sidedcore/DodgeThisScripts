using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SliderBar : MonoBehaviour
{
    public enum SliderTarget
    {
        Health = 0,
        Energy,
        Charging
    }

    public SliderTarget Target = SliderTarget.Health;

    public int MinValue = 0;
    public int MaxValue = 100;

    public int CurrentValue;

    [SerializeField]
    private Text TargetSliderText;

    [SerializeField]
    private Slider TargetSliderBar;

    [SerializeField]
    private Color BarColor;

    [SerializeField]
    private bool LeftToRight = true;

    [SerializeField]
    private bool ShowText = true;

    private void Awake()
    {
        SetBarValue(CurrentValue);
    }

    private void Update()
    {
        TargetSliderText.transform.parent.gameObject.SetActive(ShowText);

        TargetSliderBar.fillRect.GetComponent<Image>().fillOrigin = LeftToRight ? 0 : 1;
        TargetSliderBar.fillRect.GetComponent<Image>().color = BarColor;
        TargetSliderBar.minValue = MinValue;
        TargetSliderBar.maxValue = MaxValue;
    }

    public void SetBarValue(int value)
    {
        CurrentValue = value;

        TargetSliderText.text = Target.ToString() + ": " + CurrentValue.ToString();
        TargetSliderBar.value = CurrentValue;
    }

    public void SetMinMax(int min, int max)
    {
        MinValue = min;
        MaxValue = max;
    }
}