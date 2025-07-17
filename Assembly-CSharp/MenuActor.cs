using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class MenuActor : MonoBehaviour
{
	// Token: 0x060001E5 RID: 485 RVA: 0x0000BACD File Offset: 0x00009CCD
	private void Start()
	{
		base.transform.GetChild(0).gameObject.SetActive(false);
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0000BAE6 File Offset: 0x00009CE6
	private void OnMouseOver()
	{
		base.transform.GetChild(0).gameObject.SetActive(true);
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0000BAFF File Offset: 0x00009CFF
	private void OnMouseExit()
	{
		base.transform.GetChild(0).gameObject.SetActive(false);
	}
}
