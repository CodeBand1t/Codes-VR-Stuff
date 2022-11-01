using System;
using UnityEngine;

public class UIFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform currentTargetTransform;

    [Header("Movement Settings")] 
    [SerializeField] private float baseSpeed = 2;
    [SerializeField] private float minSpeed = 1, maxSpeed = 10;

    private float _currentSpeed;
    private float _step;
    private float _currentDistance;

    private Transform[] _targetTransforms;
    
    private Transform _thisTransform;

    private DialogueReferenceHolder _dialogueReferenceHolder;

    private void Awake()
    {
        _dialogueReferenceHolder = FindObjectOfType<DialogueReferenceHolder>();
    }

    private void Start()
    {
        _thisTransform = transform;
        _targetTransforms = _dialogueReferenceHolder.GetListenerTransforms();
    }

    // Update is called once per frame
    void Update()
    {
        SetSpeed();
        MoveTowardsTarget();
    }

    /// <summary>
    /// Set speed based on distance from targetTransform position
    /// </summary>
    void SetSpeed()
    {
        if (currentTargetTransform != null)
        {
            _currentDistance = Vector3.Distance(transform.position, currentTargetTransform.position);

            _currentSpeed = Mathf.Clamp(baseSpeed * _currentDistance, minSpeed, maxSpeed);
        }
    }

    /// <summary>
    /// Move towards targetTransform position & maintain rotation
    /// </summary>
    void MoveTowardsTarget()
    {
        _step =  _currentSpeed * Time.deltaTime;
        _thisTransform.position = Vector3.MoveTowards(_thisTransform.position, currentTargetTransform.position, _step);

        // always maintain the rotation exactly
        _thisTransform.rotation = currentTargetTransform.rotation;
    }

    public void ChangeTargetTransform(int targetIndex)
    {
        if (_targetTransforms == null)
        {
            _targetTransforms = FindObjectOfType<DialogueReferenceHolder>().GetListenerTransforms();
        }
        
        currentTargetTransform = _targetTransforms[targetIndex].transform;
    }
}
