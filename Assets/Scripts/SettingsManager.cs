using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SettingsManager : MonoBehaviour
{
	[SerializeField] private TMP_Dropdown resolutionDropdown;

	private bool _fullScreen = true;
	private int _resIndex = 0;
	private List<string> options = new List<string>();
	Resolution[] resolutions;

	private void Awake()
	{
		resolutionDropdown.onValueChanged.AddListener(delegate
		{
			ResValueChanged(resolutionDropdown);
		});
	}

	private void Start()
	{
		resolutions = Screen.resolutions;
		resolutionDropdown.ClearOptions();

		int currentResolutionIndex = 0;

		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);
			
			if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
			{
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();
	}

	public void Apply()
	{
		Screen.SetResolution(resolutions[_resIndex].width, resolutions[_resIndex].height, _fullScreen);
	}

	public void SetFullscreen(bool fullscreen)
	{
		_fullScreen = fullscreen;
	}

	private void ResValueChanged(TMP_Dropdown resolutionDropdown)
	{
		_resIndex = resolutionDropdown.value;
	}
}
