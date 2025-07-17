using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.VFX;

// Token: 0x02000149 RID: 329
public class UISpeaker : MonoBehaviour
{
	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000BFA RID: 3066 RVA: 0x0004531D File Offset: 0x0004351D
	// (set) Token: 0x06000BFB RID: 3067 RVA: 0x0004533F File Offset: 0x0004353F
	private Image image
	{
		get
		{
			if (this._image == null)
			{
				this._image = base.GetComponentInChildren<Image>();
			}
			return this._image;
		}
		set
		{
			this._image = value;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00045348 File Offset: 0x00043548
	// (set) Token: 0x06000BFD RID: 3069 RVA: 0x0004536A File Offset: 0x0004356A
	private RectTransform rect
	{
		get
		{
			if (this._rect == null)
			{
				this._rect = base.GetComponent<RectTransform>();
			}
			return this._rect;
		}
		set
		{
			this._rect = value;
		}
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x00045373 File Offset: 0x00043573
	private void Awake()
	{
		this.targetAnchoredPosition = this.rect.anchoredPosition;
		this.startingYPos = this.rect.anchoredPosition.y;
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x0004539C File Offset: 0x0004359C
	private void OnValidate()
	{
		if (this.angryEmotePS == null)
		{
			this.angryEmotePS = this.angryEmote.GetComponent<VisualEffect>();
		}
		if (this.joyEmotePS == null)
		{
			this.joyEmotePS = this.joyEmote.GetComponent<VisualEffect>();
		}
		if (this.disgustEmotePS == null)
		{
			this.disgustEmotePS = this.disgustEmote.GetComponent<VisualEffect>();
		}
		if (this.loveEmotePS == null)
		{
			this.loveEmotePS = this.loveEmote.GetComponent<VisualEffect>();
		}
		if (this.sweatEmotePS == null)
		{
			this.sweatEmotePS = this.sweatEmote.GetComponent<VisualEffect>();
		}
		if (this.badEmotePS == null)
		{
			this.badEmotePS = this.badEmote.GetComponent<VisualEffect>();
		}
		if (this.goodEmotePS == null)
		{
			this.goodEmotePS = this.goodEmote.GetComponent<VisualEffect>();
		}
		if (this.violentEmotePS == null)
		{
			this.violentEmotePS = this.violentEmote.GetComponent<VisualEffect>();
		}
		if (this.resolveEmotePS == null)
		{
			this.resolveEmotePS = this.resolveEmote.GetComponent<VisualEffect>();
		}
		if (this.singEmotePS == null)
		{
			this.singEmotePS = this.singEmote.GetComponent<VisualEffect>();
		}
		if (this.love1EmotePS == null)
		{
			this.love1EmotePS = this.love1Emote.GetComponent<VisualEffect>();
		}
		if (this.love2EmotePS == null)
		{
			this.love2EmotePS = this.love2Emote.GetComponent<VisualEffect>();
		}
		if (this.love3EmotePS == null)
		{
			this.love3EmotePS = this.love3Emote.GetComponent<VisualEffect>();
		}
		if (this.sleepEmotePS == null)
		{
			this.sleepEmotePS = this.sleepEmote.GetComponent<VisualEffect>();
		}
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x0004555B File Offset: 0x0004375B
	private void OnDisable()
	{
		this.isActiveOnStage = false;
		this.EndEmotes();
		this.image.enabled = false;
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x00045578 File Offset: 0x00043778
	public void EndEmotes()
	{
		this.angryEmotePS.Stop();
		this.joyEmotePS.Stop();
		this.disgustEmotePS.Stop();
		this.loveEmotePS.Stop();
		this.sweatEmotePS.Stop();
		this.badEmotePS.Stop();
		this.goodEmotePS.Stop();
		this.violentEmotePS.Stop();
		this.resolveEmotePS.Stop();
		this.singEmotePS.Stop();
		this.love1EmotePS.Stop();
		this.love2EmotePS.Stop();
		this.love3EmotePS.Stop();
		this.sleepEmotePS.Stop();
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x00045620 File Offset: 0x00043820
	private void Update()
	{
		if (this.isActiveOnStage)
		{
			this.targetAnchoredPosition.x = this.targetPosition;
			if (Mathf.Abs(this.rect.anchoredPosition.x - this.targetAnchoredPosition.x) > 0.1f)
			{
				this.isAnimating = true;
				Vector2 anchoredPosition = this.rect.anchoredPosition;
				anchoredPosition.x = Mathf.Lerp(anchoredPosition.x, this.targetAnchoredPosition.x, this.currentMultiplier * this.speed);
				this.rect.anchoredPosition = anchoredPosition;
				return;
			}
			this.targetAnchoredPosition.y = this.rect.anchoredPosition.y;
			if (Mathf.Abs(this.rect.anchoredPosition.y - this.targetAnchoredPosition.y) > 0.1f)
			{
				this.isAnimating = true;
				Vector2 anchoredPosition2 = this.rect.anchoredPosition;
				anchoredPosition2.y = Mathf.Lerp(anchoredPosition2.y, this.targetAnchoredPosition.y, 3f * this.speed);
				this.rect.anchoredPosition = anchoredPosition2;
				return;
			}
			this.rect.anchoredPosition = this.targetAnchoredPosition;
			this.isAnimating = false;
			this.CheckBlinkingSprite();
			if (this.isExiting)
			{
				this.Deactivate();
			}
		}
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x00045774 File Offset: 0x00043974
	private void CheckBlinkingSprite()
	{
		if (!this.isBlinking && this.blinkSprite != null)
		{
			this.millisecondsSinceLastBlink -= (long)((int)(Time.deltaTime * 1000f));
			if (this.millisecondsSinceLastBlink < 0L || Random.Range(0f, (float)this.millisecondsSinceLastBlink) < 10f)
			{
				this.isBlinking = true;
				if (this.blinkSprite2Lucinda != null)
				{
					this.millisecondsSinceLastBlink = 200L;
					this.blinkinStep = 1;
					this.image.sprite = this.blinkSprite1Lucinda;
					return;
				}
				this.millisecondsSinceLastBlink = 300L;
				this.blinkinStep = 0;
				this.image.sprite = this.blinkSprite;
				return;
			}
		}
		else if (this.blinkinStep == 1)
		{
			this.millisecondsSinceLastBlink -= (long)((int)(Time.deltaTime * 1000f));
			if (this.millisecondsSinceLastBlink < 0L || Random.Range(0f, (float)this.millisecondsSinceLastBlink) < 20f)
			{
				this.millisecondsSinceLastBlink = 200L;
				this.image.sprite = this.blinkSprite2Lucinda;
				this.blinkinStep = 2;
				return;
			}
		}
		else if (this.blinkinStep == 2)
		{
			this.millisecondsSinceLastBlink -= (long)((int)(Time.deltaTime * 1000f));
			if (this.millisecondsSinceLastBlink < 0L || Random.Range(0f, (float)this.millisecondsSinceLastBlink) < 20f)
			{
				this.millisecondsSinceLastBlink = 200L;
				this.image.sprite = this.blinkSprite;
				this.blinkinStep = 3;
				return;
			}
		}
		else if (this.blinkinStep == 3)
		{
			this.millisecondsSinceLastBlink -= (long)((int)(Time.deltaTime * 1000f));
			if (this.millisecondsSinceLastBlink < 0L || Random.Range(0f, (float)this.millisecondsSinceLastBlink) < 20f)
			{
				this.millisecondsSinceLastBlink = 200L;
				this.image.sprite = this.blinkSprite2Lucinda;
				this.blinkinStep = 4;
				return;
			}
		}
		else if (this.blinkinStep == 4)
		{
			this.millisecondsSinceLastBlink -= (long)((int)(Time.deltaTime * 1000f));
			if (this.millisecondsSinceLastBlink < 0L || Random.Range(0f, (float)this.millisecondsSinceLastBlink) < 20f)
			{
				this.millisecondsSinceLastBlink = 200L;
				this.image.sprite = this.blinkSprite1Lucinda;
				this.blinkinStep = 5;
				return;
			}
		}
		else
		{
			this.millisecondsSinceLastBlink -= (long)((int)(Time.deltaTime * 1000f));
			if (this.millisecondsSinceLastBlink < 0L || Random.Range(0f, (float)this.millisecondsSinceLastBlink) < 20f)
			{
				this.isBlinking = false;
				this.millisecondsSinceLastBlink = 5000L;
				this.image.sprite = this.regularSprite;
				this.blinkinStep = 0;
			}
		}
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x00045A56 File Offset: 0x00043C56
	public void SetName(string internalName)
	{
		this.character = internalName;
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x00045A5F File Offset: 0x00043C5F
	public void SetTargetPositionName(string _targetPosName)
	{
		this.targetPosName = _targetPosName;
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x00045A68 File Offset: 0x00043C68
	public void SetData(string _targetPosName, float _position, float _targetPosition, bool _activeOnStage, bool isSlow)
	{
		this.SetPosition(_position);
		this.SetTargetPosition(_targetPosition);
		this.targetPosName = _targetPosName;
		this.isActiveOnStage = _activeOnStage;
		if (isSlow)
		{
			this.currentMultiplier = 0.3f;
			return;
		}
		this.currentMultiplier = 1f;
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x00045AA2 File Offset: 0x00043CA2
	public void Initialize(string characterNameIn)
	{
		this.character = characterNameIn;
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x00045AAB File Offset: 0x00043CAB
	public void SetTargetPosition(float x)
	{
		this.currentMultiplier = this.speedMultiplier;
		this.targetPosition = x;
		this.isAnimating = true;
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00045AC7 File Offset: 0x00043CC7
	public void SetTargetPositionMove(float x)
	{
		this.currentMultiplier = this.moveMultiplier;
		this.targetPosition = x;
		this.isAnimating = true;
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x00045AE4 File Offset: 0x00043CE4
	public void SetPosition(float x)
	{
		Vector2 anchoredPosition = this.rect.anchoredPosition;
		anchoredPosition.x = x;
		this.rect.anchoredPosition = anchoredPosition;
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x00045B14 File Offset: 0x00043D14
	public void SetImage(CharacterUtility characterUtility, Sprite spriteIn, Sprite spriteInBlink, bool isNeutralPoseExpresion)
	{
		this.ReturnCurrentCharacterSprites();
		if (this.image == null)
		{
			this.image = base.GetComponentInChildren<Image>();
		}
		base.gameObject.SetActive(true);
		this.image.enabled = true;
		this.image.sprite = spriteIn;
		this._characterUtility = characterUtility;
		this.regularSprite = spriteIn;
		this.blinkSprite = spriteInBlink;
		if (characterUtility != null && characterUtility.internalName == "lucinda" && isNeutralPoseExpresion)
		{
			this.blinkSprite1Lucinda = characterUtility.GetSpriteFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.neutral_blink1, true, false);
			this.blinkSprite2Lucinda = characterUtility.GetSpriteFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.neutral_blink2, true, false);
			return;
		}
		this.blinkSprite1Lucinda = null;
		this.blinkSprite2Lucinda = null;
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x00045BD4 File Offset: 0x00043DD4
	private void ReturnCurrentCharacterSprites()
	{
		if (this.regularSprite != null)
		{
			CharacterUtility.ReturnCharacterSprite(this.regularSprite);
		}
		if (this.blinkSprite != null)
		{
			CharacterUtility.ReturnCharacterSprite(this.blinkSprite);
		}
		if (this.blinkSprite1Lucinda != null)
		{
			CharacterUtility.ReturnCharacterSprite(this.blinkSprite1Lucinda);
		}
		if (this.blinkSprite2Lucinda != null)
		{
			CharacterUtility.ReturnCharacterSprite(this.blinkSprite2Lucinda);
		}
		this._characterUtility = null;
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x00045C4C File Offset: 0x00043E4C
	public void FaceDirection(UISpeaker.E_Direction direction)
	{
		Vector3 localScale = this.rect.localScale;
		if (direction == UISpeaker.E_Direction.Left)
		{
			localScale.x = -1f;
		}
		else
		{
			localScale.x = 1f;
		}
		this.rect.localScale = localScale;
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00045C90 File Offset: 0x00043E90
	public void Deactivate()
	{
		this.isActiveOnStage = false;
		this.isExiting = false;
		this.character = "";
		this.isSpeaking = false;
		this.ReturnCurrentCharacterSprites();
		this.EndEmotes();
		base.gameObject.SetActive(false);
		this.image.enabled = false;
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x00045CE4 File Offset: 0x00043EE4
	public void StartExit()
	{
		Vector2 anchoredPosition = this.rect.anchoredPosition;
		this.rect.anchoredPosition = anchoredPosition;
		this.targetAnchoredPosition = anchoredPosition;
		this.targetPosition = -1680f;
		this.isExiting = true;
		this.EndEmotes();
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00045D28 File Offset: 0x00043F28
	public void StartEmoteHop()
	{
		Vector2 anchoredPosition = this.rect.anchoredPosition;
		this.rect.anchoredPosition = anchoredPosition;
		this.targetAnchoredPosition = anchoredPosition;
		this.targetYPos = 100f;
		this.isHopping = true;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00045D68 File Offset: 0x00043F68
	public void Activate()
	{
		Vector2 anchoredPosition = this.rect.anchoredPosition;
		this.rect.anchoredPosition = anchoredPosition;
		this.targetAnchoredPosition = anchoredPosition;
		this.targetPosition = this.targetAnchoredPosition.x;
		this.isActiveOnStage = true;
		this.isAnimating = false;
		this.isSpeaking = true;
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00045DBC File Offset: 0x00043FBC
	public void OnSpeaking()
	{
		this.isSpeaking = true;
		base.gameObject.GetComponent<DoCodeAnimation>().Trigger();
		base.gameObject.transform.localScale = Vector3.one;
		this.image.DOColor(Color.white, 0.5f);
		base.transform.SetAsLastSibling();
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x00045E16 File Offset: 0x00044016
	public void OnNotSpeaking()
	{
		this.isSpeaking = false;
		this.image.DOColor(Color.Lerp(Color.black, Color.white, 0.8f), 0.5f);
		base.transform.SetAsFirstSibling();
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x00045E4F File Offset: 0x0004404F
	public void Hide()
	{
		this.image.enabled = false;
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00045E5D File Offset: 0x0004405D
	public void UnHide()
	{
		this.image.enabled = true;
	}

	// Token: 0x04000AB2 RID: 2738
	private Image _image;

	// Token: 0x04000AB3 RID: 2739
	private Sprite regularSprite;

	// Token: 0x04000AB4 RID: 2740
	private Sprite blinkSprite;

	// Token: 0x04000AB5 RID: 2741
	private Sprite blinkSprite1Lucinda;

	// Token: 0x04000AB6 RID: 2742
	private Sprite blinkSprite2Lucinda;

	// Token: 0x04000AB7 RID: 2743
	private bool isBlinking;

	// Token: 0x04000AB8 RID: 2744
	private int blinkinStep;

	// Token: 0x04000AB9 RID: 2745
	private long millisecondsSinceLastBlink = 5000L;

	// Token: 0x04000ABA RID: 2746
	public bool isActiveOnStage;

	// Token: 0x04000ABB RID: 2747
	public string character;

	// Token: 0x04000ABC RID: 2748
	public string targetPosName;

	// Token: 0x04000ABD RID: 2749
	public bool isAnimating;

	// Token: 0x04000ABE RID: 2750
	public bool isExiting;

	// Token: 0x04000ABF RID: 2751
	[FormerlySerializedAs("isHoping")]
	public bool isHopping;

	// Token: 0x04000AC0 RID: 2752
	public bool isSpeaking;

	// Token: 0x04000AC1 RID: 2753
	public float speed = 0.5f;

	// Token: 0x04000AC2 RID: 2754
	private float speedMultiplier = 1f;

	// Token: 0x04000AC3 RID: 2755
	private float moveMultiplier = 3f;

	// Token: 0x04000AC4 RID: 2756
	private float currentMultiplier = 1f;

	// Token: 0x04000AC5 RID: 2757
	public float targetPosition;

	// Token: 0x04000AC6 RID: 2758
	public float targetYPos;

	// Token: 0x04000AC7 RID: 2759
	public float startingYPos;

	// Token: 0x04000AC8 RID: 2760
	public Transform emotes;

	// Token: 0x04000AC9 RID: 2761
	public GameObject angryEmote;

	// Token: 0x04000ACA RID: 2762
	public GameObject joyEmote;

	// Token: 0x04000ACB RID: 2763
	public GameObject disgustEmote;

	// Token: 0x04000ACC RID: 2764
	public GameObject loveEmote;

	// Token: 0x04000ACD RID: 2765
	public GameObject sweatEmote;

	// Token: 0x04000ACE RID: 2766
	public GameObject badEmote;

	// Token: 0x04000ACF RID: 2767
	public GameObject goodEmote;

	// Token: 0x04000AD0 RID: 2768
	public GameObject violentEmote;

	// Token: 0x04000AD1 RID: 2769
	public GameObject resolveEmote;

	// Token: 0x04000AD2 RID: 2770
	public GameObject singEmote;

	// Token: 0x04000AD3 RID: 2771
	public GameObject love1Emote;

	// Token: 0x04000AD4 RID: 2772
	public GameObject love2Emote;

	// Token: 0x04000AD5 RID: 2773
	public GameObject love3Emote;

	// Token: 0x04000AD6 RID: 2774
	public GameObject sleepEmote;

	// Token: 0x04000AD7 RID: 2775
	public VisualEffect angryEmotePS;

	// Token: 0x04000AD8 RID: 2776
	public VisualEffect joyEmotePS;

	// Token: 0x04000AD9 RID: 2777
	public VisualEffect disgustEmotePS;

	// Token: 0x04000ADA RID: 2778
	public VisualEffect loveEmotePS;

	// Token: 0x04000ADB RID: 2779
	public VisualEffect sweatEmotePS;

	// Token: 0x04000ADC RID: 2780
	public VisualEffect badEmotePS;

	// Token: 0x04000ADD RID: 2781
	public VisualEffect goodEmotePS;

	// Token: 0x04000ADE RID: 2782
	public VisualEffect violentEmotePS;

	// Token: 0x04000ADF RID: 2783
	public VisualEffect resolveEmotePS;

	// Token: 0x04000AE0 RID: 2784
	public VisualEffect singEmotePS;

	// Token: 0x04000AE1 RID: 2785
	public VisualEffect love1EmotePS;

	// Token: 0x04000AE2 RID: 2786
	public VisualEffect love2EmotePS;

	// Token: 0x04000AE3 RID: 2787
	public VisualEffect love3EmotePS;

	// Token: 0x04000AE4 RID: 2788
	public VisualEffect sleepEmotePS;

	// Token: 0x04000AE5 RID: 2789
	private Vector2 targetAnchoredPosition;

	// Token: 0x04000AE6 RID: 2790
	private RectTransform _rect;

	// Token: 0x04000AE7 RID: 2791
	private CharacterUtility _characterUtility;

	// Token: 0x02000355 RID: 853
	public enum E_Direction
	{
		// Token: 0x04001324 RID: 4900
		Left,
		// Token: 0x04001325 RID: 4901
		Right
	}
}
