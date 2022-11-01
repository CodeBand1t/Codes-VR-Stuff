using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SighManip : MonoBehaviour
{
    [SerializeField] private bool trigger;
    [SerializeField] private float triggerCooldown;
    
    [Header("Settings")] 
    [SerializeField] private float sighDuration;
    [SerializeField] private float fadeOutDuration;

    [Header("References")] 
    [SerializeField] private GameObject sighPrefab;
    [SerializeField] private Transform originTransform;
    [SerializeField] private Transform targetTransform;

    private GameObject _currentSigh;
    private SpriteRenderer _currentSighSpriteRenderer;
    
    private bool _triggered;
    private float _triggeredTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_triggered)
        {
            _triggeredTimer += Time.deltaTime;
            if (_triggeredTimer < triggerCooldown)
                return;
            _triggered = false;
            _triggeredTimer = 0;
        }
        
        if (trigger)
        {
            trigger = false;
            _triggered = true;
            StartSigh();
        }
    }

    [ContextMenu("Spawn Sigh")]
    public void StartSigh()
    {
        _currentSigh = Instantiate(sighPrefab, originTransform.position, Quaternion.identity);
        _currentSighSpriteRenderer = _currentSigh.GetComponent<SpriteRenderer>();

        StartCoroutine(SighMovementCoroutine());
    }

    IEnumerator SighMovementCoroutine()
    {
        var t = 0f;
        var mStartPosition = _currentSigh.transform.position;

        while (t < sighDuration)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, sighDuration);
            _currentSigh.transform.position = Vector3.Slerp(mStartPosition, targetTransform.position, t / sighDuration);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(SighFadeOutCoroutine());
        yield return null;
    }

    IEnumerator SighFadeOutCoroutine()
    {
        var t = 0f;
        var mStartAlpha = 1;
        var mEndAlpha = 0;
        var mSighColor = _currentSighSpriteRenderer.color;
        

        while (t < fadeOutDuration)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, fadeOutDuration);
            _currentSighSpriteRenderer.color = new Color(mSighColor.r, mSighColor.g, mSighColor.b, Mathf.Lerp(mStartAlpha, mEndAlpha, t / fadeOutDuration));
            yield return new WaitForEndOfFrame();
        }
        
        Destroy(_currentSigh);
    }
}
