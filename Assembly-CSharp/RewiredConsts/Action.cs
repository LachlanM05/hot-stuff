using System;
using Rewired.Dev;

namespace RewiredConsts
{
	// Token: 0x020001BD RID: 445
	public static class Action
	{
		// Token: 0x04000D4D RID: 3405
		[ActionIdFieldInfo(categoryName = "Default", friendlyName = "Pause")]
		public const int Pause = 5;

		// Token: 0x04000D4E RID: 3406
		[ActionIdFieldInfo(categoryName = "Dialog", friendlyName = "Continue")]
		public const int Continue = 0;

		// Token: 0x04000D4F RID: 3407
		[ActionIdFieldInfo(categoryName = "Dialog", friendlyName = "Dialog Up")]
		public const int Dialog_Up = 1;

		// Token: 0x04000D50 RID: 3408
		[ActionIdFieldInfo(categoryName = "Dialog", friendlyName = "Dialog Down")]
		public const int Dialog_Down = 15;

		// Token: 0x04000D51 RID: 3409
		[ActionIdFieldInfo(categoryName = "Dialog", friendlyName = "Primary Response")]
		public const int PrimaryResponse = 39;

		// Token: 0x04000D52 RID: 3410
		[ActionIdFieldInfo(categoryName = "Dialog", friendlyName = "Secondary Response")]
		public const int SecondaryResponse = 40;

		// Token: 0x04000D53 RID: 3411
		[ActionIdFieldInfo(categoryName = "Dialog", friendlyName = "Tetriary Response")]
		public const int TetriaryResponse = 41;

		// Token: 0x04000D54 RID: 3412
		[ActionIdFieldInfo(categoryName = "Dialog", friendlyName = "Quaternary Response")]
		public const int QuaternaryResponse = 42;

		// Token: 0x04000D55 RID: 3413
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "Talk")]
		public const int Talk = 2;

		// Token: 0x04000D56 RID: 3414
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "Move Horizontal")]
		public const int Move_Horizontal = 3;

		// Token: 0x04000D57 RID: 3415
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "Move Vertical")]
		public const int Move_Vertical = 4;

		// Token: 0x04000D58 RID: 3416
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "Look Horizontal")]
		public const int Look_Horizontal = 9;

		// Token: 0x04000D59 RID: 3417
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "Look Vertical")]
		public const int Look_Vertical = 10;

		// Token: 0x04000D5A RID: 3418
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "View Message Log")]
		public const int Message_Log = 11;

		// Token: 0x04000D5B RID: 3419
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "Interact")]
		public const int Interact = 12;

		// Token: 0x04000D5C RID: 3420
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "Awaken Dateable")]
		public const int Awaken = 13;

		// Token: 0x04000D5D RID: 3421
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "Examen")]
		public const int Examen = 18;

		// Token: 0x04000D5E RID: 3422
		[ActionIdFieldInfo(categoryName = "CharacterController", friendlyName = "Crouch")]
		public const int Crouch = 33;

		// Token: 0x04000D5F RID: 3423
		[ActionIdFieldInfo(categoryName = "Engagement Input", friendlyName = "Engagement Input")]
		public const int Engagement = 19;

		// Token: 0x04000D60 RID: 3424
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UI Up")]
		public const int UI_Up = 24;

		// Token: 0x04000D61 RID: 3425
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UI Down")]
		public const int UI_Down = 25;

		// Token: 0x04000D62 RID: 3426
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UI Right")]
		public const int UI_Right = 26;

		// Token: 0x04000D63 RID: 3427
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UI Left")]
		public const int UI_Left = 27;

		// Token: 0x04000D64 RID: 3428
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "Confirm")]
		public const int Confirm = 28;

		// Token: 0x04000D65 RID: 3429
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "Cancel")]
		public const int Cancel = 29;

		// Token: 0x04000D66 RID: 3430
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "Scroll")]
		public const int Scroll = 36;

		// Token: 0x04000D67 RID: 3431
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "Cycle List Up")]
		public const int CycleListUp = 37;

		// Token: 0x04000D68 RID: 3432
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "Cycle List Down")]
		public const int CycleListDown = 38;

		// Token: 0x04000D69 RID: 3433
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UIMenuExtraAction")]
		public const int UIMenuExtraAction = 50;

		// Token: 0x04000D6A RID: 3434
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UIMenuExtraSecondAction")]
		public const int UIMenuExtraSecondAction = 51;

		// Token: 0x04000D6B RID: 3435
		[ActionIdFieldInfo(categoryName = "Debug", friendlyName = "ToggleDebug")]
		public const int ToggleDebug = 22;

		// Token: 0x04000D6C RID: 3436
		[ActionIdFieldInfo(categoryName = "Debug", friendlyName = "Starts a new Performance Marker")]
		public const int NextPerformanceMarker = 31;

		// Token: 0x04000D6D RID: 3437
		[ActionIdFieldInfo(categoryName = "Debug", friendlyName = "Toggle Performance Capture")]
		public const int TogglePerformanceCapture = 32;

		// Token: 0x04000D6E RID: 3438
		[ActionIdFieldInfo(categoryName = "Debug", friendlyName = "DebugPanel")]
		public const int DebugPanel = 43;

		// Token: 0x04000D6F RID: 3439
		[ActionIdFieldInfo(categoryName = "Debug", friendlyName = "IncrementTime")]
		public const int IncrementTime = 44;

		// Token: 0x04000D70 RID: 3440
		[ActionIdFieldInfo(categoryName = "Debug", friendlyName = "DecrementTime")]
		public const int DecrementTime = 45;

		// Token: 0x04000D71 RID: 3441
		[ActionIdFieldInfo(categoryName = "Debug", friendlyName = "ResetCharges")]
		public const int ResetCharges = 46;

		// Token: 0x04000D72 RID: 3442
		[ActionIdFieldInfo(categoryName = "Debug", friendlyName = "ToggleUI")]
		public const int ToggleUI = 47;

		// Token: 0x04000D73 RID: 3443
		[ActionIdFieldInfo(categoryName = "Debug", friendlyName = "UnlockMouse")]
		public const int UnlockMouse = 48;

		// Token: 0x04000D74 RID: 3444
		[ActionIdFieldInfo(categoryName = "Toggle Dateviators", friendlyName = "Toggle Dateviators")]
		public const int Toggle_Dateviators = 52;
	}
}
