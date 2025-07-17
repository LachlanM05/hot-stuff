using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class InkVariableTool : MonoBehaviour
{
	// Token: 0x0600033E RID: 830 RVA: 0x000151EA File Offset: 0x000133EA
	private void Update()
	{
	}

	// Token: 0x0600033F RID: 831 RVA: 0x000151EC File Offset: 0x000133EC
	protected void OnGUI()
	{
	}

	// Token: 0x06000340 RID: 832 RVA: 0x000151F0 File Offset: 0x000133F0
	private void DoMyWindow(int windowID)
	{
		GUIStyle guistyle = new GUIStyle();
		guistyle.fontSize = 18;
		GUIStyle guistyle2 = new GUIStyle();
		guistyle2.fontSize = 18;
		guistyle2.border.left = 1;
		guistyle2.border.top = 1;
		guistyle2.border.right = 1;
		guistyle2.border.bottom = 1;
		int num = 0;
		this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, guistyle);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("ALL INK VARIABLES:", Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		foreach (string text in Singleton<InkController>.Instance.story.variablesState.OrderBy((string x) => x).ToList<string>())
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label(text + ":", Array.Empty<GUILayoutOption>());
			GUILayout.Label(Singleton<InkController>.Instance.story.variablesState[text].ToString(), Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			num++;
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	// Token: 0x04000346 RID: 838
	private bool _toolEnabled;

	// Token: 0x04000347 RID: 839
	private const KeyCode _KEY2 = KeyCode.F8;

	// Token: 0x04000348 RID: 840
	private Vector2 scrollPosition;
}
