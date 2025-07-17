using System;

// Token: 0x02000184 RID: 388
public interface IRateLimiter
{
	// Token: 0x06000D91 RID: 3473
	bool CanMakeRequest(float time);

	// Token: 0x06000D92 RID: 3474
	bool CanMakeRequests(float time, int requestCount);
}
