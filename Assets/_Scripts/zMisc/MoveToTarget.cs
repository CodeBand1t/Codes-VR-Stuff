using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        transform.position = targetTransform.position;
    }
}
