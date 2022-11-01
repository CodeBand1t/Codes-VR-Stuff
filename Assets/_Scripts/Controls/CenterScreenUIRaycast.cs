using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CenterScreenUIRaycast : MonoBehaviour
{
    private Transform _cameraTransform;

    public EventSystem m_EventSystem;
    public PointerEventData m_PointerEventData;
    public GraphicRaycaster m_Raycaster;
    
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        FirstAttempt();
    }

    void FirstAttempt()
    {
        /*RaycastHit hit = new RaycastHit();
{
if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, 100))
{
    Debug.Log($"Raycast Hit: {hit.transform.name}");
    //Debug.DrawRay(_cameraTransform.position, _cameraTransform.forward * 100, Color.magenta);
}*/
        m_EventSystem = FindObjectOfType<EventSystem>();
        m_PointerEventData = new PointerEventData(null);
        m_PointerEventData.position = Camera.main.ViewportPointToRay(new Vector3(1f, 1f, 0f)).GetPoint(10);//Input.mousePosition;
        //Debug.Log($"Center Screen Position (1): {Camera.main.ViewportPointToRay(new Vector3(1f, 1f, 0f)).GetPoint(100)}");
        //Debug.Log($"Center Screen Position (0.5): {Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).GetPoint(100)}");

        
        List<RaycastResult> results = new List<RaycastResult>();
        
        m_Raycaster.Raycast(m_PointerEventData, results);

        foreach (var result in results)
        {
            Debug.Log($"Hit {result.gameObject.name}");
        }
        
        Debug.DrawRay(_cameraTransform.position, _cameraTransform.forward * 100, Color.magenta);
        Debug.DrawRay(Camera.main.ViewportPointToRay(new Vector3(1f, 1f, 0f)).GetPoint(0), _cameraTransform.forward * 100, Color.blue);
    }
}
