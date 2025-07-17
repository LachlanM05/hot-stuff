using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class AtticDoorUnlocker : MonoBehaviour
{
	// Token: 0x060003C7 RID: 967 RVA: 0x00017C73 File Offset: 0x00015E73
	private void Awake()
	{
		AtticDoorUnlocker.instance = this;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x00017C7C File Offset: 0x00015E7C
	public void StartSequence()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("AtticLocked");
		if (gameObject)
		{
			gameObject.gameObject.SetActive(false);
		}
		this.cam.Priority = 100;
		Singleton<AudioManager>.Instance.PlayTrack(this.clip, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		this.animator.SetInteger("ending", 2);
		this.animator.SetTrigger("magicalAnimStart");
		PlayerPauser.Pause();
		CursorLocker.Lock();
		Singleton<PhoneManager>.Instance.BlockPhoneOpening = true;
		MovingDateable.MoveDateable("MovingAtticDoor", "open", true);
		MovingDateable.MoveDateable("MovingKeyBox", "keith_unlock", true);
		Save.AutoSaveGame();
		base.StartCoroutine(this.PlayerControl());
		this.electricalClosetDoor.CloseDoor(1f);
		MovingDateable.GetDateable("MovingAtticDoor", "open").SetActive(false);
		MovingDateable.MoveDateable("MovingKeyBox", "default", true);
		base.Invoke("MidSequence", 24f);
		base.Invoke("EndSequence", 28f);
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00017D99 File Offset: 0x00015F99
	private IEnumerator PlayerControl()
	{
		for (;;)
		{
			this.LockPlayer();
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00017DA8 File Offset: 0x00015FA8
	public void LockPlayer()
	{
		BetterPlayerControl.Instance.ChangePlayerState(BetterPlayerControl.PlayerState.CantControl);
	}

	// Token: 0x060003CB RID: 971 RVA: 0x00017DB8 File Offset: 0x00015FB8
	public void MidSequence()
	{
		MovingDateable.MoveDateable("MovingAtticDoor", "open", true);
		MovingDateable.MoveDateable("MovingKeyBox", "keith_unlock", true);
		this.atticUnlocked.GetComponentInChildren<Door>().OpenDoor(0f);
		this.visuals.SetActive(false);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00017E08 File Offset: 0x00016008
	public void EndSequence()
	{
		this.cam.Priority = -1;
		BetterPlayerControl.Instance.ChangePlayerState(BetterPlayerControl.PlayerState.CanControl);
		PlayerPauser.Unpause();
		CursorLocker.Unlock();
		base.StopCoroutine(this.PlayerControl());
		Singleton<PhoneManager>.Instance.BlockPhoneOpening = false;
		base.gameObject.SetActive(false);
		Save.AutoSaveGame();
	}

	// Token: 0x040003C1 RID: 961
	public static AtticDoorUnlocker instance;

	// Token: 0x040003C2 RID: 962
	[SerializeField]
	private CinemachineVirtualCamera cam;

	// Token: 0x040003C3 RID: 963
	[SerializeField]
	private Animator animator;

	// Token: 0x040003C4 RID: 964
	[SerializeField]
	private GameObject visuals;

	// Token: 0x040003C5 RID: 965
	[SerializeField]
	private AudioClip clip;

	// Token: 0x040003C6 RID: 966
	[SerializeField]
	private GameObject atticUnlocked;

	// Token: 0x040003C7 RID: 967
	[SerializeField]
	private Door electricalClosetDoor;
}
