using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000192 RID: 402
public static class LogUtil
{
	// Token: 0x06000DD4 RID: 3540 RVA: 0x0004D783 File Offset: 0x0004B983
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void Log(string strLog, string strSubject = "")
	{
		LogUtil.StartLogReflection();
		Debug.Log(strLog + LogUtil.GetStackTrace(2));
		LogUtil.StopLogReflection();
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0004D7A0 File Offset: 0x0004B9A0
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void Log(bool bMustBeTrue, string strLog, string strSubject = "")
	{
		if (bMustBeTrue)
		{
			object obj = strLog + LogUtil.GetStackTrace(2);
			LogUtil.StartLogReflection();
			Debug.Log(obj);
			LogUtil.StopLogReflection();
		}
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x0004D7C0 File Offset: 0x0004B9C0
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void Log(string strLog, int iStackIndex)
	{
		LogUtil.StartLogReflection();
		Debug.Log(strLog + LogUtil.GetStackTrace(2));
		LogUtil.StopLogReflection();
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0004D7DD File Offset: 0x0004B9DD
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void LogError(string strLog, string strSubject = "")
	{
		LogUtil.StartLogReflection();
		Debug.LogError(strLog + LogUtil.GetStackTrace(2));
		LogUtil.StopLogReflection();
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x0004D7FA File Offset: 0x0004B9FA
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void LogError(bool bMustBeTrue, string strLog, string strSubject = "")
	{
		if (bMustBeTrue)
		{
			LogUtil.StartLogReflection();
			Debug.LogError(strLog + LogUtil.GetStackTrace(2));
			LogUtil.StopLogReflection();
		}
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x0004D81A File Offset: 0x0004BA1A
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void LogError(string strLog, int iStackIndex)
	{
		LogUtil.StartLogReflection();
		Debug.LogError(strLog + LogUtil.GetStackTrace(2));
		LogUtil.StopLogReflection();
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x0004D837 File Offset: 0x0004BA37
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void LogWarning(string strLog, string strSubject = "")
	{
		LogUtil.StartLogReflection();
		Debug.LogWarning(strLog + LogUtil.GetStackTrace(2));
		LogUtil.StopLogReflection();
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x0004D854 File Offset: 0x0004BA54
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void LogWarning(bool bMustBeTrue, string strLog, string strSubject = "")
	{
		if (bMustBeTrue)
		{
			LogUtil.StartLogReflection();
			Debug.LogWarning(strLog + LogUtil.GetStackTrace(2));
			LogUtil.StopLogReflection();
		}
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x0004D874 File Offset: 0x0004BA74
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void LogWarning(string strLog, int iStackIndex)
	{
		LogUtil.StartLogReflection();
		Debug.LogWarning(strLog + LogUtil.GetStackTrace(2));
		LogUtil.StopLogReflection();
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x0004D891 File Offset: 0x0004BA91
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void LogException(Exception e, string strSubject = "")
	{
		LogUtil.StartLogReflection();
		Debug.LogException(e);
		LogUtil.StopLogReflection();
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x0004D8A3 File Offset: 0x0004BAA3
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void LogException(bool bMustBeTrue, Exception e, string strSubject = "")
	{
		if (bMustBeTrue)
		{
			LogUtil.StartLogReflection();
			Debug.LogException(e);
			LogUtil.StopLogReflection();
		}
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x0004D8B8 File Offset: 0x0004BAB8
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void LogException(Exception e, int iStackIndex)
	{
		LogUtil.StartLogReflection();
		Debug.LogException(e);
		LogUtil.StopLogReflection();
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x0004D8CC File Offset: 0x0004BACC
	private static string GetStackTrace(int skipFrames = 2)
	{
		StackFrame frame = new StackTrace(skipFrames, true).GetFrame(0);
		string text = frame.GetFileName();
		int num = text.LastIndexOf("Assets\\");
		if (num >= 0)
		{
			text = text.Substring(num + 7);
		}
		string[] array = new string[7];
		array[0] = "\n";
		array[1] = text;
		array[2] = ": <b>";
		int num2 = 3;
		MethodBase method = frame.GetMethod();
		array[num2] = ((method != null) ? method.ToString() : null);
		array[4] = "</b> - Line <b>";
		array[5] = frame.GetFileLineNumber().ToString();
		array[6] = "</b>\n\n";
		return string.Concat(array);
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x0004D960 File Offset: 0x0004BB60
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void GUILogError(string strLog, string strSubject)
	{
		if (LogUtil.m_ImmediateGUILogText == null)
		{
			LogUtil.CreateLogGUI();
		}
		if (LogUtil.m_ImmediateGUILogText != null)
		{
			LogUtil.CheckGUILimit();
			Text immediateGUILogText = LogUtil.m_ImmediateGUILogText;
			immediateGUILogText.text = immediateGUILogText.text + "<color=#800000ff>ERROR: " + strLog + "\n</color>";
			LogUtil.m_LineBreaks++;
		}
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x0004D9C0 File Offset: 0x0004BBC0
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void GUILogWarning(string strLog, string strSubject)
	{
		if (LogUtil.m_ImmediateGUILogText == null)
		{
			LogUtil.CreateLogGUI();
		}
		if (LogUtil.m_ImmediateGUILogText != null)
		{
			LogUtil.CheckGUILimit();
			Text immediateGUILogText = LogUtil.m_ImmediateGUILogText;
			immediateGUILogText.text = immediateGUILogText.text + "<color=#ffa500ff>WARNING: " + strLog + "\n</color>";
			LogUtil.m_LineBreaks++;
		}
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x0004DA20 File Offset: 0x0004BC20
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void GUILogDebug(string strLog, string strSubject)
	{
		Debug.Log(strLog + LogUtil.GetStackTrace(2));
		if (LogUtil.m_ImmediateGUILogText == null)
		{
			LogUtil.CreateLogGUI();
		}
		if (LogUtil.m_ImmediateGUILogText != null)
		{
			LogUtil.CheckGUILimit();
			Text immediateGUILogText = LogUtil.m_ImmediateGUILogText;
			immediateGUILogText.text = immediateGUILogText.text + "<color=#222222ff>DEBUG: " + strLog + "</color>\n";
			LogUtil.m_LineBreaks++;
		}
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x0004DA90 File Offset: 0x0004BC90
	[Conditional("THIS_DOESN_DO_ANYTHING")]
	public static void GUILogDebug_Updating(string strLog, string strSubject, string colour = "222222")
	{
		if (LogUtil.m_ImmediateUpdatingGUILogText == null)
		{
			LogUtil.CreateLogGUI_Updating();
		}
		if (LogUtil.m_ImmediateUpdatingGUILogText != null)
		{
			LogUtil.m_ImmediateUpdatingGUILogText.text = string.Concat(new string[] { "DEBUG: <color=#", colour, "ff>", strLog, "</color>" });
		}
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x0004DAF4 File Offset: 0x0004BCF4
	private static void CreateLogGUI()
	{
		LogUtil.m_ImmediateGUILogText = new GameObject("LogUtil_GUITEXT").AddComponent<Text>();
		LogUtil.m_ImmediateGUILogText.alignment = TextAnchor.UpperLeft;
		LogUtil.m_ImmediateGUILogText.transform.position = new Vector3(0f, 0.85f, 0f);
		LogUtil.m_ImmediateGUILogText.fontSize = 15;
		LogUtil.m_ImmediateGUILogText.fontStyle = FontStyle.BoldAndItalic;
		LogUtil.m_ImmediateGUILogText.text = "";
		LogUtil.m_ImmediateGUILogText.supportRichText = true;
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x0004DB74 File Offset: 0x0004BD74
	private static void CreateLogGUI_Updating()
	{
		LogUtil.m_ImmediateUpdatingGUILogText = new GameObject("LogUtil_GUITEXT_UPDATING").AddComponent<Text>();
		LogUtil.m_ImmediateUpdatingGUILogText.alignment = TextAnchor.UpperRight;
		LogUtil.m_ImmediateUpdatingGUILogText.transform.position = new Vector3(0.99f, 0.85f, 0f);
		LogUtil.m_ImmediateUpdatingGUILogText.fontSize = 15;
		LogUtil.m_ImmediateUpdatingGUILogText.fontStyle = FontStyle.BoldAndItalic;
		LogUtil.m_ImmediateUpdatingGUILogText.text = "";
		LogUtil.m_ImmediateUpdatingGUILogText.supportRichText = true;
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x0004DBF4 File Offset: 0x0004BDF4
	private static void CheckGUILimit()
	{
		if (LogUtil.m_ImmediateGUILogText.text.Length > LogUtil.m_GUICharacterClearLimit || LogUtil.m_LineBreaks >= LogUtil.m_LineBreakLimit)
		{
			LogUtil.LogGUIClear();
		}
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x0004DC1D File Offset: 0x0004BE1D
	public static void LogGUIClear()
	{
		LogUtil.m_LineBreaks = 0;
		if (LogUtil.m_ImmediateGUILogText != null)
		{
			LogUtil.m_ImmediateGUILogText.text = "";
		}
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x0004DC44 File Offset: 0x0004BE44
	public static void ToggleGUILog()
	{
		if (LogUtil.m_ImmediateUpdatingGUILogText != null)
		{
			LogUtil.m_ImmediateUpdatingGUILogText.enabled = !LogUtil.m_ImmediateUpdatingGUILogText.enabled;
		}
		if (LogUtil.m_ImmediateGUILogText != null)
		{
			LogUtil.m_ImmediateGUILogText.enabled = !LogUtil.m_ImmediateGUILogText.enabled;
		}
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x0004DC99 File Offset: 0x0004BE99
	public static void SetGUIMaxLineBreaks(int maxLinebreaks = 20)
	{
		LogUtil.m_LineBreakLimit = maxLinebreaks;
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x0004DCA1 File Offset: 0x0004BEA1
	public static void SetGUIMaxCharacters(int charLimit = 3000)
	{
		LogUtil.m_GUICharacterClearLimit = charLimit;
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0004DCA9 File Offset: 0x0004BEA9
	private static void StartLogReflection()
	{
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x0004DCAB File Offset: 0x0004BEAB
	private static void StopLogReflection()
	{
	}

	// Token: 0x04000C65 RID: 3173
	private static Text m_ImmediateGUILogText = null;

	// Token: 0x04000C66 RID: 3174
	private static Text m_ImmediateUpdatingGUILogText = null;

	// Token: 0x04000C67 RID: 3175
	private static int m_GUICharacterClearLimit = 3000;

	// Token: 0x04000C68 RID: 3176
	private static int m_LineBreakLimit = 20;

	// Token: 0x04000C69 RID: 3177
	private static int m_LineBreaks = 0;
}
