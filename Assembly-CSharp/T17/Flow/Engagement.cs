using System;
using T17.Flow.Tasks;
using T17.Services;
using Team17.Platform.EngagementService;
using Team17.Platform.User;
using Team17.Platform.UserService;
using Team17.Scripts.Services.Input;
using TMPro;
using UnityEngine;

namespace T17.Flow
{
	// Token: 0x02000249 RID: 585
	public class Engagement : MonoBehaviour
	{
		// Token: 0x06001317 RID: 4887 RVA: 0x0005B992 File Offset: 0x00059B92
		private bool ConnectEngagementService()
		{
			if (this.m_EngagementService == null)
			{
				this.m_EngagementService = Services.EngagementService;
				if (this.m_EngagementService == null)
				{
					return false;
				}
				this.m_EngagementService.EnterEngagementState();
			}
			return true;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0005B9BF File Offset: 0x00059BBF
		private bool ConnectUserService()
		{
			if (this.m_UserService == null)
			{
				this.m_UserService = Services.UserService;
				return this.m_UserService != null;
			}
			return true;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0005B9DF File Offset: 0x00059BDF
		private void Start()
		{
			this.m_PreEngagementTasks = null;
			this.m_CursorController = base.GetComponent<CursorController>();
			this.m_PostEngagementTasks = new IGameTask[]
			{
				new LoadSettingsTask(),
				new LoadSaveSlotMetadataTask()
			};
			this.m_Text_EngagementTitle.enabled = false;
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0005BA1C File Offset: 0x00059C1C
		private void Update()
		{
			switch (this.m_EngagementFlowState)
			{
			case Engagement.EngementFlowState.PreEngaging:
				if (this.UpdateTasks(this.m_PreEngagementTasks) != TaskResult.NotAvailableYet)
				{
					this.m_InputModeHandle = Services.InputService.PushMode(IMirandaInputService.EInputMode.Engagement, this);
					this.m_Text_EngagementTitle.enabled = true;
					this.m_Text_EngagementTitle.text = "Press {engage_control} to continue";
					this.m_EngagementFlowState = Engagement.EngementFlowState.Engaging;
					return;
				}
				break;
			case Engagement.EngementFlowState.Engaging:
				this.Update_Engaging();
				return;
			case Engagement.EngementFlowState.PostEngaging:
			{
				TaskResult taskResult = this.UpdateTasks(this.m_PostEngagementTasks);
				if (!this.playedAudio)
				{
					this.playedAudio = true;
					this.m_AudioSource.PlayOneShot(this.clip);
				}
				this.HandlePostEngagementTaskResult(taskResult);
				break;
			}
			case Engagement.EngementFlowState.Completed:
				break;
			default:
				return;
			}
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0005BAC8 File Offset: 0x00059CC8
		private void HandlePostEngagementTaskResult(TaskResult result)
		{
			switch (result)
			{
			case TaskResult.NotAvailableYet:
				break;
			case TaskResult.Success:
				this.m_AudioSource.PlayOneShot(this.clip2);
				if (DeluxeEditionController.IS_DEMO_EDITION && this.m_SceneToLoad == SceneConsts.kMainMenu)
				{
					SceneTransitionManager.TransitionToScene(SceneConsts.kDemoMenu);
					this.SetEnabledState(false);
				}
				else if (!string.IsNullOrWhiteSpace(this.m_SceneToLoad))
				{
					SceneTransitionManager.TransitionToScene(this.m_SceneToLoad);
					this.SetEnabledState(false);
				}
				this.m_EngagementFlowState = Engagement.EngementFlowState.Completed;
				this.OnEngagementCompleted();
				return;
			case TaskResult.Failure:
				this.m_CurrentTaskindex = 0;
				this.m_Text_EngagementTitle.enabled = true;
				this.m_Text_EngagementTitle.text = "Press {engage_control} to continue";
				this.m_EngagementFlowState = Engagement.EngementFlowState.Engaging;
				this.m_EngagementState = default(Engagement.EngagementContext);
				this.m_UserService.RemoveAllUsers();
				this.m_EngagementService.EnterEngagementState();
				break;
			default:
				return;
			}
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0005BBA1 File Offset: 0x00059DA1
		private void OnEngagementCompleted()
		{
			this.m_InputModeHandle.Dispose();
			this.m_InputModeHandle = null;
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0005BBB8 File Offset: 0x00059DB8
		private void Update_Engaging()
		{
			if (!this.ConnectEngagementService() || !this.ConnectUserService())
			{
				return;
			}
			EngagementState state = this.m_EngagementService.State;
			int count = this.m_EngagementService.LostUsers.Count;
			if (state == this.m_EngagementState.m_EngagementState && count == this.m_EngagementState.m_LostUserCount)
			{
				return;
			}
			this.m_EngagementState.m_EngagementState = state;
			if (this.m_Text_EngagementState == null)
			{
				return;
			}
			switch (state)
			{
			case EngagementState.Engagement:
				this.m_Text_EngagementState.text = "";
				return;
			case EngagementState.Idle:
			case EngagementState.Lobby:
			{
				IUser user;
				if (this.m_UserService.TryGetPrimaryUser(out user))
				{
					this.m_Text_EngagementTitle.enabled = false;
					this.m_Text_EngagementState.text = "";
					this.m_EngagementFlowState = Engagement.EngementFlowState.PostEngaging;
					return;
				}
				this.m_Text_EngagementTitle.enabled = true;
				this.m_Text_EngagementState.text = "Error: No primary User!";
				return;
			}
			case EngagementState.Reengagement:
				this.m_Text_EngagementState.text = "Press SPACE to Rejoin: " + this.m_EngagementService.LostUsers[0].m_User.DisplayName;
				this.m_Text_EngagementTitle.enabled = true;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0005BCE4 File Offset: 0x00059EE4
		private TaskResult UpdateTasks(IGameTask[] engagementPhaseTasks)
		{
			TaskResult taskResult = TaskResult.NotAvailableYet;
			if (engagementPhaseTasks != null && engagementPhaseTasks.Length != 0)
			{
				IGameTask gameTask = engagementPhaseTasks[this.m_CurrentTaskindex];
				switch (gameTask.Status)
				{
				case TaskStatus.NotStarted:
					gameTask.Start();
					break;
				case TaskStatus.InProgress:
					gameTask.Update(Time.deltaTime);
					break;
				case TaskStatus.Complete:
					gameTask.End();
					if (gameTask.Result == TaskResult.Failure)
					{
						taskResult = TaskResult.Failure;
					}
					else
					{
						this.m_CurrentTaskindex++;
						if (this.m_CurrentTaskindex >= engagementPhaseTasks.Length)
						{
							this.m_CurrentTaskindex = 0;
							taskResult = TaskResult.Success;
						}
					}
					break;
				}
			}
			else
			{
				taskResult = TaskResult.Success;
			}
			return taskResult;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0005BD70 File Offset: 0x00059F70
		private string GetLostUsersString()
		{
			string text = string.Empty;
			int i = 0;
			int count = this.m_EngagementService.LostUsers.Count;
			while (i < count)
			{
				text += this.m_EngagementService.LostUsers[i].m_User.DisplayName;
				if (i < count - 1)
				{
					text += ", ";
				}
				i++;
			}
			return text;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0005BDD8 File Offset: 0x00059FD8
		private void AutoEngagePrimaryUser()
		{
			if (!this.m_AutoEngagePrimaryUser)
			{
				return;
			}
			IUserService userService = Services.UserService;
			IUser user;
			if (userService.TryGetPrimaryUser(out user))
			{
				userService.AddUserAsync(0, null, AddUser.Options.AddDefaultUserSilently, null);
			}
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0005BE08 File Offset: 0x0005A008
		private void SetEnabledState(bool enableState)
		{
			base.enabled = enableState;
			if (this.m_CursorController != null)
			{
				this.m_CursorController.enabled = enableState;
				this.m_CursorController.ForceHideCursor();
			}
		}

		// Token: 0x04000EEF RID: 3823
		private const string kEngagementPrompt = "Press {engage_control} to continue";

		// Token: 0x04000EF0 RID: 3824
		[SerializeField]
		private TMP_Text m_Text_EngagementState;

		// Token: 0x04000EF1 RID: 3825
		[SerializeField]
		private TMP_Text m_Text_EngagementTitle;

		// Token: 0x04000EF2 RID: 3826
		[SerializeField]
		private bool m_AutoEngagePrimaryUser;

		// Token: 0x04000EF3 RID: 3827
		[SerializeField]
		private short m_UserIndex;

		// Token: 0x04000EF4 RID: 3828
		[SerializeField]
		private string m_SceneToLoad = "";

		// Token: 0x04000EF5 RID: 3829
		[SerializeField]
		private AudioSource m_AudioSource;

		// Token: 0x04000EF6 RID: 3830
		[SerializeField]
		private AudioClip clip;

		// Token: 0x04000EF7 RID: 3831
		[SerializeField]
		private AudioClip clip2;

		// Token: 0x04000EF8 RID: 3832
		private bool playedAudio;

		// Token: 0x04000EF9 RID: 3833
		private IUserService m_UserService;

		// Token: 0x04000EFA RID: 3834
		private IEngagementService m_EngagementService;

		// Token: 0x04000EFB RID: 3835
		private Engagement.EngagementContext m_EngagementState;

		// Token: 0x04000EFC RID: 3836
		private IGameTask[] m_PreEngagementTasks;

		// Token: 0x04000EFD RID: 3837
		private IGameTask[] m_PostEngagementTasks;

		// Token: 0x04000EFE RID: 3838
		private int m_CurrentTaskindex;

		// Token: 0x04000EFF RID: 3839
		private Engagement.EngementFlowState m_EngagementFlowState;

		// Token: 0x04000F00 RID: 3840
		private InputModeHandle m_InputModeHandle;

		// Token: 0x04000F01 RID: 3841
		private CursorController m_CursorController;

		// Token: 0x020003C9 RID: 969
		private struct EngagementContext
		{
			// Token: 0x040014F1 RID: 5361
			public EngagementState m_EngagementState;

			// Token: 0x040014F2 RID: 5362
			public int m_LostUserCount;
		}

		// Token: 0x020003CA RID: 970
		private enum EngementFlowState
		{
			// Token: 0x040014F4 RID: 5364
			PreEngaging,
			// Token: 0x040014F5 RID: 5365
			Engaging,
			// Token: 0x040014F6 RID: 5366
			PostEngaging,
			// Token: 0x040014F7 RID: 5367
			Completed
		}
	}
}
