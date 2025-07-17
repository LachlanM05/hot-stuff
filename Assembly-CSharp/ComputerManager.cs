using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

// Token: 0x02000153 RID: 339
public class ComputerManager : Singleton<ComputerManager>
{
	// Token: 0x06000C99 RID: 3225 RVA: 0x00048D52 File Offset: 0x00046F52
	public void Start()
	{
		this._readyToStart = false;
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x00048D5C File Offset: 0x00046F5C
	public bool ReadyToExit()
	{
		if (Singleton<ChatMaster>._instance.WorkspaceChats.Count < 3)
		{
			return false;
		}
		if (!Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK) && Singleton<ChatMaster>._instance.CanopyChats.Count < 5)
		{
			return false;
		}
		int num = 0;
		using (List<ParallelChat>.Enumerator enumerator = Singleton<ChatMaster>._instance.CanopyChats.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.chatEnded)
				{
					num++;
				}
			}
		}
		if (num > 3)
		{
			return false;
		}
		foreach (ParallelChat parallelChat in Singleton<ChatMaster>._instance.WorkspaceChats)
		{
			if (!parallelChat.chatEnded || parallelChat.appMessage.NextStitches.Count > 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00048E58 File Offset: 0x00047058
	public bool ThiscordReadyToExit()
	{
		foreach (ParallelChat parallelChat in Singleton<ChatMaster>._instance.ThiscordChats)
		{
			if (!parallelChat.chatEnded || parallelChat.appMessage.NextStitches.Count > 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00048ECC File Offset: 0x000470CC
	public int UnreadWrkspceMessages()
	{
		int num = 0;
		foreach (Save.AppMessage appMessage in Singleton<Save>.Instance.GetAllChatHistory())
		{
			if (appMessage.ChatType == ChatType.Wrkspce)
			{
				num += appMessage.NextStitches.Count;
			}
		}
		return num;
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x00048F38 File Offset: 0x00047138
	public int UnreadThiscordMessages()
	{
		int num = 0;
		foreach (Save.AppMessage appMessage in Singleton<Save>.Instance.GetAllChatHistory())
		{
			if (appMessage.ChatType == ChatType.Thiscord)
			{
				num += appMessage.NextStitches.Count;
			}
		}
		return num;
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00048FA4 File Offset: 0x000471A4
	public bool ReadyToStart()
	{
		return this._readyToStart;
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x00048FAC File Offset: 0x000471AC
	public List<string> GenerateDailyChats(int day)
	{
		if (this.tovisit == null)
		{
			this.tovisit = new Queue<string>();
		}
		List<string> list = new List<string>();
		if (day == 0)
		{
			for (int i = 0; i < ComputerManager.JOBS_PER_DAY; i++)
			{
				if (this.tovisit.Count == 0)
				{
					this.tovisit = new Queue<string>(Singleton<InkController>.Instance.story.TagsForContentAtPath("canopy_work.random_jobs"));
				}
				string text = this.tovisit.Dequeue();
				list.Add(text);
			}
		}
		this._readyToStart = true;
		return list;
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0004902C File Offset: 0x0004722C
	public void ExitOutOfCanopyMenu()
	{
		this.ManageCanopyButton();
		Singleton<CanvasUIManager>.Instance.Back();
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x0004903E File Offset: 0x0004723E
	public void SwapToCanopyMenu()
	{
		Singleton<CanvasUIManager>.Instance.SwitchMenu(Singleton<CanvasUIManager>.Instance.FindMenuComponent("Computer"));
		Singleton<ChatMaster>.Instance.SwitchtoCNPY();
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00049064 File Offset: 0x00047264
	public void ManageCanopyButton()
	{
		if (!Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK))
		{
			Singleton<ChatMaster>.Instance.closeButton.gameObject.SetActive(false);
			Singleton<ChatMaster>.Instance.canopyButton.gameObject.SetActive(true);
			return;
		}
		Singleton<ChatMaster>.Instance.closeButton.gameObject.SetActive(true);
		Singleton<ChatMaster>.Instance.canopyButton.gameObject.SetActive(false);
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x000490D8 File Offset: 0x000472D8
	public void SwapToWorkspaceMenu()
	{
		this.ManageCanopyButton();
		Singleton<CanvasUIManager>.Instance.SwitchMenu(Singleton<CanvasUIManager>.Instance.FindMenuComponent("Computer"));
		Singleton<ChatMaster>.Instance.SwitchtoWRK();
		if (Singleton<ChatMaster>.Instance.WorkspaceChats.Count > 0)
		{
			Button.ButtonClickedEvent onClick = Singleton<ChatMaster>.Instance.WorkspaceChats.First<ParallelChat>().button.GetComponent<Button>().onClick;
			if (onClick == null)
			{
				return;
			}
			onClick.Invoke();
		}
	}

	// Token: 0x04000B5B RID: 2907
	private static int JOBS_PER_DAY = 5;

	// Token: 0x04000B5C RID: 2908
	public bool canPassOffFocus;

	// Token: 0x04000B5D RID: 2909
	public Queue<string> tovisit;

	// Token: 0x04000B5E RID: 2910
	private bool _readyToStart;
}
