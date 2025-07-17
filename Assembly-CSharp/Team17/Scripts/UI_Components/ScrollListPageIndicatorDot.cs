using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x0200020F RID: 527
	[RequireComponent(typeof(Image))]
	public class ScrollListPageIndicatorDot : MonoBehaviour
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x0005790C File Offset: 0x00055B0C
		private void OnValidate()
		{
			this._image = base.GetComponent<Image>();
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0005791C File Offset: 0x00055B1C
		public void OnLeftThisPage()
		{
			Tween bobTween = this._bobTween;
			if (bobTween != null)
			{
				bobTween.Kill(false);
			}
			this._image.sprite = this._pageClosedSprite;
			this._bobTween = base.transform.DOScale(1f, this._bobDuration / 2f).SetEase(Ease.OutQuad);
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00057974 File Offset: 0x00055B74
		public void OnOpenedThisPage()
		{
			Tween bobTween = this._bobTween;
			if (bobTween != null)
			{
				bobTween.Kill(false);
			}
			this._image.sprite = this._pageOpenSprite;
			this._bobTween = base.transform.DOScale(this._bobScale, this._bobDuration / 2f).SetEase(Ease.OutQuad).OnComplete(delegate
			{
				base.transform.DOScale(this._openScale, this._bobDuration / 2f).SetEase(Ease.InQuad);
			});
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000579DE File Offset: 0x00055BDE
		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x000579EC File Offset: 0x00055BEC
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000E43 RID: 3651
		private Tween _bobTween;

		// Token: 0x04000E44 RID: 3652
		[SerializeField]
		private float _bobScale = 1.5f;

		// Token: 0x04000E45 RID: 3653
		[SerializeField]
		private float _bobDuration = 0.5f;

		// Token: 0x04000E46 RID: 3654
		[SerializeField]
		private float _openScale = 1.25f;

		// Token: 0x04000E47 RID: 3655
		[SerializeField]
		private Image _image;

		// Token: 0x04000E48 RID: 3656
		[SerializeField]
		private Sprite _pageOpenSprite;

		// Token: 0x04000E49 RID: 3657
		[SerializeField]
		private Sprite _pageClosedSprite;
	}
}
