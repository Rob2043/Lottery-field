using System;
using System.Runtime.InteropServices.WindowsRuntime;
using CustomEventBus;
using UnityEngine;
using UnityEngine.UI;
public class SafeArea : MonoBehaviour
{
    [SerializeField] private RectTransform panelSafeArea;
    private RectTransform _curretTransform;
    private bool wasChangingUI = false;

    void Awake()
    {
        _curretTransform = GetComponent<RectTransform>();
        if (Screen.safeArea.position.y > _curretTransform.anchorMin.y)
        {
            ApplySafeArea();
            wasChangingUI = true;
        }
    }
    private void OnEnable() {
        EventBus.ChangeUI += SituauionWithUI;
    }
    private void OnDisable() {
        EventBus.ChangeUI -= SituauionWithUI;
    }
    private bool SituauionWithUI()
    {
        return wasChangingUI;
    }
    private void ApplySafeArea()
    {
        panelSafeArea.anchoredPosition = new Vector2(panelSafeArea.anchoredPosition.x, panelSafeArea.anchoredPosition.y - 130f);
    }
}
