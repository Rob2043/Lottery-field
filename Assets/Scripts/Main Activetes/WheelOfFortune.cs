using System.Collections;
using UnityEngine;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] private float[] segmentProbabilities;
    [SerializeField] private float rotationDuration = 3f;
    private float totalProbability;

    void Start()
    {
        totalProbability = 0f;
        foreach (float probability in segmentProbabilities)
        {
            totalProbability += probability;
        }
        StartCoroutine(SpinWheel());
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
        float endAngle = startAngle + targetAngle + 360f * Random.Range(2, 4); // 2-3 полных оборота
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            transform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, 0, targetAngle);
        Debug.Log("Selected Segment: " + selectedSegment);
    }
}
