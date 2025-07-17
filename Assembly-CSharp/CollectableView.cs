using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F1 RID: 241
public class CollectableView : MonoBehaviour
{
	// Token: 0x060007FD RID: 2045 RVA: 0x0002DA3D File Offset: 0x0002BC3D
	public void UpdateData(Sprite sprite, Tuple<string, string, string> name_desc_hint, bool unlocked)
	{
		this.image.sprite = (unlocked ? sprite : this.lockedSprite);
		this._collectable_Names_Desc_Hint = name_desc_hint;
		this._isUnlocked = unlocked;
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x0002DA64 File Offset: 0x0002BC64
	public void SetupNavigation(Selectable previous, Selectable next, Selectable down)
	{
		Navigation navigation = default(Navigation);
		navigation.mode = Navigation.Mode.Explicit;
		if (previous)
		{
			navigation.selectOnLeft = previous;
		}
		if (next)
		{
			navigation.selectOnRight = next;
		}
		navigation.selectOnDown = down;
		this.Button.navigation = navigation;
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x0002DAB5 File Offset: 0x0002BCB5
	public void SetSelected(bool flag)
	{
		this.selected.gameObject.SetActive(flag);
		if (flag)
		{
			base.transform.parent.SetAsLastSibling();
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0002DADB File Offset: 0x0002BCDB
	public void AlignWithParent()
	{
		base.transform.localPosition = Vector3.zero;
		base.transform.localEulerAngles = Vector3.zero;
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0002DAFD File Offset: 0x0002BCFD
	public void ClearData()
	{
		this.image.sprite = null;
		this._collectable_Names_Desc_Hint = null;
		this._isUnlocked = false;
		this.Button.onClick.RemoveAllListeners();
		this.SetSelected(false);
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0002DB30 File Offset: 0x0002BD30
	private void ClearNavigation()
	{
		Navigation navigation = default(Navigation);
		navigation.mode = Navigation.Mode.Automatic;
		this.Button.navigation = navigation;
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0002DB59 File Offset: 0x0002BD59
	public string GetName()
	{
		if (this._collectable_Names_Desc_Hint == null)
		{
			return "????";
		}
		if (!this._isUnlocked)
		{
			return "???";
		}
		return this._collectable_Names_Desc_Hint.Item1;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0002DB82 File Offset: 0x0002BD82
	public string GetDescription()
	{
		if (this._collectable_Names_Desc_Hint == null)
		{
			return "";
		}
		if (!this._isUnlocked)
		{
			return this.GetHint();
		}
		return this._collectable_Names_Desc_Hint.Item2;
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x0002DBAC File Offset: 0x0002BDAC
	public string GetHint()
	{
		if (this._collectable_Names_Desc_Hint == null)
		{
			return "";
		}
		return DateADex.Instance.TreatUnmetCharacters(this._collectable_Names_Desc_Hint.Item3);
	}

	// Token: 0x04000735 RID: 1845
	[SerializeField]
	private Image image;

	// Token: 0x04000736 RID: 1846
	[SerializeField]
	private Image selected;

	// Token: 0x04000737 RID: 1847
	[SerializeField]
	private Sprite lockedSprite;

	// Token: 0x04000738 RID: 1848
	public Button Button;

	// Token: 0x04000739 RID: 1849
	private Tuple<string, string, string> _collectable_Names_Desc_Hint;

	// Token: 0x0400073A RID: 1850
	private bool _isUnlocked;
}
