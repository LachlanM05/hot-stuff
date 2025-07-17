using System;
using System.Collections;
using UnityEngine;

namespace Date_Everything.Animation.UI
{
	// Token: 0x02000270 RID: 624
	public class UIAnimatorBlockHelper : MonoBehaviour
	{
		// Token: 0x06001428 RID: 5160 RVA: 0x00060C6F File Offset: 0x0005EE6F
		public void QueueRemoveBlocker(float delayInSeconds)
		{
			if (this._isRemovingCoroutineRunning)
			{
				return;
			}
			this._removeBlockerCoroutine = base.StartCoroutine(this.RemoveBlockerWithDelay(delayInSeconds));
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00060C8D File Offset: 0x0005EE8D
		public void RemoveOngoingQueuedRequest()
		{
			if (this._isRemovingCoroutineRunning)
			{
				base.StopCoroutine(this._removeBlockerCoroutine);
				this._removeBlockerCoroutine = null;
				this._isRemovingCoroutineRunning = false;
			}
			this._isRemovingCoroutineRunning = false;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00060CB8 File Offset: 0x0005EEB8
		private IEnumerator RemoveBlockerWithDelay(float delayInSeconds)
		{
			this._isRemovingCoroutineRunning = true;
			yield return new WaitForSeconds(delayInSeconds);
			this.RemoveBlocker();
			this._isRemovingCoroutineRunning = false;
			yield break;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00060CCE File Offset: 0x0005EECE
		private void RemoveBlocker()
		{
			this.RemoveOngoingQueuedRequest();
			InteractionBlockAnimatorBehaviour.RemoveBlocker(base.GetComponent<Animator>());
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00060CE1 File Offset: 0x0005EEE1
		private void OnDestroy()
		{
			if (this._isRemovingCoroutineRunning)
			{
				this.RemoveBlocker();
			}
		}

		// Token: 0x04000F81 RID: 3969
		private bool _isRemovingCoroutineRunning;

		// Token: 0x04000F82 RID: 3970
		private Coroutine _removeBlockerCoroutine;
	}
}
