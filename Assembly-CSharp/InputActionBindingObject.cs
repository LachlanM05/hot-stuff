using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
[CreateAssetMenu(menuName = "ScriptableObjects/Team17/Input Action Binding Definition ", fileName = "InputActionBindingDefinition")]
public class InputActionBindingObject : ScriptableObject
{
	// Token: 0x06000D41 RID: 3393 RVA: 0x0004BF3C File Offset: 0x0004A13C
	public string GetRewiredActionName(string gameActionTag)
	{
		int i = 0;
		int num = this.m_GameBindingTags.Length;
		while (i < num)
		{
			if (this.m_GameBindingTags[i].GameMarkupTag == gameActionTag)
			{
				return this.m_GameBindingTags[i].RewiredAction;
			}
			i++;
		}
		return string.Empty;
	}

	// Token: 0x04000BFD RID: 3069
	[SerializeField]
	private InputActionBindingObject.GameRewiredMarkupPair[] m_GameBindingTags;

	// Token: 0x02000369 RID: 873
	[Serializable]
	internal struct GameRewiredMarkupPair
	{
		// Token: 0x0400136F RID: 4975
		public string GameMarkupTag;

		// Token: 0x04001370 RID: 4976
		public string RewiredAction;
	}
}
