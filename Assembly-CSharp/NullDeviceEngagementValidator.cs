using System;
using Team17.Input;
using Team17.Platform.EngagementService;

// Token: 0x02000170 RID: 368
public class NullDeviceEngagementValidator : IDeviceEngagementValidator
{
	// Token: 0x06000D1B RID: 3355 RVA: 0x0004B5CA File Offset: 0x000497CA
	public bool IsDeviceAllowedToEngage(IDeviceId deviceId)
	{
		return true;
	}
}
