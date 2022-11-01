using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class TransformLerp : MonoBehaviour
{
    [SerializeField] private Transform lerpingTransform;

    [SerializeField] private Vector3 destinationPosition;
    [SerializeField] private float lerpDuration;
    [SerializeField] private AnimationCurve lerpCurve;

    private void Awake()
    {
        if (lerpingTransform == null)
            lerpingTransform = this.transform;
    }

    public void StartLerp()
    {
        StartCoroutine(TransformLerpCoroutine());
    }

    IEnumerator TransformLerpCoroutine()
    {
        Vector3 _startPosition = lerpingTransform.position;

        float _timer = 0;
        Vector3 currentPosition = _startPosition;

        while (_timer <= lerpDuration)
        {
            _timer = Mathf.Clamp(_timer + Time.deltaTime, 0, lerpDuration);
            currentPosition = Vector3.Lerp(_startPosition, destinationPosition,
                lerpCurve.Evaluate(_timer / lerpDuration));

            lerpingTransform.position = currentPosition;
            
            yield return new WaitForEndOfFrame();
        }
    }
}
