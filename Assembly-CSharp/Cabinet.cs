using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class Cabinet : Interactable
{
	// Token: 0x060003E5 RID: 997 RVA: 0x000186AB File Offset: 0x000168AB
	public override string noderequired()
	{
		if (!this.overrideNodeLock)
		{
			return "dorian_door";
		}
		return this.NodeLockAlt;
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x000186C4 File Offset: 0x000168C4
	public void Initialize()
	{
		if (!this.initialized)
		{
			this.startforward = base.transform.up;
			this.startrot = base.transform.eulerAngles;
			this.currentrot = this.startrot.z;
			this.Name = ((!this.Open) ? "Open " : "Close ") + this.type;
			this.initialized = true;
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x00018738 File Offset: 0x00016938
	public void Start()
	{
		this.Initialize();
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00018740 File Offset: 0x00016940
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			this.collidedWithPlayer = true;
		}
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0001875B File Offset: 0x0001695B
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			this.collidedWithPlayer = true;
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x00018776 File Offset: 0x00016976
	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.collidedWithPlayer = false;
		}
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x00018791 File Offset: 0x00016991
	public void Stop()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001879C File Offset: 0x0001699C
	public void OpenDoor(Vector3 Playerposition, float speed)
	{
		this.dot = Vector3.Dot(this.startforward, base.transform.position - Playerposition);
		float num = this.startrot.z + this.range;
		if (this.startAudioClips != null && this.startAudioClips.Count > 0 && !this.loadingIn)
		{
			int num2 = Random.Range(0, this.startAudioClips.Count);
			Singleton<AudioManager>.Instance.PlayTrack(this.startAudioClips[num2], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
		this.loadingIn = false;
		this.moving = true;
		base.StartCoroutine(this.changedoorrot(this.currentrot, num, this.openSpeed));
		this.Name = "Close Cabinet";
		this.Open = true;
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00018878 File Offset: 0x00016A78
	public void CloseDoor(float speed)
	{
		if (this.endAudioClips != null && this.endAudioClips.Count > 0 && !this.loadingIn)
		{
			int num = Random.Range(0, this.endAudioClips.Count);
			Singleton<AudioManager>.Instance.PlayTrack(this.endAudioClips[num], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
		this.loadingIn = false;
		this.moving = true;
		base.StartCoroutine(this.changedoorrot(this.currentrot, this.startrot.z, this.closeSpeed));
		this.Name = "Open Cabinet";
		this.Open = false;
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00018926 File Offset: 0x00016B26
	private IEnumerator changedoorrot(float from, float to, float speed)
	{
		InteractableObj obj = base.GetComponent<InteractableObj>();
		for (float f = 0f; f < speed; f += Time.deltaTime)
		{
			float num = Mathf.Lerp(from, to, f / speed);
			base.transform.eulerAngles = this.startrot + this.correctUP * num;
			if (obj != null)
			{
				obj.startrot = base.transform.eulerAngles;
			}
			if (this.collidedWithPlayer)
			{
				this.moving = false;
				yield break;
			}
			yield return new WaitForEndOfFrame();
		}
		base.transform.eulerAngles = this.startrot + this.correctUP * to;
		this.currentrot = to;
		this.moving = false;
		this.collidedWithPlayer = false;
		yield break;
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001894C File Offset: 0x00016B4C
	public override void Interact()
	{
		if (this.moving || this.collidedWithPlayer)
		{
			return;
		}
		if (!this.initialized)
		{
			this.Initialize();
		}
		if (Singleton<Dateviators>.Instance.Equipped && this.interactableObj)
		{
			this.animator.SetTrigger("magicalAnimStart");
			return;
		}
		if (this.Open)
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

	// Token: 0x060003F0 RID: 1008 RVA: 0x000189EF File Offset: 0x00016BEF
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(base.transform.position, base.transform.position + this.correctUP);
	}

	// Token: 0x040003D8 RID: 984
	public bool Open;

	// Token: 0x040003D9 RID: 985
	public bool overrideNodeLock;

	// Token: 0x040003DA RID: 986
	public string NodeLockAlt;

	// Token: 0x040003DB RID: 987
	public string type = "Door";

	// Token: 0x040003DC RID: 988
	public float range = 120f;

	// Token: 0x040003DD RID: 989
	private float dot;

	// Token: 0x040003DE RID: 990
	private Vector3 startforward;

	// Token: 0x040003DF RID: 991
	private float currentrot;

	// Token: 0x040003E0 RID: 992
	[SerializeField]
	private float openSpeed = 0.5f;

	// Token: 0x040003E1 RID: 993
	[SerializeField]
	private float closeSpeed = 0.5f;

	// Token: 0x040003E2 RID: 994
	[SerializeField]
	private AnimationCurve easingCurve;

	// Token: 0x040003E3 RID: 995
	private Vector3 startrot;

	// Token: 0x040003E4 RID: 996
	private Vector3 correctUP = new Vector3(0f, 1f, 0f);

	// Token: 0x040003E5 RID: 997
	[SerializeField]
	private bool moving;

	// Token: 0x040003E6 RID: 998
	private bool initialized;

	// Token: 0x040003E7 RID: 999
	public Animator animator;

	// Token: 0x040003E8 RID: 1000
	public InteractableObj interactableObj;

	// Token: 0x040003E9 RID: 1001
	public List<AudioClip> startAudioClips;

	// Token: 0x040003EA RID: 1002
	public List<AudioClip> endAudioClips;

	// Token: 0x040003EB RID: 1003
	[SerializeField]
	private bool collidedWithPlayer;
}
