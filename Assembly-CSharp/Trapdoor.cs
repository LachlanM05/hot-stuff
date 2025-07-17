using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class Trapdoor : MonoBehaviour
{
	// Token: 0x06000534 RID: 1332 RVA: 0x0001EBA7 File Offset: 0x0001CDA7
	public void Open()
	{
		Singleton<InkController>.Instance.story.variablesState["trap_door_opened"] = true;
		this.rug.Interact(false, false);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x0001EBD5 File Offset: 0x0001CDD5
	private void LateUpdate()
	{
	}

	// Token: 0x0400051F RID: 1311
	[SerializeField]
	private MeshFilter mesh;

	// Token: 0x04000520 RID: 1312
	[SerializeField]
	private GenericInteractable rug;
}
