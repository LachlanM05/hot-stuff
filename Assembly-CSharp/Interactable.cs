using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
public abstract class Interactable : MonoBehaviour
{
	// Token: 0x06000378 RID: 888
	public abstract string noderequired();

	// Token: 0x06000379 RID: 889 RVA: 0x0001640B File Offset: 0x0001460B
	public virtual void Awake()
	{
		this.startscale = base.transform.localScale;
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00016420 File Offset: 0x00014620
	public virtual bool CheckCanUse()
	{
		return (this.noderequired() == "" && !this.stateLock) || (Singleton<InkController>.Instance.story.state.VisitCountAtPathString(this.noderequired()) > 0 && !this.stateLock);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00016471 File Offset: 0x00014671
	public virtual void ToggleInteractedWith(Vector3 positionWhenInteracting, bool loading = false)
	{
		this.interactedWithState = !this.interactedWithState;
		this.interactedPosition = positionWhenInteracting;
	}

	// Token: 0x0600037C RID: 892
	public abstract void Interact();

	// Token: 0x0600037D RID: 893 RVA: 0x00016489 File Offset: 0x00014689
	public virtual void OnLoadNoInteract()
	{
	}

	// Token: 0x04000372 RID: 882
	[Header("Interactable")]
	public string Name;

	// Token: 0x04000373 RID: 883
	public bool interactedWithState;

	// Token: 0x04000374 RID: 884
	public bool stateLock;

	// Token: 0x04000375 RID: 885
	public Vector3 interactedPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x04000376 RID: 886
	public bool loadingIn;

	// Token: 0x04000377 RID: 887
	[HideInInspector]
	public Vector3 startscale = Vector3.one;
}
