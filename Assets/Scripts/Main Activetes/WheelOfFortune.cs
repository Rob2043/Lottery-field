using System.Collections;
using System.Data;
using CustomEventBus;
using TMPro;
using UnityEngine;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] private float[] segmentProbabilities;
    [SerializeField] private float rotationDuration = 3f;
    [SerializeField] private int[] ArrayOfWining;
    [Header("UI elements")]
    [SerializeField] private GameObject SpinPanel;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TMP_Text _textOfMoneyWin;
    private float totalProbability;
    private int _freeSpins;
    private int _minimumForStart = 600;

    void Start()
    {
        _freeSpins = Iinstance.instance.FreeSpins;
        totalProbability = 0f;
        foreach (float probability in segmentProbabilities)
        {
            totalProbability += probability;
        }
    }

    public void StartFortune(bool WasNotMoney)
    {
        _freeSpins = Iinstance.instance.FreeSpins;
        if (_minimumForStart <= EventBus.GetCoins.Invoke())
        {
            SpinPanel.SetActive(true);
            StartCoroutine(SpinWheel());
            EventBus.SetCoins.Invoke(-_minimumForStart);
        }
        else if (_freeSpins > 0)
        {
            SpinPanel.SetActive(true);
            StartCoroutine(SpinWheel());
            _freeSpins--;
            EventBus.AddFreeSpin.Invoke(-1);
        }
        else if (WasNotMoney == true)
        {
            SpinPanel.SetActive(true);
            StartCoroutine(SpinWheel());
        }
    }
    IEnumerator SpinWheel()
    {
        float rand = Random.Range(0f, totalProbability);
        int selectedSegment = -1;
        float cumulativeProbability = 0f;
        for (int i = 0; i < segmentProbabilities.Length; i++)
        {
            cumulativeProbability += segmentProbabilities[i];
            if (rand <= cumulativeProbability)
            {
                selectedSegment = i;
                break;
            }
        }
        float anglePerSegment = 360f / segmentProbabilities.Length;
        float targetAngle = selectedSegment * anglePerSegment;
        float startAngle = transform.eulerAngles.z;
        float endAngle = startAngle + targetAngle + 360f * Random.Range(2, 4);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            transform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, 0, targetAngle);
        int wining = ArrayOfWining[selectedSegment];
        WinPanel.SetActive(true);
        _textOfMoneyWin.text = $"{wining}";
        EventBus.SetCoins.Invoke(wining);
    }
}
