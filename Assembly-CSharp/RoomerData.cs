using System;
using System.Linq;
using Team17.Common;
using UnityEngine;

// Token: 0x02000129 RID: 297
[Serializable]
public class RoomerData
{
	// Token: 0x06000A3D RID: 2621 RVA: 0x0003B1CC File Offset: 0x000393CC
	private string IsAwakenedFlag()
	{
		if (this.DateableObjectName == null)
		{
			T17Debug.LogError("DateableObjectName is null. Did you forget to assign a name in the inspector?");
		}
		return this.DateableObjectName.ToLowerInvariant();
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x0003B1EB File Offset: 0x000393EB
	public bool HasActiveClue()
	{
		return this.Clues[1].IsRevealed() || this.Clues[2].IsRevealed();
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0003B20C File Offset: 0x0003940C
	public bool GetIsAwakened()
	{
		InteractableObj interactableObj = Object.FindObjectsOfType<InteractableObj>().FirstOrDefault((InteractableObj i) => i.inkFileName == this.DateableObjectName);
		return !(interactableObj == null) && Singleton<Save>.Instance.GetDateStatus(interactableObj.internalCharacterName) > RelationshipStatus.Unmet;
	}

	// Token: 0x0400095F RID: 2399
	public string Title;

	// Token: 0x04000960 RID: 2400
	public string DateableObjectName;

	// Token: 0x04000961 RID: 2401
	[Range(1f, 5f)]
	public int Priority = 1;

	// Token: 0x04000962 RID: 2402
	public RoomerData.Clue[] Clues = new RoomerData.Clue[3];

	// Token: 0x0200032F RID: 815
	[Serializable]
	public class Clue
	{
		// Token: 0x06001730 RID: 5936 RVA: 0x0006A4D1 File Offset: 0x000686D1
		public Clue()
		{
			this.Description = "Lorem ipsum descriptum.";
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x0006A4E4 File Offset: 0x000686E4
		public Clue(string description, params string[] args)
		{
			this.Description = description;
			this.InkFlags = args;
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x0006A4FC File Offset: 0x000686FC
		public bool IsRevealed()
		{
			bool flag = true;
			foreach (string text in this.InkFlags)
			{
				object obj = Singleton<InkController>.Instance.story.variablesState[text];
				if (flag)
				{
					flag = obj != null && (bool)obj;
				}
			}
			return flag;
		}

		// Token: 0x04001299 RID: 4761
		public string Description;

		// Token: 0x0400129A RID: 4762
		public string[] InkFlags;
	}
}
