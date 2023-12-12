using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] Slider soundsManagerSlider;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("AudioSlider"))
        {
            PlayerPrefs.SetFloat("AudioSlider", 0.50f);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = soundsManagerSlider.value;
        Save();
    }

    private void Load()
    {
        float loadedValue = PlayerPrefs.GetFloat("AudioSlider");
        soundsManagerSlider.value = loadedValue;
        Debug.Log("Loaded AudioSlider value: " + loadedValue);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("AudioSlider", soundsManagerSlider.value);
        PlayerPrefs.Save(); // Add PlayerPrefs.Save() to ensure the data is saved immediately
        Debug.Log("Saved AudioSlider value: " + soundsManagerSlider.value);
    }
}
