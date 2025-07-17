using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Date_Everything.Scripts.UI.Loading
{
	// Token: 0x0200025B RID: 603
	internal class LoadingFacts : MonoBehaviour
	{
		// Token: 0x060013A5 RID: 5029 RVA: 0x0005D9A4 File Offset: 0x0005BBA4
		public void Start()
		{
			int num = Random.Range(0, this.facts.Count);
			base.GetComponent<TextMeshPro_T17>().text = this.facts[num];
		}

		// Token: 0x04000F48 RID: 3912
		[SerializeField]
		private List<string> facts = new List<string>();
	}
}
