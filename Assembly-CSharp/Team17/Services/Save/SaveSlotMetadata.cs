using System;
using System.Linq;
using Newtonsoft.Json;

namespace Team17.Services.Save
{
	// Token: 0x020001D7 RID: 471
	[Serializable]
	public class SaveSlotMetadata
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x0005409A File Offset: 0x0005229A
		// (set) Token: 0x06000FA6 RID: 4006 RVA: 0x000540A2 File Offset: 0x000522A2
		public int SlotNumber { get; protected set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x000540AB File Offset: 0x000522AB
		// (set) Token: 0x06000FA8 RID: 4008 RVA: 0x000540B3 File Offset: 0x000522B3
		public string Name { get; protected set; } = "";

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x000540BC File Offset: 0x000522BC
		// (set) Token: 0x06000FAA RID: 4010 RVA: 0x000540C4 File Offset: 0x000522C4
		public string TimeStamp { get; protected set; } = "";

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x000540CD File Offset: 0x000522CD
		// (set) Token: 0x06000FAC RID: 4012 RVA: 0x000540D5 File Offset: 0x000522D5
		public bool NewGamePlus { get; protected set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x000540DE File Offset: 0x000522DE
		// (set) Token: 0x06000FAE RID: 4014 RVA: 0x000540E6 File Offset: 0x000522E6
		public string SlotName { get; protected set; } = "";

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x000540EF File Offset: 0x000522EF
		// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x000540F7 File Offset: 0x000522F7
		public string PlayTime { get; protected set; } = "";

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x00054100 File Offset: 0x00052300
		// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x00054108 File Offset: 0x00052308
		public int NumberOfDays { get; protected set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x00054111 File Offset: 0x00052311
		// (set) Token: 0x06000FB4 RID: 4020 RVA: 0x00054119 File Offset: 0x00052319
		public int NumberOfDatablesAwakened { get; protected set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00054122 File Offset: 0x00052322
		// (set) Token: 0x06000FB6 RID: 4022 RVA: 0x0005412A File Offset: 0x0005232A
		public int NumberOfDatablesRealised { get; protected set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x00054133 File Offset: 0x00052333
		// (set) Token: 0x06000FB8 RID: 4024 RVA: 0x0005413B File Offset: 0x0005233B
		public int NumberOfDeluxeDatablesAwakened { get; protected set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x00054144 File Offset: 0x00052344
		// (set) Token: 0x06000FBA RID: 4026 RVA: 0x0005414C File Offset: 0x0005234C
		public int NumberOfDeluxeDatablesRealised { get; protected set; }

		// Token: 0x06000FBB RID: 4027 RVA: 0x00054158 File Offset: 0x00052358
		[JsonConstructor]
		public SaveSlotMetadata(int slotNumber, string name, string timeStamp, bool newGamePlus, string slotName, string playTime, int numberOfDays, int numberOfDatablesAwakened, int numberOfDatablesRealised, int numberOfDeluxeDatablesAwakened, int numberOfDeluxeDatablesRealised)
		{
			this.SlotNumber = slotNumber;
			this.Name = name;
			this.TimeStamp = timeStamp;
			this.NewGamePlus = newGamePlus;
			this.SlotName = slotName;
			this.PlayTime = playTime;
			this.NumberOfDays = numberOfDays;
			this.NumberOfDatablesAwakened = numberOfDatablesAwakened;
			this.NumberOfDatablesRealised = numberOfDatablesRealised;
			this.NumberOfDeluxeDatablesAwakened = numberOfDeluxeDatablesAwakened;
			this.NumberOfDeluxeDatablesRealised = numberOfDeluxeDatablesRealised;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x000541EC File Offset: 0x000523EC
		public SaveSlotMetadata(int slotNumber, Save.SaveData data)
		{
			this.Set(slotNumber, data);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00054228 File Offset: 0x00052428
		public void Set(int slotNumber, Save.SaveData data)
		{
			this.SlotNumber = slotNumber;
			if (data == null)
			{
				this.Name = "";
				this.TimeStamp = Save.GetFormattedTimeNow();
				this.NewGamePlus = false;
				this.SlotName = "";
				this.PlayTime = "";
				this.NumberOfDays = 0;
				this.NumberOfDatablesAwakened = 0;
				this.NumberOfDatablesRealised = 0;
				this.NumberOfDeluxeDatablesAwakened = 0;
				this.NumberOfDeluxeDatablesRealised = 0;
				return;
			}
			this.Name = data.PlayerData.Name;
			this.TimeStamp = data.TimeStamp;
			this.NewGamePlus = data.newGamePlus;
			this.SlotName = string.Empty;
			this.PlayTime = data.playTimeInMillis.ToString();
			this.NumberOfDatablesAwakened = (from x in data.datestates.Take(DeluxeEditionController.NUMBER_OF_BASEGAME_CHARACTERS)
				where x != 0
				select x).Count<int>();
			this.NumberOfDatablesRealised = (from x in data.datestatesRealized.Take(DeluxeEditionController.NUMBER_OF_BASEGAME_CHARACTERS)
				where x != 0
				select x).Count<int>();
			this.NumberOfDeluxeDatablesAwakened = (from x in data.datestates.TakeLast(DeluxeEditionController.NUMBER_OF_DELUXE_CHARACTERS)
				where x != 0
				select x).Count<int>();
			this.NumberOfDeluxeDatablesRealised = (from x in data.datestatesRealized.TakeLast(DeluxeEditionController.NUMBER_OF_DELUXE_CHARACTERS)
				where x != 0
				select x).Count<int>();
			if (Singleton<DayNightCycle>.Instance == null)
			{
				this.NumberOfDays = data.dayNightCycle_daysSinceStart;
				return;
			}
			this.NumberOfDays = Singleton<DayNightCycle>.Instance.GetDaysSinceStart();
		}

		// Token: 0x0200039A RID: 922
		public static class Version
		{
			// Token: 0x170001FA RID: 506
			// (get) Token: 0x0600182D RID: 6189 RVA: 0x0006E47A File Offset: 0x0006C67A
			public static uint Latest
			{
				get
				{
					return 2U;
				}
			}

			// Token: 0x0400142C RID: 5164
			private const int Initial = 0;

			// Token: 0x0400142D RID: 5165
			private const int CombinedSaves = 1;

			// Token: 0x0400142E RID: 5166
			public const int JsonAsBase64 = 2;
		}
	}
}
