using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPosition : MonoBehaviour
{
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float lerpPosDuration;
    
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    public void StartLerpPosition()
    {
        StartCoroutine(LerpPositionCoroutine(lerpPosDuration));
    }

    public void StartLerpPosition(float duration)
    {
        StartCoroutine(LerpPositionCoroutine(duration));
    }

    IEnumerator LerpPositionCoroutine(float duration)
    {
        float t = 0;
        float lerpDuration = duration;

        while (t < lerpDuration)
        {
            transform.position = new Vector3(Mathf.Lerp(startPosition.x, endPosition.x, t / lerpDuration),
                Mathf.Lerp(startPosition.y, endPosition.y, t / lerpDuration),
                Mathf.Lerp(startPosition.z, endPosition.z, t / lerpDuration)); 
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
