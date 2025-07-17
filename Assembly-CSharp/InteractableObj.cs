using System;
using System.Collections.Generic;
using System.IO;
using Date_Everything.Scripts.Interactables;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Team17.Common;
using Team17.Scripts;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200006D RID: 109
[RequireComponent(typeof(UniqueId))]
public class InteractableObj : MonoBehaviour
{
	// Token: 0x06000391 RID: 913 RVA: 0x00016B66 File Offset: 0x00014D66
	public string KnotName()
	{
		return this.inkFileName.Replace(".ink", "");
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000392 RID: 914 RVA: 0x00016B7D File Offset: 0x00014D7D
	public string Id
	{
		get
		{
			return this.uniqId.uniqueId;
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00016B8C File Offset: 0x00014D8C
	public string GetInkFileNamePrefix()
	{
		if (this.inkFileName.Contains("."))
		{
			return this.inkFileName.Replace(".ink", "").Split('.', StringSplitOptions.None)[0];
		}
		return this.inkFileName.Replace(".ink", "");
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00016BE0 File Offset: 0x00014DE0
	public string BaseFileName()
	{
		return this.inkFileName.Replace(".ink", "");
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00016BF7 File Offset: 0x00014DF7
	public string InternalName()
	{
		if (!string.IsNullOrEmpty(this.internalCharacterName))
		{
			return this.internalCharacterName;
		}
		return this.inkFileName.Replace(".ink", "").Split("_", StringSplitOptions.None)[0];
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000396 RID: 918 RVA: 0x00016C2F File Offset: 0x00014E2F
	// (set) Token: 0x06000397 RID: 919 RVA: 0x00016C37 File Offset: 0x00014E37
	public string InteractionPrompt { get; private set; } = "Talk";

	// Token: 0x06000398 RID: 920 RVA: 0x00016C40 File Offset: 0x00014E40
	public void Init()
	{
		this.Initialize();
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00016C48 File Offset: 0x00014E48
	private void Awake()
	{
		if (this.mr == null)
		{
			this.mr = base.GetComponent<Renderer>();
		}
		this.internalCharacterName = "";
		this._inkFileNameAtStartup = this.inkFileName;
		this.Initialize();
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00016C84 File Offset: 0x00014E84
	public void DoMagicWiggle()
	{
		if (!Singleton<Dateviators>.Instance.Equipped || this.strength == 0f || this.randomness == 0f)
		{
			return;
		}
		base.transform.DOScale(this.startscaleInAnimation, this.resetDuration).OnComplete(delegate
		{
			base.transform.DOShakeScale(this.duration, this.strength, this.vibrato, this.randomness, this.fadeOut, this.shakeRandomnessMode).OnComplete(delegate
			{
				base.transform.DOScale(this.startscale, this.resetDuration);
			});
		});
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00016CE4 File Offset: 0x00014EE4
	private void Initialize()
	{
		this.EnsureUniqueIdIsAssigned();
		if (this.init)
		{
			return;
		}
		if (Singleton<InkController>.Instance == null)
		{
			return;
		}
		this.startpos = base.transform.position;
		this.startrot = base.transform.eulerAngles;
		this.startscale = base.transform.localScale;
		if (this.mr == null)
		{
			this.mr = base.GetComponent<Renderer>();
		}
		if (this.collider == null)
		{
			this.collider = base.GetComponent<Collider>();
		}
		this.baseFilePath = this.inkFileName.Replace(".ink", "");
		this.knotName = this.inkFileName.Replace(".ink", "");
		if (string.IsNullOrEmpty(this.mainText))
		{
			this.mainText = "Default hover text for " + this.inkFileName;
		}
		if (Singleton<Dateviators>.Instance != null)
		{
			Singleton<Dateviators>.Instance.EquippedDateviators.AddListener(new UnityAction(this.TurnOnRealizedEffect));
			Singleton<Dateviators>.Instance.UnequippedDateviators.AddListener(new UnityAction(this.TurnOffRealizedEffect));
		}
		base.gameObject.layer = 0;
		this.TurnOnRealizedEffect();
		this.CleanInteractable(false);
		if (Singleton<GameController>.Instance != null)
		{
			UnityEvent<List<string>> dialogueEndings = Singleton<GameController>.Instance.DialogueEndings;
			if (dialogueEndings != null)
			{
				dialogueEndings.AddListener(new UnityAction<List<string>>(this.CheckToClean));
			}
		}
		this.init = true;
		if (Save.GetSaveData(false) != null && !Save.GetSaveData(false).objectSaveDataDictionary.TryGetValue(this.Id, out this.objSaveData))
		{
			Save.GetSaveData(false).objectSaveDataDictionary.TryGetValue(base.gameObject.name, out this.objSaveData);
		}
		if (this.objSaveData == null)
		{
			this.objSaveData = new ObjectSaveData("");
		}
		if (this.AlternateInteractions != null && this.AlternateInteractions.Count == 0)
		{
			this.AlternateInteractions.Add(null);
		}
		if (this.AlternateInteractions == null)
		{
			this.AlternateInteractions = new List<Interactable>();
			this.AlternateInteractions.Add(null);
		}
		this._inkFileNameAtStartup = this.inkFileName;
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00016F0C File Offset: 0x0001510C
	private void ReadMainTags(string tag)
	{
		if (tag.StartsWith("main_text"))
		{
			string[] array = new string[] { ":" };
			string[] array2 = tag.Split(array, StringSplitOptions.None);
			if (array2.Length == 2)
			{
				this.mainText = array2[1];
				return;
			}
			T17Debug.LogError("Main Text tag not formatted properly: " + tag);
		}
	}

	// Token: 0x0600039D RID: 925 RVA: 0x00016F5E File Offset: 0x0001515E
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player") && this.bumpable && BetterPlayerControl.Instance._currentSpeed > 1f)
		{
			this.PlayBumpSound();
		}
	}

	// Token: 0x0600039E RID: 926 RVA: 0x00016F91 File Offset: 0x00015191
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && this.bumpable && BetterPlayerControl.Instance._currentSpeed > 1f)
		{
			this.PlayBumpSound();
		}
	}

	// Token: 0x0600039F RID: 927 RVA: 0x00016FC4 File Offset: 0x000151C4
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && this.bumpable)
		{
			base.Invoke("ResetBump", this.bumpDelay);
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00016FF1 File Offset: 0x000151F1
	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("Player") && this.bumpable)
		{
			base.Invoke("ResetBump", this.bumpDelay);
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001701E File Offset: 0x0001521E
	private void OnCollisionStay(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.CompareTag("Player") && this.bumpable)
		{
			this.bumped = true;
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00017044 File Offset: 0x00015244
	public void PlayBumpSound()
	{
		if (this.InternalName() == string.Empty)
		{
			return;
		}
		if (this.bumped || !Singleton<Dateviators>.Instance.Equipped || Singleton<Save>.Instance.GetDateStatus(this.InternalName()) == RelationshipStatus.Unmet || Singleton<Save>.Instance.GetDateStatusRealized(this.InternalName()) == RelationshipStatus.Realized || BetterPlayerControl.Instance.STATE != BetterPlayerControl.PlayerState.CanControl)
		{
			return;
		}
		this.bumped = true;
		if ((float)Random.Range(0, 100) > this.bumpChance)
		{
			return;
		}
		string text = Path.Combine("VoiceOver", "bumped", this.InternalName());
		if (this.bumpLineOverride == null)
		{
			Singleton<AudioManager>.Instance.PlayTrack(text, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, true);
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack(this.bumpLineOverride, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00017134 File Offset: 0x00015334
	private void ResetBump()
	{
		this.bumped = false;
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00017140 File Offset: 0x00015340
	public void TurnOnRealizedEffect()
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized(this.InternalName()) != RelationshipStatus.Realized)
		{
			return;
		}
		this.IsRealized = true;
		if (Singleton<Dateviators>.Instance != null && Singleton<Dateviators>.Instance.Equipped)
		{
			base.gameObject.layer = 15;
			VFXEvents.Instance.UpdateOutline(1f);
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001719D File Offset: 0x0001539D
	public void TurnOffRealizedEffect()
	{
		if (!this.IsRealized)
		{
			return;
		}
		base.gameObject.layer = 0;
		VFXEvents.Instance.UpdateOutline(0f);
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x000171C3 File Offset: 0x000153C3
	private void ForceRealisedEffectState()
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized(this.InternalName()) == RelationshipStatus.Realized)
		{
			VFXEvents.Instance.UpdateOutline(1f);
			return;
		}
		VFXEvents.Instance.UpdateOutline(0f);
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x000171F8 File Offset: 0x000153F8
	public void ShowHighlight()
	{
		if (this.isHighlighted || BetterPlayerControl.Instance.STATE != BetterPlayerControl.PlayerState.CanControl || !Singleton<Dateviators>.Instance.Equipped)
		{
			return;
		}
		this.ForceRealisedEffectState();
		if (Singleton<Save>.Instance.GetDateStatusRealized(this.InternalName()) == RelationshipStatus.Realized)
		{
			return;
		}
		base.gameObject.layer = 14;
		this.isHighlighted = true;
		this.isLightHighlighted = false;
		GenericInteractable genericInteractable;
		if (this.AlternateInteractions[0] != null && this.AlternateInteractions[0].TryGetComponent<GenericInteractable>(out genericInteractable))
		{
			foreach (InteractableObj interactableObj in genericInteractable.GetComponentsInChildren<InteractableObj>())
			{
				if (interactableObj.AlternateInteractions[0].GetComponent<GenericInteractable>() == genericInteractable && interactableObj.inkFileName == this.inkFileName)
				{
					interactableObj.ShowHighlight();
				}
			}
		}
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x000172D0 File Offset: 0x000154D0
	public void ShowLightHighlight()
	{
		if (this.isLightHighlighted || BetterPlayerControl.Instance.STATE != BetterPlayerControl.PlayerState.CanControl || !Singleton<Dateviators>.Instance.Equipped)
		{
			return;
		}
		this.ForceRealisedEffectState();
		if (Singleton<Save>.Instance.GetDateStatusRealized(this.InternalName()) == RelationshipStatus.Realized)
		{
			return;
		}
		base.gameObject.layer = 14;
		this.isHighlighted = false;
		this.isLightHighlighted = true;
		GenericInteractable genericInteractable;
		if (this.AlternateInteractions[0] != null && this.AlternateInteractions[0].TryGetComponent<GenericInteractable>(out genericInteractable))
		{
			foreach (InteractableObj interactableObj in genericInteractable.GetComponentsInChildren<InteractableObj>())
			{
				if (interactableObj.AlternateInteractions[0].GetComponent<GenericInteractable>() == genericInteractable && interactableObj.inkFileName == this.inkFileName)
				{
					interactableObj.ShowLightHighlight();
				}
			}
		}
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x000173A7 File Offset: 0x000155A7
	public void StartPulse()
	{
	}

	// Token: 0x060003AA RID: 938 RVA: 0x000173A9 File Offset: 0x000155A9
	public void StopPulse()
	{
	}

	// Token: 0x060003AB RID: 939 RVA: 0x000173AC File Offset: 0x000155AC
	public void HideHighlight()
	{
		if (!this.isHighlighted && !this.isLightHighlighted)
		{
			return;
		}
		base.gameObject.layer = 0;
		this.isWiggling = false;
		this.isHighlighted = false;
		this.isLightHighlighted = false;
		this.TurnOnRealizedEffect();
		GenericInteractable genericInteractable;
		if (this.AlternateInteractions[0] != null && this.AlternateInteractions[0].TryGetComponent<GenericInteractable>(out genericInteractable))
		{
			foreach (InteractableObj interactableObj in genericInteractable.GetComponentsInChildren<InteractableObj>())
			{
				if (interactableObj.AlternateInteractions[0].GetComponent<GenericInteractable>() == genericInteractable)
				{
					interactableObj.HideHighlight();
				}
			}
		}
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00017452 File Offset: 0x00015652
	public void StartDialogue()
	{
		if (this.DialogueEvent != null)
		{
			this.DialogueEvent.Invoke();
		}
		Singleton<Save>.Instance.MeetDatableIfUnmet(this.InternalName());
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00017478 File Offset: 0x00015678
	public void EnableWiggle()
	{
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0001747A File Offset: 0x0001567A
	public void DisableWiggle()
	{
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001747C File Offset: 0x0001567C
	public void CheckToClean(List<string> characters)
	{
		this.inkFileName = this._inkFileNameAtStartup;
		if (characters == null)
		{
			return;
		}
		if (!characters.Contains(this.InternalName()))
		{
			return;
		}
		this.CleanInteractable(true);
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x000174A4 File Offset: 0x000156A4
	public void CleanInteractable(bool playGleam = false)
	{
		switch (Singleton<Save>.Instance.GetDateStatus(this.InternalName()))
		{
		case RelationshipStatus.Love:
		{
			this.isClean = true;
			if (playGleam)
			{
				base.Invoke("PlayGleam", 1f);
			}
			UnityEvent cleanEvent = this.CleanEvent;
			if (cleanEvent != null)
			{
				cleanEvent.Invoke();
			}
			this.CleanAltInteractions();
			break;
		}
		case RelationshipStatus.Friend:
		{
			this.isClean = true;
			if (playGleam)
			{
				base.Invoke("PlayGleam", 1f);
			}
			UnityEvent cleanEvent2 = this.CleanEvent;
			if (cleanEvent2 != null)
			{
				cleanEvent2.Invoke();
			}
			this.CleanAltInteractions();
			break;
		}
		}
		this.init = true;
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0001754B File Offset: 0x0001574B
	public void PlayGleam()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		VFXEvents.Instance.SpawnVisualEffect(base.transform.position, VFXEvents.VisualEffectType.GLEAM);
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00017574 File Offset: 0x00015774
	private void CleanAltInteractions()
	{
		if (this.AlternateInteractions.Count > 0 && this.AlternateInteractions[0] != null)
		{
			GenericInteractable component = this.AlternateInteractions[0].GetComponent<GenericInteractable>();
			if (component != null)
			{
				component.clean = true;
			}
		}
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x000175C8 File Offset: 0x000157C8
	public void LoadSaveData()
	{
		if (string.IsNullOrEmpty(this.objSaveData.gameObjectName))
		{
			return;
		}
		this.LoadSaveData(this.objSaveData.gameObjectName, this.objSaveData.activeSelf, this.objSaveData.activatedAnimation, this.objSaveData.isClean, this.objSaveData.hasNormalInteracted, this.objSaveData.positionWhenInteracted);
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00017630 File Offset: 0x00015830
	public void LoadSaveData(string gameObjectName, bool activeSelf, bool activatedAnimation, bool isClean, bool hasNormalInteracted, Vector3 positionWhenInteracted)
	{
		this.objSaveData.UpdateSaveData(gameObjectName, activeSelf, activatedAnimation, isClean, hasNormalInteracted, positionWhenInteracted);
		if (base.gameObject.GetComponent<NeverSelfInactive>() == null)
		{
			base.gameObject.SetActive(activeSelf & this.allowActivationOnLoad);
		}
		else
		{
			base.gameObject.SetActive(true);
		}
		if (isClean)
		{
			this.init = false;
			this.CleanInteractable(false);
		}
		if (this.AlternateInteractions != null && this.AlternateInteractions.Count > 0 && this.AlternateInteractions[0] != null && this.AlternateInteractions[0].interactedWithState != hasNormalInteracted)
		{
			this.AlternateInteractions[0].ToggleInteractedWith(positionWhenInteracted, true);
			if (this.InteractOnLoad)
			{
				this.AlternateInteractions[0].loadingIn = true;
				this.AlternateInteractions[0].Interact();
				return;
			}
			this.AlternateInteractions[0].OnLoadNoInteract();
		}
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0001772C File Offset: 0x0001592C
	public void StoreSaveData()
	{
		if (this.objSaveData == null)
		{
			T17Debug.LogError("[InteractbleObject] interactable object " + base.name + " has a NULL SaveData object");
			return;
		}
		if (this.AlternateInteractions != null && this.AlternateInteractions.Count > 0 && this.AlternateInteractions[0] != null)
		{
			this.objSaveData.UpdateSaveData(base.gameObject.name, base.gameObject.activeSelf, false, this.isClean, this.AlternateInteractions[0].interactedWithState, this.AlternateInteractions[0].interactedPosition);
			return;
		}
		this.objSaveData.UpdateSaveData(base.gameObject.name, base.gameObject.activeSelf, false, this.isClean, false, new Vector3(0f, 0f, 0f));
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00017814 File Offset: 0x00015A14
	private void OnValidate()
	{
		if (this.mr == null)
		{
			this.mr = base.GetComponent<Renderer>();
		}
		if (this.collider == null)
		{
			this.collider = base.GetComponent<Collider>();
		}
		if (this.linkedWigglers.Count > 0)
		{
			this.linkedWigglers.ForEach(delegate(InteractableObj wiggler)
			{
				if (wiggler != null && !wiggler.linkedWigglers.Contains(this))
				{
					wiggler.linkedWigglers.Add(this);
				}
			});
		}
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x0001787A File Offset: 0x00015A7A
	public void EnsureUniqueIdIsAssigned()
	{
		if (!this.uniqId)
		{
			this.uniqId = base.GetComponent<UniqueId>();
			if (this.uniqId == null)
			{
				this.uniqId = base.gameObject.AddComponent<UniqueId>();
			}
		}
	}

	// Token: 0x04000387 RID: 903
	public float InteractionRadius = 7.5f;

	// Token: 0x04000388 RID: 904
	private bool init;

	// Token: 0x04000389 RID: 905
	private float currentPulseValue;

	// Token: 0x0400038A RID: 906
	public bool InteractOnLoad;

	// Token: 0x0400038B RID: 907
	[HideInInspector]
	public bool isWiggling;

	// Token: 0x0400038C RID: 908
	[HideInInspector]
	public bool isHighlighted;

	// Token: 0x0400038D RID: 909
	[HideInInspector]
	public bool isLightHighlighted;

	// Token: 0x0400038E RID: 910
	[HideInInspector]
	public bool isPulsing;

	// Token: 0x0400038F RID: 911
	[HideInInspector]
	public bool bumped;

	// Token: 0x04000390 RID: 912
	[SerializeField]
	private float bumpChance = 15f;

	// Token: 0x04000391 RID: 913
	public bool isClean;

	// Token: 0x04000392 RID: 914
	public bool IsRealized;

	// Token: 0x04000393 RID: 915
	[Header("Metadata")]
	[Tooltip("You don't need to include '.ink'!")]
	public string inkFileName = "";

	// Token: 0x04000394 RID: 916
	private string _inkFileNameAtStartup;

	// Token: 0x04000395 RID: 917
	[Tooltip("If this is empty, try and put the internal name of the character here!")]
	public string internalCharacterName = "";

	// Token: 0x04000396 RID: 918
	private string knotName = "";

	// Token: 0x04000397 RID: 919
	private string baseFilePath = "";

	// Token: 0x04000398 RID: 920
	public bool allowActivationOnLoad = true;

	// Token: 0x04000399 RID: 921
	[HideInInspector]
	public UniqueId uniqId;

	// Token: 0x0400039A RID: 922
	[SerializeField]
	public bool bumpable = true;

	// Token: 0x0400039B RID: 923
	[SerializeField]
	public float bumpDelay = 1f;

	// Token: 0x0400039C RID: 924
	[SerializeField]
	public AudioClip bumpLineOverride;

	// Token: 0x0400039D RID: 925
	[HideInInspector]
	public string mainText = "";

	// Token: 0x0400039F RID: 927
	[Header("Interaction")]
	[Tooltip("When this is true, dialogue will NOT be initiated. Instead, an Alternate Interaction is executed. An Alternate Interaction always causes a change in the environment.")]
	public List<Interactable> AlternateInteractions;

	// Token: 0x040003A0 RID: 928
	[Header("Events")]
	[Tooltip("These listeners will be triggered prior to transitioning to dialogue with the Dateable.")]
	public UnityEvent DialogueEvent;

	// Token: 0x040003A1 RID: 929
	public UnityEvent CleanEvent;

	// Token: 0x040003A2 RID: 930
	public Renderer mr;

	// Token: 0x040003A3 RID: 931
	public Collider collider;

	// Token: 0x040003A4 RID: 932
	public Vector3 startpos;

	// Token: 0x040003A5 RID: 933
	public Vector3 startrot;

	// Token: 0x040003A6 RID: 934
	[HideInInspector]
	public Vector3 startscale = Vector3.one;

	// Token: 0x040003A7 RID: 935
	public Vector3 startscaleInAnimation = Vector3.one;

	// Token: 0x040003A8 RID: 936
	public ObjectSaveData objSaveData;

	// Token: 0x040003A9 RID: 937
	[Header("Wiggle Configuration")]
	[SerializeField]
	private float duration = 0.05f;

	// Token: 0x040003AA RID: 938
	[SerializeField]
	private float strength = 0.02f;

	// Token: 0x040003AB RID: 939
	[SerializeField]
	private int vibrato = 1;

	// Token: 0x040003AC RID: 940
	[SerializeField]
	private float randomness = 10f;

	// Token: 0x040003AD RID: 941
	[SerializeField]
	private bool fadeOut = true;

	// Token: 0x040003AE RID: 942
	[SerializeField]
	private float resetDuration = 0.03f;

	// Token: 0x040003AF RID: 943
	[SerializeField]
	private ShakeRandomnessMode shakeRandomnessMode = ShakeRandomnessMode.Harmonic;

	// Token: 0x040003B0 RID: 944
	[SerializeField]
	public List<InteractableObj> linkedWigglers = new List<InteractableObj>();
}
