using System;
using Team17.Platform.SaveGame;

// Token: 0x02000189 RID: 393
public static class SaveResult
{
	// Token: 0x06000DAB RID: 3499 RVA: 0x0004D000 File Offset: 0x0004B200
	public static SaveResult<T> FromResult<T>(in T result, uint version = 4294967295U)
	{
		SaveGameErrorType saveGameErrorType = SaveGameErrorType.None;
		return new SaveResult<T>(in saveGameErrorType, in result, version);
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x0004D018 File Offset: 0x0004B218
	public static SaveResult<T> FromError<T>(in SaveGameErrorType error)
	{
		T t = default(T);
		return new SaveResult<T>(in error, in t, uint.MaxValue);
	}
}
