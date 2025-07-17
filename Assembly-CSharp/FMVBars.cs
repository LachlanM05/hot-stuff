using System;
using Rewired;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class FMVBars : Singleton<FMVBars>
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0004EC3B File Offset: 0x0004CE3B
	// (set) Token: 0x06000E45 RID: 3653 RVA: 0x0004EC32 File Offset: 0x0004CE32
	public FMVBars.ShowState CurrentShowState { get; private set; }

	// Token: 0x06000E47 RID: 3655 RVA: 0x0004EC43 File Offset: 0x0004CE43
	public override void AwakeSingleton()
	{
		this.TopBar.SetActive(false);
		this.BottomBar.SetActive(false);
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x0004EC60 File Offset: 0x0004CE60
	public void Show(float time = -1f, float delay = 0f)
	{
		switch (this.CurrentShowState)
		{
		case FMVBars.ShowState.OffScreen:
			this.CurrentShowState = FMVBars.ShowState.Showing;
			this.TopBar.SetActive(true);
			this.BottomBar.SetActive(true);
			this._transitionStartTime = Time.realtimeSinceStartup;
			this.SetBarsPositions(0f);
			return;
		case FMVBars.ShowState.Showing:
		case FMVBars.ShowState.OnScreen:
			break;
		case FMVBars.ShowState.Hiding:
		{
			this.CurrentShowState = FMVBars.ShowState.Showing;
			float num = (Time.realtimeSinceStartup - this._transitionStartTime) / this.HideDuration;
			this._transitionStartTime = Time.realtimeSinceStartup - (1f - num) * this.ShowDuration;
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x0004ECF8 File Offset: 0x0004CEF8
	public void Hide(float time = -1f)
	{
		switch (this.CurrentShowState)
		{
		case FMVBars.ShowState.OffScreen:
		case FMVBars.ShowState.Hiding:
			break;
		case FMVBars.ShowState.Showing:
		{
			this.CurrentShowState = FMVBars.ShowState.Hiding;
			float num = (Time.realtimeSinceStartup - this._transitionStartTime) / this.ShowDuration;
			this._transitionStartTime = Time.realtimeSinceStartup - (1f - num) * this.HideDuration;
			break;
		}
		case FMVBars.ShowState.OnScreen:
			this.CurrentShowState = FMVBars.ShowState.Hiding;
			this._transitionStartTime = Time.realtimeSinceStartup;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x0004ED6B File Offset: 0x0004CF6B
	private void Start()
	{
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x0004ED70 File Offset: 0x0004CF70
	private void Update()
	{
		Player player = ReInput.players.GetPlayer(0);
		if (player.GetButtonDown("IncrementTime"))
		{
			this.Show(-1f, 0f);
		}
		if (player.GetButtonDown("DecrementTime"))
		{
			this.Hide(-1f);
		}
		if (player.GetButtonDown("ToggleUI"))
		{
			this.Hide(-1f);
		}
		if (player.GetButtonDown("ResetCharges"))
		{
			this.Hide(-1f);
		}
		switch (this.CurrentShowState)
		{
		case FMVBars.ShowState.OffScreen:
		case FMVBars.ShowState.OnScreen:
			break;
		case FMVBars.ShowState.Showing:
		{
			float num = (Time.realtimeSinceStartup - this._transitionStartTime) / this.ShowDuration;
			if (num >= 1f)
			{
				this.SetBarsPositions(1f);
				this.CurrentShowState = FMVBars.ShowState.OnScreen;
				return;
			}
			this.SetBarsPositions(num);
			return;
		}
		case FMVBars.ShowState.Hiding:
		{
			float num2 = (Time.realtimeSinceStartup - this._transitionStartTime) / this.HideDuration;
			if (num2 >= 1f)
			{
				this.SetBarsPositions(0f);
				this.TopBar.SetActive(false);
				this.BottomBar.SetActive(false);
				this.CurrentShowState = FMVBars.ShowState.OffScreen;
				return;
			}
			this.SetBarsPositions(1f - num2);
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x0004EE98 File Offset: 0x0004D098
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

	// Token: 0x04000CA5 RID: 3237
	[SerializeField]
	private GameObject TopBar;

	// Token: 0x04000CA6 RID: 3238
	[SerializeField]
	private GameObject BottomBar;

	// Token: 0x04000CA7 RID: 3239
	[Tooltip("Time it takes for the FMV bars to scroll on to the screen (in seconds)")]
	[SerializeField]
	private float ShowDuration = 0.333f;

	// Token: 0x04000CA8 RID: 3240
	[Tooltip("Time it takes for the FMV bars to scroll off the screen (in seconds)")]
	[SerializeField]
	private float HideDuration = 0.333f;

	// Token: 0x04000CAA RID: 3242
	private float _transitionStartTime;

	// Token: 0x02000383 RID: 899
	public enum ShowState
	{
		// Token: 0x040013D6 RID: 5078
		OffScreen,
		// Token: 0x040013D7 RID: 5079
		Showing,
		// Token: 0x040013D8 RID: 5080
		OnScreen,
		// Token: 0x040013D9 RID: 5081
		Hiding
	}
}
