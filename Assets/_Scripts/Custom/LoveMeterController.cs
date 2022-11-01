using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LoveMeterController : MonoBehaviour
{
    public static LoveMeterController Instance;
    
    [Range(0, 10)] public int currentValue;
    public int[] maxValues;
    [SerializeField] private int loveIndex = 0;

    [Header("Meter Reference")] public Image meterImage;
    public Image secondaryMeterImage;

    [Header("Meter Settings")] 
    public float meterMinPosition;
    public float meterMaxPosition;

    [Header("Popup")]
    [SerializeField] private Transform popupSpawnTransform;

    [Header("Event")] public UnityEvent[] loveEvents;

    private ObjectPooler _objectPooler;
    private RectTransform meterImageRect, secondaryMeterImageRect;
    

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _objectPooler = ObjectPooler.Instance;
        currentValue = 0;
        meterImageRect = meterImage.rectTransform;
        secondaryMeterImageRect = secondaryMeterImage.rectTransform;
    }

    public void AddLove(int addedAmount)
    {
        currentValue += addedAmount;
        currentValue = Mathf.Clamp(currentValue, 0, maxValues[loveIndex]);


        _objectPooler.SpawnFromPool("Hearts", popupSpawnTransform.position, Quaternion.identity);
        //Instantiate(popupPrefab, popupSpawnTransform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMeter();

        if (currentValue >= maxValues[loveIndex])
        {
            currentValue = 0;
            loveEvents[loveIndex].Invoke();
            loveIndex++;

            if (loveIndex >= maxValues.Length)
            {
                EndLove();
            }
        }
    }

    void UpdateMeter()
    {
        Vector3 localPosition = meterImageRect.localPosition;
        meterImageRect.localPosition = new Vector3(
                meterMinPosition - (float)currentValue / maxValues[loveIndex] * meterMinPosition,
                localPosition.y,
                localPosition.z
            );
        
        secondaryMeterImageRect.localPosition = new Vector3(
            meterMinPosition - (float)currentValue / maxValues[loveIndex] * meterMinPosition,
            localPosition.y,
            localPosition.z
        );
    }

    void EndLove()
    {
        Debug.Log("End Love");
        ResetInteraction();
    }

    public void ResetInteraction()
    {
        currentValue = 0;
        loveIndex = 0;
    }

    #region Testing

    [ContextMenu("Add Love x1")]
    void ContextAddLove()
    {
        AddLove(1);
    }
    
    [ContextMenu("Progress to Next Love Level")]
    void ContextProgressToNextLoveLevel()
    {
        AddLove(maxValues[loveIndex] - currentValue);
    }
    
    [ContextMenu("Minimize Max Values")]
    void ContextMinimizeMaxValues()
    {
        for (int i = 0; i < maxValues.Length; ++i)
        {
            maxValues[i] = 1;
        }
    }

    #endregion
}
