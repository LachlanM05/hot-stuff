using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class HouseReenterTrigger : MonoBehaviour
{
	// Token: 0x0600025D RID: 605 RVA: 0x0000DCF5 File Offset: 0x0000BEF5
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Dresser.Instance.Inside();
			base.gameObject.SetActive(false);
		}
	}
}
