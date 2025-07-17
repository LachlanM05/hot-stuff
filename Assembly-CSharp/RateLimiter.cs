using System;
using System.Collections.Generic;

// Token: 0x02000186 RID: 390
public class RateLimiter : IRateLimiter
{
	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000D95 RID: 3477 RVA: 0x0004CC04 File Offset: 0x0004AE04
	public int Limit
	{
		get
		{
			return this.m_Limit;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0004CC0C File Offset: 0x0004AE0C
	public float TimeIntervalSeconds
	{
		get
		{
			return this.m_TimeIntervalSeconds;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0004CC14 File Offset: 0x0004AE14
	public int RequestsWithinPeriod
	{
		get
		{
			return this.m_RequestsWithinTimeInterval.Count;
		}
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x0004CC21 File Offset: 0x0004AE21
	public RateLimiter(int limit, float timeIntervalSeconds)
	{
		this.m_Limit = limit;
		this.m_TimeIntervalSeconds = timeIntervalSeconds;
		this.m_RequestsWithinTimeInterval = new Queue<float>();
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x0004CC42 File Offset: 0x0004AE42
	public bool CanMakeRequest(float time)
	{
		return this.CanMakeRequests(time, 1);
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x0004CC4C File Offset: 0x0004AE4C
	public bool CanMakeRequests(float time, int requestCount)
	{
		this.DequeueExpiredRequests(time);
		if (this.m_Limit - this.m_RequestsWithinTimeInterval.Count < requestCount)
		{
			return false;
		}
		for (int i = 0; i < requestCount; i++)
		{
			this.m_RequestsWithinTimeInterval.Enqueue(time);
		}
		return true;
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x0004CC90 File Offset: 0x0004AE90
	private void DequeueExpiredRequests(float time)
	{
		while (this.m_RequestsWithinTimeInterval.Count > 0)
		{
			if (time - this.m_RequestsWithinTimeInterval.Peek() <= this.m_TimeIntervalSeconds)
			{
				return;
			}
			this.m_RequestsWithinTimeInterval.Dequeue();
		}
	}

	// Token: 0x04000C37 RID: 3127
	public static readonly IRateLimiter Unlimited = new RateLimiter.UnlimitedRateLimiter();

	// Token: 0x04000C38 RID: 3128
	private int m_Limit;

	// Token: 0x04000C39 RID: 3129
	private float m_TimeIntervalSeconds;

	// Token: 0x04000C3A RID: 3130
	private readonly Queue<float> m_RequestsWithinTimeInterval;

	// Token: 0x0200036E RID: 878
	private class UnlimitedRateLimiter : IRateLimiter
	{
		// Token: 0x060017EE RID: 6126 RVA: 0x0006CB0B File Offset: 0x0006AD0B
		public bool CanMakeRequest(float time)
		{
			return true;
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0006CB0E File Offset: 0x0006AD0E
		public bool CanMakeRequests(float time, int requestCount)
		{
			return true;
		}
	}
}
