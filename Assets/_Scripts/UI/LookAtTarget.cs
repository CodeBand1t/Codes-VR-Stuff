using Unity.Mathematics;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private bool targetIsMainCamera;
    [SerializeField] private Transform targetTransform;

    [SerializeField] private float zEulerAdjustment;
    
    private Quaternion origRotation;

    private void Start()
    {
        origRotation = transform.rotation;

        if (targetIsMainCamera)
            targetTransform = Camera.main.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = targetTransform.rotation * origRotation;
        transform.LookAt(targetTransform);

        transform.rotation = Quaternion.Euler( new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));

        if (zEulerAdjustment != 0)
        {
            transform.rotation = Quaternion.Euler( new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, zEulerAdjustment));
        }
    }
}
