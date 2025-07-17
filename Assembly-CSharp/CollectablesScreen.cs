using System;
using System.Collections.Generic;
using T17.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F0 RID: 240
public class CollectablesScreen : MonoBehaviour
{
	// Token: 0x060007F2 RID: 2034 RVA: 0x0002D3DC File Offset: 0x0002B5DC
	public void SetupCollectables(DateADexEntry entry)
	{
		if (!this.initializedPool)
		{
			this.InitPool();
		}
		this.ClearLayout();
		this._currentEntry = entry;
		Singleton<Save>.Instance.GetDateableCollectables(entry.internalName);
		int dateableCollectablesNumber = Singleton<Save>.Instance.GetDateableCollectablesNumber(entry.internalName);
		foreach (GameObject gameObject in this.sparkles)
		{
			if (dateableCollectablesNumber == entry.NumberOfCollectables)
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
		if (entry.Collectable_Names_Desc_Hint.Count == 3 || entry.internalName == "daemon")
		{
			this._activeLayout = this.layout3;
		}
		else if (entry.Collectable_Names_Desc_Hint.Count == 4)
		{
			this._activeLayout = this.layout4;
		}
		else if (entry.Collectable_Names_Desc_Hint.Count == 5)
		{
			this._activeLayout = this.layout5;
		}
		else if (entry.Collectable_Names_Desc_Hint.Count == 6)
		{
			this._activeLayout = this.layout6;
		}
		else if (entry.Collectable_Names_Desc_Hint.Count == 7)
		{
			this._activeLayout = this.layout7;
		}
		else if (entry.Collectable_Names_Desc_Hint.Count == 8)
		{
			this._activeLayout = this.layout8;
		}
		else if (entry.Collectable_Names_Desc_Hint.Count == 9)
		{
			this._activeLayout = this.layout9;
		}
		else if (entry.Collectable_Names_Desc_Hint.Count == 10)
		{
			this._activeLayout = this.layout10;
		}
		else if (entry.Collectable_Names_Desc_Hint.Count == 11)
		{
			this._activeLayout = this.layout11;
		}
		else
		{
			this._activeLayout = this.scrollingLayout;
		}
		this._activeLayout.gameObject.SetActive(true);
		List<CollectableView> list = new List<CollectableView>();
		for (int i = 0; i < entry.Collectable_Names_Desc_Hint.Count; i++)
		{
			CollectableView newView = this.GetNewView();
			int num = i + 1;
			Sprite sprite = null;
			if (Singleton<CharacterHelper>.Instance._characters.IsNameInSet(entry.internalName))
			{
				sprite = Singleton<CharacterHelper>.Instance._characters[entry.internalName].GetSpriteFromCollectableNum(entry.internalName, num);
				this.AddToLoadedResources(sprite);
			}
			bool flag = Singleton<Save>.Instance.IsCollectableUnlocked(entry.internalName, num);
			Tuple<string, string, string> tuple = new Tuple<string, string, string>("", "", "");
			if (entry.Collectable_Names_Desc_Hint.Count > i)
			{
				tuple = entry.Collectable_Names_Desc_Hint[i];
			}
			newView.UpdateData(sprite, tuple, flag);
			if (!(entry.internalName == "daemon") || num <= 3)
			{
				list.Add(newView);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			CollectableView collectableView = ((j > 0) ? list[j - 1] : null);
			CollectableView collectableView2 = ((j < list.Count - 1) ? list[j + 1] : null);
			list[j].SetupNavigation((collectableView != null) ? collectableView.Button : null, (collectableView2 != null) ? collectableView2.Button : null, this.backButton);
		}
		this._activeLayout.LayoutElements(list);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x0002D728 File Offset: 0x0002B928
	private void ClearLayout()
	{
		if (this._activeLayout == null)
		{
			return;
		}
		this.UnloadAddressableResources();
		foreach (CollectableView collectableView in this._activeLayout.GetViews())
		{
			this.ReturnViewToPool(collectableView);
		}
		this._activeLayout.ClearViews();
		this._activeLayout.gameObject.SetActive(false);
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x0002D7B4 File Offset: 0x0002B9B4
	public void UnloadAddressableResources()
	{
		int i = 0;
		int count = this.loadedSprites.Count;
		while (i < count)
		{
			if (this.loadedSprites[i] != null)
			{
				Services.AssetProviderService.UnloadResourceAsset(this.loadedSprites[i]);
			}
			i++;
		}
		this.loadedSprites.Clear();
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x0002D80E File Offset: 0x0002BA0E
	private void AddToLoadedResources(Sprite sprite)
	{
		this.loadedSprites.Add(sprite);
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0002D81C File Offset: 0x0002BA1C
	private CollectableView GetNewView()
	{
		CollectableView newView;
		if (this.collectableViewsPool.childCount == 0)
		{
			newView = this.CreateNewCollectableView();
			newView.AlignWithParent();
		}
		else
		{
			newView = this.collectableViewsPool.GetChild(0).GetComponent<CollectableView>();
		}
		newView.gameObject.SetActive(true);
		newView.Button.onClick.RemoveAllListeners();
		newView.Button.onClick.AddListener(delegate
		{
			this.InspectCollectable(newView);
		});
		newView.transform.SetParent(base.transform, false);
		return newView;
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x0002D8D7 File Offset: 0x0002BAD7
	private void ReturnViewToPool(CollectableView view)
	{
		view.SetSelected(false);
		view.transform.SetParent(this.collectableViewsPool, false);
		view.AlignWithParent();
		view.ClearData();
		view.gameObject.SetActive(false);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x0002D90C File Offset: 0x0002BB0C
	private void InitPool()
	{
		if (this.collectableViewsPool.childCount < this.initialPoolSize)
		{
			while (this.collectableViewsPool.childCount < this.initialPoolSize)
			{
				CollectableView collectableView = this.CreateNewCollectableView();
				collectableView.AlignWithParent();
				collectableView.gameObject.SetActive(false);
			}
		}
		this.initializedPool = true;
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0002D95F File Offset: 0x0002BB5F
	private CollectableView CreateNewCollectableView()
	{
		CollectableView collectableView = Object.Instantiate<CollectableView>(this.collectableViewTemplate, this.collectableViewsPool);
		collectableView.SetSelected(false);
		return collectableView;
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0002D97C File Offset: 0x0002BB7C
	private void InspectCollectable(CollectableView collectable)
	{
		string text = DateADex.Instance.TreatConditionalTag(collectable.GetName());
		string text2 = DateADex.Instance.TreatConditionalTag(collectable.GetDescription());
		this._activeLayout.DeselectViews();
		this.collectableName.text = text;
		this.collectableDesc.text = text2;
		collectable.SetSelected(true);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x0002D9D8 File Offset: 0x0002BBD8
	public Selectable GetInitialCollectable()
	{
		if (!(this._activeLayout != null))
		{
			return null;
		}
		CollectableView initialCollectable = this._activeLayout.GetInitialCollectable();
		if (initialCollectable.gameObject.GetComponentInChildren<Selectable>() != null)
		{
			return initialCollectable.gameObject.GetComponentInChildren<Selectable>();
		}
		return null;
	}

	// Token: 0x0400071F RID: 1823
	public int initialPoolSize = 5;

	// Token: 0x04000720 RID: 1824
	[SerializeField]
	private Transform collectableViewsPool;

	// Token: 0x04000721 RID: 1825
	[SerializeField]
	private CollectableView collectableViewTemplate;

	// Token: 0x04000722 RID: 1826
	[SerializeField]
	private CollectableLayout layout3;

	// Token: 0x04000723 RID: 1827
	[SerializeField]
	private CollectableLayout layout4;

	// Token: 0x04000724 RID: 1828
	[SerializeField]
	private CollectableLayout layout5;

	// Token: 0x04000725 RID: 1829
	[SerializeField]
	private CollectableLayout layout6;

	// Token: 0x04000726 RID: 1830
	[SerializeField]
	private CollectableLayout layout7;

	// Token: 0x04000727 RID: 1831
	[SerializeField]
	private CollectableLayout layout8;

	// Token: 0x04000728 RID: 1832
	[SerializeField]
	private CollectableLayout layout9;

	// Token: 0x04000729 RID: 1833
	[SerializeField]
	private CollectableLayout layout10;

	// Token: 0x0400072A RID: 1834
	[SerializeField]
	private CollectableLayout layout11;

	// Token: 0x0400072B RID: 1835
	[SerializeField]
	private ScrollingCollectableLayout scrollingLayout;

	// Token: 0x0400072C RID: 1836
	[SerializeField]
	private TextMeshProUGUI collectableName;

	// Token: 0x0400072D RID: 1837
	[SerializeField]
	private TextMeshProUGUI collectableDesc;

	// Token: 0x0400072E RID: 1838
	[SerializeField]
	private Image collectableIcon;

	// Token: 0x0400072F RID: 1839
	[SerializeField]
	private Selectable backButton;

	// Token: 0x04000730 RID: 1840
	[SerializeField]
	private List<GameObject> sparkles;

	// Token: 0x04000731 RID: 1841
	private CollectableLayout _activeLayout;

	// Token: 0x04000732 RID: 1842
	private DateADexEntry _currentEntry;

	// Token: 0x04000733 RID: 1843
	private bool initializedPool;

	// Token: 0x04000734 RID: 1844
	private List<Sprite> loadedSprites = new List<Sprite>(20);
}
