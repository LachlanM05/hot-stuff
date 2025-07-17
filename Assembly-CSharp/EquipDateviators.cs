using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using T17.Services;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x02000080 RID: 128
public class EquipDateviators : Interactable
{
	// Token: 0x06000451 RID: 1105 RVA: 0x0001A69F File Offset: 0x0001889F
	private void Start()
	{
		if (Singleton<Save>.Instance.GetTutorialFinished() || Singleton<Save>.Instance.GetDateStatus("skylar_specs") != RelationshipStatus.Unmet)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0001A6C9 File Offset: 0x000188C9
	private void OnEnable()
	{
		this.FadeInBeforeSound();
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0001A6D1 File Offset: 0x000188D1
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0001A6D8 File Offset: 0x000188D8
	public override void Interact()
	{
		if (Services.GameSettings.GetInt(Save.SettingKeySkipTutorial, 0) == 1)
		{
			Singleton<InkController>.Instance.story.variablesState["can_skip_tutorial"] = true;
		}
		Singleton<AudioManager>.Instance.StopTrack("Foley/Mystery_Box_SFX", 0f);
		this.HudCanvas.GetComponent<HudCanvasManager>().GlassesAcquired();
		Singleton<Dateviators>.Instance.EquipOverride();
		Singleton<GameController>.Instance.ForceDialogue("skylar_specs.intro_specs", new GameController.DelegateAfterChatEndsEvents(Singleton<TutorialController>.Instance.TutorialState4AfterSkylarIsAwakened), false, false);
		PlayerPauser.Unpause();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0001A776 File Offset: 0x00018976
	public void FadeInBeforeSound()
	{
		this.before.DOFade(1f, 0.3f);
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x0001A78E File Offset: 0x0001898E
	public void FadeOutBeforeSound()
	{
		this.before.DOFade(0f, 0.3f).OnComplete(delegate
		{
			this.before.Stop();
		});
	}

	// Token: 0x04000451 RID: 1105
	public CutsceneController _controller;

	// Token: 0x04000452 RID: 1106
	public VideoClip _videoClip;

	// Token: 0x04000453 RID: 1107
	public GameObject HudCanvas;

	// Token: 0x04000454 RID: 1108
	[SerializeField]
	private AudioSource before;
}
