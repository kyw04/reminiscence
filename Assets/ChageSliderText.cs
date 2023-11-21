using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChageSliderText : MonoBehaviour
{
    TextMeshProUGUI text;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
        text = GetComponent<TextMeshProUGUI>();
        slider = GetComponentInParent<Slider>();
    }

    public void ChangeSliderTextValue()
    {
        text.text = slider.value.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
