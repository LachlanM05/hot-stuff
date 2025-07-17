using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000094 RID: 148
public class Sofa : Interactable, IReloadHandler
{
	// Token: 0x0600050C RID: 1292 RVA: 0x0001E4E0 File Offset: 0x0001C6E0
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x0001E4E7 File Offset: 0x0001C6E7
	public void Start()
	{
		this.isShown = false;
		UnityEvent dialogueExit = Singleton<GameController>.Instance.DialogueExit;
		if (dialogueExit == null)
		{
			return;
		}
		dialogueExit.AddListener(new UnityAction(this.CheckBunnyStatus));
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0001E510 File Offset: 0x0001C710
	public override void Interact()
	{
		if (this.stateLock || this.isShown)
		{
			return;
		}
		this.MoveSofa();
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x0001E529 File Offset: 0x0001C729
	private void CheckBunnyStatus()
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized("dolly") == RelationshipStatus.Realized)
		{
			this.dustBunny.SetActive(false);
		}
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x0001E549 File Offset: 0x0001C749
	public void MoveSofa()
	{
		this.MoveSofa(false);
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x0001E554 File Offset: 0x0001C754
	public void PostMoveChecks()
	{
		if (this.isShown)
		{
			return;
		}
		this.interactable.standardSfx_activate = new List<AudioClip>();
		this.interactable.standardSfx_activate.Add(this.sofaClip);
		this.isShown = true;
		this.interactable.InteractStarted.RemoveAllListeners();
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0001E5A8 File Offset: 0x0001C7A8
	public void MoveSofa(bool force)
	{
		if (this.isShown && !force && Singleton<Save>.Instance.GetDateStatusRealized("dolly") != RelationshipStatus.Realized)
		{
			return;
		}
		if (!this.dustBunny.gameObject.activeSelf)
		{
			if (!force)
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.sofaClip, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			}
			this.dustBunny.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0001E620 File Offset: 0x0001C820
	public void LoadState()
	{
		this.isShown = Singleton<Save>.Instance.GetInteractableState(base.gameObject.name);
		if (this.isShown)
		{
			this.MoveSofa(true);
			return;
		}
		this.dustBunny.SetActive(false);
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x0001E659 File Offset: 0x0001C859
	public void SaveState()
	{
		Singleton<Save>.Instance.SetInteractableState(base.gameObject.name, this.isShown);
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x0001E676 File Offset: 0x0001C876
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x0001E680 File Offset: 0x0001C880
	private void SetDustBunnyStartPosition()
	{
		Vector3 position = this.dustBunny.transform.position;
		position.x = 7f;
		this.dustBunny.transform.position = position;
	}

	// Token: 0x04000502 RID: 1282
	private const float DustBunnyStartXPos = 7f;

	// Token: 0x04000503 RID: 1283
	private const float DustBunnyEndXPos = 4.2f;

	// Token: 0x04000504 RID: 1284
	private const float DustBunnyTransitionDuration = 2.5f;

	// Token: 0x04000505 RID: 1285
	public GameObject dustBunny;

	// Token: 0x04000506 RID: 1286
	private bool isShown;

	// Token: 0x04000507 RID: 1287
	[SerializeField]
	private AudioClip sofaClip;

	// Token: 0x04000508 RID: 1288
	[SerializeField]
	private GenericInteractable interactable;
}
