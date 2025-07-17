using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000144 RID: 324
public class UIButtonAnimation : MonoBehaviour
{
	// Token: 0x06000BDF RID: 3039 RVA: 0x0004478C File Offset: 0x0004298C
	private void Start()
	{
		UnityEvent equipOverridden = Singleton<Dateviators>.Instance.EquipOverridden;
		if (equipOverridden != null)
		{
			equipOverridden.AddListener(new UnityAction(this.TriggerTransitionOn));
		}
		UnityEvent<string> buttonPressed = BetterPlayerControl.Instance.ButtonPressed;
		if (buttonPressed != null)
		{
			buttonPressed.AddListener(new UnityAction<string>(this.OnButtonPressed));
		}
		UnityEvent transitionOn = Singleton<Dateviators>.Instance.TransitionOn;
		if (transitionOn != null)
		{
			transitionOn.AddListener(new UnityAction(this.TriggerTransitionOn));
		}
		UnityEvent transitionOff = Singleton<Dateviators>.Instance.TransitionOff;
		if (transitionOff == null)
		{
			return;
		}
		transitionOff.AddListener(new UnityAction(this.TriggerTransitionOff));
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0004481C File Offset: 0x00042A1C
	private void Update()
	{
		this.UpdateGraphic();
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x00044824 File Offset: 0x00042A24
	private void UpdateGraphic()
	{
		if (this.currentState == UIButtonAnimation.CurrentState.Transitioning)
		{
			if (this.glassesOn)
			{
				this.animationTime += Time.deltaTime;
			}
			else
			{
				this.animationTime -= Time.deltaTime;
			}
			this.animationTime = Mathf.Clamp(this.animationTime, 0f, this.transitionDuration);
			float num = this.animationTime / this.transitionDuration;
			int num2 = (int)((float)(this.transitionSprites.Length - 1) * num);
			if (num2 != this.spriteIndex)
			{
				this.image.sprite = this.transitionSprites[num2];
				this.spriteIndex = num2;
			}
			if (this.animationTime == 0f || Mathf.Approximately(this.animationTime, this.transitionDuration))
			{
				this.currentState = this.stateBeforeTransition;
				if (this.currentState == UIButtonAnimation.CurrentState.Transitioning)
				{
					this.currentState = UIButtonAnimation.CurrentState.Default;
				}
				if (this.stateBeforeTransition == UIButtonAnimation.CurrentState.Alert)
				{
					if (this.actionName == "Phone")
					{
						int num3 = Singleton<ComputerManager>.Instance.UnreadWrkspceMessages();
						if (Singleton<ComputerManager>.Instance.UnreadThiscordMessages() + num3 == 0)
						{
							this.SetDefault();
						}
						else
						{
							this.SetAlert();
						}
					}
					else
					{
						this.SetAlert();
					}
				}
			}
			if (num <= 0.5f)
			{
				if (this.equippedLegend != null && this.unequippedLegend != null)
				{
					if (!this.equippedLegend.activeInHierarchy)
					{
						this.equippedLegend.SetActive(true);
					}
					if (this.unequippedLegend.activeInHierarchy)
					{
						this.unequippedLegend.SetActive(false);
						return;
					}
				}
			}
			else if (num >= 0.5f && this.equippedLegend != null && this.unequippedLegend != null)
			{
				if (this.equippedLegend.activeInHierarchy)
				{
					this.equippedLegend.SetActive(false);
				}
				if (!this.unequippedLegend.activeInHierarchy)
				{
					this.unequippedLegend.SetActive(true);
					return;
				}
			}
		}
		else if (this.currentState == UIButtonAnimation.CurrentState.Pressed)
		{
			this.animationTime += Time.deltaTime;
			this.animationTime = Mathf.Clamp(this.animationTime, 0f, this.pressedDuration);
			float num4 = this.animationTime / this.pressedDuration;
			if (this.glassesOn && this.pressedEquippedSprites != null && this.pressedEquippedSprites.Length != 0)
			{
				int num5 = (int)((float)(this.pressedEquippedSprites.Length - 1) * num4);
				if (num5 != this.spriteIndex)
				{
					this.image.sprite = this.pressedEquippedSprites[num5];
					this.spriteIndex = num5;
				}
				if (this.spriteIndex == this.pressedEquippedSprites.Length - 1)
				{
					this.animationTime = 0f;
					this.spriteIndex = -1;
					this.currentState = this.stateBeforeTransition;
					if (this.currentState == UIButtonAnimation.CurrentState.Pressed)
					{
						this.currentState = UIButtonAnimation.CurrentState.Default;
						return;
					}
				}
			}
			else if (!this.glassesOn && this.pressedUnequippedSprites != null && this.pressedUnequippedSprites.Length != 0)
			{
				int num6 = (int)((float)(this.pressedUnequippedSprites.Length - 1) * num4);
				if (num6 != this.spriteIndex)
				{
					this.image.sprite = this.pressedUnequippedSprites[num6];
					this.spriteIndex = num6;
				}
				if (this.spriteIndex == this.pressedUnequippedSprites.Length - 1)
				{
					this.animationTime = 0f;
					this.spriteIndex = -1;
					this.currentState = this.stateBeforeTransition;
					if (this.currentState == UIButtonAnimation.CurrentState.Pressed)
					{
						this.currentState = UIButtonAnimation.CurrentState.Default;
						return;
					}
				}
			}
		}
		else if (this.currentState == UIButtonAnimation.CurrentState.Alert)
		{
			this.animationTime += Time.deltaTime;
			this.animationTime = Mathf.Clamp(this.animationTime, 0f, this.alertDuration);
			float num7 = this.animationTime / this.alertDuration;
			if (this.glassesOn && this.alertEquippedSprites != null && this.alertEquippedSprites.Length != 0)
			{
				int num8 = (int)((float)(this.alertEquippedSprites.Length - 1) * num7);
				if (num8 != this.spriteIndex)
				{
					this.image.sprite = this.alertEquippedSprites[num8];
					this.spriteIndex = num8;
				}
				if (this.spriteIndex == this.alertEquippedSprites.Length - 1)
				{
					this.animationTime = 0f;
					this.spriteIndex = -1;
				}
			}
			else if (!this.glassesOn && this.alertUnequippedSprites != null && this.alertUnequippedSprites.Length != 0)
			{
				int num9 = (int)((float)(this.alertUnequippedSprites.Length - 1) * num7);
				if (num9 != this.spriteIndex)
				{
					this.image.sprite = this.alertUnequippedSprites[num9];
					this.spriteIndex = num9;
				}
				if (this.spriteIndex == this.alertUnequippedSprites.Length - 1)
				{
					this.animationTime = 0f;
					this.spriteIndex = -1;
				}
			}
			if (this.actionName == "Phone")
			{
				int num10 = Singleton<ComputerManager>.Instance.UnreadWrkspceMessages();
				if (Singleton<ComputerManager>.Instance.UnreadThiscordMessages() + num10 == 0)
				{
					this.SetDefault();
					return;
				}
				this.SetAlert();
			}
		}
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x00044D03 File Offset: 0x00042F03
	private void OnButtonPressed(string buttonName)
	{
		if (string.CompareOrdinal(this.actionName, buttonName) == 0)
		{
			this.TriggerPressed();
		}
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x00044D19 File Offset: 0x00042F19
	public void SetDefault()
	{
		this.currentState = UIButtonAnimation.CurrentState.Default;
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x00044D22 File Offset: 0x00042F22
	public void SetAlert()
	{
		if (this.animationTime != 0f && !Mathf.Approximately(this.animationTime, this.transitionDuration))
		{
			return;
		}
		this.animationTime = 0f;
		this.spriteIndex = -1;
		this.currentState = UIButtonAnimation.CurrentState.Alert;
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00044D60 File Offset: 0x00042F60
	private void TriggerPressed()
	{
		if (this.animationTime != 0f && !Mathf.Approximately(this.animationTime, this.transitionDuration))
		{
			return;
		}
		this.animationTime = 0f;
		this.spriteIndex = -1;
		this.stateBeforeTransition = this.currentState;
		this.currentState = UIButtonAnimation.CurrentState.Pressed;
		this.UpdateGraphic();
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x00044DB9 File Offset: 0x00042FB9
	public void TriggerTransitionOn()
	{
		this.glassesOn = true;
		if (this.currentState != UIButtonAnimation.CurrentState.Transitioning)
		{
			this.spriteIndex = -1;
			this.animationTime = 0f;
			this.stateBeforeTransition = this.currentState;
			this.currentState = UIButtonAnimation.CurrentState.Transitioning;
			this.UpdateGraphic();
		}
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x00044DF6 File Offset: 0x00042FF6
	public void TriggerTransitionOff()
	{
		this.glassesOn = false;
		if (this.currentState != UIButtonAnimation.CurrentState.Transitioning)
		{
			this.spriteIndex = -1;
			this.stateBeforeTransition = this.currentState;
			this.animationTime = this.transitionDuration;
			this.currentState = UIButtonAnimation.CurrentState.Transitioning;
			this.UpdateGraphic();
		}
	}

	// Token: 0x04000A91 RID: 2705
	[SerializeField]
	private string actionName = "";

	// Token: 0x04000A92 RID: 2706
	[SerializeField]
	private float transitionDuration = 1f;

	// Token: 0x04000A93 RID: 2707
	[SerializeField]
	private float pressedDuration = 1f;

	// Token: 0x04000A94 RID: 2708
	[SerializeField]
	private float alertDuration = 1f;

	// Token: 0x04000A95 RID: 2709
	[SerializeField]
	private Image image;

	// Token: 0x04000A96 RID: 2710
	[SerializeField]
	private Sprite[] pressedEquippedSprites;

	// Token: 0x04000A97 RID: 2711
	[SerializeField]
	private Sprite[] pressedUnequippedSprites;

	// Token: 0x04000A98 RID: 2712
	[SerializeField]
	private Sprite[] transitionSprites;

	// Token: 0x04000A99 RID: 2713
	[SerializeField]
	private Sprite[] alertEquippedSprites;

	// Token: 0x04000A9A RID: 2714
	[SerializeField]
	private Sprite[] alertUnequippedSprites;

	// Token: 0x04000A9B RID: 2715
	[FormerlySerializedAs("normalizedTime")]
	[SerializeField]
	private float animationTime;

	// Token: 0x04000A9C RID: 2716
	[SerializeField]
	private int spriteIndex = -1;

	// Token: 0x04000A9D RID: 2717
	[SerializeField]
	private UIButtonAnimation.CurrentState currentState;

	// Token: 0x04000A9E RID: 2718
	[SerializeField]
	private UIButtonAnimation.CurrentState stateBeforeTransition;

	// Token: 0x04000A9F RID: 2719
	[SerializeField]
	private GameObject equippedLegend;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	private GameObject unequippedLegend;

	// Token: 0x04000AA1 RID: 2721
	[SerializeField]
	private bool glassesOn;

	// Token: 0x02000352 RID: 850
	private enum CurrentState
	{
		// Token: 0x04001317 RID: 4887
		Default,
		// Token: 0x04001318 RID: 4888
		Transitioning,
		// Token: 0x04001319 RID: 4889
		Alert,
		// Token: 0x0400131A RID: 4890
		Pressed
	}
}
