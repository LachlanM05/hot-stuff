using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class Door : Interactable
{
	// Token: 0x0600042E RID: 1070 RVA: 0x00019A49 File Offset: 0x00017C49
	public override string noderequired()
	{
		if (!this.overrideNodeLock)
		{
			return "dorian_door";
		}
		return this.NodeLockAlt;
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00019A60 File Offset: 0x00017C60
	private void OnValidate()
	{
		InteractableObj interactableObj;
		if (!this.interactableObj && base.TryGetComponent<InteractableObj>(out interactableObj))
		{
			this.interactableObj = interactableObj;
		}
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00019A8C File Offset: 0x00017C8C
	public void Initialize()
	{
		if (!this.initialized)
		{
			this.portal = base.GetComponent<OcclusionPortal>();
			this.startForward = base.transform.up;
			this.startRot = base.transform.eulerAngles;
			this.currentRot = this.startRot.z;
			if (this.locked)
			{
				this.Name = "Locked Door";
			}
			else
			{
				this.Name = ((!this.open) ? "Open " : "Close ") + this.type;
			}
			this.blockInteraction = false;
			this.initialized = true;
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00019B2B File Offset: 0x00017D2B
	public void Start()
	{
		this.Initialize();
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00019B33 File Offset: 0x00017D33
	public void ToggleBlockInteraction()
	{
		this.blockInteraction = !this.blockInteraction;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00019B44 File Offset: 0x00017D44
	public void OpenDoor(Vector3 playerPosition, float speed)
	{
		if (this.blockInteraction)
		{
			return;
		}
		Vector3 position = base.transform.position;
		Vector3 vector = playerPosition;
		vector.y = position.y;
		this.dot = Vector3.Dot(this.startForward, position - vector);
		float num = 0f;
		switch (this.doorOpenType)
		{
		case Door.DoorOpenType.BothWays:
			num = this.startRot.z + ((this.dot > 0f) ? this.range : (-this.range));
			break;
		case Door.DoorOpenType.OnlyPositive:
			num = this.startRot.z + this.range;
			break;
		case Door.DoorOpenType.OnlyNegative:
			num = this.startRot.z - this.range;
			break;
		}
		if (this.startAudioClips != null && this.startAudioClips.Count > 0 && !this.loadingIn)
		{
			int num2 = Random.Range(0, this.startAudioClips.Count);
			Singleton<AudioManager>.Instance.PlayTrack(this.startAudioClips[num2], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
		this.moving = true;
		if (this.portal != null)
		{
			this.portal.open = true;
		}
		this.open = true;
		base.StartCoroutine(this.changeDoorRot(this.currentRot, num, this.openSpeed, false, this.loadingIn));
		this.loadingIn = false;
		this.Name = "Close Door";
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00019CBC File Offset: 0x00017EBC
	public void OpenDoor(float speed)
	{
		if (this.blockInteraction)
		{
			return;
		}
		if (this.open)
		{
			return;
		}
		Vector3 position = base.transform.position;
		Vector3 position2 = BetterPlayerControl.Instance.transform.position;
		position2.y = position.y;
		this.dot = Vector3.Dot(this.startForward, position - position2);
		float num = 0f;
		switch (this.doorOpenType)
		{
		case Door.DoorOpenType.BothWays:
			num = this.startRot.z + ((this.dot > 0f) ? this.range : (-this.range));
			break;
		case Door.DoorOpenType.OnlyPositive:
			num = this.startRot.z + this.range;
			break;
		case Door.DoorOpenType.OnlyNegative:
			num = this.startRot.z - this.range;
			break;
		}
		if (this.startAudioClips != null && this.startAudioClips.Count > 0 && !this.loadingIn)
		{
			int num2 = Random.Range(0, this.startAudioClips.Count);
			Singleton<AudioManager>.Instance.PlayTrack(this.startAudioClips[num2], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
		this.moving = true;
		if (this.portal != null)
		{
			this.portal.open = true;
		}
		this.open = true;
		base.StartCoroutine(this.changeDoorRot(this.currentRot, num, this.openSpeed, false, this.loadingIn));
		this.loadingIn = false;
		this.Name = "Close Door";
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00019E4C File Offset: 0x0001804C
	public void CloseDoor(float speed)
	{
		if (this.blockInteraction)
		{
			return;
		}
		if (this.endAudioClips != null && this.endAudioClips.Count > 0 && !this.loadingIn)
		{
			int num = Random.Range(0, this.endAudioClips.Count);
			Singleton<AudioManager>.Instance.PlayTrack(this.endAudioClips[num], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
		this.moving = true;
		this.open = false;
		base.StartCoroutine(this.changeDoorRot(this.currentRot, this.startRot.z, this.closeSpeed, true, this.loadingIn));
		this.loadingIn = false;
		this.Name = "Open Door";
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00019F0A File Offset: 0x0001810A
	private IEnumerator changeDoorRot(float from, float to, float speed, bool closing, bool instant)
	{
		InteractableObj obj = base.GetComponent<InteractableObj>();
		if (instant)
		{
			base.transform.eulerAngles = this.startRot + this.correctUP * to;
			if (obj != null)
			{
				obj.startrot = base.transform.eulerAngles;
			}
		}
		else
		{
			float f = 0f;
			while (f < speed && (!this.collidedWithPlayer || !this.stopOnCollision))
			{
				float num = Mathf.Lerp(from, to, f / speed);
				base.transform.eulerAngles = this.startRot + this.correctUP * num;
				if (obj != null)
				{
					obj.startrot = base.transform.eulerAngles;
				}
				this.currentRot = num;
				yield return new WaitForEndOfFrame();
				f += Time.deltaTime;
			}
		}
		if (!this.collidedWithPlayer)
		{
			base.transform.eulerAngles = this.startRot + this.correctUP * to;
			this.currentRot = to;
		}
		if (closing && this.portal != null && !this.collidedWithPlayer)
		{
			this.portal.open = false;
		}
		this.moving = false;
		this.collidedWithPlayer = false;
		yield break;
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00019F40 File Offset: 0x00018140
	public override void Interact()
	{
		if (this.moving)
		{
			this.loadingIn = false;
			return;
		}
		if (!this.initialized)
		{
			this.Initialize();
		}
		if (Singleton<Dateviators>.Instance.Equipped && this.altInteractable != null)
		{
			this.altInteractable.Interact();
			return;
		}
		if (!this.open)
		{
			if (Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()) == RelationshipStatus.Love && Singleton<Save>.Instance.GetDateStatusRealized(this.interactableObj.InternalName()) != RelationshipStatus.Realized && Singleton<Dateviators>.Instance.Equipped)
			{
				string text = Path.Combine("VoiceOver", "magical_love", this.interactableObj.InternalName());
				if (this.loveClipOverride == null)
				{
					Singleton<AudioManager>.Instance.PlayTrack(text, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, true);
				}
				else
				{
					Singleton<AudioManager>.Instance.PlayTrack(this.loveClipOverride, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
				}
			}
			if (!this.interactableObj.InternalName().Contains("dorian") && Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()) == RelationshipStatus.Friend && Singleton<Save>.Instance.GetDateStatusRealized(this.interactableObj.InternalName()) != RelationshipStatus.Realized && Singleton<Dateviators>.Instance.Equipped)
			{
				string text2 = Path.Combine("VoiceOver", "magical_friend", this.interactableObj.InternalName());
				if (this.loveClipOverride == null)
				{
					Singleton<AudioManager>.Instance.PlayTrack(text2, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, true);
				}
				else
				{
					Singleton<AudioManager>.Instance.PlayTrack(this.friendClipOverride, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
				}
			}
		}
		if (this.locked)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.lockedClip, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			Animator animator;
			if (base.TryGetComponent<Animator>(out animator))
			{
				animator.SetTrigger("standardAnimStart");
			}
			return;
		}
		if (this.open)
		{
			this.CloseDoor(0.5f);
			return;
		}
		if (BetterPlayerControl.Instance != null)
		{
			this.OpenDoor(BetterPlayerControl.Instance.transform.position, 0.5f);
			return;
		}
		this.OpenDoor(this.interactedPosition, 0.5f);
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0001A1BA File Offset: 0x000183BA
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(base.transform.position, base.transform.position + this.correctUP);
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0001A1E2 File Offset: 0x000183E2
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			this.collidedWithPlayer = true;
		}
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x0001A1FD File Offset: 0x000183FD
	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.collidedWithPlayer = false;
		}
	}

	// Token: 0x04000425 RID: 1061
	public bool open;

	// Token: 0x04000426 RID: 1062
	public bool overrideNodeLock;

	// Token: 0x04000427 RID: 1063
	public bool locked;

	// Token: 0x04000428 RID: 1064
	public string NodeLockAlt;

	// Token: 0x04000429 RID: 1065
	public string type = "Door";

	// Token: 0x0400042A RID: 1066
	public float range = 85f;

	// Token: 0x0400042B RID: 1067
	private OcclusionPortal portal;

	// Token: 0x0400042C RID: 1068
	private float dot;

	// Token: 0x0400042D RID: 1069
	private Vector3 startForward;

	// Token: 0x0400042E RID: 1070
	private float currentRot;

	// Token: 0x0400042F RID: 1071
	[SerializeField]
	private float openSpeed = 0.5f;

	// Token: 0x04000430 RID: 1072
	[SerializeField]
	private float closeSpeed = 0.5f;

	// Token: 0x04000431 RID: 1073
	[SerializeField]
	private AnimationCurve easingCurve;

	// Token: 0x04000432 RID: 1074
	private Vector3 startRot;

	// Token: 0x04000433 RID: 1075
	private Vector3 correctUP = new Vector3(0f, 1f, 0f);

	// Token: 0x04000434 RID: 1076
	private bool moving;

	// Token: 0x04000435 RID: 1077
	public Door.DoorOpenType doorOpenType = Door.DoorOpenType.OnlyPositive;

	// Token: 0x04000436 RID: 1078
	private bool initialized;

	// Token: 0x04000437 RID: 1079
	public Interactable altInteractable;

	// Token: 0x04000438 RID: 1080
	public AudioClip lockedClip;

	// Token: 0x04000439 RID: 1081
	public AudioClip loveClipOverride;

	// Token: 0x0400043A RID: 1082
	public AudioClip friendClipOverride;

	// Token: 0x0400043B RID: 1083
	public List<AudioClip> startAudioClips;

	// Token: 0x0400043C RID: 1084
	public List<AudioClip> endAudioClips;

	// Token: 0x0400043D RID: 1085
	public InteractableObj interactableObj;

	// Token: 0x0400043E RID: 1086
	public bool blockInteraction;

	// Token: 0x0400043F RID: 1087
	[SerializeField]
	private bool collidedWithPlayer;

	// Token: 0x04000440 RID: 1088
	[SerializeField]
	private bool stopOnCollision;

	// Token: 0x020002C9 RID: 713
	public enum DoorOpenType
	{
		// Token: 0x040010FF RID: 4351
		BothWays,
		// Token: 0x04001100 RID: 4352
		OnlyPositive,
		// Token: 0x04001101 RID: 4353
		OnlyNegative
	}
}
