using System;
using TMPro;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class SelfHelpButton : MonoBehaviour
{
	// Token: 0x06000ABB RID: 2747 RVA: 0x0003D954 File Offset: 0x0003BB54
	public void ClickButton(int number)
	{
		if (!this.isOpen)
		{
			this.SelfHelpScreen.GetComponent<Animator>().SetTrigger("ShowSelfHelp" + number.ToString());
			this.isOpen = true;
			this.titleGameObject.SetActive(true);
			this.descriptionGameObject.SetActive(true);
			base.Invoke("InitiateRoomersData", 1f);
			return;
		}
		this.SelfHelpScreen.GetComponent<Animator>().SetTrigger("HideSelfHelp" + number.ToString());
		this.isOpen = false;
		this.titleGameObject.GetComponent<TextMeshProUGUI>().SetText("", true);
		this.descriptionGameObject.GetComponent<TextMeshProUGUI>().SetText("", true);
		this.titleGameObject.SetActive(false);
		this.descriptionGameObject.SetActive(false);
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0003DA28 File Offset: 0x0003BC28
	private void InitiateRoomersData()
	{
		Singleton<InkController>.Instance.JumpToKnot("healpth." + this.inkTag);
		if (Singleton<InkController>.Instance.story.state.currentPathString.Contains(this.inkTag) && Singleton<InkController>.Instance.story.canContinue)
		{
			Singleton<InkController>.Instance.ContinueStory();
			string text = "";
			string text2 = "";
			foreach (string text3 in Singleton<InkController>.Instance.story.currentTags)
			{
				if (text3.ToLowerInvariant().StartsWith("title:"))
				{
					text = text3.Substring(text3.IndexOf("title:") + 6).Trim();
				}
				else if (text3.ToLowerInvariant().StartsWith("description:"))
				{
					text2 = text3.Substring(text3.IndexOf("description:") + 13).Trim();
				}
			}
			if (text != "")
			{
				this.titleGameObject.GetComponent<TextMeshProUGUI>().SetText(text, true);
				this.descriptionGameObject.GetComponent<TextMeshProUGUI>().SetText(text2, true);
				text = "";
				text2 = "";
			}
		}
	}

	// Token: 0x040009BC RID: 2492
	private bool isOpen;

	// Token: 0x040009BD RID: 2493
	public GameObject SelfHelpScreen;

	// Token: 0x040009BE RID: 2494
	public string inkTag;

	// Token: 0x040009BF RID: 2495
	public GameObject titleGameObject;

	// Token: 0x040009C0 RID: 2496
	public GameObject descriptionGameObject;
}
