using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000076 RID: 118
public class Clock : MonoBehaviour
{
	// Token: 0x060003FF RID: 1023 RVA: 0x00019077 File Offset: 0x00017277
	private void Awake()
	{
		Singleton<DayNightCycle>.Instance.DayPhaseUpdated.AddListener(new UnityAction(this.UpdateClock));
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x00019094 File Offset: 0x00017294
	private void UpdateClock()
	{
		if (!this.initiaized)
		{
			this.Initialize();
		}
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
		if (this.animator)
		{
			if (this.timeHash != -1)
			{
				int num;
				switch (Singleton<DayNightCycle>.Instance.GetTime())
				{
				case DayPhase.MORNING:
					num = 9;
					break;
				case DayPhase.NOON:
					num = 12;
					break;
				case DayPhase.AFTERNOON:
					num = 3;
					break;
				case DayPhase.EVENING:
					num = 6;
					break;
				case DayPhase.NIGHT:
					num = 9;
					break;
				case DayPhase.MIDNIGHT:
					num = 12;
					break;
				default:
					num = 9;
					break;
				}
				this.animator.SetInteger(this.timeHash, num);
			}
			if (this.dayPhaseHash != -1)
			{
				this.animator.SetTrigger(this.dayPhaseHash);
			}
		}
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x000191B0 File Offset: 0x000173B0
	private void Initialize()
	{
		if (!this.initiaized)
		{
			this.initiaized = true;
			if (this.animator)
			{
				int num = 0;
				int parameterCount = this.animator.parameterCount;
				while (num < parameterCount && (this.timeHash == -1 || this.dayPhaseHash == -1))
				{
					AnimatorControllerParameter parameter = this.animator.GetParameter(num);
					if (this.timeHash == -1 && string.CompareOrdinal("time", parameter.name) == 0)
					{
						this.timeHash = parameter.nameHash;
					}
					else if (this.dayPhaseHash == -1 && string.CompareOrdinal("dayPhaseUpdate", parameter.name) == 0)
					{
						this.dayPhaseHash = parameter.nameHash;
					}
					num++;
				}
			}
		}
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00019266 File Offset: 0x00017466
	public void PlayTick()
	{
		if (this.kitchenAudioSource == null)
		{
			return;
		}
		this.kitchenAudioSource.PlayOneShot(this.kitchenTickClip);
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00019288 File Offset: 0x00017488
	public void PlayTock()
	{
		if (this.kitchenAudioSource == null)
		{
			return;
		}
		this.kitchenAudioSource.PlayOneShot(this.kitchenTockClip);
	}

	// Token: 0x040003F8 RID: 1016
	[SerializeField]
	private Animator animator;

	// Token: 0x040003F9 RID: 1017
	private const int invalid = -1;

	// Token: 0x040003FA RID: 1018
	private bool initiaized;

	// Token: 0x040003FB RID: 1019
	private int timeHash = -1;

	// Token: 0x040003FC RID: 1020
	private int dayPhaseHash = -1;

	// Token: 0x040003FD RID: 1021
	[SerializeField]
	private GenericInteractable interactable;

	// Token: 0x040003FE RID: 1022
	[SerializeField]
	private AudioClip timFriendClip;

	// Token: 0x040003FF RID: 1023
	[SerializeField]
	private AudioClip timLoveClip;

	// Token: 0x04000400 RID: 1024
	[SerializeField]
	private AudioClip timothyFriendClip;

	// Token: 0x04000401 RID: 1025
	[SerializeField]
	private AudioClip timothyLoveClip;

	// Token: 0x04000402 RID: 1026
	[Header("Kitchen Tick Tocks")]
	[SerializeField]
	private AudioSource kitchenAudioSource;

	// Token: 0x04000403 RID: 1027
	[SerializeField]
	private AudioClip kitchenTickClip;

	// Token: 0x04000404 RID: 1028
	[SerializeField]
	private AudioClip kitchenTockClip;
}
