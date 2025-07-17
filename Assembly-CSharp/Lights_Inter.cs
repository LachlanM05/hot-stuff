using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200008B RID: 139
public class Lights_Inter : Interactable
{
	// Token: 0x060004D3 RID: 1235 RVA: 0x0001D18A File Offset: 0x0001B38A
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x0001D194 File Offset: 0x0001B394
	public void Initialize()
	{
		if (!this.initialized)
		{
			this.lightOn.SetActive(this.lightsOn);
			this.lightOff.SetActive(!this.lightsOn);
			this.Name = (this.lightsOn ? ("Turn " + this.type + " Off") : ("Turn " + this.type + " On"));
			this.FlipSwitch(this.lightsOn, false);
			if (this.LightUpdated == null)
			{
				this.LightUpdated = new UnityEvent();
			}
			this.initialized = true;
		}
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x0001D232 File Offset: 0x0001B432
	public void Start()
	{
		this.loadingIn = true;
		this.Initialize();
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0001D244 File Offset: 0x0001B444
	public override void Interact()
	{
		if (!Singleton<Dateviators>.Instance.Equipped)
		{
			this.Interact(false);
			return;
		}
		if (Singleton<Save>.Instance.GetDateStatus("lux") == RelationshipStatus.Friend)
		{
			string text = Path.Combine("VoiceOver", "magical_friend", "lux");
			Singleton<AudioManager>.Instance.PlayTrack(text, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, true);
			return;
		}
		if (Singleton<Save>.Instance.GetDateStatus("lux") == RelationshipStatus.Love)
		{
			string text2 = Path.Combine("VoiceOver", "magical_love", "lux");
			Singleton<AudioManager>.Instance.PlayTrack(text2, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, true);
		}
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x0001D304 File Offset: 0x0001B504
	public void Interact(bool silently)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.lightsOn = !this.lightsOn;
		this.lightOn.SetActive(this.lightsOn);
		this.lightOff.SetActive(!this.lightsOn);
		this.Name = (this.lightsOn ? ("Turn " + this.type + " Off") : ("Turn " + this.type + " On"));
		this.FlipSwitch(this.lightsOn, silently);
		this.LightUpdated.Invoke();
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0001D3A8 File Offset: 0x0001B5A8
	public void Interact(bool state, bool silently)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.lightsOn = state;
		this.lightOn.SetActive(state);
		this.lightOff.SetActive(!state);
		this.Name = (this.lightsOn ? ("Turn " + this.type + " Off") : ("Turn " + this.type + " On"));
		this.FlipSwitch(this.lightsOn, silently);
		this.LightUpdated.Invoke();
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0001D438 File Offset: 0x0001B638
	private void FlipSwitch(bool onOff, bool silentFlip = false)
	{
		if (this.animator)
		{
			this.animator.ResetTrigger("animReset");
			this.animator.ResetTrigger("standardAnimStart");
		}
		if (this.lightSwitch != null)
		{
			if (Application.isPlaying && !silentFlip)
			{
				if (this.startAudioClips != null && this.startAudioClips.Count > 0 && !this.loadingIn)
				{
					int num = Random.Range(0, this.startAudioClips.Count);
					Singleton<AudioManager>.Instance.StopTrack(this.startAudioClips[num].name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(this.startAudioClips[num], AUDIO_TYPE.SFX, false, false, 0f, false, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
				}
				else if (!this.loadingIn)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.light_switch_standard.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.light_switch_standard, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
				}
			}
			if (this.scaleAnimate && !this.animator)
			{
				int num2 = (this.lightsOn ? 180 : 0);
				this.lightSwitch.transform.localEulerAngles = new Vector3(this.lightSwitch.transform.localEulerAngles.x, this.lightSwitch.transform.transform.localEulerAngles.y, (float)num2);
			}
			if (this.animator)
			{
				if (this.lightsOn)
				{
					this.animator.SetTrigger("animReset");
					this.animator.SetTrigger("standardAnimStart");
				}
				else
				{
					this.animator.SetTrigger("standardAnimStart");
				}
			}
			this.loadingIn = false;
		}
	}

	// Token: 0x040004CD RID: 1229
	public string type = "Lights";

	// Token: 0x040004CE RID: 1230
	public GameObject lightOn;

	// Token: 0x040004CF RID: 1231
	public GameObject lightOff;

	// Token: 0x040004D0 RID: 1232
	public bool scaleAnimate = true;

	// Token: 0x040004D1 RID: 1233
	public Animator animator;

	// Token: 0x040004D2 RID: 1234
	public bool lightsOn;

	// Token: 0x040004D3 RID: 1235
	public GameObject lightSwitch;

	// Token: 0x040004D4 RID: 1236
	private Vector3 onRot = new Vector3(-40f, 0f, 0f);

	// Token: 0x040004D5 RID: 1237
	private Vector3 offRot = new Vector3(0f, 0f, 0f);

	// Token: 0x040004D6 RID: 1238
	public UnityEvent LightUpdated;

	// Token: 0x040004D7 RID: 1239
	private bool initialized;

	// Token: 0x040004D8 RID: 1240
	public List<AudioClip> startAudioClips;

	// Token: 0x040004D9 RID: 1241
	public List<AudioClip> endAudioClips;
}
