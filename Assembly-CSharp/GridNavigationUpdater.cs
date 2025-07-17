using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A2 RID: 418
public class GridNavigationUpdater : MonoBehaviour
{
	// Token: 0x06000E4F RID: 3663 RVA: 0x0004EF4C File Offset: 0x0004D14C
	private void Awake()
	{
		this.trans = base.transform;
		GridLayoutGroup component = base.GetComponent<GridLayoutGroup>();
		if (component == null)
		{
			base.enabled = false;
			return;
		}
		this.constraintWidth = component.constraintCount;
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x0004EF89 File Offset: 0x0004D189
	private void LateUpdate()
	{
		if (this.CheckChildrenChanged())
		{
			this.FixNavigation();
		}
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0004EF9C File Offset: 0x0004D19C
	private bool CheckChildrenChanged()
	{
		int count = this.childrenActive.Count;
		int num = 0;
		bool flag = false;
		int num2 = 0;
		int childCount = this.trans.childCount;
		while (num2 < childCount && !flag)
		{
			GameObject gameObject = this.trans.GetChild(num2).gameObject;
			if (gameObject.activeInHierarchy)
			{
				if (num >= count)
				{
					flag = true;
				}
				else if (this.childrenActive[num] == null || this.childrenActive[num].gameObject.GetInstanceID() != gameObject.GetInstanceID())
				{
					flag = true;
				}
				else
				{
					num++;
				}
			}
			num2++;
		}
		if (num != count)
		{
			flag = true;
		}
		if (flag)
		{
			this.GetActiveChildren();
		}
		return flag;
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x0004F048 File Offset: 0x0004D248
	private void GetActiveChildren()
	{
		this.childrenActive.Clear();
		int i = 0;
		int childCount = this.trans.childCount;
		while (i < childCount)
		{
			GameObject gameObject = this.trans.GetChild(i).gameObject;
			if (gameObject.activeInHierarchy)
			{
				Selectable component = gameObject.GetComponent<Selectable>();
				if (component != null)
				{
					this.childrenActive.Add(component);
				}
			}
			i++;
		}
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x0004F0B0 File Offset: 0x0004D2B0
	private void FixNavigation()
	{
		int i = 0;
		int count = this.childrenActive.Count;
		while (i < count)
		{
			Navigation navigation = this.childrenActive[i].navigation;
			if (i < this.constraintWidth)
			{
				navigation.selectOnUp = null;
			}
			else
			{
				navigation.selectOnUp = this.childrenActive[i - this.constraintWidth];
			}
			if (i + this.constraintWidth < count)
			{
				navigation.selectOnDown = this.childrenActive[i + this.constraintWidth];
			}
			else
			{
				navigation.selectOnDown = null;
			}
			if (i % this.constraintWidth == 0)
			{
				navigation.selectOnLeft = null;
			}
			else
			{
				navigation.selectOnLeft = this.childrenActive[i - 1];
			}
			if (i + 1 == count || i % this.constraintWidth == this.constraintWidth - 1)
			{
				navigation.selectOnRight = null;
			}
			else
			{
				navigation.selectOnRight = this.childrenActive[i + 1];
			}
			navigation.mode = Navigation.Mode.Explicit;
			this.childrenActive[i].navigation = navigation;
			i++;
		}
	}

	// Token: 0x04000CAB RID: 3243
	private List<Selectable> childrenActive = new List<Selectable>();

	// Token: 0x04000CAC RID: 3244
	private Transform trans;

	// Token: 0x04000CAD RID: 3245
	private int constraintWidth;
}
