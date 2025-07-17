using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x02000081 RID: 129
public class Faucet : Interactable
{
	// Token: 0x06000459 RID: 1113 RVA: 0x0001A7CC File Offset: 0x000189CC
	private string DateableName()
	{
		if (!this.isBathsheba)
		{
			return "sinclaire";
		}
		return "bathsheba";
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0001A7E1 File Offset: 0x000189E1
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0001A7E8 File Offset: 0x000189E8
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x0001A7F0 File Offset: 0x000189F0
	private void Start()
	{
		if (this.waterInteractableBox)
		{
			this.waterInteractableBox.enabled = false;
		}
		if (this.waterInteractableMesh)
		{
			this.waterInteractableMesh.enabled = false;
		}
		this.turnedOn = false;
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0001A82C File Offset: 0x00018A2C
	public void TurnOn()
	{
		base.StopAllCoroutines();
		this.waterAnim.enabled = true;
		this.waterAnim.SetFloat("waterSpeed", -1f * this.waterAnim.GetFloat("waterSpeed"));
		this.turnedOn = true;
		if (this.waterInteractableBox)
		{
			this.waterInteractableBox.enabled = true;
		}
		if (this.waterInteractableMesh)
		{
			this.waterInteractableMesh.enabled = true;
		}
		Singleton<AudioManager>.Instance.PlayTrack(this.faucetOn, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		this.PlayLoopedTrack();
		if (this.faucetHandle != null)
		{
			base.StartCoroutine(this.ChangeRot(this.faucetHandle.transform, this.FaucetOff, this.FaucetOn, this.speed));
		}
		this.waterAnim.Play("WaterLevelRise", 0, 0f);
		this.StartVFX();
		this.waterSurface.SetActive(true);
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0001A938 File Offset: 0x00018B38
	public void TurnOff()
	{
		base.StopAllCoroutines();
		this.waterAnim.SetFloat("waterSpeed", -1f * this.waterAnim.GetFloat("waterSpeed"));
		float num = Mathf.Clamp(this.waterAnim.GetCurrentAnimatorStateInfo(0).normalizedTime, 0f, 1f);
		this.turnedOn = false;
		if (this.waterInteractableBox)
		{
			this.waterInteractableBox.enabled = false;
		}
		if (this.waterInteractableMesh)
		{
			this.waterInteractableMesh.enabled = false;
		}
		this.StopLoopedTrack();
		this.StopVFX();
		base.Invoke("PlayDrainSound", 0.5f);
		if (this.faucetHandle != null)
		{
			base.StartCoroutine(this.ChangeRot(this.faucetHandle.transform, this.FaucetOn, this.FaucetOff, this.speed));
		}
		this.waterAnim.Play("WaterLevelRise", 0, num);
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0001AA34 File Offset: 0x00018C34
	public override void Interact()
	{
		RelationshipStatus dateStatus = Singleton<Save>.Instance.GetDateStatus(this.DateableName());
		if (Singleton<Dateviators>.Instance.Equipped && (dateStatus == RelationshipStatus.Friend || dateStatus == RelationshipStatus.Love))
		{
			if (this.turnedOn)
			{
				this.TurnOff();
			}
			if (this.interactableObj)
			{
				this.animator.SetInteger("ending", (int)dateStatus);
				this.animator.SetTrigger("magicalAnimStart");
				Singleton<AudioManager>.Instance.PlayTrack(this.magicOn, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
				if (dateStatus == RelationshipStatus.Friend)
				{
					string text = Path.Combine("VoiceOver", "magical_friend", this.DateableName());
					Singleton<AudioManager>.Instance.PlayTrack(text, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, true);
				}
				else
				{
					string text2 = Path.Combine("VoiceOver", "magical_love", this.DateableName());
					Singleton<AudioManager>.Instance.PlayTrack(text2, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, true);
				}
			}
		}
		if (Singleton<Dateviators>.Instance.Equipped)
		{
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this.waterAnim.GetCurrentAnimatorStateInfo(0);
		if (currentAnimatorStateInfo.normalizedTime <= 0f || currentAnimatorStateInfo.normalizedTime >= 1f)
		{
			if (this.turnedOn)
			{
				this.TurnOff();
				return;
			}
			this.TurnOn();
		}
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0001AB9C File Offset: 0x00018D9C
	public void PlayDrainSound()
	{
		Singleton<AudioManager>.Instance.PlayTrack(this.faucetOff, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0001ABD0 File Offset: 0x00018DD0
	public void PlayLoopedTrack()
	{
		Singleton<AudioManager>.Instance.PlayTrack(this.faucetLoop, AUDIO_TYPE.SFX, false, false, 0.5f, true, 1f, base.gameObject, true, SFX_SUBGROUP.FOLEY, false);
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0001AC04 File Offset: 0x00018E04
	public void StopLoopedTrack()
	{
		Singleton<AudioManager>.Instance.StopTrack(this.faucetLoop.name, 0.2f);
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0001AC20 File Offset: 0x00018E20
	public void StartVFX()
	{
		this.waterStream.Play();
		this.waterStream.enabled = true;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001AC39 File Offset: 0x00018E39
	public void StopVFX()
	{
		this.waterStream.Stop();
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0001AC46 File Offset: 0x00018E46
	private IEnumerator ChangeRot(Transform xform, Vector3 from, Vector3 to, float speed)
	{
		xform.localEulerAngles = from;
		for (float f = 0f; f < speed; f += Time.deltaTime)
		{
			Vector3 vector = Vector3.Lerp(from, to, f / speed);
			xform.localEulerAngles = vector;
			yield return new WaitForEndOfFrame();
		}
		xform.localEulerAngles = to;
		yield break;
	}

	// Token: 0x04000455 RID: 1109
	[SerializeField]
	private GameObject waterSurface;

	// Token: 0x04000456 RID: 1110
	[SerializeField]
	private VisualEffect waterStream;

	// Token: 0x04000457 RID: 1111
	[SerializeField]
	private BoxCollider waterInteractableBox;

	// Token: 0x04000458 RID: 1112
	[SerializeField]
	private MeshCollider waterInteractableMesh;

	// Token: 0x04000459 RID: 1113
	[SerializeField]
	private Animator waterAnim;

	// Token: 0x0400045A RID: 1114
	[SerializeField]
	private bool turnedOn;

	// Token: 0x0400045B RID: 1115
	public GameObject faucetHandle;

	// Token: 0x0400045C RID: 1116
	public Animator animator;

	// Token: 0x0400045D RID: 1117
	public InteractableObj interactableObj;

	// Token: 0x0400045E RID: 1118
	public AudioClip magicOn;

	// Token: 0x0400045F RID: 1119
	public AudioClip faucetOn;

	// Token: 0x04000460 RID: 1120
	public AudioClip faucetLoop;

	// Token: 0x04000461 RID: 1121
	public AudioClip faucetOff;

	// Token: 0x04000462 RID: 1122
	public Vector3 FaucetOn;

	// Token: 0x04000463 RID: 1123
	public Vector3 FaucetOff;

	// Token: 0x04000464 RID: 1124
	private float speed = 0.4f;

	// Token: 0x04000465 RID: 1125
	[SerializeField]
	private bool isBathsheba;
}
