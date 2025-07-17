using System;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class SPECS_Debugger : MonoBehaviour
{
	// Token: 0x06000B4A RID: 2890 RVA: 0x000406DF File Offset: 0x0003E8DF
	private void Start()
	{
		this.GetAllStats();
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x000406E7 File Offset: 0x0003E8E7
	private void OnEnable()
	{
		this.GetAllStats();
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x000406F0 File Offset: 0x0003E8F0
	private void Update()
	{
		foreach (SPECS_Debugger.StatTracker statTracker in this.Stats)
		{
			if (statTracker.CheckForUpdate())
			{
				Singleton<SpecStatMain>.Instance.SetStatPoint(statTracker.Name, statTracker.Value, 0);
			}
		}
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x00040738 File Offset: 0x0003E938
	private void GetAllStats()
	{
		if (Singleton<DeluxeEditionController>.Instance != null)
		{
			foreach (SPECS_Debugger.StatTracker statTracker in this.Stats)
			{
				statTracker.SetValue(Singleton<SpecStatMain>.Instance.GetStatLevel(statTracker.Name, Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION));
			}
			return;
		}
		foreach (SPECS_Debugger.StatTracker statTracker2 in this.Stats)
		{
			statTracker2.SetValue(Singleton<SpecStatMain>.Instance.GetStatLevel(statTracker2.Name, false));
		}
	}

	// Token: 0x04000A3F RID: 2623
	[Header("Stat Sliders")]
	[SerializeField]
	private SPECS_Debugger.StatTracker[] Stats = new SPECS_Debugger.StatTracker[]
	{
		new SPECS_Debugger.StatTracker("Poise"),
		new SPECS_Debugger.StatTracker("Empathy"),
		new SPECS_Debugger.StatTracker("Charm"),
		new SPECS_Debugger.StatTracker("Sass"),
		new SPECS_Debugger.StatTracker("Smarts")
	};

	// Token: 0x02000349 RID: 841
	[Serializable]
	private class StatTracker
	{
		// Token: 0x0600177D RID: 6013 RVA: 0x0006B14B File Offset: 0x0006934B
		public StatTracker(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x0006B15A File Offset: 0x0006935A
		public StatTracker(string name, int value)
		{
			this.Name = name;
			this.Value = value;
			this.cachedValue = this.Value;
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x0006B17C File Offset: 0x0006937C
		public bool CheckForUpdate()
		{
			if (this.Value != this.cachedValue)
			{
				this.cachedValue = this.Value;
				return true;
			}
			return false;
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x0006B19B File Offset: 0x0006939B
		public void SetValue(int value)
		{
			this.Value = value;
			this.cachedValue = value;
		}

		// Token: 0x040012F1 RID: 4849
		public string Name;

		// Token: 0x040012F2 RID: 4850
		[Range(0f, 100f)]
		public int Value;

		// Token: 0x040012F3 RID: 4851
		private int cachedValue;
	}
}
