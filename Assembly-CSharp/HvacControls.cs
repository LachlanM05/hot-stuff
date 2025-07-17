using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

// Token: 0x02000089 RID: 137
public class HvacControls : Interactable
{
	// Token: 0x060004C7 RID: 1223 RVA: 0x0001CDE5 File Offset: 0x0001AFE5
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001CDEC File Offset: 0x0001AFEC
	private void Start()
	{
		this.lastIndex = -1;
		this.vents = GameObject.FindGameObjectsWithTag("AirVent");
		this.UpdateTemperature(this.Temperature);
		for (int i = 0; i < this.vents.Length; i++)
		{
			this.vents[i].GetComponentInChildren<VisualEffect>(true).gameObject.SetActive(false);
		}
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0001CE48 File Offset: 0x0001B048
	private void GetVents()
	{
		this.vents = GameObject.FindGameObjectsWithTag("AirVent");
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0001CE5C File Offset: 0x0001B05C
	public override void Interact()
	{
		if (Singleton<Dateviators>.Instance.Equipped)
		{
			return;
		}
		switch (this.Temperature)
		{
		case 0:
			base.StopAllCoroutines();
			this.TurnOffLastVent();
			this.UpdateTemperature(1);
			Singleton<AudioManager>.Instance.PlayTrack(this.controlsOff, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			return;
		case 1:
			base.StartCoroutine(this.TriggerVent());
			this.UpdateTemperature(0);
			Singleton<AudioManager>.Instance.PlayTrack(this.controlsOn, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			return;
		case 2:
			this.UpdateTemperature(0);
			return;
		default:
			return;
		}
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0001CF10 File Offset: 0x0001B110
	private void TurnOffLastVent()
	{
		if (this.lastIndex == -1)
		{
			return;
		}
		Singleton<AudioManager>.Instance.StopTrack(this.ventSound.name, 0.1f);
		Singleton<AudioManager>.Instance.StopTrack(this.hvacLoopSound.name, 0.1f);
		Singleton<AudioManager>.Instance.PlayTrack(this.hvacEndSound, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, this.hvac, false, SFX_SUBGROUP.FOLEY, false);
		Singleton<AudioManager>.Instance.PlayTrack(this.ventEndSound, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, this.vents[this.lastIndex], false, SFX_SUBGROUP.FOLEY, false);
		for (int i = 0; i < this.vents.Length; i++)
		{
			this.vents[i].GetComponentInChildren<VisualEffect>(true).gameObject.SetActive(false);
		}
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x0001CFDC File Offset: 0x0001B1DC
	private IEnumerator TriggerVent()
	{
		if (this.vents == null)
		{
			this.GetVents();
		}
		this.TurnOffLastVent();
		yield return null;
		int num = Random.Range(0, this.vents.Length);
		this.lastIndex = num;
		VisualEffect componentInChildren = this.vents[this.lastIndex].GetComponentInChildren<VisualEffect>(true);
		componentInChildren.transform.GetChild(0).gameObject.SetActive(true);
		componentInChildren.gameObject.SetActive(true);
		componentInChildren.Play();
		Singleton<AudioManager>.Instance.PlayTrack(this.hvacStartSound, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, this.hvac, false, SFX_SUBGROUP.FOLEY, false);
		Singleton<AudioManager>.Instance.PlayTrack(this.hvacLoopSound, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, this.hvac, true, SFX_SUBGROUP.FOLEY, false);
		Singleton<AudioManager>.Instance.PlayTrack(this.ventStartSound, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, this.vents[this.lastIndex], false, SFX_SUBGROUP.FOLEY, false);
		Singleton<AudioManager>.Instance.PlayTrack(this.ventSound, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, this.vents[this.lastIndex], true, SFX_SUBGROUP.FOLEY, false);
		yield return null;
		base.StartCoroutine(this.NextVent());
		yield break;
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0001CFEB File Offset: 0x0001B1EB
	private IEnumerator NextVent()
	{
		yield return new WaitForSeconds(40f);
		this.TurnOffLastVent();
		yield return new WaitForSeconds(60f);
		base.StartCoroutine(this.TriggerVent());
		yield break;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0001CFFC File Offset: 0x0001B1FC
	public void UpdateTemperature(int temp)
	{
		this.Temperature = temp;
		switch (temp)
		{
		case 0:
			this.temperatureMesh.material.color = this.ColdColor;
			this.temperatureMesh.material.SetColor("_Emission", this.ColdColor);
			this.icon.gameObject.SetActive(true);
			this.icon.sprite = this.coldIconSprite;
			break;
		case 1:
			this.temperatureMesh.material.color = this.RoomTempColor;
			this.temperatureMesh.material.SetColor("_Emission", this.RoomTempColor);
			this.icon.gameObject.SetActive(false);
			break;
		case 2:
			this.temperatureMesh.material.color = this.HotColor;
			this.temperatureMesh.material.SetColor("_Emission", this.HotColor);
			this.icon.gameObject.SetActive(true);
			this.icon.sprite = this.hotIconSprite;
			break;
		}
		this.TemperatureUpdate.Invoke();
	}

	// Token: 0x040004B8 RID: 1208
	public int Temperature = 1;

	// Token: 0x040004B9 RID: 1209
	public UnityEvent TemperatureUpdate;

	// Token: 0x040004BA RID: 1210
	public Color ColdColor;

	// Token: 0x040004BB RID: 1211
	public Color RoomTempColor;

	// Token: 0x040004BC RID: 1212
	public Color HotColor;

	// Token: 0x040004BD RID: 1213
	public MeshRenderer temperatureMesh;

	// Token: 0x040004BE RID: 1214
	[SerializeField]
	private int lastIndex = -1;

	// Token: 0x040004BF RID: 1215
	[SerializeField]
	private GameObject[] vents;

	// Token: 0x040004C0 RID: 1216
	[SerializeField]
	private SpriteRenderer icon;

	// Token: 0x040004C1 RID: 1217
	[SerializeField]
	private Sprite hotIconSprite;

	// Token: 0x040004C2 RID: 1218
	[SerializeField]
	private Sprite coldIconSprite;

	// Token: 0x040004C3 RID: 1219
	[SerializeField]
	private AudioClip controlsOn;

	// Token: 0x040004C4 RID: 1220
	[SerializeField]
	private AudioClip controlsOff;

	// Token: 0x040004C5 RID: 1221
	[SerializeField]
	private AudioClip ventStartSound;

	// Token: 0x040004C6 RID: 1222
	[SerializeField]
	private AudioClip ventSound;

	// Token: 0x040004C7 RID: 1223
	[SerializeField]
	private AudioClip ventEndSound;

	// Token: 0x040004C8 RID: 1224
	[SerializeField]
	private AudioClip hvacStartSound;

	// Token: 0x040004C9 RID: 1225
	[SerializeField]
	private AudioClip hvacLoopSound;

	// Token: 0x040004CA RID: 1226
	[SerializeField]
	private AudioClip hvacEndSound;

	// Token: 0x040004CB RID: 1227
	[SerializeField]
	private GameObject hvac;
}
