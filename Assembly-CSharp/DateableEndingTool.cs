using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class DateableEndingTool : MonoBehaviour
{
	// Token: 0x0600025F RID: 607 RVA: 0x0000DD27 File Offset: 0x0000BF27
	private void Update()
	{
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0000DD29 File Offset: 0x0000BF29
	private void Start()
	{
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000DD2C File Offset: 0x0000BF2C
	protected void OnGUI()
	{
		if (this.toolEnabled)
		{
			Rect rect = new Rect(20f, 20f, 150f, 150f);
			rect = GUILayout.Window(0, rect, new GUI.WindowFunction(this.DoMyWindow), "Dateable Ending Tool", Array.Empty<GUILayoutOption>());
		}
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000DD7C File Offset: 0x0000BF7C
	private void DoMyWindow(int windowID)
	{
		GUILayout.Label("Select an Ending:", Array.Empty<GUILayoutOption>());
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		if (GUILayout.Button("Friend", Array.Empty<GUILayoutOption>()))
		{
			this.TriggerEnding(RelationshipStatus.Friend);
		}
		if (GUILayout.Button("Love", Array.Empty<GUILayoutOption>()))
		{
			this.TriggerEnding(RelationshipStatus.Love);
		}
		if (GUILayout.Button("Hate", Array.Empty<GUILayoutOption>()))
		{
			this.TriggerEnding(RelationshipStatus.Hate);
		}
		GUILayout.EndVertical();
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000DDF0 File Offset: 0x0000BFF0
	private void TriggerEnding(RelationshipStatus relationshipStatus)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		DateADex.Instance.UnlockEnding(text, relationshipStatus, false, false);
	}

	// Token: 0x0400032C RID: 812
	private bool toolEnabled;
}
