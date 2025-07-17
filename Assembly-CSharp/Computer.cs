using System;
using Cinemachine;
using T17.Services;
using Team17.Scripts.Services.Input;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class Computer : Interactable
{
	// Token: 0x06000408 RID: 1032 RVA: 0x00019301 File Offset: 0x00017501
	public void Start()
	{
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00019304 File Offset: 0x00017504
	public void OpenComputer()
	{
		Singleton<AudioManager>.Instance.PlayTrackWithIntro("wrkspce_music", "wrkspce_intro");
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_open, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		Singleton<ComputerManager>.Instance.SwapToWorkspaceMenu();
		this.setplayerpos(this.computerViewPosition, true);
		InputModeHandle inputModeHandle = this._inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.Dispose();
		}
		this._inputModeHandle = Services.InputService.PushMode(IMirandaInputService.EInputMode.UIWithChat, this);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00019385 File Offset: 0x00017585
	private void OnDisable()
	{
		InputModeHandle inputModeHandle = this._inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.Dispose();
		}
		this._inputModeHandle = null;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0001939F File Offset: 0x0001759F
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x000193A6 File Offset: 0x000175A6
	public override void Interact()
	{
		this.OpenComputer();
		Singleton<ComputerManager>.Instance.canPassOffFocus = false;
		MovingDateable.MoveDateable("Computer", "after", true);
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x000193CC File Offset: 0x000175CC
	public void setplayerpos(GameObject g, bool updateRotation = false)
	{
		if (BetterPlayerControl.Instance != null)
		{
			BetterPlayerControl.Instance.transform.position = g.transform.position;
			if (updateRotation)
			{
				BetterPlayerControl.Instance.transform.rotation = g.transform.rotation;
			}
		}
	}

	// Token: 0x04000407 RID: 1031
	public bool overrideNodeLock;

	// Token: 0x04000408 RID: 1032
	public string NodeLockAlt;

	// Token: 0x04000409 RID: 1033
	private InputModeHandle _inputModeHandle;

	// Token: 0x0400040A RID: 1034
	[SerializeField]
	private CinemachineVirtualCamera _camera;

	// Token: 0x0400040B RID: 1035
	[SerializeField]
	private Animator animator;

	// Token: 0x0400040C RID: 1036
	[SerializeField]
	private GameObject computerViewPosition;
}
