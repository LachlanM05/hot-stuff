using System;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EE RID: 238
public class CutsceneSkipper : MonoBehaviour
{
	// Token: 0x060007E6 RID: 2022 RVA: 0x0002D14A File Offset: 0x0002B34A
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		this.Hide();
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0002D164 File Offset: 0x0002B364
	private void Update()
	{
		if (this.player != null)
		{
			if (this.player.GetButton(12))
			{
				if (!this.shown)
				{
					this.Show();
				}
				this.heldTime += Time.deltaTime;
				this.UpdateTime(this.heldTime);
				return;
			}
			this.heldTime = 0f;
			if (this.shown)
			{
				this.Hide();
			}
		}
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0002D1D0 File Offset: 0x0002B3D0
	public void UpdateTime(float mouseHoldTime)
	{
		this.slider.value = Mathf.InverseLerp(0f, this.timeToSkip, mouseHoldTime);
		if (this.slider.value >= 1f)
		{
			this.Hide();
			this.skipped = true;
			Singleton<TutorialController>.Instance.SkipIntroBlackScreen();
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0002D224 File Offset: 0x0002B424
	public void Show()
	{
		this.shown = true;
		GameObject[] array = this.objectsToTurnOn;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0002D258 File Offset: 0x0002B458
	public void Hide()
	{
		this.shown = false;
		GameObject[] array = this.objectsToTurnOn;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x04000716 RID: 1814
	private Player player;

	// Token: 0x04000717 RID: 1815
	private float heldTime;

	// Token: 0x04000718 RID: 1816
	private bool shown;

	// Token: 0x04000719 RID: 1817
	public bool skipped;

	// Token: 0x0400071A RID: 1818
	[Header("References")]
	[SerializeField]
	private Slider slider;

	// Token: 0x0400071B RID: 1819
	[SerializeField]
	private GameObject[] objectsToTurnOn;

	// Token: 0x0400071C RID: 1820
	[SerializeField]
	private float timeToSkip = 2f;

	// Token: 0x02000304 RID: 772
	private enum CUTSCENE
	{
		// Token: 0x04001204 RID: 4612
		TUTORIAL,
		// Token: 0x04001205 RID: 4613
		ENDING
	}
}
