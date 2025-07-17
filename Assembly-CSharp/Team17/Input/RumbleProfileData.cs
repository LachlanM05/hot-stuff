using System;
using UnityEngine;

namespace Team17.Input
{
	// Token: 0x020001E1 RID: 481
	[CreateAssetMenu(menuName = "Settings/Controller Rumble/Rumble Profile Data For Override")]
	public class RumbleProfileData : ScriptableObject
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x00055646 File Offset: 0x00053846
		public AnimationCurve RumbleCurveLeft
		{
			get
			{
				return this._rumbleCurveLeft;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x0005564E File Offset: 0x0005384E
		public AnimationCurve RumbleCurveRight
		{
			get
			{
				return this._rumbleCurveRight;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x00055656 File Offset: 0x00053856
		public float Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x04000DE2 RID: 3554
		[Header("Rumble Values")]
		[SerializeField]
		private AnimationCurve _rumbleCurveLeft;

		// Token: 0x04000DE3 RID: 3555
		[SerializeField]
		private AnimationCurve _rumbleCurveRight;

		// Token: 0x04000DE4 RID: 3556
		[Range(0f, 5f)]
		[SerializeField]
		private float _length;
	}
}
