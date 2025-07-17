using System;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200006C RID: 108
public class InteractableManager : Singleton<InteractableManager>
{
	// Token: 0x0600037F RID: 895 RVA: 0x000164B8 File Offset: 0x000146B8
	public InteractableObj GetActiveObj()
	{
		return this._activeObject;
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000380 RID: 896 RVA: 0x000164C0 File Offset: 0x000146C0
	public bool IsPlayerInRange
	{
		get
		{
			return this.inrange;
		}
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000381 RID: 897 RVA: 0x000164C8 File Offset: 0x000146C8
	// (remove) Token: 0x06000382 RID: 898 RVA: 0x000164FC File Offset: 0x000146FC
	public static event InteractableManager.ResetTalkedEvent ResetTalked;

	// Token: 0x06000383 RID: 899 RVA: 0x0001652F File Offset: 0x0001472F
	public void ResetTalkedTo()
	{
		Singleton<DougLogic>.Instance.TalkedTo = false;
		if (InteractableManager.ResetTalked != null)
		{
			InteractableManager.ResetTalked();
		}
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00016550 File Offset: 0x00014750
	public void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		PlayerPauser._pause += this.setFrozen;
		this.dateables = Object.FindObjectsOfType<InteractableObj>(true);
		for (int i = 0; i < this.dateables.Length; i++)
		{
			this.dateables[i].Init();
		}
		Save.SetAutoSaveSlots();
	}

	// Token: 0x06000385 RID: 901 RVA: 0x000165B0 File Offset: 0x000147B0
	private void setFrozen(bool b)
	{
		this.frozen = b;
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000386 RID: 902 RVA: 0x000165B9 File Offset: 0x000147B9
	public InteractableObj activeObject
	{
		get
		{
			return this._activeObject;
		}
	}

	// Token: 0x06000387 RID: 903 RVA: 0x000165C4 File Offset: 0x000147C4
	public void Update()
	{
		if (this.frozen)
		{
			this.UI.SetActive(false);
			if (this._activeObject != null)
			{
				this._activeObject.HideHighlight();
			}
			return;
		}
		if (this._activeObject == null || BetterPlayerControl.Instance.STATE != BetterPlayerControl.PlayerState.CanControl)
		{
			this.UIon = false;
			this.UI.SetActive(false);
			return;
		}
		if (this.equipped)
		{
			if (this._activeObject.inkFileName != "")
			{
				if (this.inrange)
				{
					if (this.mousedown)
					{
						this.StartBounce();
					}
					else
					{
						this.StopBounce();
					}
					if (Singleton<Save>.Instance.GetDateStatus(this._activeObject.internalCharacterName) != RelationshipStatus.Unmet)
					{
						this._activeObject.ShowHighlight();
						if (this.UiControlGlyph != null)
						{
							Sprite sprite = null;
							if (ControlsGlyphs.Instance != null)
							{
								sprite = ControlsGlyphs.Instance.GetGlyph(this.player, this.player.controllers.maps.GetFirstButtonMapWithAction("Awaken", true), false, true);
							}
							if (sprite != null)
							{
								this.UiControlTooltip.enabled = false;
								this.UiControlGlyph.enabled = true;
								this.UiControlGlyph.sprite = sprite;
							}
							else
							{
								this.UiControlTooltip.enabled = true;
								this.UiControlGlyph.enabled = false;
							}
						}
						this.UIText.text = this._activeObject.InteractionPrompt;
						this.UiControlTooltip.text = this.player.controllers.maps.GetFirstButtonMapWithAction("Awaken", true).elementIdentifierName;
						this.UIon = true;
					}
					else
					{
						this._activeObject.ShowLightHighlight();
						this.UIon = false;
					}
				}
				else
				{
					this._activeObject.HideHighlight();
				}
			}
			else
			{
				this.UIon = false;
				this.StopBounce();
			}
		}
		else if (this.showaltinteraction)
		{
			ActionElementMap firstButtonMapWithAction = this.player.controllers.maps.GetFirstButtonMapWithAction(12, true);
			if (this.inrange && this._activeObject.AlternateInteractions != null && firstButtonMapWithAction != null)
			{
				if (this._activeObject.AlternateInteractions.Count > 0 && this._activeObject.AlternateInteractions[0] != null)
				{
					this.UIText.text = this._activeObject.AlternateInteractions[0].Name;
				}
				this.UiControlTooltip.text = firstButtonMapWithAction.elementIdentifierName;
				this.UIon = true;
			}
			else
			{
				this.UIon = false;
			}
		}
		else
		{
			this.UIon = false;
		}
		this.UI.SetActive(false);
		if (this.UIon)
		{
			if (this._activeObject.collider.GetType() == typeof(Collider))
			{
				this.UI.transform.position = Camera.main.WorldToScreenPoint(this._activeObject.mr.bounds.center);
				return;
			}
			this.UI.transform.position = Camera.main.WorldToScreenPoint(this._activeObject.collider.bounds.center);
		}
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00016903 File Offset: 0x00014B03
	public void UpdatePlayerValues(bool _mousedown, bool _inrange)
	{
		this.mousedown = _mousedown;
		this.inrange = _inrange;
		this.equipped = Singleton<Dateviators>.Instance.Equipped;
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00016924 File Offset: 0x00014B24
	public void UpdatePlayerTarget(InteractableObj hit)
	{
		if (this._activeObject != null)
		{
			this.StopBounce();
		}
		this._activeObject = hit;
		if (this._activeObject == null)
		{
			this.UIon = false;
			return;
		}
		if (this._activeObject.AlternateInteractions == null)
		{
			return;
		}
		if (this._activeObject.AlternateInteractions.Count > 0)
		{
			if (this._activeObject.AlternateInteractions[0] != null)
			{
				this.showaltinteraction = this._activeObject.AlternateInteractions.Count > 0 && this._activeObject.AlternateInteractions[0].CheckCanUse();
				return;
			}
		}
		else
		{
			this.showaltinteraction = false;
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x000169D8 File Offset: 0x00014BD8
	public void Interact(bool ignoreGlassesAwakening = false, bool forceCanInteract = false)
	{
		if ((forceCanInteract || Singleton<TutorialController>.Instance.CanInteract(this._activeObject)) && Singleton<GameController>.Instance.viewState != VIEW_STATE.TALKING && this._activeObject != null && this.inrange)
		{
			this.StopBounce();
			Singleton<GameController>.Instance.SelectObj(this._activeObject, false, null, false, ignoreGlassesAwakening, false);
			this.UIon = false;
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00016A40 File Offset: 0x00014C40
	public void Interact(InteractableObj activeObject)
	{
		this._activeObject = activeObject;
		if (Singleton<GameController>.Instance.viewState != VIEW_STATE.TALKING && this._activeObject != null)
		{
			this.StopBounce();
			Singleton<GameController>.Instance.SelectObj(this._activeObject, true, null, false, false, false);
			this.UIon = false;
		}
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00016A91 File Offset: 0x00014C91
	public void StartBounce()
	{
		if (this._activeObject == null)
		{
			return;
		}
		this._activeObject.ShowHighlight();
		this._activeObject.EnableWiggle();
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00016AB8 File Offset: 0x00014CB8
	public void StopBounce()
	{
		if (this._activeObject == null)
		{
			return;
		}
		this._activeObject.HideHighlight();
		this._activeObject.DisableWiggle();
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00016AE0 File Offset: 0x00014CE0
	public void StartPulse()
	{
		foreach (InteractableObj interactableObj in this.dateables)
		{
			if (interactableObj.inkFileName != "")
			{
				interactableObj.StartPulse();
			}
		}
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00016B20 File Offset: 0x00014D20
	public void StopPulse()
	{
		foreach (InteractableObj interactableObj in this.dateables)
		{
			if (interactableObj.inkFileName != "")
			{
				interactableObj.StopPulse();
			}
		}
	}

	// Token: 0x04000378 RID: 888
	public bool frozen;

	// Token: 0x04000379 RID: 889
	public KeyCode interactKey;

	// Token: 0x0400037A RID: 890
	public GameObject UI;

	// Token: 0x0400037B RID: 891
	public TextMeshProUGUI UIText;

	// Token: 0x0400037C RID: 892
	public TextMeshProUGUI UiControlTooltip;

	// Token: 0x0400037D RID: 893
	public Image UiControlGlyph;

	// Token: 0x0400037E RID: 894
	[SerializeField]
	private InteractableObj _activeObject;

	// Token: 0x0400037F RID: 895
	private InteractableObj[] dateables;

	// Token: 0x04000380 RID: 896
	public bool UIon;

	// Token: 0x04000381 RID: 897
	private bool equipped;

	// Token: 0x04000382 RID: 898
	private bool mousedown;

	// Token: 0x04000383 RID: 899
	private bool inrange;

	// Token: 0x04000384 RID: 900
	private bool showaltinteraction;

	// Token: 0x04000385 RID: 901
	private Player player;

	// Token: 0x020002C1 RID: 705
	// (Invoke) Token: 0x0600157E RID: 5502
	public delegate void ResetTalkedEvent();
}
