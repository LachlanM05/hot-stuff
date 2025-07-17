using System;
using T17.Services;
using Team17.Platform.SaveGame;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class SaveIndicatorController : MonoBehaviour
{
	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000D26 RID: 3366 RVA: 0x0004B8D9 File Offset: 0x00049AD9
	public bool IsInProgressIndicatorActive
	{
		get
		{
			return this._InProgressIndicator.activeInHierarchy;
		}
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x0004B8E6 File Offset: 0x00049AE6
	private void Awake()
	{
		SaveIndicatorController.Instance = this;
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x0004B8EE File Offset: 0x00049AEE
	private void Start()
	{
		if (this._ForcePermanentDisplay)
		{
			this.ShowIndicator();
			return;
		}
		this.HideAndReset();
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x0004B908 File Offset: 0x00049B08
	private void Initialise()
	{
		if (!this._IsInitialised && Services.SaveGameService != null)
		{
			Services.SaveGameService.OnSaveBegin += this.OnSaveBegin;
			Save.onGameSave += this.OnSaveBegin;
			Services.SaveGameService.OnSaveCompleted += this.OnSaveCompleted;
			Save.onGameSaveCompleted += this.OnSaveCompleted;
			this._IsInitialised = true;
		}
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x0004B97C File Offset: 0x00049B7C
	private void OnDestroy()
	{
		this.HideIndicator();
		if (Services.SaveGameService != null)
		{
			Services.SaveGameService.OnSaveBegin -= this.OnSaveBegin;
			Services.SaveGameService.OnSaveCompleted -= this.OnSaveCompleted;
		}
		Save.onGameSave -= this.OnSaveBegin;
		Save.onGameSaveCompleted -= this.OnSaveCompleted;
		this._IsInitialised = false;
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0004B9EB File Offset: 0x00049BEB
	private void Update()
	{
		if (!this._ForcePermanentDisplay)
		{
			if (!this._IsInitialised)
			{
				this.Initialise();
			}
			if (this._pendingHideTime != 3.4028235E+38f && Time.realtimeSinceStartup > this._pendingHideTime)
			{
				this.HideAndReset();
			}
		}
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x0004BA23 File Offset: 0x00049C23
	private void HideIndicator()
	{
		if (this._InProgressIndicator.activeSelf)
		{
			this._InProgressIndicator.SetActive(false);
		}
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x0004BA3E File Offset: 0x00049C3E
	public void ShowIndicator()
	{
		this._InProgressIndicator.SetActive(true);
		this._timeWhenShown = Time.realtimeSinceStartup;
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x0004BA57 File Offset: 0x00049C57
	private void OnSaveBegin()
	{
		this._refCount++;
		if (!this._InProgressIndicator.activeSelf)
		{
			this.ShowIndicator();
			this._pendingHideTime = float.MaxValue;
		}
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x0004BA88 File Offset: 0x00049C88
	private void OnSaveCompleted(SaveGameError result)
	{
		this._refCount--;
		if (this._refCount != 0)
		{
			if (this._refCount < 0)
			{
				this._refCount = 0;
			}
			return;
		}
		if (!this._InProgressIndicator.activeSelf)
		{
			this.HideAndReset();
			return;
		}
		if (Time.realtimeSinceStartup < this._timeWhenShown + this._MinimumDisplayTime)
		{
			this._pendingHideTime = this._timeWhenShown + this._MinimumDisplayTime;
			return;
		}
		this.HideAndReset();
	}

	// Token: 0x06000D30 RID: 3376 RVA: 0x0004BB00 File Offset: 0x00049D00
	public void HideAndReset()
	{
		this.HideIndicator();
		this._pendingHideTime = float.MaxValue;
		this._timeWhenShown = float.MaxValue;
		this._refCount = 0;
		if (Singleton<AudioManager>.Instance && !SaveIndicatorController.triggeredByAutoSave && BetterPlayerControl.Instance)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_save_complete, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		SaveIndicatorController.triggeredByAutoSave = true;
	}

	// Token: 0x04000BE0 RID: 3040
	public static SaveIndicatorController Instance;

	// Token: 0x04000BE1 RID: 3041
	[SerializeField]
	private GameObject _InProgressIndicator;

	// Token: 0x04000BE2 RID: 3042
	[Tooltip("Minimum length of time the save indicator stays on screen for (in seconds)")]
	[SerializeField]
	private float _MinimumDisplayTime = 3f;

	// Token: 0x04000BE3 RID: 3043
	[Tooltip("If set, then the save indicator will be permanently on screen until destroy is called.")]
	[SerializeField]
	private bool _ForcePermanentDisplay;

	// Token: 0x04000BE4 RID: 3044
	private float _pendingHideTime = float.MaxValue;

	// Token: 0x04000BE5 RID: 3045
	private float _timeWhenShown = float.MaxValue;

	// Token: 0x04000BE6 RID: 3046
	private int _refCount;

	// Token: 0x04000BE7 RID: 3047
	private bool _IsInitialised;

	// Token: 0x04000BE8 RID: 3048
	public static bool triggeredByAutoSave;
}
