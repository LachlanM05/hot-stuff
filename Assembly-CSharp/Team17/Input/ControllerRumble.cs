using System;
using UnityEngine;

namespace Team17.Input
{
	// Token: 0x020001DF RID: 479
	public class ControllerRumble
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x0005549D File Offset: 0x0005369D
		public RumbleProfileData ProfileData
		{
			get
			{
				return this._profileData;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x000554A5 File Offset: 0x000536A5
		// (set) Token: 0x0600102B RID: 4139 RVA: 0x000554AD File Offset: 0x000536AD
		public float LeftRumble { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x000554B6 File Offset: 0x000536B6
		// (set) Token: 0x0600102D RID: 4141 RVA: 0x000554BE File Offset: 0x000536BE
		public float RightRumble { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x000554C7 File Offset: 0x000536C7
		public bool IsCompleted
		{
			get
			{
				return !this._looping && this._timeLeft <= 0f;
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x000554E3 File Offset: 0x000536E3
		public void Setup(RumbleProfileData rumbleProfileData, bool isLooping)
		{
			this._timeLeft = rumbleProfileData.Length;
			this._looping = isLooping;
			this._profileData = rumbleProfileData;
			this.LeftRumble = 0f;
			this.RightRumble = 0f;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00055515 File Offset: 0x00053715
		public void Clear()
		{
			this._timeLeft = 0f;
			this._looping = false;
			this._profileData = null;
			this.LeftRumble = 0f;
			this.RightRumble = 0f;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00055546 File Offset: 0x00053746
		public void Update(float deltaTime)
		{
			this.UpdateRumbleValues();
			this.UpdateTimeLeft(deltaTime);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00055558 File Offset: 0x00053758
		private void UpdateRumbleValues()
		{
			if (this.IsCompleted)
			{
				this.LeftRumble = 0f;
				this.RightRumble = 0f;
				return;
			}
			float num = 1f - this._timeLeft / this._profileData.Length;
			this.LeftRumble = this._profileData.RumbleCurveLeft.Evaluate(num);
			this.RightRumble = this._profileData.RumbleCurveRight.Evaluate(num);
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x000555CC File Offset: 0x000537CC
		private void UpdateTimeLeft(float deltaTime)
		{
			if (this.IsCompleted)
			{
				return;
			}
			this._timeLeft -= deltaTime;
			while (this._timeLeft <= 0f && this._looping)
			{
				this._timeLeft += Mathf.Abs(this._profileData.Length);
			}
		}

		// Token: 0x04000DDC RID: 3548
		private float _timeLeft;

		// Token: 0x04000DDD RID: 3549
		private bool _looping;

		// Token: 0x04000DDE RID: 3550
		private RumbleProfileData _profileData;
	}
}
