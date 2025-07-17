using System;
using System.IO;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using T17.Services;
using Team17.Common;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class SpeakingCharacter
{
	// Token: 0x06000B01 RID: 2817 RVA: 0x0003F178 File Offset: 0x0003D378
	public SpeakingCharacter()
	{
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0003F180 File Offset: 0x0003D380
	public void SetUIPortraitPosition(float position)
	{
		this.UIPortrait.SetTargetPosition(position);
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0003F18E File Offset: 0x0003D38E
	public void SetUIPortraitMovePosition(float position)
	{
		this.UIPortrait.SetTargetPositionMove(position);
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0003F19C File Offset: 0x0003D39C
	public void SetUIPortraitDirection(UISpeaker.E_Direction direction)
	{
		this.UIPortrait.FaceDirection(direction);
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x0003F1AC File Offset: 0x0003D3AC
	public void SetUIPortraitSprite(E_General_Poses pose, E_Facial_Expressions expression)
	{
		Sprite sprite;
		Sprite sprite2;
		if (this.utilityRef != null)
		{
			sprite = this.utilityRef.GetSpriteFromPoseExpression(pose, expression, false, false);
			E_Facial_Expressions e_Facial_Expressions;
			if (Enum.TryParse<E_Facial_Expressions>(expression.ToString() + "_b", true, out e_Facial_Expressions))
			{
				sprite2 = this.utilityRef.GetSpriteFromPoseExpression(pose, e_Facial_Expressions, true, false);
			}
			else
			{
				sprite2 = null;
			}
		}
		else
		{
			sprite = this.GetCustomSpriteFromPoseExpression(this.internalName, pose, expression, false, false);
			E_Facial_Expressions e_Facial_Expressions2;
			if (Enum.TryParse<E_Facial_Expressions>(expression.ToString() + "_b", true, out e_Facial_Expressions2))
			{
				sprite2 = this.GetCustomSpriteFromPoseExpression(this.internalName, pose, e_Facial_Expressions2, true, false);
			}
			else
			{
				sprite2 = null;
			}
		}
		if (pose != this.lastPose)
		{
			this.AnimatePose(pose, this.UIPortrait.transform);
		}
		else if (expression != this.lastExpression)
		{
			this.AnimateExpression(pose, this.UIPortrait.transform);
		}
		this.lastPose = pose;
		this.lastExpression = expression;
		this.UIPortrait.SetImage(this.utilityRef, sprite, sprite2, pose == E_General_Poses.neutral && expression == E_Facial_Expressions.neutral);
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0003F2B8 File Offset: 0x0003D4B8
	public Sprite GetCustomSpriteFromPoseExpression(string internalName, E_General_Poses pose, E_Facial_Expressions expression, bool returnNullIfDontExist = false, bool isForBlinking = false)
	{
		Sprite sprite = this.GetCustomSpriteFileFromPoseExpression(internalName, pose, expression, returnNullIfDontExist);
		if (sprite == null && returnNullIfDontExist)
		{
			return null;
		}
		if (sprite == null && (expression != E_Facial_Expressions.neutral || pose != E_General_Poses.neutral))
		{
			sprite = this.GetCustomSpriteFileFromPoseExpression(internalName, pose, E_Facial_Expressions.neutral, false);
			if (sprite == null)
			{
				sprite = this.GetCustomSpriteFileFromPoseExpression(internalName, E_General_Poses.neutral, E_Facial_Expressions.neutral, false);
			}
		}
		return sprite;
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0003F310 File Offset: 0x0003D510
	public Sprite GetCustomSpriteFileFromPoseExpression(string internalName, E_General_Poses pose, E_Facial_Expressions expression, bool returnNullIfDontExist = false)
	{
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine(new string[]
		{
			"Images",
			"Reactions",
			internalName,
			pose.ToString(),
			CharacterUtility.NamePoseExpressionToFilename(internalName, pose, expression)
		}), false);
		if (sprite == null && !returnNullIfDontExist)
		{
			sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Reactions", internalName, CharacterUtility.NamePoseExpressionToFilename(internalName, pose, expression)), false);
		}
		return sprite;
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0003F398 File Offset: 0x0003D598
	private void AnimatePose(E_General_Poses pose, Transform pos)
	{
		float _velocity = 1f;
		float num = 3f;
		switch (pose)
		{
		case E_General_Poses.neutral:
			_velocity = 1f;
			break;
		case E_General_Poses.friend:
			_velocity = 1.2f;
			break;
		case E_General_Poses.love:
			_velocity = 1.2f;
			break;
		case E_General_Poses.hate:
			_velocity = 4f;
			break;
		}
		float num2 = 10f;
		RectTransform rectTransform = pos as RectTransform;
		if (rectTransform == null)
		{
			T17Debug.LogError("The UISpeaker Transform could not be converted into a RectTransform");
			return;
		}
		rectTransform.DOComplete(true);
		rectTransform.DOAnchorPosY(this.UIPortrait.startingYPos + num2 + 1f * _velocity * num, 0.5f * _velocity / 2f, false).SetEase(Ease.InBounce).OnComplete(delegate
		{
			rectTransform.DOAnchorPosY(this.UIPortrait.startingYPos, 0.5f * _velocity / 2f, false).SetEase(Ease.OutBounce);
		});
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x0003F498 File Offset: 0x0003D698
	private void AnimateExpression(E_General_Poses pose, Transform pos)
	{
		float _velocity = 1f;
		float num = 1.5f;
		float num2 = 10f;
		RectTransform rectTransform = pos as RectTransform;
		if (rectTransform == null)
		{
			T17Debug.LogError("The UISpeaker Transform could not be converted into a RectTransform");
			return;
		}
		rectTransform.DOComplete(true);
		rectTransform.DOAnchorPosY(this.UIPortrait.startingYPos + num2 + 1f * _velocity * num, 0.5f * _velocity / 2f, false).SetEase(Ease.InBounce).OnComplete(delegate
		{
			rectTransform.DOAnchorPosY(this.UIPortrait.startingYPos, 0.5f * _velocity / 2f, false).SetEase(Ease.OutBounce);
		});
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0003F54E File Offset: 0x0003D74E
	public void SetUIPortraitPoseExpression(E_General_Poses pose, E_Facial_Expressions expression)
	{
		this.SetUIPortraitSprite(pose, expression);
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0003F558 File Offset: 0x0003D758
	public void SetUIPortraitPose(E_General_Poses pose)
	{
		this.SetUIPortraitSprite(pose, E_Facial_Expressions.neutral);
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0003F562 File Offset: 0x0003D762
	public void SetUIPortraitExpression(E_Facial_Expressions expression)
	{
		this.SetUIPortraitSprite(this.lastPose, expression);
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0003F574 File Offset: 0x0003D774
	public SpeakingCharacter(string _internalName, string BaseFileNameIn, UISpeaker speakerPortrait)
	{
		string text = Singleton<CharacterHelper>.Instance._characters.GetInternalName(_internalName.ToLowerInvariant());
		if (_internalName == "volt")
		{
			text = "volt";
		}
		if (!string.IsNullOrEmpty(text))
		{
			this.internalName = text;
			this.utilityRef = Singleton<CharacterHelper>.Instance._characters[this.internalName];
		}
		else
		{
			this.internalName = _internalName.ToLowerInvariant();
		}
		this.BaseFileName = BaseFileNameIn;
		this.UIPortrait = speakerPortrait;
		this.UIPortrait.SetName(this.internalName);
		this.SetUIPortraitSprite(E_General_Poses.neutral, E_Facial_Expressions.neutral);
		this.lastPose = E_General_Poses.neutral;
		this.UIPortrait.Activate();
	}

	// Token: 0x04000A07 RID: 2567
	public UISpeaker UIPortrait;

	// Token: 0x04000A08 RID: 2568
	public CharacterUtility utilityRef;

	// Token: 0x04000A09 RID: 2569
	public string internalName;

	// Token: 0x04000A0A RID: 2570
	public string BaseFileName;

	// Token: 0x04000A0B RID: 2571
	public E_General_Poses lastPose;

	// Token: 0x04000A0C RID: 2572
	public E_Facial_Expressions lastExpression;
}
