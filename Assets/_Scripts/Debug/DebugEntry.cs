using TMPro;
using UnityEngine;

public class DebugEntry : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI entryText;

    public void SetText(string newText)
    {
        entryText.text = newText;
    }
}
