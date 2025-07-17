using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AC RID: 428
public class NavigationOverride : MonoBehaviour
{
	// Token: 0x06000E86 RID: 3718 RVA: 0x0004FE34 File Offset: 0x0004E034
	private void Start()
	{
		if (this._target == null && (this._target = base.GetComponent<Selectable>()) == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x0004FE6D File Offset: 0x0004E06D
	private void OnEnable()
	{
		this.updateNavigation = true;
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x0004FE78 File Offset: 0x0004E078
	private void Update()
	{
		if (this.updateNavigation && this._target != null)
		{
			Navigation navigation = this._target.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnDown = this.GetValidSelectable(ref this._down);
			navigation.selectOnUp = this.GetValidSelectable(ref this._up);
			navigation.selectOnLeft = this.GetValidSelectable(ref this._left);
			navigation.selectOnRight = this.GetValidSelectable(ref this._right);
			this._target.navigation = navigation;
		}
		this.updateNavigation = false;
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x0004FF10 File Offset: 0x0004E110
	private Selectable GetValidSelectable(ref Selectable[] selectables)
	{
		int i = 0;
		int num = selectables.Length;
		while (i < num)
		{
			Selectable selectable = selectables[i];
			if (selectable != null && selectable.IsInteractable())
			{
				return selectable;
			}
			i++;
		}
		return null;
	}

	// Token: 0x04000CE7 RID: 3303
	public Selectable[] _up = new Selectable[0];

	// Token: 0x04000CE8 RID: 3304
	public Selectable[] _down = new Selectable[0];

	// Token: 0x04000CE9 RID: 3305
	public Selectable[] _left = new Selectable[0];

	// Token: 0x04000CEA RID: 3306
	public Selectable[] _right = new Selectable[0];

	// Token: 0x04000CEB RID: 3307
	public Selectable _target;

	// Token: 0x04000CEC RID: 3308
	private bool updateNavigation;
}
