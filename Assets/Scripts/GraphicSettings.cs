using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicSettings : MonoBehaviour
{
    
    [SerializeField] private TMP_Dropdown graphicsDropdown; 

    private void Start()
    {
        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", 2);
        graphicsDropdown.value = savedQuality;
        SetGraphicsQuality(savedQuality);
        graphicsDropdown.onValueChanged.AddListener(OnGraphicsQualityChanged);
    }

    public void OnGraphicsQualityChanged(int index)
    {
        PlayerPrefs.SetInt("GraphicsQuality", index);
        SetGraphicsQuality(index);
    }

    private void SetGraphicsQuality(int index)
    {
        switch (index)
        {
            case 0: 
                QualitySettings.SetQualityLevel(0); 
                break;
            case 1: 
                QualitySettings.SetQualityLevel(2); 
                break;
            case 2:
                QualitySettings.SetQualityLevel(5); 
                break;
        }
    }
}
