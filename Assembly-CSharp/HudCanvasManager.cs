using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class HudCanvasManager : MonoBehaviour, IReloadHandler
{
	// Token: 0x06000941 RID: 2369 RVA: 0x00035EE8 File Offset: 0x000340E8
	private void Awake()
	{
		HudCanvasManager.Instance = this;
		this.EnablePhoneHUDElements();
		this._equippedHud.SetActive(false);
		if (this.animator == null)
		{
			this.animator = base.GetComponent<Animator>();
		}
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x00035F1C File Offset: 0x0003411C
	public void Start()
	{
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00035F1E File Offset: 0x0003411E
	public void SetDateviatorsOff()
	{
		this.animator.SetBool("DateviatorsOff", true);
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00035F31 File Offset: 0x00034131
	public void SetDateviatorsOn()
	{
		this.animator.SetBool("DateviatorsOff", false);
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00035F44 File Offset: 0x00034144
	public void ForceSlideOut()
	{
		this.animator.Play("HudSlideOutGlassesOff");
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x00035F58 File Offset: 0x00034158
	public void Update()
	{
		bool flag = !Singleton<PhoneManager>.Instance.IsPhoneMenuOpened();
		if (flag != this.inNormalUIMode)
		{
			if (flag)
			{
				this.EnableNormalHUDElements();
				return;
			}
			this.EnablePhoneHUDElements();
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00035F8C File Offset: 0x0003418C
	private void EnablePhoneHUDElements()
	{
		this.SetActiveElements(this._normalHUDUIElements, false);
		this.SetActiveElements(this._phoneHUDUIElements, true);
		this.inNormalUIMode = false;
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x00035FAF File Offset: 0x000341AF
	private void EnableNormalHUDElements()
	{
		this.SetActiveElements(this._phoneHUDUIElements, false);
		this.SetActiveElements(this._normalHUDUIElements, true);
		this.inNormalUIMode = true;
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x00035FD2 File Offset: 0x000341D2
	public void UpdateTime()
	{
		this.timebar.UpdateCurrentDayPhase();
		this.timebar2.UpdateCurrentDayPhase();
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x00035FEC File Offset: 0x000341EC
	private void SetActiveElements(GameObject[] gameObjects, bool active)
	{
		for (int i = 0; i < gameObjects.Length; i++)
		{
			if (!(gameObjects[i] == null))
			{
				gameObjects[i].SetActive(active);
			}
		}
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0003601B File Offset: 0x0003421B
	public void GlassesAcquired()
	{
		this.GlassesAcquiredInternal();
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x00036023 File Offset: 0x00034223
	private void GlassesAcquiredInternal()
	{
		this.GlassesIcon.SetActive(true);
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00036031 File Offset: 0x00034231
	public int Priority()
	{
		return 2000;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x00036038 File Offset: 0x00034238
	private bool HasDateviatorAccess()
	{
		return Singleton<Save>.Instance.GetDateStatus("skylar_specs") > RelationshipStatus.Unmet;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0003604C File Offset: 0x0003424C
	public void LoadState()
	{
		if (this.HasDateviatorAccess())
		{
			this.GlassesIcon.SetActive(true);
			return;
		}
		this.GlassesIcon.SetActive(false);
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0003606F File Offset: 0x0003426F
	public void SaveState()
	{
	}

	// Token: 0x04000885 RID: 2181
	public static HudCanvasManager Instance;

	// Token: 0x04000886 RID: 2182
	public GameObject GlassesIcon;

	// Token: 0x04000887 RID: 2183
	[SerializeField]
	private GameObject[] _normalHUDUIElements = new GameObject[0];

	// Token: 0x04000888 RID: 2184
	[SerializeField]
	private GameObject[] _phoneHUDUIElements = new GameObject[0];

	// Token: 0x04000889 RID: 2185
	[SerializeField]
	private GameObject _unequippedHud;

	// Token: 0x0400088A RID: 2186
	[SerializeField]
	private GameObject _equippedHud;

	// Token: 0x0400088B RID: 2187
	[SerializeField]
	private Animator animator;

	// Token: 0x0400088C RID: 2188
	private bool inNormalUIMode = true;

	// Token: 0x0400088D RID: 2189
	[SerializeField]
	private Timebar timebar;

	// Token: 0x0400088E RID: 2190
	[SerializeField]
	private Timebar timebar2;
}
