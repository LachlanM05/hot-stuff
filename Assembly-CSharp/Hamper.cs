using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000087 RID: 135
public class Hamper : MonoBehaviour
{
	// Token: 0x060004BE RID: 1214 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
	private void Start()
	{
		if (Singleton<Save>.Instance.GetDateStatus(this.Harper.internalCharacterName) != RelationshipStatus.Unmet)
		{
			this.RemoveLid();
			return;
		}
		this.Harper.DialogueEvent.AddListener(new UnityAction(this.InvokeRemoveLid));
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0001CD04 File Offset: 0x0001AF04
	private void InvokeRemoveLid()
	{
		base.Invoke("RemoveLid", 2f);
		this.Harper.DialogueEvent.RemoveListener(new UnityAction(this.InvokeRemoveLid));
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0001CD32 File Offset: 0x0001AF32
	private void RemoveLid()
	{
		this.DirtyLaundry.SetActive(true);
		this.HamperLid.transform.localPosition = this.HamperToPos;
		this.HamperLid.transform.localEulerAngles = this.HamperToRot;
	}

	// Token: 0x040004B3 RID: 1203
	public GameObject HamperLid;

	// Token: 0x040004B4 RID: 1204
	public GameObject DirtyLaundry;

	// Token: 0x040004B5 RID: 1205
	public InteractableObj Harper;

	// Token: 0x040004B6 RID: 1206
	public Vector3 HamperToPos;

	// Token: 0x040004B7 RID: 1207
	public Vector3 HamperToRot;
}
