using System;
using Team17.Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Assets.Date_Everything.Scripts.UI.Loading
{
	// Token: 0x0200025C RID: 604
	internal class VideoPlayer : MonoBehaviour
	{
		// Token: 0x060013A7 RID: 5031 RVA: 0x0005D9F0 File Offset: 0x0005BBF0
		public void Start()
		{
			this.InitialiseElement();
			if (this.valid)
			{
				this._renderTexture = new RenderTexture(this._renderTextureDescriptor);
				this._renderTexture.Create();
				this._video.renderMode = VideoRenderMode.RenderTexture;
				this._video.targetTexture = this._renderTexture;
				this._image.texture = this._renderTexture;
				RenderTexture active = RenderTexture.active;
				RenderTexture.active = this._renderTexture;
				GL.Clear(true, true, Color.black);
				RenderTexture.active = active;
				if (!this._video.gameObject.activeSelf)
				{
					this._video.gameObject.SetActive(true);
				}
				if (!this._videoCanvas.gameObject.activeSelf)
				{
					this._videoCanvas.gameObject.SetActive(true);
				}
				this._video.prepareCompleted += this.OnVideoPrepared;
				this._video.Prepare();
			}
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0005DAE4 File Offset: 0x0005BCE4
		public bool InitialiseElement()
		{
			this.valid = true;
			if (this._video == null)
			{
				T17Debug.LogError("VideoElement: No video player specified ");
				this.valid = false;
			}
			else if (this._video.gameObject.activeSelf)
			{
				this._video.gameObject.SetActive(false);
			}
			if (this._image == null)
			{
				T17Debug.LogError("VideoElement: No raw image specified ");
				this.valid = false;
			}
			if (this._videoCanvas == null)
			{
				T17Debug.LogError("VideoElement: No canvas specified ");
				this.valid = false;
			}
			else if (this._videoCanvas.gameObject.activeSelf)
			{
				this._videoCanvas.gameObject.SetActive(false);
			}
			if (this.valid)
			{
				this._renderTextureDescriptor = new RenderTextureDescriptor((int)this._video.width, (int)this._video.height, RenderTextureFormat.ARGB32, 0, 0);
				this.CleanUp();
			}
			this.finished = !this.valid;
			return !this.valid;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x0005DBE9 File Offset: 0x0005BDE9
		private void OnDestroy()
		{
			this.CleanUp();
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0005DBF4 File Offset: 0x0005BDF4
		private void CleanUp()
		{
			if (this.valid)
			{
				if (this._renderTexture != null)
				{
					this._renderTexture.Release();
					this._renderTexture = null;
				}
				this._video.prepareCompleted -= this.OnVideoPrepared;
				this._video.loopPointReached -= this.VideoEndReached;
			}
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x0005DC57 File Offset: 0x0005BE57
		private void Update()
		{
			if (this.valid && !this.finished && this._renderTexture != null && !this._renderTexture.IsCreated())
			{
				this._renderTexture.Create();
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0005DC90 File Offset: 0x0005BE90
		private void OnVideoPrepared(VideoPlayer player)
		{
			if (player == this._video)
			{
				player.prepareCompleted -= this.OnVideoPrepared;
				player.loopPointReached += this.VideoEndReached;
				player.Play();
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0005DCCA File Offset: 0x0005BECA
		private void VideoEndReached(VideoPlayer player)
		{
			if (player == this._video)
			{
				player.loopPointReached -= this.VideoEndReached;
				this.finished = true;
			}
		}

		// Token: 0x04000F49 RID: 3913
		[SerializeField]
		private RawImage _image;

		// Token: 0x04000F4A RID: 3914
		[SerializeField]
		private VideoPlayer _video;

		// Token: 0x04000F4B RID: 3915
		[SerializeField]
		private Canvas _videoCanvas;

		// Token: 0x04000F4C RID: 3916
		private RenderTextureDescriptor _renderTextureDescriptor;

		// Token: 0x04000F4D RID: 3917
		private RenderTexture _renderTexture;

		// Token: 0x04000F4E RID: 3918
		private bool valid;

		// Token: 0x04000F4F RID: 3919
		private bool finished;
	}
}
