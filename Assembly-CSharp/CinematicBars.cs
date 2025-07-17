using System;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class CinematicBars : MonoBehaviour
{
	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000E24 RID: 3620 RVA: 0x0004E5F7 File Offset: 0x0004C7F7
	// (set) Token: 0x06000E23 RID: 3619 RVA: 0x0004E5EF File Offset: 0x0004C7EF
	public static CinematicBars Instance { get; private set; }

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0004E606 File Offset: 0x0004C806
	// (set) Token: 0x06000E25 RID: 3621 RVA: 0x0004E5FE File Offset: 0x0004C7FE
	public static CinematicBars.ShowState CurrentShowState { get; private set; }

	// Token: 0x06000E27 RID: 3623 RVA: 0x0004E60D File Offset: 0x0004C80D
	public void Awake()
	{
		if (CinematicBars.Instance != null)
		{
			Object.Destroy(this);
			return;
		}
		CinematicBars.Instance = this;
		this.TopBar.SetActive(false);
		this.BottomBar.SetActive(false);
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x0004E641 File Offset: 0x0004C841
	public void OnDestroy()
	{
		CinematicBars.Instance = null;
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x0004E649 File Offset: 0x0004C849
	public static bool IsCinematicBarsOn()
	{
		return CinematicBars.Instance.TopBar.activeInHierarchy;
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x0004E65C File Offset: 0x0004C85C
	public static void Show(float time = -1f)
	{
		if (CinematicBars.Instance != null)
		{
			CinematicBars.Instance.InternalShow(time);
			CinematicBars.Instance.Subtitles.SetActive(true);
			Singleton<CanvasUIManager>.Instance.Hide();
			Singleton<PhoneManager>.Instance.BlockPhoneOpening = true;
			if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS))
			{
				CinematicBars.Instance.Subtitles.SetActive(false);
			}
		}
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x0004E6C7 File Offset: 0x0004C8C7
	public static void Hide(float time = -1f)
	{
		if (CinematicBars.Instance != null)
		{
			CinematicBars.Instance.InternalHide(time);
			CinematicBars.Instance.Subtitles.SetActive(false);
			Singleton<CanvasUIManager>.Instance.Show();
			Singleton<PhoneManager>.Instance.BlockPhoneOpening = false;
		}
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x0004E708 File Offset: 0x0004C908
	private void InternalShow(float time = -1f)
	{
		switch (CinematicBars.CurrentShowState)
		{
		case CinematicBars.ShowState.OffScreen:
			CinematicBars.CurrentShowState = CinematicBars.ShowState.Showing;
			this.HideHud();
			this.TopBar.SetActive(true);
			this.BottomBar.SetActive(true);
			this._transitionStartTime = Time.realtimeSinceStartup;
			this.SetBarsPositions(0f);
			return;
		case CinematicBars.ShowState.Showing:
		case CinematicBars.ShowState.OnScreen:
			break;
		case CinematicBars.ShowState.Hiding:
		{
			CinematicBars.CurrentShowState = CinematicBars.ShowState.Showing;
			float num = (Time.realtimeSinceStartup - this._transitionStartTime) / this.HideDuration;
			this._transitionStartTime = Time.realtimeSinceStartup - (1f - num) * this.ShowDuration;
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x0004E7A4 File Offset: 0x0004C9A4
	private void InternalHide(float time = -1f)
	{
		switch (CinematicBars.CurrentShowState)
		{
		case CinematicBars.ShowState.OffScreen:
		case CinematicBars.ShowState.Hiding:
			break;
		case CinematicBars.ShowState.Showing:
		{
			CinematicBars.CurrentShowState = CinematicBars.ShowState.Hiding;
			float num = (Time.realtimeSinceStartup - this._transitionStartTime) / this.ShowDuration;
			this._transitionStartTime = Time.realtimeSinceStartup - (1f - num) * this.HideDuration;
			break;
		}
		case CinematicBars.ShowState.OnScreen:
			CinematicBars.CurrentShowState = CinematicBars.ShowState.Hiding;
			this._transitionStartTime = Time.realtimeSinceStartup;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x0004E814 File Offset: 0x0004CA14
	private void Start()
	{
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x0004E818 File Offset: 0x0004CA18
	private void Update()
	{
		switch (CinematicBars.CurrentShowState)
		{
		case CinematicBars.ShowState.OffScreen:
		case CinematicBars.ShowState.OnScreen:
			break;
		case CinematicBars.ShowState.Showing:
		{
			float num = (Time.realtimeSinceStartup - this._transitionStartTime) / this.ShowDuration;
			if (num >= 1f || num < 0f)
			{
				this.SetBarsPositions(1f);
				CinematicBars.CurrentShowState = CinematicBars.ShowState.OnScreen;
				return;
			}
			this.SetBarsPositions(num);
			return;
		}
		case CinematicBars.ShowState.Hiding:
		{
			float num2 = (Time.realtimeSinceStartup - this._transitionStartTime) / this.HideDuration;
			if (num2 >= 1f || num2 < 0f)
			{
				this.SetBarsPositions(0f);
				this.TopBar.SetActive(false);
				this.BottomBar.SetActive(false);
				CinematicBars.CurrentShowState = CinematicBars.ShowState.OffScreen;
				this.RestoreHud();
				return;
			}
			this.SetBarsPositions(1f - num2);
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0004E8E4 File Offset: 0x0004CAE4
	private void SetBarsPositions(float normalisedTime)
	{
		RectTransform rectTransform = this.TopBar.transform as RectTransform;
		Vector3 vector = rectTransform.anchoredPosition;
		vector.y = -rectTransform.rect.height * normalisedTime;
		rectTransform.anchoredPosition = vector;
		rectTransform = this.BottomBar.transform as RectTransform;
		vector = rectTransform.anchoredPosition;
		vector.y = rectTransform.rect.height * normalisedTime;
		rectTransform.anchoredPosition = vector;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0004E972 File Offset: 0x0004CB72
	private void HideHud()
	{
		this._cachedHudActive = this.HUD.activeInHierarchy;
		this.HUD.SetActive(false);
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x0004E991 File Offset: 0x0004CB91
	private void RestoreHud()
	{
		this.HUD.SetActive(true);
	}

	// Token: 0x04000C90 RID: 3216
	[SerializeField]
	private GameObject TopBar;

	// Token: 0x04000C91 RID: 3217
	[SerializeField]
	private GameObject BottomBar;

	// Token: 0x04000C92 RID: 3218
	[Tooltip("Time it takes for the Bars bars to scroll on to the screen (in seconds)")]
	[SerializeField]
	private float ShowDuration = 0.333f;

	// Token: 0x04000C93 RID: 3219
	[Tooltip("Time it takes for the bars to scroll off the screen (in seconds)")]
	[SerializeField]
	private float HideDuration = 0.333f;

	// Token: 0x04000C94 RID: 3220
	[SerializeField]
	private GameObject HUD;

	// Token: 0x04000C95 RID: 3221
	[SerializeField]
	private GameObject Subtitles;

	// Token: 0x04000C96 RID: 3222
	private bool _cachedHudActive;

	// Token: 0x04000C99 RID: 3225
	private float _transitionStartTime;

	// Token: 0x02000381 RID: 897
	public enum ShowState
	{
		// Token: 0x040013CE RID: 5070
		OffScreen,
		// Token: 0x040013CF RID: 5071
		Showing,
		// Token: 0x040013D0 RID: 5072
		OnScreen,
		// Token: 0x040013D1 RID: 5073
		Hiding
	}
}
