using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using T17.Services;
using T17.UI;
using Team17.Common;
using Team17.Platform.SaveGame;
using Team17.Scripts.Platforms.Enums;
using Team17.Scripts.UI_Components;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000135 RID: 309
public class SettingsMenu : MenuComponent
{
	// Token: 0x06000AC1 RID: 2753 RVA: 0x0003DC80 File Offset: 0x0003BE80
	protected override void Awake()
	{
		base.Awake();
		this._parentCanvas = base.GetComponentInParent<Canvas>();
		List<MonoBehaviour> list = new List<MonoBehaviour>();
		list.Add(this.ResolutionSelector);
		list.Add(this.FullscreenSelector);
		list.Add(this.GraphicsQualitySelector);
		list.Add(this.VoiceLanguageSelector);
		list.Add(this.TextLanguageSelector);
		list.Add(this.CameraSensitivitySlider);
		list.Add(this.MasterVolumeSlider);
		list.Add(this.SFXVolumeSlider);
		list.Add(this.MusicVolumeSlider);
		list.Add(this.VoiceVolumeSlider);
		list.Add(this.CameraSensitivitySliderValue);
		list.Add(this.MasterVolumeSliderValue);
		list.Add(this.SFXVolumeSliderValue);
		list.Add(this.MusicVolumeSliderValue);
		list.Add(this.VoiceVolumeSliderValue);
		list.Add(this.ApplyDisplaySettingsButton);
		list.Add(this.SkipTextSelector);
		list.Add(this.OneClickInteractCheckBox);
		list.Add(this.InvertYAxisCheckBox);
		this.selectableOptions = list.Where((MonoBehaviour x) => x != null).ToList<MonoBehaviour>();
		this.RemoveAnyNullSelectableOptions();
		if (Save.GetSaveData(true) != null)
		{
			bool newGamePlus = Save.GetSaveData(false).newGamePlus;
			this.SkipTextSelector.gameObject.SetActive(false);
			if (!newGamePlus)
			{
				this.SkipTextSelector.ForceSetting(0);
				return;
			}
		}
		else
		{
			this.SkipTextSelector.gameObject.SetActive(false);
			this.SkipTextSelector.ForceSetting(0);
		}
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0003DE0D File Offset: 0x0003C00D
	private void RemoveAnyNullSelectableOptions()
	{
		this.selectableOptions.RemoveAll((MonoBehaviour x) => x == null);
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x0003DE3A File Offset: 0x0003C03A
	private IEnumerator DelaySelectFirstAvailableOption()
	{
		yield return null;
		this.SelectFirstAvailableOption();
		yield break;
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0003DE4C File Offset: 0x0003C04C
	public void SelectFirstAvailableOption()
	{
		this.SortAllSelectableOptionsInVerticalOrderOnScreen();
		foreach (MonoBehaviour monoBehaviour in this.selectableOptions)
		{
			if (this.TrySelectElement(monoBehaviour.gameObject))
			{
				return;
			}
			if (monoBehaviour.GetComponent<Selectable>() == null && this.TrySelectSubSelectableElement(monoBehaviour.gameObject))
			{
				return;
			}
		}
		T17Debug.LogError("Unable to find any selectable options to fallback to.");
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0003DED8 File Offset: 0x0003C0D8
	private void SortAllSelectableOptionsInVerticalOrderOnScreen()
	{
		Camera screenCamera = this._parentCanvas.worldCamera;
		this.RemoveAnyNullSelectableOptions();
		this.selectableOptions.Sort(delegate(MonoBehaviour a, MonoBehaviour b)
		{
			Vector3 screenPosition = this.GetScreenPosition(screenCamera, a);
			return this.GetScreenPosition(screenCamera, b).y.CompareTo(screenPosition.y);
		});
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0003DF20 File Offset: 0x0003C120
	private Vector3 GetScreenPosition(Camera screenCamera, MonoBehaviour obj)
	{
		RectTransform rectTransform = obj.transform as RectTransform;
		if (rectTransform != null && rectTransform.gameObject.activeInHierarchy)
		{
			return RectTransformUtility.WorldToScreenPoint(screenCamera, rectTransform.position);
		}
		return screenCamera.WorldToScreenPoint(obj.transform.position);
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x0003DF72 File Offset: 0x0003C172
	private bool TrySelectElement(GameObject element)
	{
		ControllerMenuUI.SetCurrentlySelected(element, ControllerMenuUI.Direction.Down, true, false);
		return !(ControllerMenuUI.GetCurrentSelectedControl() != element);
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x0003DF90 File Offset: 0x0003C190
	private bool TrySelectSubSelectableElement(GameObject componentGameObject)
	{
		Selectable componentInChildren = componentGameObject.GetComponentInChildren<Selectable>();
		return componentInChildren != null && this.TrySelectElement(componentInChildren.gameObject);
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0003DFBB File Offset: 0x0003C1BB
	private bool TrySelectLastSelectedOption()
	{
		return !(this.lastSelectedOption == null) && this.TrySelectElement(this.lastSelectedOption);
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x0003DFDC File Offset: 0x0003C1DC
	protected void OnEnable()
	{
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null)
		{
			component.alpha = 0f;
		}
		this.LoadSavedSettings();
		this._pendingExitMenu = false;
		base.StartCoroutine(this.DelaySelectFirstAvailableOption());
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x0003E020 File Offset: 0x0003C220
	protected override void OnDisable()
	{
		this._pendingExitMenu = false;
		if (SettingsMenu.AllowScreenModeChange())
		{
			DisplaySettingsMenuSelector fullscreenSelector = this.FullscreenSelector;
			fullscreenSelector.SettingCallback = (UnityAction)Delegate.Remove(fullscreenSelector.SettingCallback, new UnityAction(this.SetFullscreenMode));
		}
		ResolutionSettingsMenuSelector resolutionSelector = this.ResolutionSelector;
		resolutionSelector.SettingCallback = (UnityAction)Delegate.Remove(resolutionSelector.SettingCallback, new UnityAction(this.SetResolution));
		DisplaySettingsMenuSelector graphicsQualitySelector = this.GraphicsQualitySelector;
		graphicsQualitySelector.SettingCallback = (UnityAction)Delegate.Remove(graphicsQualitySelector.SettingCallback, new UnityAction(this.SetGraphicsQuality));
		SettingsMenuSelector voiceLanguageSelector = this.VoiceLanguageSelector;
		voiceLanguageSelector.SettingCallback = (UnityAction)Delegate.Remove(voiceLanguageSelector.SettingCallback, new UnityAction(this.SetVoiceOverLanguage));
		SettingsMenuSelector textLanguageSelector = this.TextLanguageSelector;
		textLanguageSelector.SettingCallback = (UnityAction)Delegate.Remove(textLanguageSelector.SettingCallback, new UnityAction(this.SetTextLanguage));
		SettingsMenuSelector skipTextSelector = this.SkipTextSelector;
		skipTextSelector.SettingCallback = (UnityAction)Delegate.Remove(skipTextSelector.SettingCallback, new UnityAction(this.SetSkipText));
		SettingsMenuSelector oneClickInteractCheckBox = this.OneClickInteractCheckBox;
		oneClickInteractCheckBox.SettingCallback = (UnityAction)Delegate.Remove(oneClickInteractCheckBox.SettingCallback, new UnityAction(this.SetOneClickInteract));
		SettingsMenuSelector invertYAxisCheckBox = this.InvertYAxisCheckBox;
		invertYAxisCheckBox.SettingCallback = (UnityAction)Delegate.Remove(invertYAxisCheckBox.SettingCallback, new UnityAction(this.SetInvertYAxis));
		base.OnDisable();
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0003E179 File Offset: 0x0003C379
	protected void Update()
	{
		this.HandleApplyButtonEnabledState();
		this.OnElementSelected();
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x0003E188 File Offset: 0x0003C388
	private void OnElementSelected()
	{
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (currentSelectedGameObject == null || this.previousSelectedObj == currentSelectedGameObject || !this.IsSelectableOption(currentSelectedGameObject))
		{
			return;
		}
		this.previousSelectedObj = currentSelectedGameObject;
		if (currentSelectedGameObject != this.backButton.gameObject)
		{
			this.lastSelectedOption = currentSelectedGameObject;
		}
		if (Services.InputService.IsLastActiveInputController() || !Services.InputService.WasLastControllerAPointer())
		{
			ScrollRect componentInChildren = base.GetComponentInChildren<ScrollRect>();
			if (!currentSelectedGameObject.transform.IsChildOf(componentInChildren.transform))
			{
				return;
			}
			RectTransform viewport = componentInChildren.viewport;
			RectTransform component = currentSelectedGameObject.GetComponent<RectTransform>();
			if (component == null)
			{
				return;
			}
			if (!viewport.rect.Contains(component.rect.min) || !viewport.rect.Contains(component.rect.max))
			{
				componentInChildren.FocusOnItem(component);
			}
		}
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x0003E27C File Offset: 0x0003C47C
	private bool IsSelectableOption(GameObject element)
	{
		return this.selectableOptions.Any((MonoBehaviour x) => x != null && x.gameObject == element);
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0003E2B0 File Offset: 0x0003C4B0
	public void LoadSavedSettings()
	{
		if (SettingsMenu.AllowScreenModeChange())
		{
			this.FullscreenSelector.ForceSetting(Services.GraphicsSettings.GetInt("fullscreen", 0));
			DisplaySettingsMenuSelector fullscreenSelector = this.FullscreenSelector;
			fullscreenSelector.SettingCallback = (UnityAction)Delegate.Combine(fullscreenSelector.SettingCallback, new UnityAction(this.SetFullscreenMode));
		}
		if (!this.ResolutionSelector.ForceSettings(Services.GraphicsSettings.GetString(this.ResolutionSelector.SettingKey, Screen.currentResolution.ToString())))
		{
			this.ResolutionSelector.ForceSettings(Screen.currentResolution.ToString());
		}
		ResolutionSettingsMenuSelector resolutionSelector = this.ResolutionSelector;
		resolutionSelector.SettingCallback = (UnityAction)Delegate.Combine(resolutionSelector.SettingCallback, new UnityAction(this.SetResolution));
		this.GraphicsQualitySelector.ForceSetting(Services.GraphicsSettings.GetInt("graphicsQuality", 3));
		DisplaySettingsMenuSelector graphicsQualitySelector = this.GraphicsQualitySelector;
		graphicsQualitySelector.SettingCallback = (UnityAction)Delegate.Combine(graphicsQualitySelector.SettingCallback, new UnityAction(this.SetGraphicsQuality));
		this.VoiceLanguageSelector.ForceSetting(Services.GameSettings.GetInt(SettingsMenu.VOICE_LANGUAGE_KEY, 0));
		SettingsMenuSelector voiceLanguageSelector = this.VoiceLanguageSelector;
		voiceLanguageSelector.SettingCallback = (UnityAction)Delegate.Combine(voiceLanguageSelector.SettingCallback, new UnityAction(this.SetVoiceOverLanguage));
		this.TextLanguageSelector.ForceSetting(Services.GameSettings.GetInt("textLanguage", 0));
		SettingsMenuSelector textLanguageSelector = this.TextLanguageSelector;
		textLanguageSelector.SettingCallback = (UnityAction)Delegate.Combine(textLanguageSelector.SettingCallback, new UnityAction(this.SetTextLanguage));
		this.SetSettings();
		this.SkipTextSelector.ForceSetting(Services.GameSettings.GetInt("skipText", 0));
		SettingsMenuSelector skipTextSelector = this.SkipTextSelector;
		skipTextSelector.SettingCallback = (UnityAction)Delegate.Combine(skipTextSelector.SettingCallback, new UnityAction(this.SetSkipText));
		this.OneClickInteractCheckBox.ForceSetting(Services.GameSettings.GetInt("oneClickInteract", 0));
		SettingsMenuSelector oneClickInteractCheckBox = this.OneClickInteractCheckBox;
		oneClickInteractCheckBox.SettingCallback = (UnityAction)Delegate.Combine(oneClickInteractCheckBox.SettingCallback, new UnityAction(this.SetOneClickInteract));
		this.InvertYAxisCheckBox.ForceSetting(Services.GameSettings.GetInt("invertYAxis", 0));
		SettingsMenuSelector invertYAxisCheckBox = this.InvertYAxisCheckBox;
		invertYAxisCheckBox.SettingCallback = (UnityAction)Delegate.Combine(invertYAxisCheckBox.SettingCallback, new UnityAction(this.SetInvertYAxis));
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0003E514 File Offset: 0x0003C714
	public void SetSettings()
	{
		this._maxVolumeSliderValue = (int)this.MasterVolumeSlider.maxValue;
		this._maxSensitivitySliderValue = (int)this.CameraSensitivitySlider.maxValue;
		this.SetCameraSensitivity(Services.GameSettings.GetFloat("cameraSENSITIVITY", 1f) * (float)this._maxSensitivitySliderValue);
		this.SetMasterVolume(Services.GameSettings.GetFloat("masterVolume", 1f) * (float)this._maxVolumeSliderValue);
		this.SetSFXVolume(Services.GameSettings.GetFloat("sfxVolume", 1f) * (float)this._maxVolumeSliderValue);
		this.SetMusicVolume(Services.GameSettings.GetFloat("musicVolume", 1f) * (float)this._maxVolumeSliderValue);
		this.SetVoiceOverVolume(Services.GameSettings.GetFloat("voiceVolume", 1f) * (float)this._maxVolumeSliderValue);
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0003E5F0 File Offset: 0x0003C7F0
	public void ResetSettings()
	{
		this.ResolutionSelector.ForceSetting(this.ResolutionSelector.GetNumberOfOptions() - 1);
		this.GraphicsQualitySelector.ForceSetting(3);
		if (SettingsMenu.AllowScreenModeChange())
		{
			this.FullscreenSelector.ForceSetting(0);
		}
		this.VoiceLanguageSelector.ForceSetting(0);
		this.TextLanguageSelector.ForceSetting(0);
		this.SetResolution();
		this.SetFullscreenMode();
		this.SetCameraSensitivity((float)this._maxSensitivitySliderValue);
		this.SetMasterVolume((float)this._maxVolumeSliderValue);
		this.SetSFXVolume((float)this._maxVolumeSliderValue);
		this.SetMusicVolume((float)this._maxVolumeSliderValue);
		this.SetVoiceOverVolume((float)this._maxVolumeSliderValue);
		this.SkipTextSelector.ForceSetting(0);
		this.OneClickInteractCheckBox.ForceSetting(0);
		this.InvertYAxisCheckBox.ForceSetting(0);
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0003E6BD File Offset: 0x0003C8BD
	private void Start()
	{
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x0003E6C0 File Offset: 0x0003C8C0
	public static void ForceResolutionToSavedValue()
	{
		if (SettingsMenu.AllowResolutionChange())
		{
			Resolution resolution = Services.ResolutionProviderService.FindClosestResolutionForCurrentScreen();
			FullScreenMode fullScreenMode = SettingsMenu.GetFullScreenMode();
			string @string = Services.GraphicsSettings.GetString(ResolutionSettingsMenuSelector.ResolutionSettingsKey, string.Empty);
			int num;
			int num2;
			if (!string.IsNullOrEmpty(@string) && ResolutionSettingsMenuSelector.ParseEntry(@string, out num, out num2))
			{
				Resolution resolution2 = new Resolution
				{
					width = num,
					height = num2,
					refreshRateRatio = Screen.currentResolution.refreshRateRatio
				};
				if (Services.ResolutionProviderService.IsResolutionSupported(resolution2))
				{
					resolution = resolution2;
				}
			}
			SettingsMenu.ApplyResolution(resolution.width, resolution.height, fullScreenMode, null);
		}
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x0003E774 File Offset: 0x0003C974
	private static FullScreenMode GetFullScreenMode()
	{
		int @int = Services.GraphicsSettings.GetInt("fullscreen", -1);
		if (@int != -1)
		{
			return SettingsMenu.GetUnityFullScreenMode(@int);
		}
		return Screen.fullScreenMode;
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x0003E7A4 File Offset: 0x0003C9A4
	private static void ApplyResolution(int width, int height, FullScreenMode mode, RefreshRate? refreshRate = null)
	{
		RefreshRate refreshRate2 = ((refreshRate != null) ? refreshRate.Value : Screen.currentResolution.refreshRateRatio);
		if (width != Screen.width || height != Screen.height || !refreshRate2.Equals(Screen.currentResolution.refreshRateRatio))
		{
			Screen.SetResolution(width, height, mode, refreshRate2);
		}
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0003E800 File Offset: 0x0003CA00
	public void SetResolution()
	{
		Resolution resolution;
		if (SettingsMenu.AllowResolutionChange() && this.ResolutionSelector.GetSelectedValue(out resolution))
		{
			FullScreenMode unityFullScreenMode = SettingsMenu.GetUnityFullScreenMode(this.FullscreenSelector.GetSelectedIndex());
			SettingsMenu.ApplyResolution(resolution.width, resolution.height, unityFullScreenMode, new RefreshRate?(resolution.refreshRateRatio));
		}
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0003E854 File Offset: 0x0003CA54
	public void SetGraphicsQuality()
	{
		if (SettingsMenu.AllowQualityChange())
		{
			int selectedIndex = this.GraphicsQualitySelector.GetSelectedIndex();
			if (QualitySettings.GetQualityLevel() != selectedIndex)
			{
				QualitySettings.SetQualityLevel(selectedIndex, true);
			}
		}
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x0003E884 File Offset: 0x0003CA84
	public static void ForceQualityToSavedValue()
	{
		if (SettingsMenu.AllowQualityChange())
		{
			int @int = Services.GraphicsSettings.GetInt("graphicsQuality", 3);
			if (QualitySettings.GetQualityLevel() != @int)
			{
				QualitySettings.SetQualityLevel(@int, true);
			}
		}
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0003E8B8 File Offset: 0x0003CAB8
	private static FullScreenMode GetUnityFullScreenMode(int ourIndex)
	{
		if (!SettingsMenu.AllowScreenModeChange())
		{
			return FullScreenMode.ExclusiveFullScreen;
		}
		if (ourIndex >= 0 && ourIndex < SettingsMenu.supportedModes.Length)
		{
			return SettingsMenu.supportedModes[ourIndex];
		}
		T17Debug.LogError(string.Format("Can not set fullscreen mode to {0}", ourIndex));
		return FullScreenMode.FullScreenWindow;
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x0003E8F0 File Offset: 0x0003CAF0
	public void SetFullscreenMode()
	{
		if (SettingsMenu.AllowResolutionChange() && SettingsMenu.AllowScreenModeChange())
		{
			FullScreenMode unityFullScreenMode = SettingsMenu.GetUnityFullScreenMode(this.FullscreenSelector.GetSelectedIndex());
			if (unityFullScreenMode != Screen.fullScreenMode)
			{
				Screen.fullScreenMode = unityFullScreenMode;
			}
		}
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x0003E92A File Offset: 0x0003CB2A
	public void SetVoiceOverLanguage()
	{
		this.VoiceLanguageSelector.GetSelectedIndex();
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0003E938 File Offset: 0x0003CB38
	public void SetTextLanguage()
	{
		this.TextLanguageSelector.GetSelectedIndex();
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x0003E948 File Offset: 0x0003CB48
	public void SetCameraSensitivity(float sensitivity)
	{
		float num = sensitivity / (float)this._maxSensitivitySliderValue;
		Services.GameSettings.SetFloat("cameraSENSITIVITY", num);
		this.CameraSensitivitySlider.SetValueWithoutNotify(sensitivity);
		this.CameraSensitivitySliderValue.text = string.Format("{0:F0}", sensitivity);
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0003E998 File Offset: 0x0003CB98
	public void SetMasterVolume(float volume)
	{
		float num = volume / (float)this._maxVolumeSliderValue;
		Services.GameSettings.SetFloat("masterVolume", num);
		float num2 = 20f * Mathf.Log10(num);
		num2 += 1f;
		if ((double)num < 0.01)
		{
			num2 = -80f;
		}
		this.MasterVolumeMixer.SetFloat("master_volume", num2);
		this.MasterVolumeSlider.SetValueWithoutNotify(volume);
		this.MasterVolumeSliderValue.text = string.Format("{0:F0}", volume);
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0003EA20 File Offset: 0x0003CC20
	public void SetSFXVolume(float volume)
	{
		float num = volume / (float)this._maxVolumeSliderValue;
		Services.GameSettings.SetFloat("sfxVolume", num);
		float num2 = 20f * Mathf.Log10(num);
		if ((double)num < 0.01)
		{
			num2 = -80f;
		}
		this.MasterVolumeMixer.SetFloat("sfx_volume", num2);
		this.SFXVolumeSlider.SetValueWithoutNotify(volume);
		this.SFXVolumeSliderValue.text = string.Format("{0:F0}", volume);
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0003EAA0 File Offset: 0x0003CCA0
	public void SetMusicVolume(float volume)
	{
		float num = volume / (float)this._maxVolumeSliderValue;
		Services.GameSettings.SetFloat("musicVolume", num);
		float num2 = 20f * Mathf.Log10(num);
		num2 -= 16f;
		if ((double)num < 0.01)
		{
			num2 = -80f;
		}
		this.MasterVolumeMixer.SetFloat("music_volume", num2);
		this.MusicVolumeSlider.SetValueWithoutNotify(volume);
		this.MusicVolumeSliderValue.text = string.Format("{0:F0}", volume);
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0003EB28 File Offset: 0x0003CD28
	public void SetMusicFrequency()
	{
		this.MasterVolumeMixer.SetFloat("music_frequency", 1f);
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0003EB40 File Offset: 0x0003CD40
	public void SetVoiceOverVolume(float volume)
	{
		float num = volume / (float)this._maxVolumeSliderValue;
		Services.GameSettings.SetFloat("voiceVolume", num);
		float num2 = 20f * Mathf.Log10(num);
		num2 += 1f;
		if ((double)num < 0.01)
		{
			num2 = -80f;
		}
		this.MasterVolumeMixer.SetFloat("voice_volume", num2);
		this.VoiceVolumeSlider.SetValueWithoutNotify(volume);
		this.VoiceVolumeSliderValue.text = string.Format("{0:F0}", volume);
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x0003EBC8 File Offset: 0x0003CDC8
	public void SetSkipText()
	{
		int selectedIndex = this.SkipTextSelector.GetSelectedIndex();
		Services.GameSettings.SetInt("skipText", selectedIndex);
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x0003EBF4 File Offset: 0x0003CDF4
	public void SetOneClickInteract()
	{
		int selectedIndex = this.OneClickInteractCheckBox.GetSelectedIndex();
		Services.GameSettings.SetInt("oneClickInteract", selectedIndex);
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0003EC20 File Offset: 0x0003CE20
	public void SetInvertYAxis()
	{
		int selectedIndex = this.InvertYAxisCheckBox.GetSelectedIndex();
		Services.GameSettings.SetInt("invertYAxis", selectedIndex);
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0003EC4C File Offset: 0x0003CE4C
	public void OnBackPressed()
	{
		if (this._pendingExitMenu)
		{
			return;
		}
		DoCodeAnimation component = base.GetComponent<DoCodeAnimation>();
		if (component != null && component.IsAnimating)
		{
			return;
		}
		this._pendingExitMenu = true;
		if (this.ApplyDisplaySettingsButton != null && this.ApplyDisplaySettingsButton.gameObject != null && this.ApplyDisplaySettingsButton.interactable && this.ApplyDisplaySettingsButton.gameObject.activeInHierarchy)
		{
			UIDialogManager.Instance.ShowYesNoDialog("Display Settings Modified", "The display settings have unsaved changes. Do you want to discard these changes?", new Action(this.OnDisplaySettingRevert), delegate
			{
				this._pendingExitMenu = false;
			});
			return;
		}
		this.HandleBackPressed_Phase2();
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0003ECF6 File Offset: 0x0003CEF6
	protected void HandleBackPressed_Phase2()
	{
		if (Services.GameSettings.IsDirty || Services.GraphicsSettings.IsDirty)
		{
			this.SaveSettings();
			return;
		}
		this.CloseMenu();
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0003ED1D File Offset: 0x0003CF1D
	private void CloseMenu()
	{
		UnityEvent onCloseMenu = this.OnCloseMenu;
		if (onCloseMenu == null)
		{
			return;
		}
		onCloseMenu.Invoke();
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0003ED2F File Offset: 0x0003CF2F
	private void SaveSettings()
	{
		Services.GameSettings.Save(delegate(SaveGameError result)
		{
			if (this.HandleSaveResult(result))
			{
				Services.GraphicsSettings.Save(delegate(SaveGameError result)
				{
					if (this.HandleSaveResult(result))
					{
						this.CloseMenu();
					}
				}, false);
			}
		}, false);
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0003ED48 File Offset: 0x0003CF48
	private bool HandleSaveResult(SaveGameError result)
	{
		bool flag = false;
		SaveGameErrorType errorType = result.ErrorType;
		if (errorType != SaveGameErrorType.None)
		{
			if (errorType != SaveGameErrorType.OutOfMemory)
			{
				UIDialogManager.Instance.ShowDialog("File Error", "Failed to save the settings. Select Retry to try again or Continue to continue without saving ", "Retry", new Action(this.SaveSettings), "Continue", new Action(this.CloseMenu), "", null, 0, true);
			}
			else
			{
				UIDialogManager.Instance.ShowDialog("File Error", "Out of disk space while trying to save the settings. Please free up some disk space and then select Retry to try again or select Continue to continue without saving", "Retry", new Action(this.SaveSettings), "Continue", new Action(this.CloseMenu), "", null, 0, true);
			}
		}
		else
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x0003EDF4 File Offset: 0x0003CFF4
	public void OnApplyDisplaySettings()
	{
		this.ResolutionSelector.ApplyCurrentSetting();
		this.FullscreenSelector.ApplyCurrentSetting();
		this.GraphicsQualitySelector.ApplyCurrentSetting();
		UIDialogManager.Instance.ShowDialog("Change Display Settings", "Are you sure you want to keep these settings?", "Apply", new Action(this.OnDisplaySettingKeep), "Revert", new Action(this.OnDisplaySettingRevert), "", null, 10, true);
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x0003EE62 File Offset: 0x0003D062
	private void OnDisplaySettingKeep()
	{
		this.ResolutionSelector.CommitCurrentSetting();
		this.FullscreenSelector.CommitCurrentSetting();
		this.GraphicsQualitySelector.CommitCurrentSetting();
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0003EE85 File Offset: 0x0003D085
	private void OnDisplaySettingRevert()
	{
		this.ResolutionSelector.RevertCurrentSettingToCommittedValue();
		this.FullscreenSelector.RevertCurrentSettingToCommittedValue();
		this.GraphicsQualitySelector.RevertCurrentSettingToCommittedValue();
		if (this._pendingExitMenu)
		{
			this.HandleBackPressed_Phase2();
		}
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0003EEB8 File Offset: 0x0003D0B8
	private void HandleApplyButtonEnabledState()
	{
		bool flag = false;
		if (this.ResolutionSelector.IsDirty)
		{
			flag = true;
		}
		else if (this.FullscreenSelector.IsDirty)
		{
			flag = true;
		}
		else if (this.GraphicsQualitySelector.IsDirty)
		{
			flag = true;
		}
		if (this.ApplyDisplaySettingsButton.interactable != flag)
		{
			this.ApplyDisplaySettingsButton.interactable = flag;
		}
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0003EF12 File Offset: 0x0003D112
	private static bool AllowScreenModeChange()
	{
		return Services.PlatformService == null || !Services.PlatformService.HasFlag(EPlatformFlags.ForceFullScreen);
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0003EF2B File Offset: 0x0003D12B
	private static bool AllowResolutionChange()
	{
		return Services.PlatformService == null || Services.PlatformService.HasFlag(EPlatformFlags.AllowResolutionChanges);
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0003EF42 File Offset: 0x0003D142
	private static bool AllowQualityChange()
	{
		return Services.PlatformService == null || Services.PlatformService.HasFlag(EPlatformFlags.AllowResolutionChanges);
	}

	// Token: 0x040009CA RID: 2506
	public ResolutionSettingsMenuSelector ResolutionSelector;

	// Token: 0x040009CB RID: 2507
	public DisplaySettingsMenuSelector FullscreenSelector;

	// Token: 0x040009CC RID: 2508
	public DisplaySettingsMenuSelector GraphicsQualitySelector;

	// Token: 0x040009CD RID: 2509
	public SettingsMenuSelector VoiceLanguageSelector;

	// Token: 0x040009CE RID: 2510
	public SettingsMenuSelector TextLanguageSelector;

	// Token: 0x040009CF RID: 2511
	public Slider CameraSensitivitySlider;

	// Token: 0x040009D0 RID: 2512
	public Slider MasterVolumeSlider;

	// Token: 0x040009D1 RID: 2513
	public Slider SFXVolumeSlider;

	// Token: 0x040009D2 RID: 2514
	public Slider MusicVolumeSlider;

	// Token: 0x040009D3 RID: 2515
	public Slider VoiceVolumeSlider;

	// Token: 0x040009D4 RID: 2516
	public TextMeshProUGUI CameraSensitivitySliderValue;

	// Token: 0x040009D5 RID: 2517
	public TextMeshProUGUI MasterVolumeSliderValue;

	// Token: 0x040009D6 RID: 2518
	public TextMeshProUGUI SFXVolumeSliderValue;

	// Token: 0x040009D7 RID: 2519
	public TextMeshProUGUI MusicVolumeSliderValue;

	// Token: 0x040009D8 RID: 2520
	public TextMeshProUGUI VoiceVolumeSliderValue;

	// Token: 0x040009D9 RID: 2521
	public Button ApplyDisplaySettingsButton;

	// Token: 0x040009DA RID: 2522
	public SettingsMenuSelector SkipTextSelector;

	// Token: 0x040009DB RID: 2523
	public SettingsMenuSelector OneClickInteractCheckBox;

	// Token: 0x040009DC RID: 2524
	public SettingsMenuSelector InvertYAxisCheckBox;

	// Token: 0x040009DD RID: 2525
	public AudioMixer MasterVolumeMixer;

	// Token: 0x040009DE RID: 2526
	public UnityEvent OnCloseMenu = new UnityEvent();

	// Token: 0x040009DF RID: 2527
	[SerializeField]
	private IsSelectableRegistered backButton;

	// Token: 0x040009E0 RID: 2528
	private const string CAMERA_SENSITIVITY = "cameraSENSITIVITY";

	// Token: 0x040009E1 RID: 2529
	private const string MASTER_VOL_KEY = "masterVolume";

	// Token: 0x040009E2 RID: 2530
	private const string SFX_VOL_KEY = "sfxVolume";

	// Token: 0x040009E3 RID: 2531
	private const string MUSIC_VOL_KEY = "musicVolume";

	// Token: 0x040009E4 RID: 2532
	public const string VOICE_VOL_KEY = "voiceVolume";

	// Token: 0x040009E5 RID: 2533
	public const string ONE_CLICK_INTERACT = "oneClickInteract";

	// Token: 0x040009E6 RID: 2534
	private const string FULLSCREEN_KEY = "fullscreen";

	// Token: 0x040009E7 RID: 2535
	private const string GRAPHICS_QUALITY = "graphicsQuality";

	// Token: 0x040009E8 RID: 2536
	public static string VOICE_LANGUAGE_KEY = "voiceLanguage";

	// Token: 0x040009E9 RID: 2537
	private const string TEXT_LANGUAGE_KEY = "textLanguage";

	// Token: 0x040009EA RID: 2538
	private const string SKIP_TEXT = "skipText";

	// Token: 0x040009EB RID: 2539
	public const string INVERT_YAXIS = "invertYAxis";

	// Token: 0x040009EC RID: 2540
	private const string CONFIRM_APPLY_TITLE = "Change Display Settings";

	// Token: 0x040009ED RID: 2541
	private const string CONFIRM_APPLY_BODY = "Are you sure you want to keep these settings?";

	// Token: 0x040009EE RID: 2542
	private const string CONFIRM_APPLY_KEEP = "Apply";

	// Token: 0x040009EF RID: 2543
	private const string CONFIRM_APPLY_REVERT = "Revert";

	// Token: 0x040009F0 RID: 2544
	private const string DISPLAY_SETTINGS_MODIFIED = "Display Settings Modified";

	// Token: 0x040009F1 RID: 2545
	private const string DISPLAY_SETTINGS_MODIFIED_BODY = "The display settings have unsaved changes. Do you want to discard these changes?";

	// Token: 0x040009F2 RID: 2546
	private const string CONFIRM_APPLY = "Apply";

	// Token: 0x040009F3 RID: 2547
	private const string CONFIRM_DISCARD = "Discard";

	// Token: 0x040009F4 RID: 2548
	private const int kAutoRevertDisplaySettingsTime = 10;

	// Token: 0x040009F5 RID: 2549
	private const int defaultQualityIndex = 3;

	// Token: 0x040009F6 RID: 2550
	private static FullScreenMode[] supportedModes = new FullScreenMode[]
	{
		FullScreenMode.ExclusiveFullScreen,
		FullScreenMode.FullScreenWindow,
		FullScreenMode.Windowed
	};

	// Token: 0x040009F7 RID: 2551
	private bool _pendingExitMenu;

	// Token: 0x040009F8 RID: 2552
	private GameObject previousSelectedObj;

	// Token: 0x040009F9 RID: 2553
	private GameObject lastSelectedOption;

	// Token: 0x040009FA RID: 2554
	private List<MonoBehaviour> selectableOptions;

	// Token: 0x040009FB RID: 2555
	private Canvas _parentCanvas;

	// Token: 0x040009FC RID: 2556
	private int _maxVolumeSliderValue = 10;

	// Token: 0x040009FD RID: 2557
	private int _maxSensitivitySliderValue = 10;

	// Token: 0x0200033A RID: 826
	private enum Languages
	{
		// Token: 0x040012C6 RID: 4806
		English,
		// Token: 0x040012C7 RID: 4807
		Japanese
	}
}
