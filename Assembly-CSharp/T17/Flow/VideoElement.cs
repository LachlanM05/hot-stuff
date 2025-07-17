using System;
using Team17.Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace T17.Flow
{
	// Token: 0x02000250 RID: 592
	public class VideoElement : IdentElement
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x0005C39F File Offset: 0x0005A59F
		public override bool Finished
		{
			get
			{
				return this.CheckForFinished();
			}
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0005C3A8 File Offset: 0x0005A5A8
		public override void StartElement()
		{
			if (this.valid)
			{
				this._renderTexture = new RenderTexture(this._renderTextureDescriptor);
				this._renderTexture.Create();
				this._video.renderMode = VideoRenderMode.RenderTexture;
				this._video.targetTexture = this._renderTexture;
				this._image.texture = this._renderTexture;
				RenderTexture active = RenderTexture.active;
				RenderTexture.active = this._renderTexture;
				GL.Clear(true, true, this._initialColour);
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
				VoiceOverCharacter component = base.gameObject.GetComponent<VoiceOverCharacter>();
				if (component != null)
				{
					component.PlayVoiceOver(0.5f);
				}
			}
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0005C4B8 File Offset: 0x0005A6B8
		public override bool InitialiseElement()
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
			return this.finished;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0005C5BC File Offset: 0x0005A7BC
		public override void StopElement()
		{
			if (this.valid)
			{
				if (this._video.gameObject.activeSelf)
				{
					this._video.gameObject.SetActive(false);
				}
				if (this._videoCanvas.gameObject.activeSelf)
				{
					this._videoCanvas.gameObject.SetActive(false);
				}
				this.CleanUp();
				this.finished = true;
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0005C624 File Offset: 0x0005A824
		public override void UpdateElement(float deltaTime)
		{
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0005C626 File Offset: 0x0005A826
		private void OnDestroy()
		{
			this.CleanUp();
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0005C630 File Offset: 0x0005A830
		private void CleanUp()
		{
			if (this.valid)
			{
				if (this._video.gameObject.activeSelf)
				{
					this._video.gameObject.SetActive(false);
				}
				if (this._renderTexture != null)
				{
					this._renderTexture.Release();
					this._renderTexture = null;
				}
				this._video.prepareCompleted -= this.OnVideoPrepared;
				this._video.loopPointReached -= this.VideoEndReached;
			}
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0005C6B6 File Offset: 0x0005A8B6
		private void Update()
		{
			if (this.valid && !this.finished && this._renderTexture != null && !this._renderTexture.IsCreated())
			{
				this._renderTexture.Create();
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0005C6EF File Offset: 0x0005A8EF
		private void OnVideoPrepared(VideoPlayer player)
		{
			if (player == this._video)
			{
				player.prepareCompleted -= this.OnVideoPrepared;
				player.loopPointReached += this.VideoEndReached;
				player.Play();
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0005C729 File Offset: 0x0005A929
		private void VideoEndReached(VideoPlayer player)
		{
			if (player == this._video)
			{
				player.loopPointReached -= this.VideoEndReached;
				this.finishing = true;
			}
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0005C754 File Offset: 0x0005A954
		private bool CheckForFinished()
		{
			if (this.finished)
			{
				return true;
			}
			if (this.finishing)
			{
				this.finished = true;
				this.finishing = false;
				if (this._video.gameObject.activeSelf)
				{
					this._video.gameObject.SetActive(false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x04000F10 RID: 3856
		[SerializeField]
		private RawImage _image;

		// Token: 0x04000F11 RID: 3857
		[SerializeField]
		private VideoPlayer _video;

		// Token: 0x04000F12 RID: 3858
		[SerializeField]
		private Canvas _videoCanvas;

		// Token: 0x04000F13 RID: 3859
		[SerializeField]
		private Color _initialColour = Color.black;

		// Token: 0x04000F14 RID: 3860
		private RenderTextureDescriptor _renderTextureDescriptor;

		// Token: 0x04000F15 RID: 3861
		private RenderTexture _renderTexture;

		// Token: 0x04000F16 RID: 3862
		private bool finishing;
	}
}
