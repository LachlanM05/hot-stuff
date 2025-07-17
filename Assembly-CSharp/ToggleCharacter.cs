using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class ToggleCharacter : MonoBehaviour
{
	// Token: 0x06000719 RID: 1817 RVA: 0x00027E44 File Offset: 0x00026044
	private void Start()
	{
		this._bodyParts = new GameObject[] { this.Eyes, this.Hair, this.Top, this.Bottom, this.Body, this.Eyebrows, this.Mouth };
		GameObject[] bodyParts = this._bodyParts;
		for (int i = 0; i < bodyParts.Length; i++)
		{
			bodyParts[i].SetActive(false);
		}
		Animator component = base.GetComponent<Animator>();
		if (component != null)
		{
			component.enabled = false;
		}
		base.enabled = false;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x00027ED8 File Offset: 0x000260D8
	private void OnEnable()
	{
		this.InitializeAppearance();
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x00027EE0 File Offset: 0x000260E0
	public void InitializeAppearance()
	{
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00027EE4 File Offset: 0x000260E4
	public GameObject GetBodyPart(BodyPart bodyPart)
	{
		switch (bodyPart)
		{
		case BodyPart.Eyes:
			return this.Eyes;
		case BodyPart.Hair:
			return this.Hair;
		case BodyPart.Top:
			return this.Top;
		case BodyPart.Bottom:
			return this.Bottom;
		case BodyPart.Body:
			return this.Body;
		case BodyPart.Eyebrows:
			return this.Eyebrows;
		case BodyPart.Mouth:
			return this.Mouth;
		default:
			return null;
		}
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x00027F47 File Offset: 0x00026147
	public void DisableEverything()
	{
	}

	// Token: 0x0400067F RID: 1663
	public GameObject Eyes;

	// Token: 0x04000680 RID: 1664
	public GameObject Hair;

	// Token: 0x04000681 RID: 1665
	public GameObject Top;

	// Token: 0x04000682 RID: 1666
	public GameObject Bottom;

	// Token: 0x04000683 RID: 1667
	public GameObject Body;

	// Token: 0x04000684 RID: 1668
	public GameObject Eyebrows;

	// Token: 0x04000685 RID: 1669
	public GameObject Mouth;

	// Token: 0x04000686 RID: 1670
	private GameObject[] _bodyParts;
}
