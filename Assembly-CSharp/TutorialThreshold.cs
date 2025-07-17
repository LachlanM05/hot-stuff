using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200009B RID: 155
public class TutorialThreshold : MonoBehaviour, IReloadHandler
{
	// Token: 0x0600053F RID: 1343 RVA: 0x0001ED70 File Offset: 0x0001CF70
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!Singleton<Save>.Instance.GetTutorialThresholdState(base.gameObject.name + this.inkTag))
			{
				if (this.isDroneThreshold)
				{
					Singleton<TutorialController>.Instance.GotCloseToGift();
				}
				else
				{
					this.showingFpnPopup = true;
					Singleton<InkController>.Instance.story.SwitchFlow(this.inkTag);
					Singleton<InkController>.Instance.story.ChoosePathString(this.inkTag, true, Array.Empty<object>());
					Singleton<InkController>.Instance.ContinueStory();
					this.currentText = Singleton<InkController>.Instance.story.currentText;
					this.SetText();
				}
				Singleton<Save>.Instance.SetTutorialThresholdState(base.gameObject.name + this.inkTag);
			}
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x0001EE54 File Offset: 0x0001D054
	private void SetText()
	{
		UnityEvent unityEvent = new UnityEvent();
		unityEvent.AddListener(new UnityAction(this.NextChatInFpn));
		this.currentText = this.currentText.Replace("Narrator:: ", "");
		Singleton<Popup>.Instance.CreatePopup("", this.currentText, unityEvent, true);
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0001EEAC File Offset: 0x0001D0AC
	private void NextChatInFpn()
	{
		if (Singleton<InkController>.Instance.story.canContinue)
		{
			Singleton<InkController>.Instance.ContinueStory();
			this.currentText = Singleton<InkController>.Instance.story.currentText;
			if (Singleton<InkController>.Instance.story.currentTags.Count > 0)
			{
				foreach (string text in Singleton<InkController>.Instance.story.currentTags)
				{
					string[] array = text.Split(' ', StringSplitOptions.None);
					int num;
					if (array[0] == "collectable" && int.TryParse(array[2].Trim(), out num))
					{
						PlayerPauser.Pause();
						DateADex.Instance.UnlockCollectableTutorial(array[1], num);
					}
					if (array[0] == "workspace")
					{
						Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat." + array[1], ChatType.Wrkspce, false, true);
					}
					if (array[0] == "new_message_computer" || (array[0] == "new_message" && array[1] == "computer"))
					{
						Singleton<ChatMaster>.Instance.workspacepings++;
						Singleton<ChatMaster>.Instance.UpdateVisualsWorkspace();
						Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_wkspace_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					}
					if (array[0] == "new_message" && array[1] == "phone")
					{
						Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
						Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					}
					if (array[0] == "workspace")
					{
						Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat." + array[1], ChatType.Wrkspce, false, true);
					}
					if (array[0] == "thiscord")
					{
						Singleton<ChatMaster>.Instance.StartChat("thiscord_phone." + array[1], ChatType.Thiscord, false, true);
					}
				}
				base.Invoke("UnpauseAndSetTextInFpn", 2f);
				return;
			}
			if (this.currentText.Trim() != "")
			{
				this.SetText();
				return;
			}
		}
		else
		{
			this.currentText = "";
			this.showingFpnPopup = false;
		}
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0001F118 File Offset: 0x0001D318
	public void LoadState()
	{
		if (Singleton<Save>.Instance.GetTutorialThresholdState(base.gameObject.name + this.inkTag))
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetDateStatus("skylar") == RelationshipStatus.Unmet)
		{
			Singleton<TutorialController>.Instance.DroneHoveringTutorialFix();
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0001F18C File Offset: 0x0001D38C
	public void SaveState()
	{
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0001F18E File Offset: 0x0001D38E
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x04000529 RID: 1321
	public string inkTag;

	// Token: 0x0400052A RID: 1322
	public string hintInkTag;

	// Token: 0x0400052B RID: 1323
	private string currentText = "";

	// Token: 0x0400052C RID: 1324
	public bool showingFpnPopup;

	// Token: 0x0400052D RID: 1325
	public bool isDroneThreshold;
}
