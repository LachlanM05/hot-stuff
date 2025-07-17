using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class CanopyInfoUpdater : MonoBehaviour
{
	// Token: 0x06000C5D RID: 3165 RVA: 0x00046C29 File Offset: 0x00044E29
	public void Start()
	{
		this.UpdateInfo();
		this.initialized = true;
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00046C38 File Offset: 0x00044E38
	public void UpdateInfo()
	{
		float num = this.currentrefunded;
		float num2 = this.currentrating;
		this.currentrefunded = Singleton<InkController>.Instance.story.variablesState.GetFloatVariableByName("totalRefundGranted");
		if (num < this.currentrefunded && Singleton<AudioManager>.Instance != null && this.initialized)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_refund, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		this.currentrating = Mathf.Clamp(Singleton<InkController>.Instance.story.variablesState.GetFloatVariableByName("star"), 0f, 5f);
		if (num2 < this.currentrating)
		{
			if (Singleton<AudioManager>.Instance != null && this.initialized)
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_star_increase, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
		}
		else if (num2 > this.currentrating && Singleton<AudioManager>.Instance != null && this.initialized)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_star_decrease, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		Singleton<InkController>.Instance.story.variablesState["star"] = this.currentrating;
		if (Math.Abs(num - this.currentrefunded) > 0.05f)
		{
			base.StartCoroutine(this.Nicebuxanim(num, this.currentrefunded, 0.25f));
		}
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x00046DBF File Offset: 0x00044FBF
	private IEnumerator Nicebuxanim(float oldvalue, float newvalue, float speed)
	{
		for (float i = 0f; i < speed; i += Time.deltaTime)
		{
			Mathf.Lerp(oldvalue, newvalue, i * (1f / speed));
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04000B10 RID: 2832
	public float currentrefunded;

	// Token: 0x04000B11 RID: 2833
	public float currentrating;

	// Token: 0x04000B12 RID: 2834
	private bool initialized;
}
