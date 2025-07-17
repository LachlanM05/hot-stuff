using System;
using Rewired;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class AwakenUI : MonoBehaviour
{
	// Token: 0x060000E8 RID: 232 RVA: 0x000063EC File Offset: 0x000045EC
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(this.playerId);
		this.player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnAwakenButtonUp), UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Awaken");
		InteractableManager.ResetTalked += this.GoodMorning;
		this.Workspace = GameObject.FindWithTag("WorkspaceComputer");
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00006450 File Offset: 0x00004650
	private void OnAwakenButtonUp(InputActionEventData data)
	{
		if (!BetterPlayerControl.Instance.isGameEndingOn)
		{
			if (this.Textbox != null && this.Textbox.activeInHierarchy && Singleton<Popup>.Instance.canAwakenTbc)
			{
				this.TextboxChan();
				return;
			}
			if (this.Credits != null && this.Credits.activeInHierarchy)
			{
				this.DateTheDevs();
				return;
			}
			if (this.Workspace != null && this.Workspace.activeInHierarchy)
			{
				this.Wilhelmina();
			}
		}
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000064DA File Offset: 0x000046DA
	private void GoodMorning()
	{
		this.TalkedToTBC = false;
		this.TalkedToDTD = false;
		this.TalkedToWRK = false;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000064F4 File Offset: 0x000046F4
	private void TextboxChan()
	{
		if (!this.TalkedToTBC && Singleton<Save>.Instance.GetFullTutorialFinished())
		{
			Singleton<Popup>.Instance.ClosePopup();
			if (Singleton<PhoneManager>.Instance.IsPhoneMenuOpened())
			{
				Singleton<PhoneManager>.Instance.ClosePhoneAsync(null, true);
			}
			this.TalkedToTBC = true;
			Singleton<Save>.Instance.MeetDatableIfUnmet("tbc");
			Singleton<GameController>.Instance.ForceDialogue("tbc_ui", null, false, false);
		}
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00006560 File Offset: 0x00004760
	private void DateTheDevs()
	{
		if (!this.TalkedToDTD && Singleton<Save>.Instance.GetFullTutorialFinished())
		{
			this.TalkedToDTD = true;
			Singleton<Save>.Instance.MeetDatableIfUnmet("sassy");
			Singleton<GameController>.Instance.ForceDialogue("sassy_chap", null, false, false);
		}
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0000659F File Offset: 0x0000479F
	private void Wilhelmina()
	{
		if (!this.TalkedToWRK && Singleton<Save>.Instance.GetFullTutorialFinished())
		{
			this.TalkedToWRK = true;
			Singleton<Save>.Instance.MeetDatableIfUnmet("willi");
			Singleton<GameController>.Instance.ForceDialogue("willi_work", null, false, false);
		}
	}

	// Token: 0x040000E1 RID: 225
	public int playerId;

	// Token: 0x040000E2 RID: 226
	private Player player;

	// Token: 0x040000E3 RID: 227
	public GameObject Textbox;

	// Token: 0x040000E4 RID: 228
	public GameObject Credits;

	// Token: 0x040000E5 RID: 229
	public GameObject Workspace;

	// Token: 0x040000E6 RID: 230
	public bool TalkedToTBC;

	// Token: 0x040000E7 RID: 231
	public bool TalkedToDTD;

	// Token: 0x040000E8 RID: 232
	public bool TalkedToWRK;
}
