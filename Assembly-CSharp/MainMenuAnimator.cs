using System;
using Rewired;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class MainMenuAnimator : MonoBehaviour
{
	// Token: 0x06000978 RID: 2424 RVA: 0x00036E30 File Offset: 0x00035030
	private void Start()
	{
		if (this.PhoneMenu.gameObject.activeInHierarchy)
		{
			this.PhoneMenu.gameObject.SetActive(false);
			this.MainMenu.gameObject.SetActive(true);
		}
		if (this.player == null)
		{
			this.player = ReInput.players.GetPlayer(0);
		}
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00036E8A File Offset: 0x0003508A
	public void AnimationFinished()
	{
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00036E8C File Offset: 0x0003508C
	private void Update()
	{
		if (this.player == null)
		{
			return;
		}
		if (this.player.GetButtonDown("Confirm") || this.player.GetButtonDown("Interact"))
		{
			this.animator.speed = 10f;
		}
	}

	// Token: 0x040008B9 RID: 2233
	public GameObject PhoneMenu;

	// Token: 0x040008BA RID: 2234
	public GameObject MainMenu;

	// Token: 0x040008BB RID: 2235
	private Player player;

	// Token: 0x040008BC RID: 2236
	[SerializeField]
	private Animator animator;
}
