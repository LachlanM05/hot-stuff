using System;
using Team17.Platform.SaveGame;

// Token: 0x02000188 RID: 392
public readonly struct SaveResult<T>
{
	// Token: 0x06000DA9 RID: 3497 RVA: 0x0004CFD5 File Offset: 0x0004B1D5
	internal SaveResult(in SaveGameErrorType error, in T result, uint version)
	{
		this.Error = error;
		this.Result = result;
		this.Version = version;
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x0004CFF2 File Offset: 0x0004B1F2
	public bool IsSuccess()
	{
		return this.Error == SaveGameErrorType.None;
	}

	// Token: 0x04000C47 RID: 3143
	public readonly SaveGameErrorType Error;

	// Token: 0x04000C48 RID: 3144
	public readonly T Result;

	// Token: 0x04000C49 RID: 3145
	public readonly uint Version;
}
