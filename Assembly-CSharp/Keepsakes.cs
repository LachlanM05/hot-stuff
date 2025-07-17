using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A1 RID: 161
public class Keepsakes : MonoBehaviour
{
	// Token: 0x06000555 RID: 1365 RVA: 0x0001F3E2 File Offset: 0x0001D5E2
	private void Awake()
	{
		Keepsakes.Instance = this;
		UnityEvent dialogueExit = Singleton<GameController>.Instance.DialogueExit;
		if (dialogueExit == null)
		{
			return;
		}
		dialogueExit.AddListener(new UnityAction(this.UpdateExamines));
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0001F40C File Offset: 0x0001D60C
	public void UpdateExamines()
	{
		this.GetKeepsakeLevel();
		for (int i = 0; i < this.Examines.Length; i++)
		{
			this.Examines[i].InkNode = this.GetKeepsakeString();
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0001F448 File Offset: 0x0001D648
	private void GetKeepsakeLevel()
	{
		for (int i = 0; i < this.Examines.Length; i++)
		{
			if (this.Examines[i].gameObject.activeInHierarchy)
			{
				this.keepsakeLevel = i + 1;
			}
		}
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0001F488 File Offset: 0x0001D688
	private string GetKeepsakeString()
	{
		switch (this.keepsakeLevel)
		{
		case 1:
			return "Keepsakes_Attic_3percent";
		case 2:
			return "Keepsakes_Attic_50percent";
		case 3:
			return "Keepsakes_Attic_75percent";
		default:
			return "Keepsakes_Attic_100percent";
		}
	}

	// Token: 0x04000538 RID: 1336
	public static Keepsakes Instance;

	// Token: 0x04000539 RID: 1337
	[SerializeField]
	private ObjectExamine[] Examines;

	// Token: 0x0400053A RID: 1338
	private int keepsakeLevel = 4;
}
