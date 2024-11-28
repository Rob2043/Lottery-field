using UnityEngine;
using UnityEngine.UI;
public class SafeArea : MonoBehaviour
{
    [SerializeField] private RectTransform panelSafeArea;
    private RectTransform _curretTransform;

    void Start()
    {
        _curretTransform = GetComponent<RectTransform>();
        if (Screen.safeArea.position.y > _curretTransform.anchorMin.y)
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        Debug.Log("Test");
        panelSafeArea.anchoredPosition = new Vector2(panelSafeArea.anchoredPosition.x, panelSafeArea.anchoredPosition.y - 90f);
    }
}
