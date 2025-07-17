using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class WashfordDrysdale : MonoBehaviour
{
	// Token: 0x0600054E RID: 1358 RVA: 0x0001F2B4 File Offset: 0x0001D4B4
	public void CheckForShipped()
	{
		if (!Singleton<Dateviators>.Instance.Equipped)
		{
			return;
		}
		bool flag;
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("washford_drysdale_shipped"), out flag);
		if (flag)
		{
			this._interactable.blockMagical = false;
			this._interactable.allowMagicalLines = true;
			this._washerInteractable.allowMagicalLines = false;
			this._dryerInteractable.allowMagicalLines = false;
			this._interactable.Interact(true, false);
			return;
		}
		this._interactable.blockMagical = true;
		this._interactable.allowMagicalLines = false;
		this._washerInteractable.allowMagicalLines = true;
		this._dryerInteractable.allowMagicalLines = true;
	}

	// Token: 0x04000532 RID: 1330
	[SerializeField]
	private GenericInteractable _interactable;

	// Token: 0x04000533 RID: 1331
	[SerializeField]
	private GenericInteractable _washerInteractable;

	// Token: 0x04000534 RID: 1332
	[SerializeField]
	private GenericInteractable _dryerInteractable;
}
