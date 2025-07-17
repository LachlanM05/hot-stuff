using System;
using TMPro;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class CreditsScreen : MonoBehaviour
{
	// Token: 0x060007E0 RID: 2016 RVA: 0x0002CD2E File Offset: 0x0002AF2E
	private void Awake()
	{
		if (this.uiUtilities == null)
		{
			this.uiUtilities = Object.FindObjectOfType<UIUtilities>();
		}
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x0002CD49 File Offset: 0x0002AF49
	private void OnEnable()
	{
		this.waitTimer = 0f;
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0002CD58 File Offset: 0x0002AF58
	private void Update()
	{
		if (this.waitTimer != 0f)
		{
			if (this.playCredits && (this.waitTimer -= Time.deltaTime) <= 0f)
			{
				this.playCredits = false;
				if (this.mainMenuCredits)
				{
					base.gameObject.GetComponent<MainMenuSubScreen>().ReturnToMainMenuScreen();
					return;
				}
				if (!Locket.IsLocketEnabled())
				{
					Singleton<PhoneManager>.Instance.BackCredits();
					return;
				}
				if (Locket.s_Instance != null)
				{
					Locket.s_Instance.Next();
					return;
				}
			}
		}
		else if (this.playCredits && this.childGameCredits)
		{
			this.childGameCredits.transform.localPosition += new Vector3(0f, Time.deltaTime * this.speed, 0f);
			if (this.childGameCredits.transform.localPosition.y > 35800f)
			{
				this.waitTimer = 5f;
			}
		}
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0002CE58 File Offset: 0x0002B058
	public void StartCredits()
	{
		if (Singleton<InkController>.Instance)
		{
			bool flag;
			bool.TryParse(Singleton<InkController>.Instance.GetVariable("credit"), out flag);
			if (flag)
			{
				this.tmp_credits.text = this.tmp_credits.text.Replace("***", Singleton<Save>.Instance.GetPlayerName());
			}
			else
			{
				this.tmp_credits.text = this.tmp_credits.text.Replace("***", "");
			}
		}
		else
		{
			this.tmp_credits.text = this.tmp_credits.text.Replace("***", "");
		}
		if (Singleton<PhoneManager>.Instance.phoneOpened && this.uiUtilities != null)
		{
			this.uiUtilities.SetMusicFrequency(1f);
			this.uiUtilities.SetMusicResonance(1f);
		}
		this.childGameCredits.transform.localPosition = new Vector3(this.childGameCredits.transform.localPosition.x, 0f, this.childGameCredits.transform.localPosition.z);
		this.playCredits = true;
		Singleton<AudioManager>.Instance.StopSFXTrackGroup(SFX_SUBGROUP.STINGER, 1f);
		if (this.trueCredits)
		{
			Singleton<AudioManager>.Instance.StopTrack("epilogue_ending", 1f);
			Singleton<AudioManager>.Instance.StopTrack("locket", 1f);
			Singleton<AudioManager>.Instance.PlayTrack("credits", AUDIO_TYPE.SPECIAL, true, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
		}
		if (!this.mainMenuCredits && !this.trueCredits)
		{
			Singleton<AudioManager>.Instance.StopTrack("locket", 1f);
			Singleton<AudioManager>.Instance.PlayTrack("epilogue_short", AUDIO_TYPE.MUSIC, true, false, 1f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
		}
		if (!this.trueCredits)
		{
			ControllerMenuUI.SetCurrentlySelected(this.backButton, ControllerMenuUI.Direction.Down, false, false);
		}
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0002D04C File Offset: 0x0002B24C
	public void StopCredits()
	{
		if (!this.mainMenuCredits && !this.trueCredits)
		{
			Singleton<AudioManager>.Instance.StopTrack("epilogue_short", 1f);
		}
		if (Locket.IsLocketEnabled() && Locket.GetCurrScene() > 0)
		{
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
			Singleton<AudioManager>.Instance.StopTrack("credits", 1f);
			Singleton<AudioManager>.Instance.PlayTrack("locket", AUDIO_TYPE.MUSIC, false, false, 1f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
			return;
		}
		if (Singleton<PhoneManager>.Instance.phoneOpened)
		{
			this.uiUtilities.SetMusicFrequency(800f);
			this.uiUtilities.SetMusicResonance(1f);
		}
		this.playCredits = false;
		if (!Locket.IsLocketEnabled())
		{
			Singleton<AudioManager>.Instance.StopTrack("credits", 4f);
			Singleton<AudioManager>.Instance.StopTrack("locket", 4f);
		}
	}

	// Token: 0x0400070B RID: 1803
	public bool playCredits;

	// Token: 0x0400070C RID: 1804
	public GameObject childGameCredits;

	// Token: 0x0400070D RID: 1805
	public float speed = 140f;

	// Token: 0x0400070E RID: 1806
	public bool trueCredits;

	// Token: 0x0400070F RID: 1807
	public bool mainMenuCredits;

	// Token: 0x04000710 RID: 1808
	private float waitTimer;

	// Token: 0x04000711 RID: 1809
	private const float c_waitTime = 5f;

	// Token: 0x04000712 RID: 1810
	private const float c_endCreditsYPosition = 35800f;

	// Token: 0x04000713 RID: 1811
	[SerializeField]
	private TextMeshProUGUI tmp_credits;

	// Token: 0x04000714 RID: 1812
	[SerializeField]
	private GameObject backButton;

	// Token: 0x04000715 RID: 1813
	private UIUtilities uiUtilities;
}
