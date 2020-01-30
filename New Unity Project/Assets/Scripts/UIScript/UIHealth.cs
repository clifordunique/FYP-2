using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public static UIHealth Instance { get; private set; }

    public Image bar;

    float originalSize;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        originalSize = bar.rectTransform.rect.height;
    }

    public void SetValue(float value)
    {
        bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize * value);
    }
}
