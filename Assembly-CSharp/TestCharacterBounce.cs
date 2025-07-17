using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001BA RID: 442
public class TestCharacterBounce : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000EF6 RID: 3830 RVA: 0x000516EF File Offset: 0x0004F8EF
	private void Start()
	{
		this.pos = base.gameObject.transform;
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x00051704 File Offset: 0x0004F904
	public void OnPointerClick(PointerEventData data)
	{
		E_Facial_Expressions e_Facial_Expressions = (E_Facial_Expressions)Random.Range(0, (int)Enum.GetValues(typeof(E_Facial_Expressions)).Cast<E_Facial_Expressions>().Max<E_Facial_Expressions>());
		this.AnimatePose(e_Facial_Expressions, this.pos);
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x00051740 File Offset: 0x0004F940
	private void AnimatePose(E_Facial_Expressions exp, Transform pos)
	{
		float num = 1f;
		int num2 = 2;
		switch (exp)
		{
		case E_Facial_Expressions.angry:
			num = 4f;
			num2 = 4;
			break;
		case E_Facial_Expressions.angry_b:
			num = 4f;
			num2 = 4;
			break;
		case E_Facial_Expressions.blush:
			num = 2f;
			break;
		case E_Facial_Expressions.blush_b:
		case E_Facial_Expressions.flirt:
		case E_Facial_Expressions.flirt_b:
		case E_Facial_Expressions.happy:
		case E_Facial_Expressions.happy_b:
			break;
		case E_Facial_Expressions.joy:
			num = 2f;
			break;
		case E_Facial_Expressions.joy_b:
			num = 2f;
			break;
		case E_Facial_Expressions.shock:
			num = 3f;
			num2 = 3;
			break;
		case E_Facial_Expressions.shock_b:
			num = 3f;
			num2 = 3;
			break;
		default:
			if (exp == E_Facial_Expressions.custom_trap)
			{
				num = 2f;
			}
			break;
		}
		pos.DOPunchPosition(Vector3.up * num * this.punchMultiplier, 0.4f * num * this.durationMultiplier, num2, 1f, false);
	}

	// Token: 0x04000D37 RID: 3383
	[SerializeField]
	private Transform pos;

	// Token: 0x04000D38 RID: 3384
	[SerializeField]
	private float punchMultiplier = 1f;

	// Token: 0x04000D39 RID: 3385
	[SerializeField]
	private float durationMultiplier = 1f;
}
