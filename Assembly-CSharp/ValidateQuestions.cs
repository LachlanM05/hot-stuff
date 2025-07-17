using System;
using System.Collections;
using System.Text;
using Assets.Date_Everything.Scripts.SaveVersionAutoFix;
using Date_Everything.Scripts.Ink;
using Rewired;
using T17.Services;
using Team17.Platform.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200014B RID: 331
public class ValidateQuestions : MonoBehaviour
{
	// Token: 0x06000C37 RID: 3127 RVA: 0x000461C3 File Offset: 0x000443C3
	public void IsVisible()
	{
		this.visible = true;
		base.Invoke("StartCoffeeVfx", 0.2f);
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x000461DC File Offset: 0x000443DC
	private void StartCoffeeVfx()
	{
		this.coffeeEffect.gameObject.SetActive(true);
		this.coffeeEffectStart.gameObject.SetActive(false);
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x00046200 File Offset: 0x00044400
	public void NotVisible()
	{
		this.visible = false;
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x00046209 File Offset: 0x00044409
	private void Awake()
	{
		if (ValidateQuestions.Instance == null)
		{
			ValidateQuestions.Instance = this;
		}
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x0004621E File Offset: 0x0004441E
	public void Start()
	{
		this.InputBlocker.SetActive(false);
		this.SetDefaultUserName();
		this.ResetProxyFields();
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x00046238 File Offset: 0x00044438
	public void OnEnable()
	{
		this.InputBlocker.SetActive(false);
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x00046246 File Offset: 0x00044446
	public void OnDisable()
	{
		this.PendingClearQuestionAnswers = false;
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x00046250 File Offset: 0x00044450
	public void PlayMusic()
	{
		Singleton<AudioManager>.Instance.PlayTrack("file_select", AUDIO_TYPE.MUSIC, true, false, 1f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
		this.ClearQuestionAnswers();
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x00046287 File Offset: 0x00044487
	public void StopMusic()
	{
		Singleton<AudioManager>.Instance.StopTrack("file_select", 0.5f);
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x0004629D File Offset: 0x0004449D
	public void StopAllMusic()
	{
		Singleton<AudioManager>.Instance.StopTrack("file_select", 0.1f);
		Singleton<AudioManager>.Instance.StopTrack("main_menu", 0.1f);
		Singleton<AudioManager>.Instance.StopTrack("Main Menu ambience", 0f);
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x000462DC File Offset: 0x000444DC
	public void StopAndMainMenuAgain()
	{
		this.coffeeEffect.gameObject.SetActive(false);
		this.coffeeEffectStart.gameObject.SetActive(true);
		Singleton<AudioManager>.Instance.StopTrack("file_select", 0.5f);
		Object.FindObjectOfType<UIUtilities>().PlayMusic("main_menu");
		this.ResetValidation();
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x00046334 File Offset: 0x00044534
	public bool CheckNameValidity()
	{
		if (string.IsNullOrEmpty(this.nameTextField.text))
		{
			return false;
		}
		string text = this.SanitiseInput(this.nameTextField.text);
		if (string.CompareOrdinal(text, this.nameTextField.text) != 0)
		{
			this.nameTextField.text = text;
		}
		return !string.IsNullOrEmpty(this.nameTextField.text);
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x0004639C File Offset: 0x0004459C
	public bool CheckTownValidity()
	{
		if (string.IsNullOrEmpty(this.townTextField.text))
		{
			return false;
		}
		string text = this.SanitiseInput(this.townTextField.text);
		if (string.CompareOrdinal(text, this.townTextField.text) != 0)
		{
			this.townTextField.text = text;
		}
		return !string.IsNullOrEmpty(this.townTextField.text);
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x00046401 File Offset: 0x00044601
	public void SetPronoums(int pronoums)
	{
		this.selectedPronoums = pronoums;
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x0004640A File Offset: 0x0004460A
	public bool CheckFavThingValidity()
	{
		return !string.IsNullOrEmpty(this.favThingTextField.text);
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0004641F File Offset: 0x0004461F
	public void UpdateFontColor(TMP_Text text)
	{
		text.color = this.fontColor;
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x00046430 File Offset: 0x00044630
	public void ClearQuestionAnswers()
	{
		if (!this.PendingClearQuestionAnswers)
		{
			this.PendingClearQuestionAnswers = true;
			return;
		}
		this.PendingClearQuestionAnswers = false;
		this.SetDefaultUserName();
		this.selectedPronoums = 3;
		this.defaultPronoun.SetIsOnWithoutNotify(true);
		if (this.defaultPronoun.group.IsActive())
		{
			this.defaultPronoun.group.NotifyToggleOn(this.defaultPronoun, false);
		}
		this.townTextField.text = "Coolsville";
		this.favThingTextField.text = "Papaya";
		this.ResetProxyFields();
		this.nextButton.enabled = true;
		this.InputBlocker.SetActive(false);
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x000464D4 File Offset: 0x000446D4
	private bool IsInputNextButtonDown()
	{
		if (Services.UIInputService.HasInputControllerChangedRecently())
		{
			return false;
		}
		if (!Services.InputService.IsLastActiveInputController())
		{
			return false;
		}
		Player player = ReInput.players.GetPlayer(0);
		return player != null && player.GetButtonDown(5);
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x00046509 File Offset: 0x00044709
	private void Update()
	{
		if (this.IsInputNextButtonDown())
		{
			this.SetQuestionAnswers();
		}
		if (this.PendingClearQuestionAnswers)
		{
			this.ClearQuestionAnswers();
		}
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00046528 File Offset: 0x00044728
	public void SetDefaultUserName()
	{
		IUser user;
		if (Services.UserService.TryGetPrimaryUser(out user))
		{
			this.nameTextField.text = this.SanitiseInput(user.DisplayName);
		}
		this.ValidateAnswers();
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00046560 File Offset: 0x00044760
	public void SetQuestionAnswers()
	{
		if (this.nameTextField.text != "")
		{
			Singleton<Save>.Instance.SetPlayerName(this.RemoveTags(this.nameTextField.text));
		}
		else
		{
			this.nameRedBox.SetActive(true);
		}
		if (this.selectedPronoums > 0)
		{
			Singleton<Save>.Instance.SetPlayerPronouns(this.selectedPronoums);
		}
		else
		{
			this.pronounRedBox.SetActive(true);
		}
		if (this.townTextField.text != "")
		{
			Singleton<Save>.Instance.SetPlayerTown(this.RemoveTags(this.townTextField.text));
		}
		else
		{
			this.townRedBox.SetActive(true);
		}
		if (this.mandatoryToggle.isOn)
		{
			this.mandatoryRedBox.GetComponent<Animator>().SetBool("MissingField", false);
		}
		else
		{
			this.mandatoryRedBox.SetActive(true);
			this.mandatoryRedBox.GetComponent<Animator>().SetBool("MissingField", true);
		}
		if (!this.mandatoryRedBox.activeInHierarchy && !this.nameRedBox.activeInHierarchy && !this.pronounRedBox.activeInHierarchy && !this.townRedBox.activeInHierarchy)
		{
			this.nextButton.interactable = false;
			this.nextButton.enabled = false;
			this.InputBlocker.SetActive(true);
			base.StartCoroutine(this.AnimateToNewGame());
		}
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x000466C0 File Offset: 0x000448C0
	public void ValidateAnswers()
	{
		if (!this.CheckNameValidity())
		{
			this.nextButton.interactable = false;
			return;
		}
		if (!this.CheckTownValidity())
		{
			this.nextButton.interactable = false;
			return;
		}
		if (!this.CheckFavThingValidity())
		{
			this.nextButton.interactable = false;
			return;
		}
		if (!this.nameTextField.isFocused)
		{
			this.nameRedBox.SetActive(false);
		}
		this.pronounRedBox.SetActive(false);
		if (!this.townTextField.isFocused)
		{
			this.townRedBox.SetActive(false);
		}
		this.mandatoryRedBox.GetComponent<Animator>().SetBool("MissingField", false);
		this.mandatoryRedBox.SetActive(false);
		this.nextButton.interactable = true;
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00046778 File Offset: 0x00044978
	private IEnumerator AnimateToNewGame()
	{
		yield return new WaitForEndOfFrame();
		Singleton<InkStoryProvider>.Instance.Story.ResetState();
		Singleton<Save>.Instance.SetInkVariables();
		if (Services.GameSettings.GetInt(Save.SettingKeySkipTutorial, 0) == 1)
		{
			Singleton<InkController>.Instance.story.variablesState["can_skip_tutorial"] = true;
		}
		this.approvedStamp.SetActive(true);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.approved_stamp, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		this.dustContainerObj.SetActive(true);
		this.dustInnerContainerObj.SetActive(true);
		int childCount = this.dustContainerObj.transform.childCount;
		int childCount2 = this.dustInnerContainerObj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			this.dustContainerObj.transform.GetChild(i).GetComponent<DoCodeAnimation>().TriggerAll();
		}
		for (int j = 0; j < childCount2; j++)
		{
			this.dustInnerContainerObj.transform.GetChild(j).GetComponent<DoCodeAnimation>().TriggerAll();
		}
		this.StopAllMusic();
		yield return new WaitForSeconds(0.2f);
		this.coffeeEffect.gameObject.SetActive(false);
		this.coffeeEffectStart.gameObject.SetActive(true);
		this.screenShake.Trigger();
		base.gameObject.transform.position = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y - 0.1f, base.gameObject.transform.position.z);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.start_game, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		yield return new WaitForSeconds(1f);
		this.uiUtilities.ShowLoadingScreen(true);
		yield return new WaitForSeconds(0.5f);
		this.mainMenuSubScreen.HideMainMenu();
		this.canvasUIManager.SwitchMenu(this.menuComponent);
		Singleton<ChatMaster>.Instance.ClearChatHistory();
		Singleton<Save>.Instance.CallGameLoad();
		SaveAutoFixManager.SetHotfixVariables();
		this.uiUtilities.FadeOutTrack("main_menu");
		this.uiUtilities.LoadSceneAsyncSingle(SceneConsts.kGameScene, false);
		this.visible = false;
		this.ResetValidation();
		yield break;
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x00046788 File Offset: 0x00044988
	private void ResetValidation()
	{
		base.StopCoroutine(this.AnimateToNewGame());
		this.approvedStamp.GetComponent<DoCodeAnimation>().ResetAnim(false);
		this.approvedStamp.SetActive(false);
		this.dustContainerObj.SetActive(false);
		this.dustInnerContainerObj.SetActive(false);
		this.mandatoryToggle.SetIsOnWithoutNotify(false);
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x000467E2 File Offset: 0x000449E2
	public void ReceivedOSKNameInputCallback(bool success, string text)
	{
		if (success && !string.IsNullOrEmpty(text))
		{
			this.nameTextField.text = this.SanitiseInput(text);
			this.nameTextField.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00046812 File Offset: 0x00044A12
	public void ReceivedOSKTownInputCallback(bool success, string text)
	{
		if (success && !string.IsNullOrEmpty(text))
		{
			this.townTextField.text = this.SanitiseInput(text);
			this.townTextField.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00046844 File Offset: 0x00044A44
	protected string RemoveTags(string text)
	{
		if (text.IndexOf("<") < 0)
		{
			return text;
		}
		while (text.IndexOf("<") >= 0)
		{
			int num = text.IndexOf("<");
			int num2 = text.IndexOf(">");
			text = text.Remove(num, num2 + 1 - num);
		}
		return text;
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00046898 File Offset: 0x00044A98
	protected string SanitiseInput(string inputText)
	{
		inputText = inputText.TrimStart();
		StringBuilder stringBuilder;
		if (inputText.Length > this.CharacterLimit)
		{
			stringBuilder = new StringBuilder(this.CharacterLimit);
		}
		else
		{
			stringBuilder = new StringBuilder(inputText.Length);
		}
		int i = 0;
		int length = inputText.Length;
		while (i < length)
		{
			if ((char.IsLetterOrDigit(inputText[i]) || char.IsWhiteSpace(inputText[i])) && this.nameTextField.fontAsset.HasCharacter(inputText[i], true, false))
			{
				stringBuilder.Append(inputText[i]);
				if (stringBuilder.Length == stringBuilder.Capacity)
				{
					break;
				}
			}
			i++;
		}
		if (stringBuilder.Length != inputText.Length)
		{
			return stringBuilder.ToString();
		}
		return inputText;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00046951 File Offset: 0x00044B51
	public void OnInputFieldChangeFocus(GameObject relatedRedBox)
	{
		relatedRedBox.SetActive(true);
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x0004695C File Offset: 0x00044B5C
	private void ResetProxyFields()
	{
		if (!Services.OnScreenKeyboard.IsSupported())
		{
			return;
		}
		T17TextInputProxy componentInParent = this.nameTextField.GetComponentInParent<T17TextInputProxy>();
		if (componentInParent)
		{
			componentInParent.SetText(this.nameTextField.text, true);
			this.nameTextField.gameObject.SetActive(true);
		}
		T17TextInputProxy componentInParent2 = this.townTextField.GetComponentInParent<T17TextInputProxy>();
		if (componentInParent2)
		{
			componentInParent2.SetText(this.townTextField.text, true);
			this.townTextField.gameObject.SetActive(true);
		}
	}

	// Token: 0x04000AF0 RID: 2800
	public static ValidateQuestions Instance;

	// Token: 0x04000AF1 RID: 2801
	public TMP_InputField nameTextField;

	// Token: 0x04000AF2 RID: 2802
	public TMP_InputField townTextField;

	// Token: 0x04000AF3 RID: 2803
	public TMP_InputField favThingTextField;

	// Token: 0x04000AF4 RID: 2804
	public Toggle mandatoryToggle;

	// Token: 0x04000AF5 RID: 2805
	public Toggle defaultPronoun;

	// Token: 0x04000AF6 RID: 2806
	public Button nextButton;

	// Token: 0x04000AF7 RID: 2807
	public GameObject InputBlocker;

	// Token: 0x04000AF8 RID: 2808
	private Color fontColor = new Color(0.1960784f, 0.1960784f, 0.1960784f);

	// Token: 0x04000AF9 RID: 2809
	private int selectedPronoums = 3;

	// Token: 0x04000AFA RID: 2810
	public GameObject nameRedBox;

	// Token: 0x04000AFB RID: 2811
	public GameObject pronounRedBox;

	// Token: 0x04000AFC RID: 2812
	public GameObject townRedBox;

	// Token: 0x04000AFD RID: 2813
	public GameObject mandatoryRedBox;

	// Token: 0x04000AFE RID: 2814
	public GameObject coffeeEffect;

	// Token: 0x04000AFF RID: 2815
	public GameObject coffeeEffectStart;

	// Token: 0x04000B00 RID: 2816
	public UIUtilities uiUtilities;

	// Token: 0x04000B01 RID: 2817
	public CanvasUIManager canvasUIManager;

	// Token: 0x04000B02 RID: 2818
	public MainMenuSubScreen mainMenuSubScreen;

	// Token: 0x04000B03 RID: 2819
	public MenuComponent menuComponent;

	// Token: 0x04000B04 RID: 2820
	[SerializeField]
	private GameObject approvedStamp;

	// Token: 0x04000B05 RID: 2821
	[SerializeField]
	private DoCodeAnimation screenShake;

	// Token: 0x04000B06 RID: 2822
	[SerializeField]
	private GameObject dustContainerObj;

	// Token: 0x04000B07 RID: 2823
	[SerializeField]
	private GameObject dustInnerContainerObj;

	// Token: 0x04000B08 RID: 2824
	[SerializeField]
	private int CharacterLimit;

	// Token: 0x04000B09 RID: 2825
	private bool PendingClearQuestionAnswers;

	// Token: 0x04000B0A RID: 2826
	public bool visible;
}
