using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class SlidingDoor : Interactable
{
	// Token: 0x06000500 RID: 1280 RVA: 0x0001DF6C File Offset: 0x0001C16C
	public override string noderequired()
	{
		if (!this.overrideNodeLock)
		{
			return "dorian_door";
		}
		return this.nodeLockAlt;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0001DF84 File Offset: 0x0001C184
	public void Initialize()
	{
		if (!this.initialized)
		{
			if (this.open)
			{
				this.openPos = base.transform.position;
				this.closedPos = this.openPos;
				switch (this.axis)
				{
				case SlidingDoor.Axis.X:
					this.closedPos += Vector3.right * (float)this.sign * this.dist;
					break;
				case SlidingDoor.Axis.Y:
					this.closedPos += Vector3.up * (float)this.sign * this.dist;
					break;
				case SlidingDoor.Axis.Z:
					this.closedPos += Vector3.forward * (float)this.sign * this.dist;
					break;
				}
			}
			else
			{
				this.closedPos = base.transform.position;
				this.openPos = this.closedPos;
				switch (this.axis)
				{
				case SlidingDoor.Axis.X:
					this.openPos += Vector3.right * (float)this.sign * this.dist;
					break;
				case SlidingDoor.Axis.Y:
					this.openPos += Vector3.up * (float)this.sign * this.dist;
					break;
				case SlidingDoor.Axis.Z:
					this.openPos += Vector3.forward * (float)this.sign * this.dist;
					break;
				}
			}
			this.Name = ((!this.open) ? "Open " : "Close ") + this.type;
			this.initialized = true;
		}
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0001E16A File Offset: 0x0001C36A
	public void Start()
	{
		this.Initialize();
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0001E174 File Offset: 0x0001C374
	public void OpenDoor(float speed)
	{
		Vector3 position = base.transform.position;
		if (this.startAudioClips != null && this.startAudioClips.Count > 0 && !this.loadingIn)
		{
			int num = Random.Range(0, this.startAudioClips.Count);
			Singleton<AudioManager>.Instance.PlayTrack(this.startAudioClips[num], AUDIO_TYPE.SFX, false, false, 0f, false, 1f, base.gameObject, false, SFX_SUBGROUP.NONE, false);
		}
		this.moving = true;
		this.stateLock = true;
		if (Singleton<Save>.Instance.GetDateStatus("dorian") == RelationshipStatus.Love && Singleton<Save>.Instance.GetDateStatusRealized("dorian") != RelationshipStatus.Realized && Singleton<Dateviators>.Instance.Equipped && this.initialized)
		{
			string text = Path.Combine("VoiceOver", "magical_love", "dorian");
			if (this.loveClipOverride == null)
			{
				Singleton<AudioManager>.Instance.PlayTrack(text, AUDIO_TYPE.SFX, false, false, 0.5f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, false);
			}
			else
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.loveClipOverride, AUDIO_TYPE.SFX, false, false, 0.1f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			}
		}
		base.StartCoroutine(this.changeDoorPos(position, this.openPos, speed, false, this.loadingIn));
		this.<OpenDoor>g__OnOpen|22_0();
		this.loadingIn = false;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0001E2D0 File Offset: 0x0001C4D0
	private IEnumerator changeDoorPos(Vector3 from, Vector3 to, float speed, bool closing, bool instant)
	{
		base.GetComponent<InteractableObj>();
		if (instant)
		{
			base.transform.position = to;
		}
		else
		{
			bool treatCollision = false;
			for (float f = 0f; f < speed; f += Time.deltaTime)
			{
				if (this.collidedWithPlayer && this.stopOnCollision)
				{
					treatCollision = true;
					break;
				}
				float num = Mathf.Lerp(from.x, to.x, f / speed);
				float num2 = Mathf.Lerp(from.y, to.y, f / speed);
				float num3 = Mathf.Lerp(from.z, to.z, f / speed);
				base.transform.position = new Vector3(num, num2, num3);
				yield return new WaitForEndOfFrame();
			}
			if (treatCollision)
			{
				treatCollision = false;
				Vector3 tempFromPosition = base.transform.position;
				for (float f = 0f; f < speed; f += Time.deltaTime)
				{
					float num4 = Mathf.Lerp(tempFromPosition.x, from.x, f / speed);
					float num5 = Mathf.Lerp(tempFromPosition.y, from.y, f / speed);
					float num6 = Mathf.Lerp(tempFromPosition.z, from.z, f / speed);
					base.transform.position = new Vector3(num4, num5, num6);
					yield return new WaitForEndOfFrame();
				}
				tempFromPosition = default(Vector3);
			}
		}
		if (closing && this.portal != null)
		{
			this.portal.open = false;
		}
		this.moving = false;
		this.collidedWithPlayer = false;
		yield break;
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0001E304 File Offset: 0x0001C504
	public void CloseDoor(float speed)
	{
		Vector3 position = base.transform.position;
		if (this.endAudioClips != null && this.endAudioClips.Count > 0 && !this.loadingIn)
		{
			int num = Random.Range(0, this.startAudioClips.Count);
			Singleton<AudioManager>.Instance.PlayTrack(this.endAudioClips[num], AUDIO_TYPE.SFX, false, false, 0f, false, 1f, base.gameObject, false, SFX_SUBGROUP.NONE, false);
		}
		this.stateLock = true;
		this.moving = true;
		base.StartCoroutine(this.changeDoorPos(position, this.closedPos, speed, false, this.loadingIn));
		this.<CloseDoor>g__OnClosed|24_0();
		this.loadingIn = false;
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x0001E3B4 File Offset: 0x0001C5B4
	public override void Interact()
	{
		if (this.moving || this.stateLock)
		{
			return;
		}
		if (!this.initialized)
		{
			this.Initialize();
		}
		if (this.locked)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.lockedClip, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, base.gameObject, false, SFX_SUBGROUP.NONE, false);
			return;
		}
		if (this.open)
		{
			this.CloseDoor(0.5f);
			return;
		}
		if (BetterPlayerControl.Instance != null)
		{
			this.OpenDoor(0.5f);
			return;
		}
		this.OpenDoor(0.5f);
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x0001E448 File Offset: 0x0001C648
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			this.collidedWithPlayer = true;
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x0001E463 File Offset: 0x0001C663
	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.collidedWithPlayer = false;
		}
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x0001E49C File Offset: 0x0001C69C
	[CompilerGenerated]
	private void <OpenDoor>g__OnOpen|22_0()
	{
		this.Name = "Close Door";
		this.open = true;
		this.moving = false;
		this.stateLock = false;
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0001E4BE File Offset: 0x0001C6BE
	[CompilerGenerated]
	private void <CloseDoor>g__OnClosed|24_0()
	{
		this.Name = "Open Door";
		this.open = false;
		this.moving = false;
		this.stateLock = false;
	}

	// Token: 0x040004EF RID: 1263
	[Header("Bools")]
	public bool open;

	// Token: 0x040004F0 RID: 1264
	public bool overrideNodeLock;

	// Token: 0x040004F1 RID: 1265
	public bool locked;

	// Token: 0x040004F2 RID: 1266
	private bool moving;

	// Token: 0x040004F3 RID: 1267
	private bool initialized;

	// Token: 0x040004F4 RID: 1268
	[Header("Strings")]
	public string nodeLockAlt;

	// Token: 0x040004F5 RID: 1269
	public string type = "Sliding Door";

	// Token: 0x040004F6 RID: 1270
	[Header("Config")]
	public float dist = 3.75f;

	// Token: 0x040004F7 RID: 1271
	[SerializeField]
	private SlidingDoor.Sign sign;

	// Token: 0x040004F8 RID: 1272
	[SerializeField]
	private SlidingDoor.Axis axis;

	// Token: 0x040004F9 RID: 1273
	private Vector3 openPos;

	// Token: 0x040004FA RID: 1274
	private Vector3 closedPos;

	// Token: 0x040004FB RID: 1275
	private OcclusionPortal portal;

	// Token: 0x040004FC RID: 1276
	[Header("Sound")]
	public AudioClip lockedClip;

	// Token: 0x040004FD RID: 1277
	public List<AudioClip> startAudioClips;

	// Token: 0x040004FE RID: 1278
	public List<AudioClip> endAudioClips;

	// Token: 0x040004FF RID: 1279
	public AudioClip loveClipOverride;

	// Token: 0x04000500 RID: 1280
	[SerializeField]
	private bool collidedWithPlayer;

	// Token: 0x04000501 RID: 1281
	[SerializeField]
	private bool stopOnCollision;

	// Token: 0x020002D6 RID: 726
	private enum Axis
	{
		// Token: 0x04001137 RID: 4407
		X,
		// Token: 0x04001138 RID: 4408
		Y,
		// Token: 0x04001139 RID: 4409
		Z
	}

	// Token: 0x020002D7 RID: 727
	private enum Sign
	{
		// Token: 0x0400113B RID: 4411
		Pos = 1,
		// Token: 0x0400113C RID: 4412
		Neg = -1
	}
}
