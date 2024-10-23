using UnityEngine;
using UnityEngine.UI;
public class SafeArea : MonoBehaviour
{
    private RectTransform panelSafeArea;
    private Rect currentSafeArea;

    void Start()
    {
        panelSafeArea = GetComponent<RectTransform>();
        currentSafeArea = Screen.safeArea;
        //ApplySafeArea();
        if (!currentSafeArea.Equals(Screen.safeArea))
        {
            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        Debug.Log("Test");
        Vector2 size = panelSafeArea.sizeDelta;
        size.y = 0f;
        panelSafeArea.sizeDelta = size;
    }
}
