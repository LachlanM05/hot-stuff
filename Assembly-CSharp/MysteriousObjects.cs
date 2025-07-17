using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008E RID: 142
public class MysteriousObjects : MonoBehaviour
{
	// Token: 0x060004E6 RID: 1254 RVA: 0x0001D82A File Offset: 0x0001BA2A
	private void Start()
	{
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x0001D82C File Offset: 0x0001BA2C
	private void Update()
	{
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0001D830 File Offset: 0x0001BA30
	public void StartAnimTrigger()
	{
		this.nightBlackScreenTransition.SetActive(true);
		this.blackNightImage = this.nightBlackScreenTransition.GetComponent<Image>();
		this.transparentColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 0f);
		this.solidColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 1f);
		this.blackNightImage.color = this.transparentColor;
		this.interactable.gameObject.isStatic = false;
		CinematicBars.Show(-1f);
		base.StartCoroutine(this.StartAnim());
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0001D913 File Offset: 0x0001BB13
	public IEnumerator StartAnim()
	{
		Singleton<AudioManager>.Instance.PlayTrack(this.interactable.magicalSfx_deactivate[0], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		yield return new WaitForSeconds(1.5f);
		DOTween.Sequence().Append(this.blackNightImage.DOBlendableColor(this.solidColor, 0.5f)).SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				base.StartCoroutine(this.MidAnim());
			});
		yield break;
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x0001D922 File Offset: 0x0001BB22
	public IEnumerator MidAnim()
	{
		yield return new WaitForSeconds(2.5f);
		this.EndAnim();
		yield break;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0001D934 File Offset: 0x0001BB34
	public void EndAnim()
	{
		this.blackNightImage.color = this.solidColor;
		DOTween.Sequence().Append(this.blackNightImage.DOBlendableColor(this.transparentColor, 0.5f)).OnComplete(delegate
		{
			this.nightBlackScreenTransition.SetActive(false);
			CinematicBars.Hide(-1f);
		});
	}

	// Token: 0x040004E2 RID: 1250
	private Color transparentColor;

	// Token: 0x040004E3 RID: 1251
	private Color solidColor;

	// Token: 0x040004E4 RID: 1252
	private Image blackNightImage;

	// Token: 0x040004E5 RID: 1253
	public GameObject nightBlackScreenTransition;

	// Token: 0x040004E6 RID: 1254
	public GenericInteractable interactable;
}
