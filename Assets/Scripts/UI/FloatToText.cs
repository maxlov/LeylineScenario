using System;
using TMPro;
using UnityEngine;

enum FloatToTextStyle
{
    Plain,
    Time,
    Distance,
}

public class FloatToText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private FloatToTextStyle style;
    
    public void SetText(float value)
    {
        text.text = style switch
        {
            FloatToTextStyle.Time => TimeSpan.FromSeconds(value).ToString("mm':'ss"),
            FloatToTextStyle.Distance => $"{value:F1}m",
            _ => $"{value}"
        };
    }
}
