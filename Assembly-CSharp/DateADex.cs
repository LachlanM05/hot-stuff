using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AirFishLab.ScrollingList;
using Date_Everything.Scripts.ScriptableObjects;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Rewired;
using T17.Services;
using Team17.Common;
using Team17.Scripts.UI_Components;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000F4 RID: 244
public class DateADex : MonoBehaviour
{
	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000807 RID: 2055 RVA: 0x0002DBD9 File Offset: 0x0002BDD9
	public bool IsInEntryScreen
	{
		get
		{
			return this.currentScreen == DateADex.Dex_Screen.Entry;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000808 RID: 2056 RVA: 0x0002DBE4 File Offset: 0x0002BDE4
	public bool IsCollectableShowing
	{
		get
		{
			return this._collectableShowing;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000809 RID: 2057 RVA: 0x0002DBEC File Offset: 0x0002BDEC
	public bool IsCollectableShowingOrHiding
	{
		get
		{
			return this._collectableShowing | (this._currentReleaseCollectableCoroutine != null);
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x0600080A RID: 2058 RVA: 0x0002DBFE File Offset: 0x0002BDFE
	public string LastCollectableStagePosition
	{
		get
		{
			return this.lastCollectableStagePosition;
		}
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x0002DC08 File Offset: 0x0002BE08
	public void UpdateDateADexEntries()
	{
		this._ids = new Dictionary<string, int>();
		this._names = new Dictionary<int, string>();
		this.EntriesOrdered = new DateADexEntry[102];
		using (List<string>.Enumerator enumerator = Singleton<InkController>.Instance.GetStitchesAtLocation("DateADex").GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string s = enumerator.Current;
				if (!s.EndsWith("_pair"))
				{
					try
					{
						bool flag = false;
						if (this.EntriesCompleted != null)
						{
							DateADexEntry dateADexEntry = this.EntriesCompleted.Find((DateADexEntry Ent) => Ent.internalName == s.Replace("DateADex.", "").Replace("_dex_entry", ""));
							if (dateADexEntry != null)
							{
								flag = dateADexEntry.notification;
							}
						}
						DateADexEntry dateADexEntry2 = this.setupEntry(s);
						dateADexEntry2.notification = flag;
						this.EntriesOrdered[dateADexEntry2.CharIndex] = dateADexEntry2;
					}
					catch (Exception ex)
					{
						T17Debug.LogError("DateADex Exception: " + ex.Message);
					}
				}
			}
		}
		this.EntriesCompleted = new List<DateADexEntry>(this.EntriesOrdered);
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0002DD30 File Offset: 0x0002BF30
	public string TreatUnmetCharacters(string hint)
	{
		foreach (CollectableCharacterInternalName collectableCharacterInternalName in this.CharacterInternalNameMap)
		{
			if (hint.Contains(collectableCharacterInternalName.characterName) && Singleton<Save>.Instance.GetDateStatus(collectableCharacterInternalName.internalName) == RelationshipStatus.Unmet)
			{
				hint = hint.Replace(collectableCharacterInternalName.characterName, "???");
			}
		}
		return hint;
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0002DDB0 File Offset: 0x0002BFB0
	public List<CollectableCharacterInternalName> GetCharacterInternalNameMap()
	{
		return this.CharacterInternalNameMap;
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x0002DDB8 File Offset: 0x0002BFB8
	private void Start()
	{
		if (DateADex.Instance != null && DateADex.Instance != this)
		{
			Object.Destroy(this);
			return;
		}
		DateADex.Instance = this;
		Save.onGameLoad += this.OnGameLoad;
		this._collectableShowing = false;
		this._currentReleaseCollectableCoroutine = null;
		this.ActivateIfNeeded();
		this.backButtonComponent = this.backButton.GetComponent<Button>();
		this.UpdateBackAction();
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x0002DE28 File Offset: 0x0002C028
	public void OnGameLoad()
	{
		this.startedEnding = false;
		this.UpdateDateADexEntries();
		if (!Singleton<InkController>.Instance.story.HasExternalFunction("unlockCollectable"))
		{
			Singleton<InkController>.Instance.story.BindExternalFunction<string, int>("unlockCollectable", delegate(string name, int number)
			{
				this.UnlockCollectable(name, number, false, false, true);
			}, false);
		}
		Singleton<Save>.Instance.SetDexEntriesInDateADex();
		Roomers.Instance.LoadInitialDataOnGameLoad();
		this.setup = true;
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0002DE94 File Offset: 0x0002C094
	public void Awake()
	{
		if (this.VoActorRectTransform != null)
		{
			this.initialVOActorOffest = this.VoActorRectTransform.anchorMin.x;
		}
		this.player = ReInput.players.GetPlayer(0);
		this.ActivateIfNeeded();
		RectTransform rectTransform = base.transform as RectTransform;
		if (rectTransform != null)
		{
			rectTransform.sizeDelta = Vector2.zero;
			rectTransform.anchoredPosition = Vector2.zero;
		}
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x0002DF08 File Offset: 0x0002C108
	private void OnEnable()
	{
		if (this.confirmIcon != null)
		{
			this.confirmIcon.OnISOkToDisplayGlyth += this.ShouldWeDisplayConfirmIcon;
		}
		if (this.actionIcon != null)
		{
			this.actionIcon.OnISOkToDisplayGlyth += this.ShouldWeDisplayActionIcon;
		}
		if (this.navigateIcon != null)
		{
			this.navigateIcon.OnISOkToDisplayGlyth += this.ShouldWeDisplayNavigationIcon;
		}
		this.UpdateLegends();
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x0002DF8C File Offset: 0x0002C18C
	private void OnDisable()
	{
		if (this.confirmIcon != null)
		{
			this.confirmIcon.OnISOkToDisplayGlyth -= this.ShouldWeDisplayConfirmIcon;
		}
		if (this.actionIcon != null)
		{
			this.actionIcon.OnISOkToDisplayGlyth -= this.ShouldWeDisplayActionIcon;
		}
		if (this.navigateIcon != null)
		{
			this.navigateIcon.OnISOkToDisplayGlyth -= this.ShouldWeDisplayNavigationIcon;
		}
		Save.onGameLoad -= this.OnGameLoad;
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x0002E019 File Offset: 0x0002C219
	private IEnumerator SelectBackButton()
	{
		yield return new WaitUntil(() => this.backButton.IsRegistered);
		ControllerMenuUI.SetCurrentlySelected(this.backButton.gameObject, ControllerMenuUI.Direction.Down, false, false);
		yield break;
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x0002E028 File Offset: 0x0002C228
	private void SelectInitialElement()
	{
		if (this.currentSelectionDelay != null)
		{
			base.StopCoroutine(this.currentSelectionDelay);
			this.currentSelectionDelay = null;
		}
		Selectable selectable = null;
		if (this.currentScreen == DateADex.Dex_Screen.List)
		{
			selectable = this.GetLastSelectedEntry().gameObject.GetComponent<Selectable>();
		}
		else if (this.currentScreen == DateADex.Dex_Screen.Entry)
		{
			selectable = this.CollectableButton;
		}
		else if (this.currentScreen == DateADex.Dex_Screen.Collectables)
		{
			selectable = this._collectablesScreen.GetInitialCollectable();
		}
		if (selectable != null)
		{
			this.currentSelectionDelay = base.StartCoroutine(this.WaitToSelectCollectable(selectable));
		}
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0002E0B0 File Offset: 0x0002C2B0
	private IEnumerator WaitToSelectCollectable(Selectable itemToSelect)
	{
		yield return new WaitUntil(() => itemToSelect.IsInteractable());
		ControllerMenuUI.SetCurrentlySelected(itemToSelect.gameObject, ControllerMenuUI.Direction.Down, false, false);
		this.currentSelectionDelay = null;
		yield break;
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0002E0C6 File Offset: 0x0002C2C6
	private void SelectFirstDateable()
	{
		ControllerMenuUI.SetCurrentlySelected(this.GetLastSelectedEntry().gameObject, ControllerMenuUI.Direction.Down, false, false);
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x0002E0DB File Offset: 0x0002C2DB
	private void SelectEntryElement()
	{
		ControllerMenuUI.SetCurrentlySelected(this.CollectableButton.gameObject, ControllerMenuUI.Direction.Down, false, false);
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x0002E0F0 File Offset: 0x0002C2F0
	private void UpdateBackAction()
	{
		if (this.backButton.GetComponentInChildren<ActionOnDisable>() != null)
		{
			ActionOnDisable componentInChildren = this.backButton.GetComponentInChildren<ActionOnDisable>();
			componentInChildren.onDisable = (Action)Delegate.Combine(componentInChildren.onDisable, new Action(this.RefocusFromDisabledBackButton));
		}
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0002E13C File Offset: 0x0002C33C
	private void RefocusFromDisabledBackButton()
	{
		if (this.backButton.gameObject == ControllerMenuUI.GetCurrentSelectedControl())
		{
			if (this.player == null)
			{
				this.player = ReInput.players.GetPlayer(0);
			}
			if (this.player.GetButtonDown(28))
			{
				base.StartCoroutine(this.RefocusBackButtonDelay());
				return;
			}
			this.SelectInitialElement();
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0002E19C File Offset: 0x0002C39C
	private IEnumerator RefocusBackButtonDelay()
	{
		this.backButtonLostFocus = true;
		yield return new WaitForSeconds(0.3f);
		if (this.backButtonLostFocus)
		{
			this.backButtonLostFocus = false;
			this.RefocusFromDisabledBackButton();
		}
		yield break;
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0002E1AB File Offset: 0x0002C3AB
	public void CancelBackLostFocus()
	{
		this.backButtonLostFocus = false;
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x0002E1B4 File Offset: 0x0002C3B4
	private void ShouldWeDisplayActionIcon(ControllerGlyphComponent.ResultEvent result)
	{
		if (this.currentScreen == DateADex.Dex_Screen.List)
		{
			result.result = true;
			return;
		}
		if (this.currentScreen == DateADex.Dex_Screen.Entry)
		{
			if (this.currentEntry.internalName == "lucinda")
			{
				result.result = false;
				return;
			}
			result.result = true;
			return;
		}
		else
		{
			if (this.currentScreen == DateADex.Dex_Screen.Collectables)
			{
				result.result = false;
				return;
			}
			result.result = false;
			return;
		}
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x0002E219 File Offset: 0x0002C419
	private void ShouldWeDisplayNavigationIcon(ControllerGlyphComponent.ResultEvent result)
	{
		if (this.currentScreen == DateADex.Dex_Screen.List)
		{
			result.result = true;
			return;
		}
		if (this.currentScreen == DateADex.Dex_Screen.Entry)
		{
			result.result = false;
			return;
		}
		if (this.currentScreen == DateADex.Dex_Screen.Collectables)
		{
			result.result = true;
			return;
		}
		result.result = false;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x0002E254 File Offset: 0x0002C454
	private void ShouldWeDisplayConfirmIcon(ControllerGlyphComponent.ResultEvent result)
	{
		if (this.currentScreen == DateADex.Dex_Screen.List)
		{
			result.result = true;
			return;
		}
		if (this.currentScreen != DateADex.Dex_Screen.Entry)
		{
			result.result = false;
			return;
		}
		if (!this.IsRecipeOpen())
		{
			result.result = this.RecipeTab != null && this.RecipeTab.isActiveAndEnabled;
			return;
		}
		result.result = true;
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0002E2B4 File Offset: 0x0002C4B4
	public Selectable GetLastSelectedEntry()
	{
		if (!(this.lastSelectedEntry == null))
		{
			return this.lastSelectedEntry;
		}
		ListBox focusingBox = this._scrollingList.GetFocusingBox();
		if (focusingBox == null)
		{
			return null;
		}
		return focusingBox.GetComponent<Selectable>();
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0002E2E4 File Offset: 0x0002C4E4
	public void Update()
	{
		if (this.player == null)
		{
			this.player = (this.player = ReInput.players.GetPlayer(0));
		}
		else if (this.DateADexWindow.activeSelf && Singleton<PhoneManager>.Instance.phoneOpened && this.player.GetAxisRawDelta("Move Vertical") != 0f && this.currentScreen == DateADex.Dex_Screen.Entry)
		{
			if (this.player.GetAxisRaw("Move Vertical") >= 0.99f)
			{
				this.switchEntry(-1);
			}
			else if (this.player.GetAxisRaw("Move Vertical") <= -0.99f)
			{
				this.switchEntry(1);
			}
		}
		this.cancelCountdownSafeguard--;
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0002E39C File Offset: 0x0002C59C
	public void GoToCollection()
	{
		if ((bool)Singleton<InkController>.Instance.story.variablesState["roption"])
		{
			Singleton<Save>.Instance.SetInteractableState("DateADexRecipe", true);
		}
		if (this.currentScreen == DateADex.Dex_Screen.Entry)
		{
			this.SwitchScreen(DateADex.Dex_Screen.Collectables);
			return;
		}
		if (this.currentScreen != DateADex.Dex_Screen.Collectables && this.currentScreen == DateADex.Dex_Screen.Recipe)
		{
			this.SwitchScreen(DateADex.Dex_Screen.Recipe);
		}
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x0002E404 File Offset: 0x0002C604
	public void UpdateListSummaryData(bool isVisible)
	{
		if (isVisible)
		{
			this.ListSummaryDataRealizedItem.SetActive(false);
			if ((bool)Singleton<InkController>.Instance.story.variablesState["roption"])
			{
				this.ListSummaryDataRealized.SetText(string.Format(": {0}", Singleton<Save>.Instance.AvailableTotalRealizedDatables()), true);
			}
			this.ListSummaryDataMet.SetText(string.Format(": {0}", Singleton<Save>.Instance.AvailableTotalMetDatables()), true);
			this.ListSummaryDataLoves.SetText(string.Format(": {0}", Singleton<Save>.Instance.AvailableTotalLoveEndings()), true);
			this.ListSummaryDataFriends.SetText(string.Format(": {0}", Singleton<Save>.Instance.AvailableTotalFriendEndings()), true);
			this.ListSummaryDataHates.SetText(string.Format(": {0}", Singleton<Save>.Instance.AvailableTotalHateEndings()), true);
			this.ListSummaryData.SetActive(true);
			return;
		}
		this.ListSummaryData.SetActive(false);
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0002E515 File Offset: 0x0002C715
	public void OnClickDexButton()
	{
		if (this.currentScreen == DateADex.Dex_Screen.List)
		{
			this.UpdateListSummaryData(true);
			return;
		}
		this.OnClickBack();
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0002E530 File Offset: 0x0002C730
	public void GoToRecipeScreen()
	{
		if (this.currentScreen == DateADex.Dex_Screen.Entry)
		{
			if (!this.IsRecipeOpen())
			{
				this.MainEntryScreen.SetActive(false);
				this.RecipeScreen.SetActive(true);
				base.StopAllCoroutines();
				this.ResetRecipeBars();
				if (this.currentEntry != null)
				{
					base.StartCoroutine(this.GoToRecipeScreenCoroutine());
				}
			}
			else
			{
				base.StopAllCoroutines();
				this.MainEntryScreen.SetActive(true);
				this.RecipeScreen.SetActive(false);
			}
			this.UpdateLegends();
		}
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0002E5AD File Offset: 0x0002C7AD
	private IEnumerator GoToRecipeScreenCoroutine()
	{
		int smartsLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("smarts", true);
		int poiseLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("poise", true);
		int empathyLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("empathy", true);
		int charmLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("charm", true);
		int sassLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("sass", true);
		int smartsPoints = this.currentEntry.GetRecipe(SpecsAttributes.Smarts);
		int poisePoints = this.currentEntry.GetRecipe(SpecsAttributes.Poise);
		int empathyPoints = this.currentEntry.GetRecipe(SpecsAttributes.Empathy);
		int charmPoints = this.currentEntry.GetRecipe(SpecsAttributes.Charm);
		int sassPoints = this.currentEntry.GetRecipe(SpecsAttributes.Sass);
		bool RecipeComplete = this.CheckRecipeCompletion(smartsLevel, smartsPoints, poiseLevel, poisePoints, empathyLevel, empathyPoints, charmLevel, charmPoints, sassLevel, sassPoints);
		yield return null;
		this.SetRecipeBar(smartsPoints, smartsLevel, this.SmartsBarCurrent, "smarts", false);
		yield return new WaitForSeconds(0.5f);
		this.SetRecipeBar(poisePoints, poiseLevel, this.PoiseBarCurrent, "poise", false);
		yield return new WaitForSeconds(0.5f);
		this.SetRecipeBar(empathyPoints, empathyLevel, this.EmpathyBarCurrent, "empathy", false);
		yield return new WaitForSeconds(0.5f);
		this.SetRecipeBar(charmPoints, charmLevel, this.CharmBarCurrent, "charm", false);
		yield return new WaitForSeconds(0.5f);
		this.SetRecipeBar(sassPoints, sassLevel, this.SassBarCurrent, "sass", RecipeComplete);
		yield break;
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0002E5BC File Offset: 0x0002C7BC
	public bool CheckRecipeCompletion(int smartsLevel, int smartsPoints, int poiseLevel, int poisePoints, int empathyLevel, int empathyPoints, int charmLevel, int charmPoints, int sassLevel, int sassPoints)
	{
		return smartsLevel >= smartsPoints && poiseLevel >= poisePoints && empathyLevel >= empathyPoints && charmLevel >= charmPoints && sassLevel >= sassPoints;
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x0002E5E4 File Offset: 0x0002C7E4
	public void BackFromRecipeScreen()
	{
		base.StopAllCoroutines();
		this.MainEntryScreen.SetActive(true);
		this.RecipeScreen.SetActive(false);
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x0002E604 File Offset: 0x0002C804
	private void SwitchScreen(DateADex.Dex_Screen destination)
	{
		if (destination == this.currentScreen)
		{
			return;
		}
		switch (destination)
		{
		case DateADex.Dex_Screen.List:
			this.collectableButton.transition = Selectable.Transition.None;
			this.UpdateListSummaryData(true);
			this.currentScreen = DateADex.Dex_Screen.List;
			break;
		case DateADex.Dex_Screen.Entry:
			this.collectableButton.transition = Selectable.Transition.Animation;
			this.UpdateListSummaryData(false);
			this.currentScreen = DateADex.Dex_Screen.Entry;
			break;
		case DateADex.Dex_Screen.Collectables:
			this.collectableButton.transition = Selectable.Transition.Animation;
			this.UpdateListSummaryData(false);
			this.currentScreen = DateADex.Dex_Screen.Collectables;
			break;
		case DateADex.Dex_Screen.Recipe:
			this.collectableButton.transition = Selectable.Transition.Animation;
			this.UpdateListSummaryData(false);
			this.currentScreen = DateADex.Dex_Screen.Recipe;
			break;
		default:
			this.UpdateListSummaryData(false);
			break;
		}
		this.DateADexAnim.SetTrigger("View" + this.currentScreen.ToString());
		this.SelectInitialElement();
		this.UpdateLegends();
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0002E6DC File Offset: 0x0002C8DC
	private void UpdateLegends()
	{
		this.UpdateActionText();
		if (this.currentScreen == DateADex.Dex_Screen.List)
		{
			this.confirmIcon.SetText("{Confirm} Show Bio");
			return;
		}
		if (this.currentScreen == DateADex.Dex_Screen.Entry)
		{
			if (!this.IsRecipeOpen())
			{
				this.confirmIcon.SetText("{UIMenuExtraSecondAction} Recipe");
				return;
			}
			this.confirmIcon.SetText("{UIMenuExtraSecondAction} Show Bio");
		}
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0002E73C File Offset: 0x0002C93C
	public void OnClickBack()
	{
		this.CancelBackLostFocus();
		if (this.cancelCountdownSafeguard < 0)
		{
			switch (this.currentScreen)
			{
			case DateADex.Dex_Screen.List:
				this.UpdateListSummaryData(true);
				this.currentSelectionDelay = null;
				this.phoneManager.gameObject.GetComponent<PhoneManager>().ReturnToMainPhoneScreen();
				break;
			case DateADex.Dex_Screen.Entry:
				this.UpdateListSummaryData(false);
				this.SwitchScreen(DateADex.Dex_Screen.List);
				break;
			case DateADex.Dex_Screen.Collectables:
				this.UpdateListSummaryData(false);
				this.SwitchScreen(DateADex.Dex_Screen.Entry);
				break;
			case DateADex.Dex_Screen.Recipe:
				this.UpdateListSummaryData(false);
				this.SwitchScreen(DateADex.Dex_Screen.Entry);
				break;
			}
		}
		this.cancelCountdownSafeguard = 2;
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x0002E7D4 File Offset: 0x0002C9D4
	public string TreatConditionalTag(string tag)
	{
		string text = tag;
		foreach (string text2 in tag.Split('{', StringSplitOptions.None))
		{
			if (text2.Contains("}"))
			{
				string text3 = text2.Substring(0, text2.IndexOf("}"));
				if (text3.Contains("!="))
				{
					string text4 = text3.Substring(0, text3.IndexOf("!=")).Trim();
					string text5 = text3.Substring(text3.IndexOf("!=") + 2).Trim();
					text5 = text5.Substring(0, text5.IndexOf(":")).Replace("\"", "").Trim();
					string text6 = text3.Substring(text3.IndexOf(":") + 1).Trim();
					if (text6.Contains("}"))
					{
						text6 = text6.Substring(0, text6.IndexOf("}")).Trim();
					}
					if (Singleton<InkController>.Instance.GetVariable(text4).ToString().ToLowerInvariant() != text5.ToLowerInvariant())
					{
						text = text6;
					}
				}
				else if (text3.Contains("=="))
				{
					string text7 = text3.Substring(0, text3.IndexOf("==")).Trim();
					string text8 = text3.Substring(text3.IndexOf("==") + 2).Trim();
					text8 = text8.Substring(0, text8.IndexOf(":")).Trim();
					text8 = text8.Replace("\"", "");
					string text9 = text3.Substring(text3.IndexOf(":") + 1).Trim();
					if (text9.Contains("}"))
					{
						text9 = text9.Substring(0, text9.IndexOf("}")).Trim();
					}
					if (Singleton<InkController>.Instance.GetVariable(text7).ToString().ToLowerInvariant() == text8.ToLowerInvariant())
					{
						text = text9;
					}
				}
				else
				{
					string text10 = Singleton<InkController>.Instance.GetVariable(text3);
					if (text10 == "null")
					{
						text10 = "...";
					}
					text = text.Replace("{" + text3 + "}", text10);
				}
			}
		}
		return text;
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x0002EA34 File Offset: 0x0002CC34
	private string GetDictionaryVariable(Dictionary<string, string> VarDictionary, string key)
	{
		if (VarDictionary.ContainsKey(key))
		{
			string text = VarDictionary[key];
			if (text.Contains("{"))
			{
				text = this.TreatConditionalTag(text);
			}
			return text;
		}
		return "";
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0002EA70 File Offset: 0x0002CC70
	private DateADexEntry setupEntry(string location)
	{
		List<string> list = null;
		try
		{
			list = Singleton<InkController>.Instance.story.TagsForContentAtPath(location);
		}
		catch (Exception)
		{
		}
		string text = location.Split(".", StringSplitOptions.None)[1].Split("_", StringSplitOptions.None)[0];
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (string text2 in list)
		{
			string[] array = text2.Trim().Split(":", 2, StringSplitOptions.None);
			if (array.Length >= 2)
			{
				string text3 = array[0].ToLowerInvariant();
				string text4 = array[1];
				if (!dictionary.ContainsKey(text3.Trim()))
				{
					dictionary.Add(text3.Trim(), text4.Trim());
				}
			}
		}
		DateADexEntry dateADexEntry = new DateADexEntry();
		dateADexEntry.CharIndex = int.Parse(this.GetDictionaryVariable(dictionary, "index"));
		dateADexEntry.internalName = this.GetDictionaryVariable(dictionary, "internalname");
		dateADexEntry.NumberOfCollectables = int.Parse(this.GetDictionaryVariable(dictionary, "numberofcollectables"));
		if (dateADexEntry.internalName == "skylar" && !Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			dateADexEntry.NumberOfCollectables = 3;
		}
		dateADexEntry.CharName = this.GetDictionaryVariable(dictionary, "name");
		dateADexEntry.CharObj = this.GetDictionaryVariable(dictionary, "objname");
		dateADexEntry.VoActor = this.GetDictionaryVariable(dictionary, "voactor");
		dateADexEntry.CharLikes = this.GetDictionaryVariable(dictionary, "likes");
		dateADexEntry.CharDislikes = this.GetDictionaryVariable(dictionary, "dislikes");
		dateADexEntry.CharDYK = "<i>" + this.GetDictionaryVariable(dictionary, "didyouknow") + "</i>";
		dateADexEntry.Recipe = this.GetDictionaryVariable(dictionary, "dexrecipe");
		dateADexEntry.SpecsAttribute = this.GetDictionaryVariable(dictionary, "specs");
		this._names.Add(dateADexEntry.CharIndex, dateADexEntry.internalName);
		this._ids.Add(dateADexEntry.internalName, dateADexEntry.CharIndex);
		if (Singleton<CharacterHelper>.Instance._characters.IsNameInSet(dateADexEntry.internalName))
		{
			dateADexEntry.CharImageProvider = new DataADexCharImageProvider(dateADexEntry.internalName, Singleton<Save>.Instance.GetDateStatus(dateADexEntry.internalName));
		}
		if (dictionary.ContainsKey("dex " + 1.ToString()))
		{
			int num = 1;
			while (dictionary.ContainsKey("dex " + num.ToString()))
			{
				string dictionaryVariable = this.GetDictionaryVariable(dictionary, "dex " + num.ToString());
				dateADexEntry.DexComments.Add(dictionaryVariable);
				dateADexEntry.notification = Singleton<Save>.Instance.GetCharactersWithNewStatusOnDateADex(dateADexEntry.internalName);
				CollectableToBool dexEntriesUnlocked = Singleton<Save>.Instance.GetDexEntriesUnlocked(dateADexEntry.internalName);
				if (dexEntriesUnlocked.Keys.Contains(num) && dexEntriesUnlocked[num])
				{
					dateADexEntry.CharDYK = "\uff3f " + dictionaryVariable.Replace("uFF3F ", "") + "<br>" + dateADexEntry.CharDYK;
				}
				num++;
			}
		}
		else
		{
			dateADexEntry.DexComments.Add("No dex entries available.");
		}
		int num2 = 1;
		while (dictionary.ContainsKey("collectable" + num2.ToString()))
		{
			string dictionaryVariable2 = this.GetDictionaryVariable(dictionary, "collectable" + num2.ToString());
			string dictionaryVariable3 = this.GetDictionaryVariable(dictionary, "collectable" + num2.ToString() + "desc");
			string dictionaryVariable4 = this.GetDictionaryVariable(dictionary, "collectable" + num2.ToString() + "hint");
			if (!(dateADexEntry.internalName == "skylar") || Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION || num2 != 4)
			{
				dateADexEntry.Collectable_Names_Desc_Hint.Add(Tuple.Create<string, string, string>(dictionaryVariable2, dictionaryVariable3, dictionaryVariable4));
			}
			num2++;
		}
		return dateADexEntry;
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x0002EE68 File Offset: 0x0002D068
	public void SetupMenus()
	{
		if (!this.setup)
		{
			return;
		}
		if (!Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			int i = 0;
			while (i < this.EntriesCompleted.Count)
			{
				if (this.EntriesCompleted[i].internalName == DeluxeEditionController.DELUXE_CHARACTER_1 || this.EntriesCompleted[i].internalName == DeluxeEditionController.DELUXE_CHARACTER_2)
				{
					this.EntriesCompleted.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
		this._dexListBank.Init(this.EntriesCompleted);
		this._scrollingList.Initialize();
		this._scrollingList.Refresh(0);
		this.OnEntryFocused(this._scrollingList.GetFocusingBox(), this._scrollingList.GetFocusingBox());
		this.collectableButton.transition = Selectable.Transition.None;
		this.SetupEntryList();
		this.SetupEntryNavigation();
		this.SelectInitialElement();
		this.UpdateSortText();
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x0002EF50 File Offset: 0x0002D150
	public void OnEntryButtonSelected(ListBox listBox)
	{
		DateADexEntry dexEntry = (listBox as DexEntryButton).DexEntry;
		if (listBox.ContentID == this._scrollingList.GetFocusingContentID())
		{
			this.OpenEntry(dexEntry.CharIndex);
			return;
		}
		this._scrollingList.SelectContentID(listBox.ContentID, true);
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0002EF9C File Offset: 0x0002D19C
	public void ScrollToEntryButton(DexEntryButton entryButton)
	{
		int contentID = entryButton.ContentID;
		this._scrollingList.SelectContentID(contentID, true);
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x0002EFC0 File Offset: 0x0002D1C0
	public void PageEntries(bool up)
	{
		int focusingContentID = this._scrollingList.GetFocusingContentID();
		for (int i = 0; i < this.EntriesCompleted.Count; i++)
		{
			if (this.EntriesCompleted[i] != null && this.EntriesCompleted[i].CharIndex == focusingContentID)
			{
				int num = Math.Clamp(i + (up ? (-this.pageAmount) : this.pageAmount), 0, this.EntriesCompleted.Count - 1);
				num = this.EntriesCompleted[num].CharIndex;
				ListBox[] listBoxes = this._scrollingList.ListBoxes;
				int j = 0;
				int num2 = listBoxes.Length;
				while (j < num2)
				{
					if (listBoxes[j] != null && listBoxes[j].ContentID == num)
					{
						if (listBoxes[j].gameObject.activeSelf)
						{
							for (int k = 0; k < this.entryButtonList.Count; k++)
							{
								if (this.entryButtonList[k].ContentID == listBoxes[j].ContentID)
								{
									ControllerMenuUI.SetCurrentlySelected(this.entryButtonList[k].gameObject, up ? ControllerMenuUI.Direction.Up : ControllerMenuUI.Direction.Down, true, false);
									return;
								}
							}
							ControllerMenuUI.SetCurrentlySelected(listBoxes[j].gameObject, up ? ControllerMenuUI.Direction.Up : ControllerMenuUI.Direction.Down, true, false);
							return;
						}
						if (up)
						{
							ControllerMenuUI.SetCurrentlySelected(this.entryButtonList[0].gameObject, ControllerMenuUI.Direction.Up, true, false);
						}
						else
						{
							for (int l = this.entryButtonList.Count - 1; l > 0; l--)
							{
								if (this.entryButtonList[l].gameObject.activeSelf)
								{
									ControllerMenuUI.SetCurrentlySelected(this.entryButtonList[l].gameObject, ControllerMenuUI.Direction.Down, true, false);
									return;
								}
							}
							ControllerMenuUI.SetCurrentlySelected(this.entryButtonList[this.entryButtonList.Count - 1].gameObject, ControllerMenuUI.Direction.Down, true, false);
						}
					}
					j++;
				}
			}
		}
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0002F1B0 File Offset: 0x0002D3B0
	public void OnEntryFocused(ListBox prevFocusingBox, ListBox curFocusingBox)
	{
		DexEntryButton dexEntryButton = curFocusingBox as DexEntryButton;
		DateADexEntry dexEntry = dexEntryButton.DexEntry;
		DateADexEntry dateADexEntry = this.EntriesOrdered[dexEntry.CharIndex];
		DataADexCharImageProvider backgroundCharImageProvider = this._backgroundCharImageProvider;
		if (backgroundCharImageProvider != null)
		{
			backgroundCharImageProvider.ReleaseImage();
		}
		this._backgroundCharImageProvider = dateADexEntry.CharImageProvider;
		this.BackgroundImage.sprite = dateADexEntry.CharImage;
		this.BackgroundImage.color = (dexEntryButton.MetEntry ? Color.white : Color.black);
		this.BackgroundImageShadow.sprite = dateADexEntry.CharImage;
		this.BackgroundImageShadow.gameObject.SetActive(dexEntryButton.MetEntry);
		int dateableCollectablesNumber = Singleton<Save>.Instance.GetDateableCollectablesNumber(dexEntryButton.DexEntry.internalName);
		this.collectableButtonText.SetText(dateableCollectablesNumber.ToString() + " / " + dexEntryButton.DexEntry.NumberOfCollectables.ToString(), true);
		if (dexEntryButton.DexEntry.NumberOfCollectables == 0)
		{
			this.collectableButton.gameObject.SetActive(false);
		}
		else
		{
			this.collectableButton.gameObject.SetActive(true);
		}
		if (dateableCollectablesNumber == dexEntryButton.DexEntry.NumberOfCollectables)
		{
			this.collectableButtonSparkles.SetActive(true);
		}
		else
		{
			this.collectableButtonSparkles.SetActive(false);
		}
		if (this.entryButtonList != null)
		{
			this.SetupEntryNavigation();
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0002F300 File Offset: 0x0002D500
	public void ClearUpLoadedAssets()
	{
		this.BackgroundImage.sprite = null;
		this.BackgroundImageShadow.sprite = null;
		this.Sprite.sprite = null;
		DataADexCharImageProvider backgroundCharImageProvider = this._backgroundCharImageProvider;
		if (backgroundCharImageProvider != null)
		{
			backgroundCharImageProvider.ReleaseImage();
		}
		this._backgroundCharImageProvider = null;
		DataADexCharImageProvider spriteCharImageProvider = this._spriteCharImageProvider;
		if (spriteCharImageProvider != null)
		{
			spriteCharImageProvider.ReleaseImage();
		}
		this._spriteCharImageProvider = null;
		this.FreeLoadedSprites();
		if (this._collectablesScreen != null)
		{
			this._collectablesScreen.UnloadAddressableResources();
		}
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0002F380 File Offset: 0x0002D580
	public void ResetSort()
	{
		if (this._dexListBank.IsInitiated())
		{
			this._dexListBank.SortBy(DexListBank.SortMethod.Index);
			this._dexListBank.ResetSort();
			this.UpdateSortText();
			this._scrollingList.Refresh(0);
			this.OnEntryFocused(this._scrollingList.GetFocusingBox(), this._scrollingList.GetFocusingBox());
		}
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0002F3E0 File Offset: 0x0002D5E0
	public void OnSortButtonClicked()
	{
		if (this.currentScreen != DateADex.Dex_Screen.List)
		{
			return;
		}
		this._dexListBank.NextSort();
		this.UpdateSortText();
		this._scrollingList.Refresh(0);
		this.OnEntryFocused(this._scrollingList.GetFocusingBox(), this._scrollingList.GetFocusingBox());
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0002F430 File Offset: 0x0002D630
	private void UpdateSortText()
	{
		DexListBank.SortMethod sortMethod = this._dexListBank.GetSortMethod();
		switch (sortMethod)
		{
		case DexListBank.SortMethod.Index:
			this._sortButtonText.text = "<sprite=0 tint=1 color=#F0005C>  1-100";
			break;
		case DexListBank.SortMethod.New:
			this._sortButtonText.text = "<sprite=0 tint=1 color=#F0005C>  New";
			break;
		case DexListBank.SortMethod.Status:
			this._sortButtonText.text = "<sprite=0 tint=1 color=#F0005C>  Status";
			break;
		case DexListBank.SortMethod.Alphabetical:
			this._sortButtonText.text = "<sprite=0 tint=1 color=#F0005C>  A-Z";
			break;
		default:
			this._sortButtonText.text = "<sprite=0 tint=1 color=#F0005C>  " + sortMethod.ToString();
			break;
		}
		this.UpdateActionText();
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0002F4D4 File Offset: 0x0002D6D4
	private void UpdateActionText()
	{
		DateADex.Dex_Screen dex_Screen = this.currentScreen;
		if (dex_Screen != DateADex.Dex_Screen.List)
		{
			if (dex_Screen != DateADex.Dex_Screen.Entry)
			{
				return;
			}
			this.actionIcon.SetText("{UIMenuExtraAction} Collectables");
			return;
		}
		else
		{
			DexListBank.SortMethod sortMethod = this._dexListBank.GetSortMethod();
			switch (sortMethod)
			{
			case DexListBank.SortMethod.Index:
				this.actionIcon.SetText("{UIMenuExtraAction} Sort: 1-100");
				return;
			case DexListBank.SortMethod.New:
				this.actionIcon.SetText("{UIMenuExtraAction} Sort: New");
				return;
			case DexListBank.SortMethod.Status:
				this.actionIcon.SetText("{UIMenuExtraAction} Sort: Status");
				return;
			case DexListBank.SortMethod.Alphabetical:
				this.actionIcon.SetText("{UIMenuExtraAction} Sort: A-Z");
				return;
			default:
				this.actionIcon.SetText("{UIMenuExtraAction} Sort: " + sortMethod.ToString());
				return;
			}
		}
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0002F58E File Offset: 0x0002D78E
	public Dictionary<string, int> GetIds()
	{
		return this._ids;
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x0002F598 File Offset: 0x0002D798
	public void SetupEntryList()
	{
		this.entryButtonList = new List<DexEntryButton>();
		foreach (DexEntryButton dexEntryButton in Object.FindObjectsOfType(typeof(DexEntryButton), true))
		{
			this.entryButtonList.Add(dexEntryButton);
			dexEntryButton.SetupOnSelect(this);
		}
		this.entryButtonList = this.entryButtonList.OrderBy((DexEntryButton e) => e.gameObject.name).ToList<DexEntryButton>();
		this.entryButtonList.RemoveAt(this.entryButtonList.Count - 1);
		this.entryButtonList.RemoveAt(0);
		this.entryButtonList.Add(this.entryButtonList[2]);
		this.entryButtonList.Add(this.entryButtonList[3]);
		this.entryButtonList.Add(this.entryButtonList[4]);
		this.entryButtonList.Add(this.entryButtonList[5]);
		this.entryButtonList.Add(this.entryButtonList[6]);
		this.entryButtonList.Add(this.entryButtonList[7]);
		this.entryButtonList.Add(this.entryButtonList[8]);
		this.entryButtonList.RemoveAt(8);
		this.entryButtonList.RemoveAt(7);
		this.entryButtonList.RemoveAt(6);
		this.entryButtonList.RemoveAt(5);
		this.entryButtonList.RemoveAt(4);
		this.entryButtonList.RemoveAt(3);
		this.entryButtonList.RemoveAt(2);
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0002F73C File Offset: 0x0002D93C
	public void SetupEntryNavigation()
	{
		DateADexEntry dateADexEntry = (DateADexEntry)this._dexListBank.GetListContent(0);
		DateADexEntry dateADexEntry2 = (DateADexEntry)this._dexListBank.GetListContent(this._dexListBank.GetContentCount() - 1);
		for (int i = 0; i < this.entryButtonList.Count; i++)
		{
			Navigation navigation = this.entryButtonList[i].transform.GetComponent<Button>().navigation;
			navigation.selectOnLeft = this.backButtonComponent;
			if (this.entryButtonList[i].DexEntry == dateADexEntry)
			{
				navigation.selectOnUp = null;
				Navigation navigation2 = this.CollectableButton.navigation;
				navigation2.selectOnDown = this.entryButtonList[i].transform.GetComponent<Button>();
				navigation2.mode = Navigation.Mode.Explicit;
				this.CollectableButton.navigation = navigation2;
			}
			else if (i == 0)
			{
				navigation.selectOnUp = this.entryButtonList[this.entryButtonList.Count - 1].transform.GetComponent<Button>();
			}
			else
			{
				navigation.selectOnUp = this.entryButtonList[Mathf.Clamp(i - 1, 0, this.entryButtonList.Count - 1)].transform.GetComponent<Button>();
			}
			if (this.entryButtonList[i].DexEntry == dateADexEntry2)
			{
				navigation.selectOnDown = this.SortButton;
				Navigation navigation3 = this.SortButton.navigation;
				navigation3.selectOnUp = this.entryButtonList[i].transform.GetComponent<Button>();
				navigation3.mode = Navigation.Mode.Explicit;
				this.SortButton.navigation = navigation3;
			}
			else if (i == this.entryButtonList.Count - 1)
			{
				navigation.selectOnDown = this.entryButtonList[0].transform.GetComponent<Button>();
			}
			else
			{
				navigation.selectOnDown = this.entryButtonList[Mathf.Clamp(i + 1, 0, this.entryButtonList.Count - 1)].transform.GetComponent<Button>();
			}
			navigation.mode = Navigation.Mode.Explicit;
			this.entryButtonList[i].transform.GetComponent<Button>().navigation = navigation;
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x0002F964 File Offset: 0x0002DB64
	public bool CheckEntriesForNotifications()
	{
		foreach (DateADexEntry dateADexEntry in this.EntriesOrdered)
		{
			if (dateADexEntry != null && dateADexEntry.notification)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x0002F998 File Offset: 0x0002DB98
	public void OpenEntry(int id)
	{
		this.RecipeTab.gameObject.SetActive(Singleton<Save>.Instance.GetInteractableState("DateADexRecipe"));
		this.MainEntryScreen.SetActive(true);
		this.RecipeScreen.SetActive(false);
		this.currentEntry = this.EntriesOrdered[id];
		if (this.currentEntry.notification)
		{
			this._scrollingList.transform.GetChild(this._scrollingList.transform.childCount - 1).GetComponent<DexEntryButton>().notificationIndecator.SetActive(false);
		}
		this.currentEntry.notification = false;
		Singleton<Save>.Instance.RemoveCharactersWithNewStatusOnDateADex(this.currentEntry.internalName);
		Singleton<PhoneManager>.Instance.ChangeDateADexPing();
		this.currententry = this.EntriesCompleted.FindIndex((DateADexEntry Ent) => Ent.CharIndex == this.currentEntry.CharIndex);
		DataADexCharImageProvider spriteCharImageProvider = this._spriteCharImageProvider;
		if (spriteCharImageProvider != null)
		{
			spriteCharImageProvider.ReleaseImage();
		}
		this._spriteCharImageProvider = this.currentEntry.CharImageProvider;
		DataADexCharImageProvider backgroundCharImageProvider = this._backgroundCharImageProvider;
		if (backgroundCharImageProvider != null)
		{
			backgroundCharImageProvider.ReleaseImage();
		}
		this._backgroundCharImageProvider = this.currentEntry.CharImageProvider;
		this.Sprite.sprite = this.currentEntry.CharImage;
		this.BackgroundImageShadow.sprite = this.currentEntry.CharImage;
		this.title.SetContent(this.currentEntry);
		this.RealizedIcon1.gameObject.SetActive(Singleton<Save>.Instance.GetDateStatusRealized(this.currentEntry.CharIndex) == RelationshipStatus.Realized);
		this.RealizedIcon2.gameObject.SetActive(Singleton<Save>.Instance.GetDateStatusRealized(this.currentEntry.CharIndex) == RelationshipStatus.Realized);
		if (this.currentEntry.internalName == "mikey")
		{
			this.SpecsIcon1.sprite = this.mikeyBadge;
			this.SpecsIcon2.sprite = this.mikeyBadge;
		}
		else if (this.currentEntry.internalName == "lucinda")
		{
			this.SpecsIcon1.sprite = this.lucindaBadge;
			this.SpecsIcon2.sprite = this.lucindaBadge;
		}
		else if (this.currentEntry.SpecsAttribute == "charm")
		{
			this.SpecsIcon1.sprite = this.charmBadge;
			this.SpecsIcon2.sprite = this.charmBadge;
		}
		else if (this.currentEntry.SpecsAttribute == "empathy")
		{
			this.SpecsIcon1.sprite = this.empathyBadge;
			this.SpecsIcon2.sprite = this.empathyBadge;
		}
		else if (this.currentEntry.SpecsAttribute == "poise")
		{
			this.SpecsIcon1.sprite = this.poiseBadge;
			this.SpecsIcon2.sprite = this.poiseBadge;
		}
		else if (this.currentEntry.SpecsAttribute == "sass")
		{
			this.SpecsIcon1.sprite = this.sassBadge;
			this.SpecsIcon2.sprite = this.sassBadge;
		}
		else if (this.currentEntry.SpecsAttribute == "smarts")
		{
			this.SpecsIcon1.sprite = this.smartsBadge;
			this.SpecsIcon2.sprite = this.smartsBadge;
		}
		if (Singleton<Save>.Instance.GetDateStatus(this.currentEntry.CharIndex) != RelationshipStatus.Unmet)
		{
			this.Item.text = this.currentEntry.CharObj;
			this.Desc.text = this.currentEntry.CharDYK;
			this.VoActor.text = this.currentEntry.VoActor;
			this.Likes.text = this.currentEntry.CharLikes;
			this.Dislikes.text = this.currentEntry.CharDislikes;
			this.Sprite.color = Color.white;
			this.DateableIcon1.gameObject.SetActive(true);
			this.DateableIcon2.gameObject.SetActive(true);
			this.DateableIcon3.gameObject.SetActive(true);
			this.DateableIcon1.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Icons", this.currentEntry.internalName + "_icon"), false);
			this.DateableIcon2.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Icons", this.currentEntry.internalName + "_icon"), false);
			this.DateableIcon3.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Icons", this.currentEntry.internalName + "_icon"), false);
			this.FreeLoadedSprites();
			this.AddLoadedSprite(this.DateableIcon1.sprite);
			this.AddLoadedSprite(this.DateableIcon2.sprite);
			this.AddLoadedSprite(this.DateableIcon3.sprite);
		}
		else
		{
			this.Item.text = "???";
			this.Desc.text = "You haven't met this character yet!";
			this.VoActor.text = "???";
			this.Likes.text = "???";
			this.Dislikes.text = "???";
			this.Sprite.color = Color.black;
			this.DateableIcon1.gameObject.SetActive(false);
			this.DateableIcon2.gameObject.SetActive(false);
			this.DateableIcon3.gameObject.SetActive(false);
		}
		this.DescScroll.verticalNormalizedPosition = 1f;
		this.UpdateVOActorPosition(id);
		if (this.currentEntry.internalName == "lucinda")
		{
			this.CollectableButton.gameObject.SetActive(false);
		}
		else
		{
			this.CollectableButton.gameObject.SetActive(true);
			this._collectablesScreen.SetupCollectables(this.currentEntry);
		}
		try
		{
			switch (Singleton<Save>.Instance.GetDateStatus(this.currentEntry.CharIndex))
			{
			case RelationshipStatus.Hate:
				this.Status.sprite = this.hatesprite;
				this.Status.gameObject.SetActive(true);
				this.StatusBackground.SetActive(true);
				goto IL_071D;
			case RelationshipStatus.Single:
				this.Status.sprite = this.singlesprite;
				this.Status.gameObject.SetActive(false);
				this.StatusBackground.SetActive(false);
				goto IL_071D;
			case RelationshipStatus.Love:
				this.Status.sprite = this.lovesprite;
				this.Status.gameObject.SetActive(true);
				this.StatusBackground.SetActive(true);
				goto IL_071D;
			case RelationshipStatus.Friend:
				this.Status.sprite = this.shippedsprite;
				this.Status.gameObject.SetActive(true);
				this.StatusBackground.SetActive(true);
				goto IL_071D;
			}
			this.Status.sprite = this.errorsprite;
			this.Status.gameObject.SetActive(false);
			this.StatusBackground.SetActive(false);
			IL_071D:;
		}
		catch (Exception)
		{
			this.Status.sprite = this.errorsprite;
		}
		if (this.currentScreen != DateADex.Dex_Screen.Entry)
		{
			this.SwitchScreen(DateADex.Dex_Screen.Entry);
		}
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00030104 File Offset: 0x0002E304
	private void AddLoadedSprite(Sprite sprite)
	{
		if (sprite != null)
		{
			this.loadedSprites.Add(sprite);
		}
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x0003011C File Offset: 0x0002E31C
	private void FreeLoadedSprites()
	{
		int i = 0;
		int count = this.loadedSprites.Count;
		while (i < count)
		{
			if (this.loadedSprites[i] != null)
			{
				Services.AssetProviderService.UnloadResourceAsset(this.loadedSprites[i]);
			}
			i++;
		}
		this.loadedSprites.Clear();
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00030178 File Offset: 0x0002E378
	private void SetRecipeBar(int points, int statLevel, GameObject barCurrent, string namedStat, bool RecipeComplete = false)
	{
		float num = (Mathf.Approximately((float)points, 0f) ? 0f : ((float)points / 100f));
		float num2 = (Mathf.Approximately((float)statLevel, 0f) ? 0f : ((float)statLevel / 100f));
		num = Mathf.Clamp(num, 0.03f, 1f);
		num2 = Mathf.Clamp(num2, 0.03f, 1f);
		RecipeBarColorTrigger component = barCurrent.GetComponent<RecipeBarColorTrigger>();
		Image component2 = barCurrent.GetComponent<Image>();
		component2.DOKill(false);
		component2.fillAmount = 0f;
		if (component != null)
		{
			component.Reset();
		}
		component2.DOFillAmount(num2, 2f).SetEase(Ease.InCubic);
		if (statLevel >= points)
		{
			if (component != null)
			{
				component.SetTargetValue(namedStat, num);
			}
			if (namedStat == "sass" && RecipeComplete)
			{
				if (component != null)
				{
					component.SetTargetSoundTrigger(namedStat, num2, false, true);
					return;
				}
			}
			else if (component != null)
			{
				component.SetTargetSoundTrigger(namedStat, num2, false, false);
				return;
			}
		}
		else if (component != null)
		{
			component.SetTargetSoundTrigger(namedStat, num2, true, false);
		}
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x0003026C File Offset: 0x0002E46C
	private void ResetRecipeBars()
	{
		this.SetupRecipeBar(this.SmartsBar, this.SmartsPoints, this.SmartsBarCap, this.currentEntry.GetRecipe(SpecsAttributes.Smarts), this.SmartsBarCurrent);
		this.SetupRecipeBar(this.PoiseBar, this.PoisePoints, this.PoiseBarCap, this.currentEntry.GetRecipe(SpecsAttributes.Poise), this.PoiseBarCurrent);
		this.SetupRecipeBar(this.EmpathyBar, this.EmpathyPoints, this.EmpathyBarCap, this.currentEntry.GetRecipe(SpecsAttributes.Empathy), this.EmpathyBarCurrent);
		this.SetupRecipeBar(this.CharmBar, this.CharmPoints, this.CharmBarCap, this.currentEntry.GetRecipe(SpecsAttributes.Charm), this.CharmBarCurrent);
		this.SetupRecipeBar(this.SassBar, this.SassPoints, this.SassBarCap, this.currentEntry.GetRecipe(SpecsAttributes.Sass), this.SassBarCurrent);
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x0003034C File Offset: 0x0002E54C
	private void SetupRecipeBar(GameObject barBG, GameObject percentageText, GameObject cap, int points, GameObject barCurrent)
	{
		float num = (Mathf.Approximately((float)points, 0f) ? 0f : ((float)points / 100f));
		num = Mathf.Clamp(num, 0.03f, 1f);
		RecipeBarColorTrigger component = barCurrent.GetComponent<RecipeBarColorTrigger>();
		Image component2 = barCurrent.GetComponent<Image>();
		RectTransform component3 = cap.GetComponent<RectTransform>();
		Vector2 anchorMax = component3.anchorMax;
		Vector2 anchorMin = component3.anchorMin;
		anchorMax.y = Mathf.Lerp(0.035f, 0.965f, num);
		anchorMin.y = anchorMax.y;
		component3.anchorMax = anchorMax;
		component3.anchorMin = anchorMin;
		component3.anchoredPosition = Vector2.zero;
		barBG.GetComponent<Image>().fillAmount = num;
		percentageText.GetComponent<TextMeshProUGUI>().SetText(string.Format("{0}", points), true);
		component2.DOKill(false);
		component2.fillAmount = 0f;
		if (component == null)
		{
			return;
		}
		component.Reset();
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x0003042D File Offset: 0x0002E62D
	public void OpenEntry()
	{
		this.OpenEntry(this.pagetoswitch);
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x0003043C File Offset: 0x0002E63C
	public void switchEntry(int dir)
	{
		int num = (this.currententry + dir) % this.EntriesCompleted.Count;
		if (num < 0)
		{
			num = this.EntriesCompleted.Count - 1;
		}
		this.pagetoswitch = this.EntriesCompleted[num].CharIndex;
		this.DateADexAnim.SetTrigger("ViewEntry");
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00030497 File Offset: 0x0002E697
	public string GetInternalName(string name)
	{
		return Singleton<CharacterHelper>.Instance._characters.GetInternalName(name.Replace("-", string.Empty).ToLowerInvariant());
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x000304BD File Offset: 0x0002E6BD
	public string GetInkFileNamePrefix(string name)
	{
		if (name.Contains("."))
		{
			return name.Replace(".ink", "").Split('.', StringSplitOptions.None)[0];
		}
		return name.Replace(".ink", "");
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x000304F8 File Offset: 0x0002E6F8
	public int GetCharIndex(string name)
	{
		name = this.GetInkFileNamePrefix(name);
		string internalName = Singleton<CharacterHelper>.Instance._characters.GetInternalName(name.ToLowerInvariant());
		if (name.ToLowerInvariant().Contains("tresta") || !Singleton<CharacterHelper>.Instance._characters.IsNameInSet(internalName))
		{
			return -1;
		}
		if (name.ToLowerInvariant() == "curt" || name.ToLowerInvariant() == "rod")
		{
			return this._ids["curtrod"];
		}
		if (name.ToLowerInvariant().Contains("hank"))
		{
			return this._ids["hanks"];
		}
		int num;
		try
		{
			num = this._ids[internalName];
		}
		catch (Exception ex)
		{
			T17Debug.LogError("GetCharIndex(" + name + ") - " + ex.Message);
			num = -1;
		}
		return num;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x000305E4 File Offset: 0x0002E7E4
	public string GetCharName(int index)
	{
		return this._names[index];
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x000305F2 File Offset: 0x0002E7F2
	public string GetCharStatus(int index)
	{
		return this._names[index];
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x00030600 File Offset: 0x0002E800
	public void TestCollectable()
	{
		this.UnlockCollectable("celia", 0, false, false, true);
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00030614 File Offset: 0x0002E814
	public void UnlockCollectable(string _name, int index, int time)
	{
		string internalName = this.GetInternalName(_name);
		if (!string.IsNullOrEmpty(internalName) && !string.IsNullOrWhiteSpace(internalName))
		{
			Singleton<Save>.Instance.UnlockCollectable(internalName, index);
			Sprite spriteFromCollectableNum = Singleton<CharacterHelper>.Instance._characters[internalName].GetSpriteFromCollectableNum(internalName, index);
			this.ActivateIfNeeded();
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_collectable_appear, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			base.StartCoroutine(this.UnlockCollectable(spriteFromCollectableNum, (float)time, true));
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0003069C File Offset: 0x0002E89C
	public void UnlockCollectableHold(string _name, int index, bool hideCollectablePopup = false, bool hideDateADexCollectable = false)
	{
		string internalName = this.GetInternalName(_name);
		if (!string.IsNullOrEmpty(internalName) && !string.IsNullOrWhiteSpace(internalName))
		{
			if (!hideDateADexCollectable)
			{
				Singleton<Save>.Instance.UnlockCollectable(internalName, index);
			}
			if (!hideCollectablePopup)
			{
				Sprite spriteFromCollectableNum = Singleton<CharacterHelper>.Instance._characters[internalName].GetSpriteFromCollectableNum(internalName, index);
				this.ActivateIfNeeded();
				base.StartCoroutine(this.Co_UnlockCollectableHold(spriteFromCollectableNum));
			}
		}
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00030700 File Offset: 0x0002E900
	public void UnlockMagicCollectableHold()
	{
		int num = -1;
		int num2 = 0;
		string text = "";
		while (num2 < 50 && num == -1)
		{
			List<string> list = Singleton<CharacterHelper>.Instance._characters.ToList<string>();
			int num3 = Random.Range(0, list.Count);
			text = list[num3];
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			CollectableToBool dateableCollectables = Singleton<Save>.Instance.GetDateableCollectables(text);
			foreach (int num4 in dateableCollectables.Keys)
			{
				if (num4 == 1 && dateableCollectables[num4])
				{
					flag = true;
				}
				else if (num4 == 2 && dateableCollectables[num4])
				{
					flag2 = true;
				}
				else if (num4 == 3 && dateableCollectables[num4])
				{
					flag3 = true;
				}
				if (!dateableCollectables[num4])
				{
					num = num4;
					break;
				}
			}
			if (!flag)
			{
				num = 1;
			}
			else if (!flag2)
			{
				num = 2;
			}
			if (!flag3)
			{
				num = 3;
			}
		}
		if (num > -1 && text != "")
		{
			Singleton<Save>.Instance.UnlockCollectable(text, num);
			Sprite spriteFromCollectableNum = Singleton<CharacterHelper>.Instance._characters[text].GetSpriteFromCollectableNum(text, num);
			this.ActivateIfNeeded();
			base.StartCoroutine(this.Co_UnlockCollectableHold(spriteFromCollectableNum));
		}
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00030854 File Offset: 0x0002EA54
	public void UnlockEnding(string _name, RelationshipStatus status, bool skipEndingScreen = false, bool skipStatusChange = false)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		string text = "";
		if (_name == "luna")
		{
			flag = true;
			_name = "connie";
		}
		else if (_name == "dirk")
		{
			flag2 = true;
		}
		else if (_name == "clarence")
		{
			flag3 = true;
			_name = "dirk";
		}
		else if (_name == "volt")
		{
			flag4 = true;
		}
		else if (_name == "jonwick")
		{
			flag5 = true;
			_name = "scandalabra";
		}
		else if (_name == "timmy")
		{
			flag6 = true;
			_name = "tim";
		}
		else if (_name == "skips")
		{
			flag7 = true;
			_name = "shadow";
		}
		else if (_name == "front" || _name == "trap" || _name == "tiny" || _name == "back")
		{
			text = _name;
			_name = "dorian";
		}
		if (flag3 || flag2)
		{
			if (Singleton<InkController>.Instance.GetVariable("harper_dirk") == "healthy" && Singleton<InkController>.Instance.GetVariable("clarence_transform") != "dirk")
			{
				flag2 = false;
				flag3 = true;
				_name = "clarence";
			}
			else
			{
				flag2 = true;
				flag3 = false;
				_name = "dirk";
			}
		}
		this.ActivateIfNeeded();
		string text2 = "";
		string text3 = "";
		string text4 = "";
		DateADexEntry dateADexEntry = null;
		if (_name.Contains("_"))
		{
			foreach (string text5 in _name.Trim().Split('_', StringSplitOptions.None))
			{
				if (!skipStatusChange && status != RelationshipStatus.Realized)
				{
					Singleton<Save>.Instance.SetDateStatus(text5, status);
				}
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(_name);
			if (num <= 1379856436U)
			{
				if (num <= 138459817U)
				{
					if (num != 68106214U)
					{
						if (num == 138459817U)
						{
							if (_name == "curt_rod")
							{
								text2 = "9";
								text3 = "Curt & Rod";
								text4 = "Curtain";
							}
						}
					}
					else if (_name == "washford_drysdale")
					{
						text2 = "73 & 74";
						text3 = "Washford & Drysdale";
						text4 = "Washer & Dryer";
					}
				}
				else if (num != 762829470U)
				{
					if (num == 1379856436U)
					{
						if (_name == "eddie_volt")
						{
							text2 = "19";
							text3 = "Eddie & Volt";
							text4 = "Electricity";
						}
					}
				}
				else if (_name == "celia_florence")
				{
					text2 = "4 & 5";
					text3 = "Celia & Florence";
					text4 = "Ceiling & Floor";
				}
			}
			else if (num <= 1904873995U)
			{
				if (num != 1741711064U)
				{
					if (num == 1904873995U)
					{
						if (_name == "abel_dasha")
						{
							text2 = "11 & 53";
							text3 = "Abel & Dasha";
							text4 = "Table & Desk";
						}
					}
				}
				else if (_name == "dirk_harper")
				{
					text2 = "75 & 76";
					text3 = "Dirk Deveraux & Harper";
					text4 = "Dirty Laundry & Hamper";
				}
			}
			else if (num != 3658210494U)
			{
				if (num == 4285864528U)
				{
					if (_name == "tina_tony")
					{
						text2 = "30 & 84";
						text3 = "Tina & Tony";
						text4 = "Musical Triangle & Toolbox";
					}
				}
			}
			else if (_name == "artt_doug")
			{
				text2 = "17 & 98";
				text3 = "Artt & Doug";
				text4 = "Art & Overwhelming Sense of Existential Dread";
			}
		}
		else
		{
			if (!skipStatusChange && status != RelationshipStatus.Realized)
			{
				Singleton<Save>.Instance.SetDateStatus(_name, status);
			}
			dateADexEntry = DateADex.Instance.GetEntry(_name);
			if (dateADexEntry != null)
			{
				text3 = dateADexEntry.CharName;
				text4 = dateADexEntry.CharObj;
			}
			if (flag)
			{
				text3 = "Luna";
			}
			else if (flag2)
			{
				text3 = "Dirk Deveraux";
				text2 = "75";
				_name = "dirk";
				dateADexEntry = null;
			}
			else if (flag3)
			{
				text3 = "Clarence Couture";
				text2 = "75";
				_name = "clarence";
				dateADexEntry = null;
			}
			else if (flag4)
			{
				text3 = "Volt";
			}
			else if (flag5)
			{
				text3 = "Jon Wick";
				text2 = "63";
				_name = "jonwick";
				dateADexEntry = null;
			}
			else if (flag6)
			{
				dateADexEntry.CharName = "Timmy";
				text3 = "Timmy";
				text2 = "15";
				_name = "timmy";
				dateADexEntry = null;
			}
			else if (flag7)
			{
				text3 = "Skips";
				text2 = "96";
				_name = "skips";
				dateADexEntry = null;
			}
			else if (text != "")
			{
				text3 = "Dorian";
				text2 = "7";
				_name = text;
				dateADexEntry = null;
			}
		}
		this.UpdateDateStatusInkVariables();
		if (!skipEndingScreen)
		{
			this.OpenEndingScreen(dateADexEntry, status, _name, text2, text3, text4);
		}
		if (status == RelationshipStatus.Realized)
		{
			if (_name.Contains("_"))
			{
				foreach (string text6 in _name.Trim().Split('_', StringSplitOptions.None))
				{
					if (!skipStatusChange)
					{
						Singleton<Save>.Instance.SetDateStatus(text6, status);
					}
				}
			}
			else if (!skipStatusChange)
			{
				Singleton<Save>.Instance.SetDateStatus(_name, status);
			}
		}
		if (_name.Contains("_"))
		{
			foreach (string text7 in _name.Trim().Split('_', StringSplitOptions.None))
			{
				if (status != RelationshipStatus.Unmet && status != RelationshipStatus.Single && status != RelationshipStatus.Hate)
				{
					List<GameObject> gameObjectsByInteractableObj = this.GetGameObjectsByInteractableObj(text7);
					if (gameObjectsByInteractableObj != null)
					{
						foreach (GameObject gameObject in gameObjectsByInteractableObj)
						{
							if (status == RelationshipStatus.Love)
							{
								AssetCleaner component = gameObject.GetComponent<AssetCleaner>();
								if (component != null)
								{
									component.MakeClean();
								}
							}
							if (status == RelationshipStatus.Realized)
							{
								InteractableObj component2 = gameObject.GetComponent<InteractableObj>();
								if (component2 != null)
								{
									component2.TurnOnRealizedEffect();
								}
							}
						}
					}
				}
			}
			return;
		}
		if (status != RelationshipStatus.Unmet && status != RelationshipStatus.Single && status != RelationshipStatus.Hate)
		{
			List<GameObject> gameObjectsByInteractableObj2 = this.GetGameObjectsByInteractableObj(_name);
			if (gameObjectsByInteractableObj2 != null)
			{
				foreach (GameObject gameObject2 in gameObjectsByInteractableObj2)
				{
					if (status == RelationshipStatus.Love)
					{
						AssetCleaner component3 = gameObject2.GetComponent<AssetCleaner>();
						if (component3 != null)
						{
							component3.MakeClean();
						}
					}
					if (status == RelationshipStatus.Realized)
					{
						InteractableObj component4 = gameObject2.GetComponent<InteractableObj>();
						if (component4 != null)
						{
							component4.TurnOnRealizedEffect();
						}
					}
				}
			}
		}
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00030F18 File Offset: 0x0002F118
	public void UpdateDateStatusInkVariables()
	{
		Singleton<InkController>.Instance.story.variablesState["awakened"] = Singleton<Save>.Instance.AvailableTotalMetDatables();
		Singleton<InkController>.Instance.story.variablesState["friends"] = Singleton<Save>.Instance.AvailableTotalFriendEndings();
		Singleton<InkController>.Instance.story.variablesState["loves"] = Singleton<Save>.Instance.AvailableTotalLoveEndings();
		Singleton<InkController>.Instance.story.variablesState["hates"] = Singleton<Save>.Instance.AvailableTotalHateEndings();
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00030FC5 File Offset: 0x0002F1C5
	public void UpdateRealizedInkVariable()
	{
		Singleton<InkController>.Instance.story.variablesState["realized"] = Singleton<Save>.Instance.AvailableTotalRealizedDatables();
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00030FF0 File Offset: 0x0002F1F0
	public void UnlockAwakening(string _name, RelationshipStatus status)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		string text = "";
		if (_name == "luna")
		{
			flag = true;
			_name = "connie";
		}
		else if (_name == "dirk")
		{
			flag2 = true;
			_name = "dirk";
		}
		else if (_name == "clarence")
		{
			flag3 = true;
			_name = "dirk";
		}
		else if (_name == "volt")
		{
			flag4 = true;
			_name = "eddie";
		}
		else if (_name == "eddie")
		{
			flag5 = true;
		}
		else if (_name == "lucinda")
		{
			flag6 = true;
		}
		else if (_name == "front" || _name == "tiny" || _name == "trap" || _name == "back")
		{
			text = _name;
			_name = "dorian";
		}
		if (Singleton<Save>.Instance.GetDateStatus(_name) == RelationshipStatus.Unmet)
		{
			Singleton<Save>.Instance.SetDateStatus(_name, status);
		}
		this.ActivateIfNeeded();
		DateADexEntry dateADexEntry = DateADex.Instance.GetEntry(_name);
		string text2 = dateADexEntry.CharName;
		if (flag)
		{
			text2 = "Luna";
		}
		else if (!flag && dateADexEntry.CharName == "Luna")
		{
			dateADexEntry.CharName = "Connie Soul";
		}
		else if (flag2)
		{
			text2 = "Dirk";
		}
		else if (flag3)
		{
			text2 = "Clarence";
			_name = "clarence";
		}
		else if (flag4)
		{
			text2 = "Volt";
			_name = "volt";
		}
		else if (flag5)
		{
			text2 = "Eddie";
		}
		else if (flag6)
		{
			text2 = "Lucinda Lavish";
		}
		E_General_Poses e_General_Poses = E_General_Poses.neutral;
		E_Facial_Expressions e_Facial_Expressions = E_Facial_Expressions.neutral;
		if (_name == "diana")
		{
			e_General_Poses = E_General_Poses.neutral;
			e_Facial_Expressions = E_Facial_Expressions.shock;
		}
		else if (_name == "mateo")
		{
			e_General_Poses = E_General_Poses.hate;
			e_Facial_Expressions = E_Facial_Expressions.think;
		}
		else if (_name == "penelope")
		{
			e_General_Poses = E_General_Poses.love;
			e_Facial_Expressions = E_Facial_Expressions.joy;
		}
		else if (_name == "tina")
		{
			e_General_Poses = E_General_Poses.friend;
			e_Facial_Expressions = E_Facial_Expressions.think;
		}
		else if (_name == "tydus")
		{
			e_General_Poses = E_General_Poses.hate;
			e_Facial_Expressions = E_Facial_Expressions.angry;
		}
		else if (_name == "wallace")
		{
			e_General_Poses = E_General_Poses.hate;
			e_Facial_Expressions = E_Facial_Expressions.think;
		}
		else if (text == "front")
		{
			e_General_Poses = E_General_Poses.front;
			e_Facial_Expressions = E_Facial_Expressions.think;
		}
		else if (text == "back")
		{
			e_General_Poses = E_General_Poses.back;
			e_Facial_Expressions = E_Facial_Expressions.think;
		}
		else if (text == "tiny")
		{
			e_General_Poses = E_General_Poses.tiny;
			e_Facial_Expressions = E_Facial_Expressions.think;
		}
		else if (text == "trap")
		{
			e_General_Poses = E_General_Poses.trap;
			e_Facial_Expressions = E_Facial_Expressions.think;
		}
		else if (flag)
		{
			e_General_Poses = E_General_Poses.gun;
			e_Facial_Expressions = E_Facial_Expressions.shout;
		}
		else if (_name == "connie")
		{
			e_General_Poses = E_General_Poses.folded;
			e_Facial_Expressions = E_Facial_Expressions.smirk;
		}
		this.OpenAwokenScreen(dateADexEntry, _name, text2, flag || flag2 || flag3 || flag4 || flag5 || flag6, e_General_Poses, e_Facial_Expressions);
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x000312BB File Offset: 0x0002F4BB
	public void OpenAwokenScreen(DateADexEntry dateADexEntry, string internalName, string charName, bool forceCustomCharName, E_General_Poses PoseToUse, E_Facial_Expressions ExpressionToUse)
	{
		this.awokenSplashScreen.SetActive(false);
		this.awokenSplashScreen.SetActive(true);
		this.awokenSplashScreen.GetComponent<AwakenSplashScreen>().Initialize(dateADexEntry, internalName, charName, forceCustomCharName, PoseToUse, ExpressionToUse);
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x000312EE File Offset: 0x0002F4EE
	public void OpenEndingScreen(DateADexEntry dateADexEntry, RelationshipStatus status, string internalName, string charNumber, string charName, string charObj)
	{
		this.resultSplashScreen.SetActive(false);
		this.resultSplashScreen.SetActive(true);
		this.resultSplashScreen.GetComponent<ResultSplashScreen>().Initialize(dateADexEntry, status, internalName, charNumber, charName, charObj);
		CursorLocker.Unlock();
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00031326 File Offset: 0x0002F526
	public void OpenLocket(string internalName)
	{
		this.resultLocketScreen.SetActive(false);
		this.resultLocketScreen.SetActive(true);
		this.resultLocketScreen.GetComponent<Locket>().Initialize(internalName);
		this.startedEnding = true;
		Singleton<AchievementController>.Instance.RequestAchievement(AchievementType.LEAVE_HOUSE);
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x00031364 File Offset: 0x0002F564
	public void OpenLocketWithoutAnyEntries()
	{
		this.resultLocketScreen.SetActive(false);
		this.resultLocketScreen.SetActive(true);
		this.resultLocketScreen.GetComponent<Locket>().Initialize("skylar");
		this.startedEnding = true;
		Singleton<AchievementController>.Instance.RequestAchievement(AchievementType.LEAVE_HOUSE);
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x000313B4 File Offset: 0x0002F5B4
	public bool RecipeCheck(string _name)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		DateADexEntry dateADexEntry = DateADex.Instance.GetEntry(_name);
		if (_name == "lucinda")
		{
			if (!(bool)Singleton<InkController>.Instance.story.variablesState["limitless_points"])
			{
				num = 1;
				num2 = 1;
				num3 = 1;
				num4 = 1;
				num5 = 1;
			}
			else
			{
				num = 100;
				num2 = 100;
				num3 = 100;
				num4 = 100;
				num5 = 100;
			}
		}
		else if (dateADexEntry != null)
		{
			num = dateADexEntry.GetRecipe(SpecsAttributes.Smarts);
			num2 = dateADexEntry.GetRecipe(SpecsAttributes.Poise);
			num3 = dateADexEntry.GetRecipe(SpecsAttributes.Empathy);
			num4 = dateADexEntry.GetRecipe(SpecsAttributes.Charm);
			num5 = dateADexEntry.GetRecipe(SpecsAttributes.Sass);
		}
		else if (_name.Contains("_"))
		{
			foreach (string text in _name.Split('_', StringSplitOptions.None))
			{
				dateADexEntry = DateADex.Instance.GetEntry(_name);
				if (dateADexEntry != null)
				{
					if (num < dateADexEntry.GetRecipe(SpecsAttributes.Smarts))
					{
						num = dateADexEntry.GetRecipe(SpecsAttributes.Smarts);
					}
					if (num2 < dateADexEntry.GetRecipe(SpecsAttributes.Poise))
					{
						num2 = dateADexEntry.GetRecipe(SpecsAttributes.Poise);
					}
					if (num3 < dateADexEntry.GetRecipe(SpecsAttributes.Empathy))
					{
						num3 = dateADexEntry.GetRecipe(SpecsAttributes.Empathy);
					}
					if (num4 < dateADexEntry.GetRecipe(SpecsAttributes.Charm))
					{
						num4 = dateADexEntry.GetRecipe(SpecsAttributes.Charm);
					}
					if (num5 < dateADexEntry.GetRecipe(SpecsAttributes.Sass))
					{
						num5 = dateADexEntry.GetRecipe(SpecsAttributes.Sass);
					}
				}
			}
		}
		if (this.recipeAnim == null)
		{
			this.recipeAnim = GameObject.FindGameObjectWithTag("RecipeAnim").GetComponent<RecipeAnim>();
		}
		this.recipeAnim.startanim(num, num2, num3, num4, num5);
		return Singleton<SpecStatMain>.Instance.GetStatLevel("smarts", true) >= num && Singleton<SpecStatMain>.Instance.GetStatLevel("poise", true) >= num2 && Singleton<SpecStatMain>.Instance.GetStatLevel("empathy", true) >= num3 && Singleton<SpecStatMain>.Instance.GetStatLevel("charm", true) >= num4 && Singleton<SpecStatMain>.Instance.GetStatLevel("sass", true) >= num5;
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x000315B4 File Offset: 0x0002F7B4
	public bool RecipeCheckCustom(string _couple)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		if (!(_couple == "abel_dasha"))
		{
			if (!(_couple == "artt_doug"))
			{
				if (!(_couple == "tony_tina"))
				{
					if (!(_couple == "washford_drysdale"))
					{
						if (_couple == "harper_dirk")
						{
							num = 10;
							num2 = 10;
							num3 = 10;
							num4 = 10;
							num5 = 80;
						}
					}
					else
					{
						num = 80;
						num2 = 20;
						num3 = 20;
						num4 = 20;
						num5 = 80;
					}
				}
				else
				{
					num = 80;
					num2 = 60;
					num3 = 40;
					num4 = 60;
					num5 = 80;
				}
			}
			else
			{
				num = 80;
				num2 = 100;
				num3 = 80;
				num4 = 100;
				num5 = 10;
			}
		}
		else
		{
			num = 20;
			num2 = 50;
			num3 = 50;
			num4 = 50;
			num5 = 20;
		}
		if (this.recipeAnim == null)
		{
			this.recipeAnim = GameObject.FindGameObjectWithTag("RecipeAnim").GetComponent<RecipeAnim>();
		}
		this.recipeAnim.startanim(num, num2, num3, num4, num5);
		return Singleton<SpecStatMain>.Instance.GetStatLevel("smarts", true) >= num && Singleton<SpecStatMain>.Instance.GetStatLevel("poise", true) >= num2 && Singleton<SpecStatMain>.Instance.GetStatLevel("empathy", true) >= num3 && Singleton<SpecStatMain>.Instance.GetStatLevel("charm", true) >= num4 && Singleton<SpecStatMain>.Instance.GetStatLevel("sass", true) >= num5;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00031708 File Offset: 0x0002F908
	public List<GameObject> GetGameObjectsByInteractableObj(string name)
	{
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		GameObject gameObject = null;
		List<GameObject> list = new List<GameObject>();
		foreach (GameObject gameObject2 in rootGameObjects)
		{
			if (gameObject2.name == "===SCENE===")
			{
				gameObject = gameObject2;
				break;
			}
		}
		foreach (InteractableObj interactableObj in gameObject.GetComponentsInChildren<InteractableObj>(false))
		{
			if (interactableObj.inkFileName == name || interactableObj.InternalName() == name)
			{
				list.Add(interactableObj.gameObject);
			}
		}
		return list;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x000317AC File Offset: 0x0002F9AC
	public GameObject GetGameObjectByInteractableObj(string name)
	{
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		GameObject gameObject = null;
		foreach (GameObject gameObject2 in rootGameObjects)
		{
			if (gameObject2.name == "===SCENE===")
			{
				gameObject = gameObject2;
				break;
			}
		}
		foreach (InteractableObj interactableObj in gameObject.GetComponentsInChildren<InteractableObj>(false))
		{
			if (interactableObj.inkFileName == name || interactableObj.InternalName() == name)
			{
				return interactableObj.gameObject;
			}
		}
		return null;
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00031839 File Offset: 0x0002FA39
	private void ActivateIfNeeded()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.dexScreen.SetActive(false);
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00031860 File Offset: 0x0002FA60
	public void UnlockCollectableRelease()
	{
		if (this._collectableShowing)
		{
			this._collectableShowing = false;
			this.ActivateIfNeeded();
			this._currentReleaseCollectableCoroutine = base.StartCoroutine(this.Co_UnlockCollectableRelease());
			return;
		}
		Singleton<CollectableAnim>.Instance.ShouldHideOnRelease();
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00031894 File Offset: 0x0002FA94
	public void UnlockCollectable(string _name, int index, bool hideCollectablePopup = false, bool hideDateADexCollectable = false, bool repositionCharacters = true)
	{
		string internalName = this.GetInternalName(_name);
		if (!string.IsNullOrEmpty(internalName) && !string.IsNullOrWhiteSpace(internalName))
		{
			if (!hideDateADexCollectable)
			{
				Singleton<Save>.Instance.UnlockCollectable(internalName, index);
			}
			if (!hideCollectablePopup)
			{
				Sprite spriteFromCollectableNum = Singleton<CharacterHelper>.Instance._characters[internalName].GetSpriteFromCollectableNum(internalName, index);
				this.ActivateIfNeeded();
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_collectable_appear, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				if (repositionCharacters)
				{
					base.StartCoroutine(this.UnlockCollectable(spriteFromCollectableNum));
					return;
				}
				base.StartCoroutine(this.UnlockCollectableNotMovingCharacters(spriteFromCollectableNum));
			}
		}
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00031934 File Offset: 0x0002FB34
	public void ShowLargeCollectable(string name)
	{
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Postcards", name), false);
		this.ActivateIfNeeded();
		this._largeCollectableScreen.SetActive(true);
		Image component = this._largeCollectableScreen.GetComponent<Image>();
		component.sprite = sprite;
		if (this.loadedLargeCollectable != null)
		{
			Services.AssetProviderService.UnloadResourceAsset(this.loadedLargeCollectable);
			this.loadedLargeCollectable = null;
		}
		this.loadedLargeCollectable = sprite;
		Color color = new Color(component.color.r, component.color.b, component.color.b, 0f);
		Color color2 = new Color(component.color.r, component.color.b, component.color.b, 1f);
		component.color = color;
		DOTween.Sequence().Append(component.DOBlendableColor(color2, 5f)).SetRelative<Sequence>()
			.SetEase(Ease.Linear);
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00031A34 File Offset: 0x0002FC34
	public void HideLargeCollectable()
	{
		this.ActivateIfNeeded();
		this._largeCollectableScreen.SetActive(true);
		Image component = this._largeCollectableScreen.GetComponent<Image>();
		Color color = new Color(component.color.r, component.color.b, component.color.b, 0f);
		Color color2 = new Color(component.color.r, component.color.b, component.color.b, 1f);
		component.color = color;
		DOTween.Sequence().Append(component.DOBlendableColor(color2, 1f)).SetRelative<Sequence>()
			.SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				this._largeCollectableScreen.SetActive(false);
			});
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00031AF4 File Offset: 0x0002FCF4
	public void UnlockCollectableTutorial(string name, int index)
	{
		string text = string.Concat(new string[]
		{
			"Images/Collectables/",
			name,
			"/",
			name,
			"_",
			index.ToString()
		});
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(text, false);
		this.ActivateIfNeeded();
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_collectable_appear, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		base.StartCoroutine(this.UnlockCollectable(sprite));
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00031B7E File Offset: 0x0002FD7E
	private IEnumerator Co_UnlockCollectableHold(Sprite s)
	{
		while (this.IsCollectableShowing)
		{
			yield return null;
		}
		if (this._currentReleaseCollectableCoroutine != null)
		{
			base.StopCoroutine(this._currentReleaseCollectableCoroutine);
			this.CompleteUnlockCollectableRelease();
		}
		float stagePositionForNewCharacter = TalkingUI.Instance.StageManager.GetStagePositionForNewCharacter(out this.lastCollectableStagePosition);
		this._collectableShowing = true;
		Singleton<CollectableAnim>.Instance.StartHold(s, stagePositionForNewCharacter);
		yield return new WaitForEndOfFrame();
		yield break;
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00031B94 File Offset: 0x0002FD94
	private IEnumerator Co_UnlockCollectableRelease()
	{
		TalkingUI.Instance.StageManager.Log("Start Collectable Release");
		Singleton<CollectableAnim>.Instance.StartRelease();
		yield return new WaitForSeconds(1f);
		this.CompleteUnlockCollectableRelease();
		yield break;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00031BA3 File Offset: 0x0002FDA3
	private void CompleteUnlockCollectableRelease()
	{
		this._collectableShowing = false;
		this._currentReleaseCollectableCoroutine = null;
		TalkingUI.Instance.StageManager.RepositionEnteredCharactersAfterCollectableReleased();
		TalkingUI.Instance.StageManager.Log("Complete Collectable Release");
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00031BD6 File Offset: 0x0002FDD6
	private IEnumerator UnlockCollectable(Sprite s, float times, bool repositionCharacters = true)
	{
		while (this.IsCollectableShowing)
		{
			yield return null;
		}
		if (this._currentReleaseCollectableCoroutine != null)
		{
			base.StopCoroutine(this._currentReleaseCollectableCoroutine);
			this.CompleteUnlockCollectableRelease();
		}
		float stagePositionForNewCharacter = TalkingUI.Instance.StageManager.GetStagePositionForNewCharacter(out this.lastCollectableStagePosition);
		this._collectableShowing = true;
		Singleton<CollectableAnim>.Instance.startanim(s, stagePositionForNewCharacter);
		yield return new WaitForSeconds(times);
		yield return new WaitForSeconds(0.6f);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_collectable_leave, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		this._collectableShowing = false;
		if (repositionCharacters)
		{
			TalkingUI.Instance.StageManager.RepositionEnteredCharactersAfterCollectableReleased();
		}
		Singleton<CollectableAnim>.Instance.stopanim();
		yield break;
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00031BFA File Offset: 0x0002FDFA
	private IEnumerator UnlockCollectable(Sprite s)
	{
		return this.UnlockCollectable(s, 4.4f, true);
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00031C09 File Offset: 0x0002FE09
	private IEnumerator UnlockCollectableNotMovingCharacters(Sprite s)
	{
		return this.UnlockCollectable(s, 4.4f, false);
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00031C18 File Offset: 0x0002FE18
	public void UnlockDexEntry(string _name, int index, bool isLoad)
	{
		string _inName = this.GetInternalName(_name);
		if (!string.IsNullOrEmpty(_inName) && !string.IsNullOrWhiteSpace(_inName))
		{
			if (!isLoad)
			{
				Singleton<Save>.Instance.UnlockDexEntry(_inName, index);
			}
			DateADexEntry dateADexEntry = this.GetEntry(_inName);
			dateADexEntry.notification = Singleton<Save>.Instance.GetCharactersWithNewStatusOnDateADex(_inName);
			this.EntriesCompleted.Find((DateADexEntry Ent) => Ent.internalName == _inName).notification = Singleton<Save>.Instance.GetCharactersWithNewStatusOnDateADex(_inName);
			if (index >= dateADexEntry.DexComments.Count || index <= 0)
			{
				return;
			}
			string text = dateADexEntry.DexComments[index - 1];
			dateADexEntry.CharDYK = "\uff3f " + text.Replace("uFF3F ", "") + "<br>" + dateADexEntry.CharDYK;
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00031D10 File Offset: 0x0002FF10
	public DateADexEntry GetEntry(string name)
	{
		foreach (DateADexEntry dateADexEntry in this.EntriesOrdered)
		{
			if (dateADexEntry != null && (dateADexEntry.CharName == name || dateADexEntry.internalName == name))
			{
				return dateADexEntry;
			}
		}
		return null;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00031D58 File Offset: 0x0002FF58
	public DateADexEntry GetEntry(int index)
	{
		foreach (DateADexEntry dateADexEntry in this.EntriesOrdered)
		{
			if (dateADexEntry != null && dateADexEntry.CharIndex == index)
			{
				return dateADexEntry;
			}
		}
		return null;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00031D90 File Offset: 0x0002FF90
	public static RelationshipStatus ParseRelationshipStatus(string relationshipName)
	{
		string text = relationshipName.ToUpperInvariant();
		if (text == "LOVE")
		{
			return RelationshipStatus.Love;
		}
		if (text == "HATE")
		{
			return RelationshipStatus.Hate;
		}
		if (!(text == "FRIEND") && !(text == "SHIPPED"))
		{
			return RelationshipStatus.Single;
		}
		return RelationshipStatus.Friend;
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00031DE4 File Offset: 0x0002FFE4
	public void OnEntrySelected(Selectable entry)
	{
		this.lastSelectedEntry = entry;
		Navigation navigation = this.backButtonComponent.navigation;
		navigation.selectOnRight = entry;
		this.backButtonComponent.navigation = navigation;
		Navigation navigation2 = this.SortButton.navigation;
		navigation2.selectOnRight = entry;
		this.SortButton.navigation = navigation2;
		this._scrollingList.SelectContentID(entry.GetComponent<DexEntryButton>().ContentID, true);
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00031E50 File Offset: 0x00030050
	public string RelationshipStatusToString(int relationshipStatus)
	{
		switch (relationshipStatus)
		{
		case -1:
			return "Hate";
		case 0:
			return "Unmet";
		case 1:
			return "Single";
		case 2:
			return "Love";
		case 3:
			return "Friend";
		case 4:
			return "Realized";
		default:
			return "-";
		}
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00031EA8 File Offset: 0x000300A8
	public string CheckForAlias(string _name)
	{
		if (_name == "clarence")
		{
			return "dirk";
		}
		return _name;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00031EC0 File Offset: 0x000300C0
	private void UpdateVOActorPosition(int characterIndex)
	{
		if (characterIndex >= 0 && characterIndex < this.VOActorOffest.Length && this.VoActorRectTransform != null)
		{
			Vector2 anchorMin = this.VoActorRectTransform.anchorMin;
			Vector2 anchorMax = this.VoActorRectTransform.anchorMax;
			anchorMin.x = this.initialVOActorOffest + this.VOActorOffest[characterIndex];
			anchorMax.x = anchorMin.x + 0.24f;
			this.VoActorRectTransform.anchorMin = anchorMin;
			this.VoActorRectTransform.anchorMax = anchorMax;
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00031F43 File Offset: 0x00030143
	private bool IsRecipeOpen()
	{
		return this.RecipeScreen != null && this.RecipeScreen.activeInHierarchy;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00031F60 File Offset: 0x00030160
	public void MoveCollectableToPosition(string PositionName, float XPosition)
	{
		this.lastCollectableStagePosition = PositionName;
		Singleton<CollectableAnim>.Instance.MoveAnchoredXPosition(XPosition);
	}

	// Token: 0x04000744 RID: 1860
	private bool setup;

	// Token: 0x04000745 RID: 1861
	public bool startedEnding;

	// Token: 0x04000746 RID: 1862
	public static DateADex Instance;

	// Token: 0x04000747 RID: 1863
	public GameObject entry;

	// Token: 0x04000748 RID: 1864
	public GameObject dexScreen;

	// Token: 0x04000749 RID: 1865
	public GameObject phoneManager;

	// Token: 0x0400074A RID: 1866
	public GameObject resultSplashScreen;

	// Token: 0x0400074B RID: 1867
	public GameObject awokenSplashScreen;

	// Token: 0x0400074C RID: 1868
	public GameObject resultLocketScreen;

	// Token: 0x0400074D RID: 1869
	public RecipeAnim recipeAnim;

	// Token: 0x0400074E RID: 1870
	[Header("VisualItems")]
	[SerializeField]
	private DexEntryButton title;

	// Token: 0x0400074F RID: 1871
	public Animator DateADexAnim;

	// Token: 0x04000750 RID: 1872
	public GameObject DateADexWindow;

	// Token: 0x04000751 RID: 1873
	public TextMeshProUGUI Item;

	// Token: 0x04000752 RID: 1874
	public TextMeshProUGUI Desc;

	// Token: 0x04000753 RID: 1875
	public ScrollRect DescScroll;

	// Token: 0x04000754 RID: 1876
	public TextMeshProUGUI VoActor;

	// Token: 0x04000755 RID: 1877
	public RectTransform VoActorRectTransform;

	// Token: 0x04000756 RID: 1878
	public float[] VOActorOffest = new float[105];

	// Token: 0x04000757 RID: 1879
	private float initialVOActorOffest;

	// Token: 0x04000758 RID: 1880
	private const float voActorWidth = 0.24f;

	// Token: 0x04000759 RID: 1881
	public TextMeshProUGUI Likes;

	// Token: 0x0400075A RID: 1882
	public TextMeshProUGUI Dislikes;

	// Token: 0x0400075B RID: 1883
	public TextMeshProUGUI Pronouns;

	// Token: 0x0400075C RID: 1884
	public Image Sprite;

	// Token: 0x0400075D RID: 1885
	public Image Status;

	// Token: 0x0400075E RID: 1886
	public GameObject StatusBackground;

	// Token: 0x0400075F RID: 1887
	public Image SpecsIcon1;

	// Token: 0x04000760 RID: 1888
	public Image SpecsIcon2;

	// Token: 0x04000761 RID: 1889
	public Image DateableIcon1;

	// Token: 0x04000762 RID: 1890
	public Image DateableIcon2;

	// Token: 0x04000763 RID: 1891
	public Image DateableIcon3;

	// Token: 0x04000764 RID: 1892
	public Image RealizedIcon1;

	// Token: 0x04000765 RID: 1893
	public Image RealizedIcon2;

	// Token: 0x04000766 RID: 1894
	[SerializeField]
	private IsSelectableRegistered backButton;

	// Token: 0x04000767 RID: 1895
	[Header("Collectables")]
	[SerializeField]
	private CollectablesScreen _collectablesScreen;

	// Token: 0x04000768 RID: 1896
	[SerializeField]
	private GameObject _largeCollectableScreen;

	// Token: 0x04000769 RID: 1897
	private Sprite loadedLargeCollectable;

	// Token: 0x0400076A RID: 1898
	[SerializeField]
	private List<CollectableCharacterInternalName> CharacterInternalNameMap;

	// Token: 0x0400076B RID: 1899
	[SerializeField]
	private Button collectableButton;

	// Token: 0x0400076C RID: 1900
	[SerializeField]
	private TextMeshProUGUI collectableButtonText;

	// Token: 0x0400076D RID: 1901
	[SerializeField]
	private GameObject collectableButtonSparkles;

	// Token: 0x0400076E RID: 1902
	[Header("SPECS Resources")]
	public Sprite charmBadge;

	// Token: 0x0400076F RID: 1903
	public Sprite empathyBadge;

	// Token: 0x04000770 RID: 1904
	public Sprite poiseBadge;

	// Token: 0x04000771 RID: 1905
	public Sprite sassBadge;

	// Token: 0x04000772 RID: 1906
	public Sprite smartsBadge;

	// Token: 0x04000773 RID: 1907
	public Sprite mikeyBadge;

	// Token: 0x04000774 RID: 1908
	public Sprite lucindaBadge;

	// Token: 0x04000775 RID: 1909
	[Header("Resources")]
	public Sprite singlesprite;

	// Token: 0x04000776 RID: 1910
	public Sprite lovesprite;

	// Token: 0x04000777 RID: 1911
	public Sprite shippedsprite;

	// Token: 0x04000778 RID: 1912
	public Sprite hatesprite;

	// Token: 0x04000779 RID: 1913
	public Sprite errorsprite;

	// Token: 0x0400077A RID: 1914
	[Header("ButtonMenu")]
	[SerializeField]
	private DexListBank _dexListBank;

	// Token: 0x0400077B RID: 1915
	[SerializeField]
	public CircularScrollingList _scrollingList;

	// Token: 0x0400077C RID: 1916
	[SerializeField]
	private TextMeshProUGUI _sortButtonText;

	// Token: 0x0400077D RID: 1917
	public Image BackgroundImage;

	// Token: 0x0400077E RID: 1918
	public Image BackgroundImageShadow;

	// Token: 0x0400077F RID: 1919
	[Header("Recipe")]
	public GameObject MainEntryScreen;

	// Token: 0x04000780 RID: 1920
	public GameObject RecipeScreen;

	// Token: 0x04000781 RID: 1921
	public GameObject SmartsBar;

	// Token: 0x04000782 RID: 1922
	public GameObject SmartsBarCurrent;

	// Token: 0x04000783 RID: 1923
	public GameObject SmartsBarCap;

	// Token: 0x04000784 RID: 1924
	public GameObject SmartsPoints;

	// Token: 0x04000785 RID: 1925
	public GameObject PoiseBar;

	// Token: 0x04000786 RID: 1926
	public GameObject PoiseBarCurrent;

	// Token: 0x04000787 RID: 1927
	public GameObject PoiseBarCap;

	// Token: 0x04000788 RID: 1928
	public GameObject PoisePoints;

	// Token: 0x04000789 RID: 1929
	public GameObject EmpathyBar;

	// Token: 0x0400078A RID: 1930
	public GameObject EmpathyBarCurrent;

	// Token: 0x0400078B RID: 1931
	public GameObject EmpathyBarCap;

	// Token: 0x0400078C RID: 1932
	public GameObject EmpathyPoints;

	// Token: 0x0400078D RID: 1933
	public GameObject CharmBar;

	// Token: 0x0400078E RID: 1934
	public GameObject CharmBarCurrent;

	// Token: 0x0400078F RID: 1935
	public GameObject CharmBarCap;

	// Token: 0x04000790 RID: 1936
	public GameObject CharmPoints;

	// Token: 0x04000791 RID: 1937
	public GameObject SassBar;

	// Token: 0x04000792 RID: 1938
	public GameObject SassBarCurrent;

	// Token: 0x04000793 RID: 1939
	public GameObject SassBarCap;

	// Token: 0x04000794 RID: 1940
	public GameObject SassPoints;

	// Token: 0x04000795 RID: 1941
	[Header("Backend Stuff")]
	private List<DateADexEntry> EntriesCompleted;

	// Token: 0x04000796 RID: 1942
	private DateADexEntry[] EntriesOrdered = new DateADexEntry[102];

	// Token: 0x04000797 RID: 1943
	private int[] metstates;

	// Token: 0x04000798 RID: 1944
	private int currententry = -1;

	// Token: 0x04000799 RID: 1945
	private bool incollectables;

	// Token: 0x0400079A RID: 1946
	private int pagetoswitch;

	// Token: 0x0400079B RID: 1947
	private int pageAmount = 5;

	// Token: 0x0400079C RID: 1948
	private DateADex.Dex_Screen currentScreen;

	// Token: 0x0400079D RID: 1949
	public UnityEvent<string> RealizedCharacter = new UnityEvent<string>();

	// Token: 0x0400079E RID: 1950
	private DataADexCharImageProvider _backgroundCharImageProvider;

	// Token: 0x0400079F RID: 1951
	private DataADexCharImageProvider _spriteCharImageProvider;

	// Token: 0x040007A0 RID: 1952
	private int cancelCountdownSafeguard;

	// Token: 0x040007A1 RID: 1953
	[SerializeField]
	private ControllerGlyphComponent confirmIcon;

	// Token: 0x040007A2 RID: 1954
	[SerializeField]
	private ControllerGlyphComponent backIcon;

	// Token: 0x040007A3 RID: 1955
	[SerializeField]
	private ControllerGlyphComponent actionIcon;

	// Token: 0x040007A4 RID: 1956
	[SerializeField]
	private ControllerGlyphComponent navigateIcon;

	// Token: 0x040007A5 RID: 1957
	private bool backButtonLostFocus;

	// Token: 0x040007A6 RID: 1958
	private Coroutine currentSelectionDelay;

	// Token: 0x040007A7 RID: 1959
	private DateADexEntry currentEntry;

	// Token: 0x040007A8 RID: 1960
	public GameObject ListSummaryData;

	// Token: 0x040007A9 RID: 1961
	public GameObject ListSummaryDataRealizedItem;

	// Token: 0x040007AA RID: 1962
	public TextMeshProUGUI ListSummaryDataRealized;

	// Token: 0x040007AB RID: 1963
	public TextMeshProUGUI ListSummaryDataMet;

	// Token: 0x040007AC RID: 1964
	public TextMeshProUGUI ListSummaryDataLoves;

	// Token: 0x040007AD RID: 1965
	public TextMeshProUGUI ListSummaryDataFriends;

	// Token: 0x040007AE RID: 1966
	public TextMeshProUGUI ListSummaryDataHates;

	// Token: 0x040007AF RID: 1967
	private Dictionary<string, int> _ids;

	// Token: 0x040007B0 RID: 1968
	private Dictionary<int, string> _names;

	// Token: 0x040007B1 RID: 1969
	[Header("Rewired")]
	private Player player;

	// Token: 0x040007B2 RID: 1970
	private bool _collectableShowing;

	// Token: 0x040007B3 RID: 1971
	public Button CollectableButton;

	// Token: 0x040007B4 RID: 1972
	public Button SortButton;

	// Token: 0x040007B5 RID: 1973
	public Button RecipeTab;

	// Token: 0x040007B6 RID: 1974
	private DexEntryButton[] entryButtons;

	// Token: 0x040007B7 RID: 1975
	private List<DexEntryButton> entryButtonList;

	// Token: 0x040007B8 RID: 1976
	private Selectable lastSelectedEntry;

	// Token: 0x040007B9 RID: 1977
	private Button backButtonComponent;

	// Token: 0x040007BA RID: 1978
	private string lastCollectableStagePosition;

	// Token: 0x040007BB RID: 1979
	private Coroutine _currentReleaseCollectableCoroutine;

	// Token: 0x040007BC RID: 1980
	private List<Sprite> loadedSprites = new List<Sprite>(4);

	// Token: 0x02000306 RID: 774
	private enum Dex_Screen
	{
		// Token: 0x04001209 RID: 4617
		List,
		// Token: 0x0400120A RID: 4618
		Entry,
		// Token: 0x0400120B RID: 4619
		Collectables,
		// Token: 0x0400120C RID: 4620
		Recipe
	}
}
