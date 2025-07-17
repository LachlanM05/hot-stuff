using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rewired;
using T17.Services;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x02000208 RID: 520
	public class QuickResponseService : Singleton<QuickResponseService>
	{
		// Token: 0x060010E7 RID: 4327 RVA: 0x00056861 File Offset: 0x00054A61
		private void LateUpdate()
		{
			if (this._queueTextRefresh)
			{
				this.RefreshText();
				this._queueTextRefresh = false;
			}
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00056878 File Offset: 0x00054A78
		private void RefreshText()
		{
			foreach (QuickResponseButton quickResponseButton in this._quickResponseButtons)
			{
				string text;
				if (!this.TryGetChoiceSprite(quickResponseButton, out text))
				{
					quickResponseButton.SetIconText(string.Empty);
				}
				else
				{
					quickResponseButton.SetIconText(text);
				}
			}
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x000568E4 File Offset: 0x00054AE4
		public bool IsQuickResponseEnabled()
		{
			return this._quickResponseButtons.Count != 0 && this._quickResponseButtons.Count <= QuickResponseService.Actions.Count && !Services.UIInputService.AnyPopupsOpen();
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0005691D File Offset: 0x00054B1D
		public bool IsAnyResponseButtonOverridingUI()
		{
			return this._quickResponseButtons.Any((QuickResponseButton x) => x.ForceOverrideAllUI);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0005694C File Offset: 0x00054B4C
		public bool IsQuickResponseButtonPressed(QuickResponseButton quickResponseButton)
		{
			if (Services.UIInputService.HasInputControllerChangedRecently())
			{
				return false;
			}
			if (!this.IsQuickResponseEnabled())
			{
				return false;
			}
			int num;
			if (!this.TryGetChoiceActionId(quickResponseButton, out num))
			{
				return false;
			}
			Player primaryPlayer = this.GetPrimaryPlayer();
			return primaryPlayer != null && primaryPlayer.GetButtonDown(num);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00056990 File Offset: 0x00054B90
		public void Register(QuickResponseButton qrb)
		{
			this._quickResponseButtons.Add(qrb);
			this.SortList();
			this._forceUsePrimaryActionIfOnlySingleResponse = this.DoesAnyQuickResponseButtonRequireForcePrimaryAction();
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x000569B0 File Offset: 0x00054BB0
		public void Unregister(QuickResponseButton qrb)
		{
			this._quickResponseButtons.Remove(qrb);
			this._forceUsePrimaryActionIfOnlySingleResponse = this.DoesAnyQuickResponseButtonRequireForcePrimaryAction();
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x000569CC File Offset: 0x00054BCC
		private bool DoesAnyQuickResponseButtonRequireForcePrimaryAction()
		{
			bool flag = false;
			for (int i = 0; i < this._quickResponseButtons.Count; i++)
			{
				if (this._quickResponseButtons[i].ForceUsePrimaryActionIfOnlySingleResponse)
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00056A09 File Offset: 0x00054C09
		private void SortList()
		{
			this._quickResponseButtons.Sort((QuickResponseButton a, QuickResponseButton b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
			this._queueTextRefresh = true;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00056A3C File Offset: 0x00054C3C
		private int GetChoiceIndexForQuickResponseButton(QuickResponseButton qrb)
		{
			return this._quickResponseButtons.IndexOf(qrb);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00056A4C File Offset: 0x00054C4C
		private bool TryGetChoiceSprite(QuickResponseButton qrb, out string spriteText)
		{
			string text;
			if (!this.TryGetChoiceActionName(qrb, out text))
			{
				spriteText = string.Empty;
				return false;
			}
			spriteText = "{" + text + "}";
			return true;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00056A80 File Offset: 0x00054C80
		private bool TryGetChoiceActionId(QuickResponseButton qrb, out int actionId)
		{
			int choiceIndexForQuickResponseButton = this.GetChoiceIndexForQuickResponseButton(qrb);
			if (choiceIndexForQuickResponseButton > QuickResponseService.Actions.Count)
			{
				actionId = -1;
				return false;
			}
			if (choiceIndexForQuickResponseButton > Singleton<InkController>.Instance.story.currentChoices.Count)
			{
				actionId = -1;
				return false;
			}
			if (this._forceUsePrimaryActionIfOnlySingleResponse && this._quickResponseButtons.Count == 1)
			{
				actionId = QuickResponseService.OverrideActionIfSingleResponse.Item1;
				return true;
			}
			actionId = QuickResponseService.Actions[choiceIndexForQuickResponseButton].Item1;
			return true;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00056AFC File Offset: 0x00054CFC
		private bool TryGetChoiceActionName(QuickResponseButton qrb, out string actionName)
		{
			if (this._forceUsePrimaryActionIfOnlySingleResponse && this._quickResponseButtons.Count == 1)
			{
				actionName = QuickResponseService.OverrideActionIfSingleResponse.Item2;
				return true;
			}
			int choiceIndexForQuickResponseButton = this.GetChoiceIndexForQuickResponseButton(qrb);
			if (choiceIndexForQuickResponseButton > QuickResponseService.Actions.Count)
			{
				actionName = string.Empty;
				return false;
			}
			actionName = QuickResponseService.Actions[choiceIndexForQuickResponseButton].Item2;
			return true;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00056B63 File Offset: 0x00054D63
		private Player GetPrimaryPlayer()
		{
			return ReInput.players.GetPlayer(0);
		}

		// Token: 0x04000E23 RID: 3619
		private const int InvalidAction = -1;

		// Token: 0x04000E24 RID: 3620
		private List<QuickResponseButton> _quickResponseButtons = new List<QuickResponseButton>();

		// Token: 0x04000E25 RID: 3621
		private bool _queueTextRefresh;

		// Token: 0x04000E26 RID: 3622
		[TupleElementNames(new string[] { "actionId", "actionName" })]
		private static readonly List<ValueTuple<int, string>> Actions = new List<ValueTuple<int, string>>
		{
			new ValueTuple<int, string>(39, "PrimaryResponse"),
			new ValueTuple<int, string>(40, "SecondaryResponse"),
			new ValueTuple<int, string>(41, "TetriaryResponse"),
			new ValueTuple<int, string>(42, "QuaternaryResponse"),
			new ValueTuple<int, string>(24, "UI_Up"),
			new ValueTuple<int, string>(26, "UI_Right"),
			new ValueTuple<int, string>(27, "UI_Left"),
			new ValueTuple<int, string>(25, "UI_Down")
		};

		// Token: 0x04000E27 RID: 3623
		[TupleElementNames(new string[] { "actionId", "actionName" })]
		private static readonly ValueTuple<int, string> OverrideActionIfSingleResponse = new ValueTuple<int, string>(28, "Confirm");

		// Token: 0x04000E28 RID: 3624
		private bool _forceUsePrimaryActionIfOnlySingleResponse;
	}
}
