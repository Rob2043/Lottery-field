using System.Collections;
using CustomEventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLevel : MonoBehaviour
{
    const float startTime = 0f;
    const int countOfNumber = 2;
    [SerializeField] private GameObject _endPanel;
    [SerializeField] GameObject _levelPanel;
    [SerializeField] Slider _levelSlider;
    [SerializeField] TMP_Text[] _levelsText = new TMP_Text[countOfNumber];
    [SerializeField] Transform _index;
    float elplasedTime = 0f;
    private void Start()
    {
        EventBus.InfoLevel = StartUpdate;
        EventBus.ExitLevelPanel = ExitPanel;
    }
    private void ExitPanel()
    {
        _levelPanel.SetActive(false);
    }
    private void StartUpdate(bool situation) => StartCoroutine(UpdateSlider(situation));
    IEnumerator UpdateSlider(bool wasUpgraded)
    {
        int level = Iinstance.instance.MyLevel;
        if (wasUpgraded == true)
        {
            _levelsText[0].text = $"{level - 1}";
            _levelsText[1].text = $"{level}";
            while (_levelSlider.maxValue >= elplasedTime)
            {
                elplasedTime += Time.deltaTime;
                _levelSlider.value = elplasedTime;
                yield return null;
            }
            ChangeNumber(1);
        }
        else
        {
            _levelsText[0].text = $"{1}";
            _levelsText[1].text = $"{level}";
            elplasedTime = 3f;
            while (startTime <= elplasedTime)
            {
                elplasedTime -= Time.deltaTime;
                _levelSlider.value = elplasedTime;
                yield return null;
            }
            _index.Rotate(Vector3.down, 180f);
            ChangeNumber(-1);
        }
        _endPanel.SetActive(true);
    }
    private void ChangeNumber(int result)
    {
        int level = Iinstance.instance.MyLevel;
        _levelSlider.value = 0f;
        if (result > 0)
        {
            _levelsText[0].text = $"{level}";
            _levelsText[1].text = $"{level + 1}";
        }
        else
        {
            _levelsText[0].text = $"{1}";
            _levelsText[1].text = $"{level}";
        }
    }
}
