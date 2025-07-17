using System;
using Rewired;
using T17.UI;
using Team17.Common;
using Team17.Scripts.UI_Components;
using UnityEngine;

namespace Team17.Scripts.Input.PlatformButtonLayout
{
	// Token: 0x0200023C RID: 572
	public class UIInputService : IUIInputService, IService
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x000597B2 File Offset: 0x000579B2
		private bool ManualCycleUp
		{
			get
			{
				return this._manualCycleUpFrame == Time.frameCount;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x000597C1 File Offset: 0x000579C1
		private bool ManualCycleDown
		{
			get
			{
				return this._manualCycleDownFrame == Time.frameCount;
			}
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x000597D0 File Offset: 0x000579D0
		private bool AllowSubmitOrCancel()
		{
			return this.AnyPopupsOpen() || !Singleton<QuickResponseService>.Instance.IsAnyResponseButtonOverridingUI();
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000597E9 File Offset: 0x000579E9
		public bool IsUISubmitDown()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			if (!this.AllowSubmitOrCancel())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(28);
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00059812 File Offset: 0x00057A12
		public bool IsUISubmitUp()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			if (!this.AllowSubmitOrCancel())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonUp(28);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0005983B File Offset: 0x00057A3B
		public bool IsUICancelDown()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			if (!this.AllowSubmitOrCancel())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(29);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00059864 File Offset: 0x00057A64
		public bool IsUILeftDown()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(27);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00059883 File Offset: 0x00057A83
		public bool IsUIRightDown()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(26);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x000598A2 File Offset: 0x00057AA2
		public bool IsUIDownDown()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(25);
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x000598C1 File Offset: 0x00057AC1
		public bool IsUIUpDown()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(24);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x000598E0 File Offset: 0x00057AE0
		public bool IsUILeftPressed()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButton(27);
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x000598FF File Offset: 0x00057AFF
		public bool IsUIRightPressed()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButton(26);
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0005991E File Offset: 0x00057B1E
		public bool IsUIDownPressed()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButton(25);
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x0005993D File Offset: 0x00057B3D
		public bool IsUIUpPressed()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButton(24);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0005995C File Offset: 0x00057B5C
		public float GetScrollInput()
		{
			Player primaryPlayer = this.GetPrimaryPlayer();
			if (primaryPlayer == null)
			{
				return 0f;
			}
			return primaryPlayer.GetAxis(36);
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00059975 File Offset: 0x00057B75
		public bool IsUICycleDownDown()
		{
			if (this.ManualCycleDown)
			{
				return true;
			}
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(38);
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0005999E File Offset: 0x00057B9E
		public bool IsUICycleUpDown()
		{
			if (this.ManualCycleUp)
			{
				return true;
			}
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(37);
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x000599C7 File Offset: 0x00057BC7
		public bool IsUIMenuBackDown()
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(5);
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x000599E5 File Offset: 0x00057BE5
		public bool AnyPopupsOpen()
		{
			return (Singleton<Popup>.Instance != null && Singleton<Popup>.Instance.IsPopupOpen()) || (UIDialogManager.Instance != null && UIDialogManager.Instance.AreAnyDialogsActive());
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00059A1E File Offset: 0x00057C1E
		public bool AllowUIInteraction()
		{
			return !InteractionBlockAnimatorBehaviour.AnyMenusAnimating;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00059A28 File Offset: 0x00057C28
		private bool TryGetDialogChoiceActionId(int choiceIndex, out int action)
		{
			switch (choiceIndex)
			{
			case 0:
				action = 39;
				return true;
			case 1:
				action = 40;
				return true;
			case 2:
				action = 41;
				return true;
			case 3:
				action = 42;
				return true;
			default:
				action = -1;
				return false;
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00059A60 File Offset: 0x00057C60
		private bool TryGetDialogChoiceActionName(int choiceIndex, out string actionName)
		{
			switch (choiceIndex)
			{
			case 0:
				actionName = "PrimaryResponse";
				return true;
			case 1:
				actionName = "SecondaryResponse";
				return true;
			case 2:
				actionName = "TetriaryResponse";
				return true;
			case 3:
				actionName = "QuaternaryResponse";
				return true;
			default:
				actionName = string.Empty;
				return false;
			}
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00059AB4 File Offset: 0x00057CB4
		public bool IsDialogChoiceDown(int choiceIndex)
		{
			if (this.HasInputControllerChangedRecently())
			{
				return false;
			}
			int num;
			if (!this.TryGetDialogChoiceActionId(choiceIndex, out num))
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(num);
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00059AEC File Offset: 0x00057CEC
		public bool TryGetDialogChoiceSprite(int choiceIndex, out string spriteText)
		{
			string text;
			if (!this.TryGetDialogChoiceActionName(choiceIndex, out text))
			{
				spriteText = string.Empty;
				return false;
			}
			spriteText = "{" + text + "}";
			return true;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00059B20 File Offset: 0x00057D20
		public void OnStartUp()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ControllerChanged));
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00059B40 File Offset: 0x00057D40
		public void OnCleanUp()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ControllerChanged));
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00059B60 File Offset: 0x00057D60
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00059B62 File Offset: 0x00057D62
		private Player GetPrimaryPlayer()
		{
			return ReInput.players.GetPlayer(0);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00059B6F File Offset: 0x00057D6F
		public bool HasInputControllerChangedRecently()
		{
			return Time.frameCount - this._frameOfLastInputChange <= 1;
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00059B83 File Offset: 0x00057D83
		public void CycleDown()
		{
			this._manualCycleDownFrame = Time.frameCount + 1;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00059B92 File Offset: 0x00057D92
		public void CycleUp()
		{
			this._manualCycleUpFrame = Time.frameCount + 1;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00059BA1 File Offset: 0x00057DA1
		private void ControllerChanged(Controller controller)
		{
			this._frameOfLastInputChange = Time.frameCount;
		}

		// Token: 0x04000E97 RID: 3735
		private int _frameOfLastInputChange = -1;

		// Token: 0x04000E98 RID: 3736
		private int _manualCycleUpFrame;

		// Token: 0x04000E99 RID: 3737
		private int _manualCycleDownFrame;
	}
}
