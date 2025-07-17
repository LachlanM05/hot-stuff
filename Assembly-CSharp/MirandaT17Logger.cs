using System;
using Team17.Common;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class MirandaT17Logger : Team17.Common.ILogger
{
	// Token: 0x06000DEF RID: 3567 RVA: 0x0004DCD2 File Offset: 0x0004BED2
	public void Log(object log)
	{
		Debug.Log(this.FormatLogMessage(log));
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x0004DCE0 File Offset: 0x0004BEE0
	public void LogError(object log)
	{
		Debug.LogError(this.FormatLogMessage(log));
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0004DCEE File Offset: 0x0004BEEE
	public void LogWarning(object log)
	{
		Debug.LogWarning(this.FormatLogMessage(log));
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x0004DCFC File Offset: 0x0004BEFC
	private string FormatLogMessage(object log)
	{
		string text = DateTime.Now.ToString("HH:mm:ss.fff");
		return string.Format("[{0}]{1}", text, log);
	}
}
