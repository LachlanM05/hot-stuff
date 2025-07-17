using System;
using System.Collections;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using T17.Services;
using Team17.Common;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

// Token: 0x02000106 RID: 262
public class ArtPlayer : Singleton<ArtPlayer>
{
	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060008DB RID: 2267 RVA: 0x00034434 File Offset: 0x00032634
	public int CurrentArtIndex
	{
		get
		{
			return this.selectedArt.number - 1;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060008DC RID: 2268 RVA: 0x00034443 File Offset: 0x00032643
	public int TotalEntries
	{
		get
		{
			return this.ArtEntries.Count;
		}
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00034450 File Offset: 0x00032650
	private string TreatArtName(string filename)
	{
		return filename.Replace("_", " ");
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00034462 File Offset: 0x00032662
	public void Start()
	{
		this.animator = base.GetComponent<Animator>();
		this.PopulateItemList();
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x00034476 File Offset: 0x00032676
	public void OnEnable()
	{
		base.StartCoroutine(this.SelectFirstArtButton());
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00034485 File Offset: 0x00032685
	public void OnDisable()
	{
		this.selectedArt = null;
		this.zoom = false;
		this.animator.SetBool("ZoomedIn", false);
		this.ReleaseCurrentSprite();
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x000344AC File Offset: 0x000326AC
	public void SelectCurrentArtEntryButton()
	{
		this.blockZooming = true;
		ControllerMenuUI.SetCurrentlySelected(this.scrollingList.GetFocusingBox().gameObject, ControllerMenuUI.Direction.Down, false, false);
		this.blockZooming = false;
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x000344D4 File Offset: 0x000326D4
	private void PopulateItemList()
	{
		int num = 0;
		new List<Selectable>(this.assetReferenceSprites.Length);
		if (this.ArtEntries.Count == 0)
		{
			foreach (AssetReferenceSprite assetReferenceSprite in this.assetReferenceSprites)
			{
				if (assetReferenceSprite == null)
				{
					T17Debug.LogError("Missing art sprite. ");
				}
				else
				{
					num++;
					ArtEntry artEntry = new ArtEntry(num, this.TreatArtName(assetReferenceSprite.SubObjectName), assetReferenceSprite);
					this.artList.Add(artEntry);
				}
			}
		}
		this.artListBank.Init(this.artList);
		this.scrollingList.Initialize();
		ListBox[] listBoxes = this.scrollingList.ListBoxes;
		int j = 0;
		int num2 = listBoxes.Length;
		while (j < num2)
		{
			if (this.artList[j] != null)
			{
				ArtEntryButton artEntryButton = (ArtEntryButton)listBoxes[j];
				artEntryButton.SetLeftRightControls(this.backButton, this.scrollbar);
				artEntryButton.GetComponent<Button>().onClick.AddListener(delegate
				{
					this.SelectArt(artEntryButton);
					ScrollInView component = artEntryButton.GetComponent<ScrollInView>();
					if (component == null)
					{
						return;
					}
					component.ManualScrollToSelf();
				});
				this.ArtEntries.Add(artEntryButton);
			}
			j++;
		}
		this.SetupEntryNavigation();
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00034615 File Offset: 0x00032815
	public void OpenArtApp()
	{
		base.gameObject.SetActive(true);
		this.ArtBig.gameObject.SetActive(false);
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00034634 File Offset: 0x00032834
	public void OnFocusingBoxChanged(ListBox prevFocusingBox, ListBox curFocusingBox)
	{
		this.UpdateBackNavigation(curFocusingBox.gameObject);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x00034644 File Offset: 0x00032844
	public void UpdateBackNavigation(GameObject newFocus)
	{
		Navigation navigation = this.backButton.GetComponent<Button>().navigation;
		navigation.selectOnRight = newFocus.GetComponent<Button>();
		navigation.selectOnUp = newFocus.GetComponent<Button>();
		this.backButton.GetComponent<Button>().navigation = navigation;
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0003468D File Offset: 0x0003288D
	private IEnumerator SelectFirstArtButton()
	{
		yield return new WaitUntil(() => this.scrollingList.isActiveAndEnabled);
		ControllerMenuUI.SetCurrentlySelected(this.scrollingList.GetFocusingBox().gameObject, ControllerMenuUI.Direction.Down, false, false);
		if (this.currentSprite == null)
		{
			ArtEntryButton artEntryButton = (ArtEntryButton)this.scrollingList.GetFocusingBox();
			this.SelectArt(artEntryButton);
		}
		yield break;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0003469C File Offset: 0x0003289C
	public void CloseArtApp()
	{
		Singleton<PhoneManager>.Instance.ReturnToMainPhoneScreenRotate();
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x000346A8 File Offset: 0x000328A8
	private void SelectArt(ArtEntryButton artButton)
	{
		ArtEntry artEntry = artButton.artEntry;
		if (this.selectedArt == artEntry)
		{
			this.ToggleZoom();
			return;
		}
		this.DisplayNewArt(artEntry);
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x000346D4 File Offset: 0x000328D4
	private void DisplayNewArt(ArtEntry art)
	{
		this.ArtBig.sprite = null;
		this.ReleaseCurrentSprite();
		this.currentSprite = Services.AssetProviderService.LoadAddressableAsset<Sprite>(art.spriteReference);
		this.ArtBig.sprite = this.currentSprite;
		this.selectedArt = art;
		this.ArtBig.preserveAspect = true;
		this.ArtBig.gameObject.SetActive(true);
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0003473E File Offset: 0x0003293E
	private void ReleaseCurrentSprite()
	{
		if (this.currentSprite != null)
		{
			Services.AssetProviderService.UnloadAddressableAsset(this.currentSprite);
			this.currentSprite = null;
		}
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00034768 File Offset: 0x00032968
	public void SetupEntryNavigation()
	{
		for (int i = 0; i < this.ArtEntries.Count; i++)
		{
			Navigation navigation = this.ArtEntries[i].transform.GetComponent<Button>().navigation;
			navigation.selectOnLeft = this.backButton.GetComponent<Button>();
			if (i == 0)
			{
				navigation.selectOnUp = this.ArtEntries[this.ArtEntries.Count - 1].transform.GetComponent<Button>();
			}
			else
			{
				navigation.selectOnUp = this.ArtEntries[Mathf.Clamp(i - 1, 0, this.ArtEntries.Count - 1)].transform.GetComponent<Button>();
			}
			if (i == this.ArtEntries.Count - 1)
			{
				navigation.selectOnDown = this.ArtEntries[0].transform.GetComponent<Button>();
			}
			else
			{
				navigation.selectOnDown = this.ArtEntries[Mathf.Clamp(i + 1, 0, this.ArtEntries.Count - 1)].transform.GetComponent<Button>();
			}
			navigation.mode = Navigation.Mode.Explicit;
			this.ArtEntries[i].transform.GetComponent<Button>().navigation = navigation;
		}
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x000348A4 File Offset: 0x00032AA4
	public void PageEntries(bool up)
	{
		int num = Math.Clamp(this.scrollingList.GetFocusingContentID() + (up ? (-this.pageAmount) : this.pageAmount), 0, this.assetReferenceSprites.Length - 1);
		for (int i = 0; i < this.ArtEntries.Count; i++)
		{
			if (this.ArtEntries[i].ContentID == num)
			{
				ControllerMenuUI.SetCurrentlySelected(this.ArtEntries[i].gameObject, up ? ControllerMenuUI.Direction.Up : ControllerMenuUI.Direction.Down, true, false);
				return;
			}
		}
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0003492A File Offset: 0x00032B2A
	public void ToggleZoom()
	{
		if (!this.blockZooming)
		{
			if (this.zoom)
			{
				this.ZoomOut();
				return;
			}
			this.ZoomIn();
		}
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x00034949 File Offset: 0x00032B49
	public void ZoomIn()
	{
		this.zoom = true;
		this.blockZooming = true;
		this.animator.SetBool("ZoomedIn", true);
		this.blockZooming = false;
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00034971 File Offset: 0x00032B71
	public void ZoomOut()
	{
		this.zoom = false;
		this.blockZooming = true;
		this.animator.SetBool("ZoomedIn", false);
		this.blockZooming = false;
	}

	// Token: 0x0400081E RID: 2078
	public List<ArtEntry> artList;

	// Token: 0x0400081F RID: 2079
	public ArtEntry selectedArt;

	// Token: 0x04000820 RID: 2080
	public Image ArtBig;

	// Token: 0x04000821 RID: 2081
	public TextMeshProUGUI ArtTitle;

	// Token: 0x04000822 RID: 2082
	public GameObject entryButtonPrefab;

	// Token: 0x04000823 RID: 2083
	public GameObject buttonHolder;

	// Token: 0x04000824 RID: 2084
	[SerializeField]
	private List<ArtEntryButton> ArtEntries = new List<ArtEntryButton>();

	// Token: 0x04000825 RID: 2085
	public Selectable backButton;

	// Token: 0x04000826 RID: 2086
	public Selectable scrollbar;

	// Token: 0x04000827 RID: 2087
	[SerializeField]
	private AssetReferenceSprite[] assetReferenceSprites;

	// Token: 0x04000828 RID: 2088
	private Sprite currentSprite;

	// Token: 0x04000829 RID: 2089
	[SerializeField]
	private ArtListBank artListBank;

	// Token: 0x0400082A RID: 2090
	[SerializeField]
	public CircularScrollingList scrollingList;

	// Token: 0x0400082B RID: 2091
	private int pageAmount = 5;

	// Token: 0x0400082C RID: 2092
	private bool zoom;

	// Token: 0x0400082D RID: 2093
	private bool blockZooming;

	// Token: 0x0400082E RID: 2094
	private Animator animator;
}
