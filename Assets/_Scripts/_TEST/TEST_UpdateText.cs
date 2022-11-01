using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TEST_UpdateText : MonoBehaviour
{
    public Slider slider;

    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        
        SliderUpdateText();
    }
    
    
    public void SliderUpdateText()
    {
        string newText;

        newText = $"{(slider.value / slider.maxValue * 100).ToString("N0")}%";

        text.text = newText;
    }
}
