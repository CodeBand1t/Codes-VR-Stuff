using UnityEngine;

public class ScaleInfluence : MonoBehaviour
{
    [SerializeField] private bool applyInfluenceOnUpdate = false;
    
    [SerializeField] private Transform influencerTransform;

    private float _baseXScale, _baseYScale, _baseZScale;

    private void Awake()
    {
        _baseXScale = transform.lossyScale.x;
        _baseYScale = transform.lossyScale.y;
        _baseZScale = transform.lossyScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (applyInfluenceOnUpdate)
            ApplyInfluence();
    }

    private void OnEnable()
    {
        ApplyInfluence();
    }

    void ApplyInfluence()
    {
        float mInfluencerScale = influencerTransform.lossyScale.x;
        transform.localScale = new Vector3(_baseXScale * mInfluencerScale, _baseYScale * mInfluencerScale,
            _baseZScale * mInfluencerScale);
    }
}
