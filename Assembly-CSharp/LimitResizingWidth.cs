using System;
using TMPro;
using UnityEngine;

// Token: 0x020001A6 RID: 422
[ExecuteInEditMode]
[DefaultExecutionOrder(-10000)]
public class LimitResizingWidth : MonoBehaviour
{
	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000E67 RID: 3687 RVA: 0x0004F644 File Offset: 0x0004D844
	public float maxWidth
	{
		get
		{
			return this.m_maxWidth;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0004F64C File Offset: 0x0004D84C
	public float minWidth
	{
		get
		{
			return this.m_minWidth;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000E69 RID: 3689 RVA: 0x0004F654 File Offset: 0x0004D854
	public float maxHeight
	{
		get
		{
			return this.m_maxHeight;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0004F65C File Offset: 0x0004D85C
	public float minHeight
	{
		get
		{
			return this.m_minHeight;
		}
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x0004F664 File Offset: 0x0004D864
	protected void OnEnable()
	{
		this.Validate();
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x0004F670 File Offset: 0x0004D870
	private void LateUpdate()
	{
		if (this.valid)
		{
			Vector3 size = this.textControl.textBounds.size;
			Vector3 vector = size;
			bool flag = false;
			if (size.x != this.previousSize.x)
			{
				flag = true;
				if (this.minWidth != 0f && vector.x < this.minWidth)
				{
					vector.x = this.minWidth;
				}
				if (this.maxWidth != 0f && vector.x > this.maxWidth)
				{
					vector.x = this.maxWidth;
					this.textRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.maxWidth);
				}
				HorizontalAlignmentOptions horizontalAlignment = this.textControl.horizontalAlignment;
				if (horizontalAlignment != HorizontalAlignmentOptions.Left)
				{
					if (horizontalAlignment != HorizontalAlignmentOptions.Center)
					{
						this.textControl.transform.localPosition = new Vector3(-this.m_RightPadding, this.textControl.transform.localPosition.y);
					}
					else
					{
						this.textControl.transform.localPosition = new Vector3(this.maxWidth / 2f - vector.x / 2f - this.m_RightPadding, this.textControl.transform.localPosition.y);
					}
				}
				else
				{
					this.textControl.transform.localPosition = new Vector3(this.maxWidth - (vector.x + this.m_RightPadding), this.textControl.transform.localPosition.y);
				}
				this.targetRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x + this.m_LeftPadding + this.m_RightPadding);
			}
			if (size.y != this.previousSize.y)
			{
				flag = true;
				if (this.minHeight != 0f && vector.y < this.minHeight)
				{
					vector.y = this.minHeight;
				}
				if (this.maxHeight != 0f && vector.y > this.maxHeight)
				{
					vector.y = this.maxHeight;
					this.textRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.maxHeight);
				}
				if (this.m_TopPadding != 0f)
				{
					this.textControl.transform.localPosition = new Vector3(this.textControl.transform.localPosition.x, -this.m_TopPadding);
				}
				this.targetRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vector.y + this.m_TopPadding + this.m_BottomPadding);
			}
			if (flag)
			{
				this.previousSize = size;
			}
		}
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x0004F8F8 File Offset: 0x0004DAF8
	private bool Validate()
	{
		this.valid = this.targetRectTransform != null && this.textControl != null;
		if (this.valid)
		{
			if (this.minWidth < 0f)
			{
				this.m_minWidth = 0f;
			}
			if (this.maxWidth != 0f && this.maxWidth < this.minWidth)
			{
				this.m_maxWidth = this.minWidth;
			}
			if (this.minHeight < 0f)
			{
				this.m_minHeight = 0f;
			}
			if (this.maxHeight != 0f && this.maxHeight < this.minHeight)
			{
				this.m_maxHeight = this.minHeight;
			}
			this.textRectTransform = this.textControl.GetComponent<RectTransform>();
			if (this.maxWidth != 0f && this.textRectTransform.sizeDelta.x != this.maxWidth)
			{
				this.textRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.maxWidth);
			}
		}
		return this.valid;
	}

	// Token: 0x04000CC5 RID: 3269
	[Header("Source and Target")]
	[Tooltip("The content to use for the size")]
	public TextMeshProUGUI textControl;

	// Token: 0x04000CC6 RID: 3270
	[Tooltip("The UI GameObject to size to the content")]
	public RectTransform targetRectTransform;

	// Token: 0x04000CC7 RID: 3271
	[Header("Limits")]
	[Tooltip("Set maximum width the control can be. A value of zero means no maximum")]
	[SerializeField]
	public float m_maxWidth;

	// Token: 0x04000CC8 RID: 3272
	[Tooltip("Set minimum width the control can be. A value of zero means no minimum")]
	[SerializeField]
	public float m_minWidth;

	// Token: 0x04000CC9 RID: 3273
	[Tooltip("Set maximum height the control can be. A value of zero means no maximum")]
	[SerializeField]
	public float m_maxHeight;

	// Token: 0x04000CCA RID: 3274
	[Tooltip("Set minimum height the control can be. A value of zero means no minimum")]
	[SerializeField]
	public float m_minHeight;

	// Token: 0x04000CCB RID: 3275
	[Header("Padding")]
	public float m_LeftPadding;

	// Token: 0x04000CCC RID: 3276
	public float m_RightPadding;

	// Token: 0x04000CCD RID: 3277
	public float m_TopPadding;

	// Token: 0x04000CCE RID: 3278
	public float m_BottomPadding;

	// Token: 0x04000CCF RID: 3279
	private bool valid;

	// Token: 0x04000CD0 RID: 3280
	private Vector3 previousSize = Vector3.zero;

	// Token: 0x04000CD1 RID: 3281
	private RectTransform textRectTransform;
}
