using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x02000139 RID: 313
public class SpecStatBlock : MonoBehaviour
{
	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06000B10 RID: 2832 RVA: 0x0003F69A File Offset: 0x0003D89A
	// (set) Token: 0x06000B11 RID: 2833 RVA: 0x0003F6A2 File Offset: 0x0003D8A2
	public int currentValue
	{
		get
		{
			return this._currentValue;
		}
		set
		{
			this._currentValue = value;
			if (this._currentValue > 100)
			{
				this._currentValue = 100;
			}
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0003F6BD File Offset: 0x0003D8BD
	private void Start()
	{
		this.UpdateCandy();
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x0003F6C8 File Offset: 0x0003D8C8
	private bool TryGetCandyInkVariable(out bool value)
	{
		object obj = Singleton<InkController>.Instance.story.variablesState[this.candyInkVariable];
		if (obj is bool)
		{
			bool flag = (bool)obj;
			value = flag;
			return true;
		}
		string text = obj as string;
		if (text == null)
		{
			value = false;
			return false;
		}
		if (text == "off" || text == "false")
		{
			value = false;
			return true;
		}
		if (!(text == "on") && !(text == "true"))
		{
			value = false;
			return false;
		}
		value = true;
		return true;
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x0003F758 File Offset: 0x0003D958
	private void UpdateCandy()
	{
		bool flag;
		if (this.TryGetCandyInkVariable(out flag) && flag)
		{
			this.candyIcon.sprite = this.candyIconOn;
			return;
		}
		this.candyIcon.sprite = this.candyIconOff;
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x0003F798 File Offset: 0x0003D998
	public void Init(string name, string[] levelTitles, string[] levelDescriptions)
	{
		int statLevel = Singleton<SpecStatMain>.Instance.GetStatLevel(this.SpecsAttribute.ToString(), true);
		this.NameFirstLetter.text = name.Substring(0, 1);
		this.NameRest.text = name.Substring(1).ToUpperInvariant() + ": " + statLevel.ToString();
		this._levelTitles = levelTitles;
		this._levelDescriptions = levelDescriptions;
		this.AdjectiveLabel.text = "";
		this.icon.sprite = this.iconAtlas.GetSprite(name.ToLowerInvariant().Trim() + "_icon");
		this.UpdateCandy();
		this.StarPiece.transform.GetChild(0).gameObject.SetActive(true);
		this.StarPiece.transform.GetChild(0).transform.localScale = Vector3.one;
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0003F888 File Offset: 0x0003DA88
	public void UpdateValue(int previousValue, int value)
	{
		this.SetAdjectiveText(value);
		this.SetDescriptionText(value);
		base.StartCoroutine(this.UpdateGraphic(previousValue, value, 2f));
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0003F8AC File Offset: 0x0003DAAC
	public void OnUpdateStarCenter(float p)
	{
		this.newWidth = Mathf.Lerp(0.1f, 1f, p);
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0003F8C4 File Offset: 0x0003DAC4
	public IEnumerator UpdateGraphic(int previousValue, int value, float duration)
	{
		int previousLevel = this.currentLevel;
		if (this.currentLevel != value)
		{
			this.currentLevel = value;
			this.SetAdjectiveText(-1);
			this.SetDescriptionText(-1);
		}
		yield return new WaitForSeconds(0.01f);
		while (DateADex.Instance.resultSplashScreen.activeInHierarchy)
		{
			yield return new WaitForEndOfFrame();
		}
		if (previousValue >= 100)
		{
			this.StarPiece.gameObject.SetActive(false);
		}
		else
		{
			this.StarPiece.gameObject.SetActive(true);
			Vector3 vector = this.StarPiece.transform.GetChild(0).transform.localScale;
			if (previousValue == 0)
			{
				this.StarPiece.transform.GetChild(0).gameObject.SetActive(true);
				vector = Vector3.one;
			}
			this.StarPiece.transform.GetChild(0).transform.localScale = vector;
			int updatedValue = previousValue;
			int numberOfAnimations = (value - updatedValue) / 5;
			while (updatedValue < value && Math.Abs(value - previousValue) >= 5)
			{
				this.StarPiece.SetTrigger("anim-" + updatedValue.ToString() + "-" + (updatedValue + 5).ToString());
				this.StarPiece.speed = (float)numberOfAnimations / 2f;
				updatedValue += 5;
				yield return new WaitForSeconds(1f / (float)numberOfAnimations);
			}
			if (updatedValue == 0 && value == 0)
			{
				this.StarPieceImage.sprite = this.StarPieceStartImage;
			}
		}
		this.SetAdjectiveText(-1);
		this.SetDescriptionText(-1);
		int statLevel = Singleton<SpecStatMain>.Instance.GetStatLevel(this.SpecsAttribute.ToString(), true);
		this.NameRest.text = this.NameRest.text.Substring(0, this.NameRest.text.LastIndexOf(": ")).ToUpperInvariant() + ": " + statLevel.ToString();
		yield return new WaitForEndOfFrame();
		if (value != previousLevel && value > previousLevel)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_specs_star_maximum, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		yield break;
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0003F8E4 File Offset: 0x0003DAE4
	private void SetAdjectiveText(int level = -1)
	{
		if (level == -1)
		{
			level = this.currentLevel;
		}
		if (level == 0)
		{
			this.AdjectiveLabel.text = this._levelTitles[0].Trim().ToUpperInvariant();
			return;
		}
		int num = level / 20 + 1;
		if (num > 5)
		{
			num = 5;
		}
		this.AdjectiveLabel.text = this._levelTitles[num].Trim().ToUpperInvariant();
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0003F948 File Offset: 0x0003DB48
	private void SetDescriptionText(int level = -1)
	{
		if (level == -1)
		{
			level = this.currentLevel;
		}
		if (level == 0)
		{
			this.levelDescriptionText.text = this._levelDescriptions[0].Trim();
			return;
		}
		int num = level / 20 + 1;
		if (num > 5)
		{
			num = 5;
		}
		this.levelDescriptionText.text = this._levelDescriptions[num].Trim();
	}

	// Token: 0x04000A12 RID: 2578
	[SerializeField]
	private Animator StarPiece;

	// Token: 0x04000A13 RID: 2579
	[SerializeField]
	private TextMeshProUGUI NameFirstLetter;

	// Token: 0x04000A14 RID: 2580
	[SerializeField]
	private TextMeshProUGUI NameRest;

	// Token: 0x04000A15 RID: 2581
	[SerializeField]
	private TextMeshProUGUI AdjectiveLabel;

	// Token: 0x04000A16 RID: 2582
	[SerializeField]
	private SpriteAtlas iconAtlas;

	// Token: 0x04000A17 RID: 2583
	[SerializeField]
	private Image icon;

	// Token: 0x04000A18 RID: 2584
	[SerializeField]
	private Image candyIcon;

	// Token: 0x04000A19 RID: 2585
	[SerializeField]
	private string candyInkVariable;

	// Token: 0x04000A1A RID: 2586
	[SerializeField]
	private Sprite candyIconOn;

	// Token: 0x04000A1B RID: 2587
	[SerializeField]
	private Sprite candyIconOff;

	// Token: 0x04000A1C RID: 2588
	[SerializeField]
	private TextMeshProUGUI levelDescriptionText;

	// Token: 0x04000A1D RID: 2589
	[SerializeField]
	private SpecsAttributes SpecsAttribute;

	// Token: 0x04000A1E RID: 2590
	[SerializeField]
	private Image StarPieceImage;

	// Token: 0x04000A1F RID: 2591
	[SerializeField]
	private Sprite StarPieceStartImage;

	// Token: 0x04000A20 RID: 2592
	private string[] _levelTitles;

	// Token: 0x04000A21 RID: 2593
	private string[] _levelDescriptions;

	// Token: 0x04000A22 RID: 2594
	private int currentLevel = 1;

	// Token: 0x04000A23 RID: 2595
	private int _currentValue;

	// Token: 0x04000A24 RID: 2596
	private float currentWidth = 0.1f;

	// Token: 0x04000A25 RID: 2597
	private float newWidth;
}
