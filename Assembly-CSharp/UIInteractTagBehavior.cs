using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class UIInteractTagBehavior : MonoBehaviour
{
	// Token: 0x06000BEE RID: 3054 RVA: 0x00045084 File Offset: 0x00043284
	public void SetText(string newText)
	{
		if (newText != this._text && newText != "")
		{
			if (this._text == "")
			{
				this.anim.SetBool("Hidden", false);
			}
			this._text = newText;
			this.labelText.text = this._text;
			this.anim.SetTrigger("Bump");
			return;
		}
		if (newText == "")
		{
			this._text = "";
			this.anim.SetBool("Hidden", true);
		}
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00045124 File Offset: 0x00043324
	public void SetButtonDown(bool flag, bool triggered = false)
	{
		if (flag)
		{
			this.anim.SetBool("ButtonDown", true);
			return;
		}
		if (triggered)
		{
			this.anim.SetTrigger("TransitionTriggered");
			this.anim.SetBool("ButtonDown", true);
			base.StartCoroutine(this.ButtonWasPressed());
			return;
		}
		this.anim.SetBool("ButtonDown", false);
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00045189 File Offset: 0x00043389
	private IEnumerator ButtonWasPressed()
	{
		yield return new WaitForEndOfFrame();
		this.anim.SetBool("ButtonDown", false);
		yield break;
	}

	// Token: 0x04000AA8 RID: 2728
	[SerializeField]
	private Animator anim;

	// Token: 0x04000AA9 RID: 2729
	[SerializeField]
	private TextMeshProUGUI labelText;

	// Token: 0x04000AAA RID: 2730
	private string _text;
}
