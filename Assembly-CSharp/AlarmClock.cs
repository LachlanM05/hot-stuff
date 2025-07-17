using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000070 RID: 112
public class AlarmClock : MonoBehaviour
{
	// Token: 0x060003C4 RID: 964 RVA: 0x00017B2C File Offset: 0x00015D2C
	private void Awake()
	{
		this.material = this.meshRenderer.materials[1];
		Singleton<DayNightCycle>.Instance.DayPhaseUpdated.AddListener(new UnityAction(this.UpdateClock));
		this.UpdateClock();
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00017B64 File Offset: 0x00015D64
	private void UpdateClock()
	{
		if (this.material == null)
		{
			return;
		}
		DayPhase time = Singleton<DayNightCycle>.Instance.GetTime();
		if (this.interactable)
		{
			if ((bool)Singleton<InkController>.Instance.story.variablesState["tim_catboy_perm"])
			{
				this.interactable.friendClipOverride = this.timFriendClip;
				this.interactable.loveClipOverride = this.timLoveClip;
			}
			else
			{
				this.interactable.friendClipOverride = this.timothyFriendClip;
				this.interactable.loveClipOverride = this.timothyLoveClip;
			}
		}
		if (Singleton<DayNightCycle>.Instance.CurrentDayPhase == DayPhase.MORNING && Singleton<DeluxeEditionController>.Instance.IS_EARLY_BIRD_EDITION)
		{
			this.material.SetTexture("_Diffuse", this.earlyBird);
			return;
		}
		if (this.Textures.ContainsKey(time) && this.Textures[time] != null)
		{
			this.material.SetTexture("_Diffuse", this.Textures[time]);
		}
	}

	// Token: 0x040003B8 RID: 952
	public SerializedDictionary<DayPhase, Texture2D> Textures;

	// Token: 0x040003B9 RID: 953
	public Texture2D earlyBird;

	// Token: 0x040003BA RID: 954
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x040003BB RID: 955
	private Material material;

	// Token: 0x040003BC RID: 956
	[SerializeField]
	private GenericInteractable interactable;

	// Token: 0x040003BD RID: 957
	[SerializeField]
	private AudioClip timFriendClip;

	// Token: 0x040003BE RID: 958
	[SerializeField]
	private AudioClip timLoveClip;

	// Token: 0x040003BF RID: 959
	[SerializeField]
	private AudioClip timothyFriendClip;

	// Token: 0x040003C0 RID: 960
	[SerializeField]
	private AudioClip timothyLoveClip;
}
