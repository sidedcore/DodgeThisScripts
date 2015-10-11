using UnityEngine;

public static class ExtensionMethods
{
    public static void ToggleCanvasGroup(this CanvasGroup _CanvasGroup)
    {
        if (_CanvasGroup.alpha == 1f)
        {
            _CanvasGroup.alpha = 0f;
            _CanvasGroup.interactable = false;
            _CanvasGroup.blocksRaycasts = false;
#if UNITY_EDITOR
            _CanvasGroup.gameObject.SetActive(false);
#endif
        }
        else
        {
            _CanvasGroup.alpha = 1f;
            _CanvasGroup.interactable = true;
            _CanvasGroup.blocksRaycasts = true;
#if UNITY_EDITOR
            _CanvasGroup.gameObject.SetActive(true);
#endif
        }
    }

    public static void ToggleCanvasGroup(this CanvasGroup _CanvasGroup, bool toggleStatus)
    {
        if (toggleStatus == false)
        {
            _CanvasGroup.alpha = 0f;
            _CanvasGroup.interactable = false;
            _CanvasGroup.blocksRaycasts = false;
        }
        else
        {
            _CanvasGroup.alpha = 1f;
            _CanvasGroup.interactable = true;
            _CanvasGroup.blocksRaycasts = true;
        }
    }

    public static void ShowCanvasGroup(this CanvasGroup _CanvasGroup)
    {
        _CanvasGroup.alpha = 1f;
        _CanvasGroup.interactable = true;
        _CanvasGroup.blocksRaycasts = true;
    }

    public static void HideCanvasGroup(this CanvasGroup _CanvasGroup)
    {
        _CanvasGroup.alpha = 0f;
        _CanvasGroup.interactable = false;
        _CanvasGroup.blocksRaycasts = false;
    }

    public static Transform ClearChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }

    public static Transform HideChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        return transform;
    }

    public static Transform EnableChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        return transform;
    }
}