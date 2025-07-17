using System;
using System.Collections;
using T17.Services;
using Team17.Services.Save;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EA RID: 234
public class ContinueButton : MonoBehaviour
{
	// Token: 0x060007B3 RID: 1971 RVA: 0x0002B1FE File Offset: 0x000293FE
	private void Awake()
	{
		this._continueInProgress = false;
		if (this.buttonObject == null)
		{
			this.buttonObject = base.gameObject;
		}
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x0002B221 File Offset: 0x00029421
	public void OnEnable()
	{
		this.LockButtons(false);
		this.UpdateContinueButton(true);
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x0002B231 File Offset: 0x00029431
	public void OnDisable()
	{
		this.LockButtons(false);
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x0002B23A File Offset: 0x0002943A
	public void ContinueGame()
	{
		if (this._continueInProgress)
		{
			return;
		}
		base.StartCoroutine(this.ContinueGameInternal());
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x0002B252 File Offset: 0x00029452
	private IEnumerator ContinueGameInternal()
	{
		SaveSlotMetadata latestSlot = Services.SaveGameService.GetLatestSlotInfo();
		if (latestSlot != null)
		{
			this._continueInProgress = true;
			this.LockButtons(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.start_game, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
			yield return new WaitForSeconds(0.5f);
			Singleton<Save>.Instance.LoadGameAsync(latestSlot.SlotNumber);
			this._continueInProgress = false;
		}
		yield break;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x0002B261 File Offset: 0x00029461
	private void Update()
	{
		this.UpdateContinueButton(false);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0002B26C File Offset: 0x0002946C
	private void UpdateContinueButton(bool justEnabled)
	{
		int numberSaveSlotsInUse = Services.SaveGameService.GetNumberSaveSlotsInUse();
		if (justEnabled || numberSaveSlotsInUse != this.saveSlotsInUse)
		{
			this.saveSlotsInUse = numberSaveSlotsInUse;
			bool flag = this.saveSlotsInUse != 0;
			if (this.buttonObject.activeSelf != flag)
			{
				this.buttonObject.SetActive(flag);
			}
			if (this.loadButtonObject != null && this.loadButtonObject.activeSelf != flag)
			{
				this.loadButtonObject.SetActive(flag);
			}
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0002B2E4 File Offset: 0x000294E4
	private void LockButtons(bool toLock = true)
	{
		Button[] componentsInChildren = base.transform.GetComponentsInChildren<Button>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].interactable = !toLock;
		}
	}

	// Token: 0x040006ED RID: 1773
	private bool _continueInProgress;

	// Token: 0x040006EE RID: 1774
	private int saveSlotsInUse;

	// Token: 0x040006EF RID: 1775
	public GameObject buttonObject;

	// Token: 0x040006F0 RID: 1776
	public GameObject loadButtonObject;
}
