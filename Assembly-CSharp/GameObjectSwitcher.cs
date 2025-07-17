using System;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class GameObjectSwitcher : MonoBehaviour
{
	// Token: 0x06000CF6 RID: 3318 RVA: 0x0004AD75 File Offset: 0x00048F75
	private void Start()
	{
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x0004AD78 File Offset: 0x00048F78
	private void Update()
	{
		if (this.standardCleanModel != null && this.magicalCleanModel != null)
		{
			if (Singleton<Dateviators>.Instance.Equipped)
			{
				if (this.standardCleanModel.activeInHierarchy && !this.magicalCleanModel.activeInHierarchy)
				{
					this.standardCleanModel.SetActive(false);
					this.magicalCleanModel.SetActive(true);
				}
			}
			else if (!this.standardCleanModel.activeInHierarchy && this.magicalCleanModel.activeInHierarchy)
			{
				this.standardCleanModel.SetActive(true);
				this.magicalCleanModel.SetActive(false);
			}
		}
		if (this.standardCleanModel != null && this.standardMessyModel != null)
		{
			if (this.objectLinked != null && !this.objectLinked.isClean)
			{
				if (this.standardCleanModel.activeInHierarchy && !this.standardMessyModel.activeInHierarchy)
				{
					this.standardCleanModel.SetActive(false);
					this.standardMessyModel.SetActive(true);
					return;
				}
			}
			else if (!this.standardCleanModel.activeInHierarchy && this.standardMessyModel.activeInHierarchy)
			{
				this.standardCleanModel.SetActive(true);
				this.standardMessyModel.SetActive(false);
			}
		}
	}

	// Token: 0x04000BBE RID: 3006
	public GameObject magicalMessyModel;

	// Token: 0x04000BBF RID: 3007
	public GameObject standardMessyModel;

	// Token: 0x04000BC0 RID: 3008
	public GameObject magicalCleanModel;

	// Token: 0x04000BC1 RID: 3009
	public GameObject standardCleanModel;

	// Token: 0x04000BC2 RID: 3010
	public InteractableObj objectLinked;
}
