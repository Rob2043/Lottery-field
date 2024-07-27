using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private float _time = 0;
    private float Amp = 14f;
    private float Freq = 3;
    private float Offset = 0;
    private Vector3 StartPos;

    void Start()
    {
        StartPos = transform.position;
    }

    void Update()
    {
        _time += Time.deltaTime;
        Offset = Amp * Mathf.Sin(_time * Freq);
        transform.position = StartPos + new Vector3(0, 0, Offset);
    }
}
