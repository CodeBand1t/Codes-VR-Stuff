using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    [Header("Movement Settings")] 
    [SerializeField] private float baseSpeed = 2;
    [SerializeField] private float minSpeed = 1, maxSpeed = 10;

    private float currentSpeed;
    private float step;
    private float currentDistance;

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
        currentDistance = Vector3.Distance(transform.position, targetTransform.position);

        currentSpeed = Mathf.Clamp(baseSpeed * currentDistance, minSpeed, maxSpeed);
    }

    /// <summary>
    /// Move towards targetTransform position & maintain rotation
    /// </summary>
    void MoveTowardsTarget()
    {
        step =  currentSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, step);

        // always maintain the rotation exactly
        transform.rotation = targetTransform.rotation;
    }
}
