using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider volume;
    [SerializeField] private AudioMixer masterMixer;
    private float curretVolume;

    [Space]
    [SerializeField] private Toggle fullScreen;
    [Space]
    [SerializeField] Dropdown resolutionDropdown;
    private Resolution[] availableResolutins;
    [Space]
    [SerializeField] Dropdown qualityDropdown;
    private string[] qualityLevels;

    void Start()
    {
        volume.onValueChanged.AddListener(OnVolumeChanged);
        fullScreen.onValueChanged.AddListener(OnFullScreenChanged);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);

        masterMixer.GetFloat("Volume", out curretVolume);
        volume.value = curretVolume;

        availableResolutins = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentIndex = 0;
        for (int i = 0; i < availableResolutins.Length; i++) 
        {
            if (availableResolutins[i].width <= 800)
                continue;

            options.Add(availableResolutins[i].width + "x" + availableResolutins[i].height);
            if (availableResolutins[i].width == Screen.currentResolution.width
                && availableResolutins[i].height == Screen.currentResolution.height)
                currentIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();

        qualityLevels = QualitySettings.names;
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(qualityLevels.ToList());
        int qulityLvl = QualitySettings.GetQualityLevel();
        qualityDropdown.value = qulityLvl;
        qualityDropdown.RefreshShownValue();

    }
    private void OnDestroy()
    {
        volume.onValueChanged.RemoveListener(OnVolumeChanged);
        fullScreen.onValueChanged.RemoveListener(OnFullScreenChanged);
        resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
        qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);

    }
    void Update()
    {
        
    }
    private void OnResolutionChanged(int resolutionIndex)
    {
        Resolution resolution = availableResolutins[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    private void OnFullScreenChanged(bool value)
    {
        Screen.fullScreen = value;
    }
    private void OnVolumeChanged(float volume)
    {
        masterMixer.SetFloat("Volume", volume);
    }
    private void OnQualityChanged(int qualityLvl)
    {
        QualitySettings.SetQualityLevel(qualityLvl,true);
    }
}
