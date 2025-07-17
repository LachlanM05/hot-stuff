using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200007E RID: 126
public class Dresser : MonoBehaviour, IReloadHandler
{
	// Token: 0x0600043F RID: 1087 RVA: 0x0001A319 File Offset: 0x00018519
	private void Awake()
	{
		Dresser.Instance = this;
		Singleton<DayNightCycle>.Instance.DayPhaseUpdated.AddListener(new UnityAction(this.UpdateGlitch));
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x0001A33C File Offset: 0x0001853C
	private void Start()
	{
		this.UpdateGlitch();
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0001A344 File Offset: 0x00018544
	private void LateUpdate()
	{
		this.renderer.material.SetVector("_Glitch_Range", new Vector2(0f, this.glitchRange));
		this.renderer.material.SetFloat("_Glitch_Alpha_Clip", this.glitchAlpha);
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0001A396 File Offset: 0x00018596
	public void IncreaseGlitch()
	{
		this.UpdateGlitch();
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x0001A3A0 File Offset: 0x000185A0
	public void EnableCollision()
	{
		this.meshCollider.isTrigger = true;
		Collider[] array = this.meshColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x0001A3D8 File Offset: 0x000185D8
	public void DisableCollision()
	{
		this.meshCollider.isTrigger = false;
		Collider[] array = this.meshColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0001A410 File Offset: 0x00018610
	public void UpdateGlitch()
	{
		bool flag;
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("daemon_glitch_enabled"), out flag);
		bool flag2;
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("leave_house_hint"), out flag2);
		if (flag2)
		{
			this.DisableCollision();
		}
		if (flag)
		{
			this.disabled = false;
		}
		else
		{
			this.disabled = true;
		}
		int daysSinceStart = Singleton<DayNightCycle>.Instance.DaysSinceStart;
		this.glitchRange = this.glitchIncrease + (float)daysSinceStart * this.glitchIncrease;
		if (this.glitchRange > this.glitchMax)
		{
			this.glitchRange = this.glitchMax;
		}
		if (Singleton<Save>.Instance.GetDateStatusRealized("daemon") == RelationshipStatus.Realized || this.disabled)
		{
			this.glitchRange = 0f;
		}
		if (this.glitchRange == 0f)
		{
			this.glitchAlpha = 0f;
		}
		else
		{
			this.glitchAlpha = this.glitchRange / this.glitchMax;
			this.glitchRange = 0.9f;
		}
		this.renderer.material.SetVector("_Glitch_Range", new Vector2(0f, this.glitchRange));
		this.renderer.material.SetFloat("_Glitch_Alpha_Clip", this.glitchAlpha);
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0001A544 File Offset: 0x00018744
	public void Outside()
	{
		if (!Singleton<Save>.Instance.GetTutorialFinished())
		{
			return;
		}
		this.frontDoor.gameObject.SetActive(false);
		this.backDoor.gameObject.SetActive(false);
		this.frontDoorTrigger.gameObject.SetActive(true);
		this.backDoorTrigger.gameObject.SetActive(true);
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0001A5A4 File Offset: 0x000187A4
	public void Inside()
	{
		if (!Singleton<Save>.Instance.GetTutorialFinished())
		{
			return;
		}
		this.frontDoor.gameObject.SetActive(true);
		this.backDoor.gameObject.SetActive(true);
		this.frontDoorTrigger.gameObject.SetActive(false);
		this.backDoorTrigger.gameObject.SetActive(false);
		BetterPlayerControl.Instance.ResetSpeed();
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x0001A60C File Offset: 0x0001880C
	public void LoadState()
	{
		this.UpdateGlitch();
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x0001A614 File Offset: 0x00018814
	public void SaveState()
	{
		Singleton<Save>.Instance.SetDaemonGlitchStrength(this.glitchRange);
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0001A626 File Offset: 0x00018826
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x04000443 RID: 1091
	public static Dresser Instance;

	// Token: 0x04000444 RID: 1092
	public float glitchRange;

	// Token: 0x04000445 RID: 1093
	private float glitchIncrease = 0.25f;

	// Token: 0x04000446 RID: 1094
	private float glitchMax = 1.5f;

	// Token: 0x04000447 RID: 1095
	private float glitchAlpha;

	// Token: 0x04000448 RID: 1096
	public bool disabled = true;

	// Token: 0x04000449 RID: 1097
	[SerializeField]
	private Renderer renderer;

	// Token: 0x0400044A RID: 1098
	[SerializeField]
	private MeshCollider meshCollider;

	// Token: 0x0400044B RID: 1099
	[SerializeField]
	private Collider[] meshColliders;

	// Token: 0x0400044C RID: 1100
	[SerializeField]
	private GameObject frontDoor;

	// Token: 0x0400044D RID: 1101
	[SerializeField]
	private GameObject backDoor;

	// Token: 0x0400044E RID: 1102
	[SerializeField]
	private HouseReenterTrigger frontDoorTrigger;

	// Token: 0x0400044F RID: 1103
	[SerializeField]
	private HouseReenterTrigger backDoorTrigger;
}
