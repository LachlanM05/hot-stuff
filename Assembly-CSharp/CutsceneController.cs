using System;
using Rewired;
using Team17.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

// Token: 0x0200015F RID: 351
public class CutsceneController : MonoBehaviour
{
	// Token: 0x06000CE4 RID: 3300 RVA: 0x0004AA38 File Offset: 0x00048C38
	private void Start()
	{
		if (this._videoPlayer == null)
		{
			this._videoPlayer = base.gameObject.GetComponent<VideoPlayer>();
		}
		if (this.onCutsceneCompleted == null)
		{
			this.onCutsceneCompleted = new UnityEvent();
		}
		this.player = ReInput.players.GetPlayer(0);
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0004AA88 File Offset: 0x00048C88
	private void Update()
	{
		if (this.isPlaying && this.player.GetButtonDown("Talk"))
		{
			this.End(this._videoPlayer);
		}
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0004AAB0 File Offset: 0x00048CB0
	public void Play(VideoClip vc)
	{
		this.isPlaying = true;
		this._videoPlayer.loopPointReached += this.End;
		if (vc == null)
		{
			T17Debug.LogError("CutsceneController.Play called with a null video clip!");
			return;
		}
		this._videoPlayer.clip = vc;
		this._videoPlayer.gameObject.SetActive(true);
		this._videoPlayer.Play();
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x0004AB17 File Offset: 0x00048D17
	public void End(VideoPlayer vp)
	{
		this.isPlaying = false;
		vp.loopPointReached -= this.End;
		vp.gameObject.SetActive(false);
		this.onCutsceneCompleted.Invoke();
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0004AB49 File Offset: 0x00048D49
	public void HideVideoPlayer()
	{
		if (this._videoPlayer != null)
		{
			this._videoPlayer.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000B8A RID: 2954
	public VideoPlayer _videoPlayer;

	// Token: 0x04000B8B RID: 2955
	public UnityEvent onCutsceneCompleted;

	// Token: 0x04000B8C RID: 2956
	public bool isPlaying;

	// Token: 0x04000B8D RID: 2957
	private Player player;
}
