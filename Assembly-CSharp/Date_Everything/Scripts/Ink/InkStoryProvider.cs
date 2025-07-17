using System;
using System.Threading;
using System.Threading.Tasks;
using Ink;
using Ink.Runtime;
using Team17.Common;
using UnityEngine;

namespace Date_Everything.Scripts.Ink
{
	// Token: 0x0200026F RID: 623
	public class InkStoryProvider : Singleton<InkStoryProvider>
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x00060B17 File Offset: 0x0005ED17
		public Story Story
		{
			get
			{
				return this._story;
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x00060B1F File Offset: 0x0005ED1F
		public bool IsStoryAvailable()
		{
			return this._story != null;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x00060B2D File Offset: 0x0005ED2D
		public override void AwakeSingleton()
		{
			base.AwakeSingleton();
			if (!this.IsStoryAvailable())
			{
				this.BeginLoadStory();
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x00060B43 File Offset: 0x0005ED43
		public void ResetStory()
		{
			this.OnDestroy();
			this.Awake();
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00060B54 File Offset: 0x0005ED54
		private void BeginLoadStory()
		{
			this._cancellationTokenSource = new CancellationTokenSource();
			CancellationToken token = this._cancellationTokenSource.Token;
			string text = this.inkJSONAsset.text;
			this._loadingTask = Task.Run(delegate
			{
				this.ParseStory(text, token);
			});
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x00060BB2 File Offset: 0x0005EDB2
		private void Update()
		{
			if (this.HasStoryLoadingJustCompleted())
			{
				this.SetStoryLoadingComplete();
			}
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00060BC2 File Offset: 0x0005EDC2
		private void SetStoryLoadingComplete()
		{
			this._loadingTask = null;
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x00060BCB File Offset: 0x0005EDCB
		private bool HasStoryLoadingJustCompleted()
		{
			return this._loadingTask != null && this._loadingTask.IsCompleted;
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00060BE2 File Offset: 0x0005EDE2
		private void OnDestroy()
		{
			CancellationTokenSource cancellationTokenSource = this._cancellationTokenSource;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			this._cancellationTokenSource = null;
			if (this.IsStoryAvailable())
			{
				this._story.onError -= this.StoryOnError;
				this._story = null;
			}
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00060C24 File Offset: 0x0005EE24
		private void ParseStory(string json, CancellationToken cancellationToken)
		{
			Story story = new Story(json);
			story.onError += this.StoryOnError;
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}
			this._story = story;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00060C5B File Offset: 0x0005EE5B
		private void StoryOnError(string message, ErrorType type)
		{
			if (type != ErrorType.Warning)
			{
				T17Debug.LogError(message);
			}
		}

		// Token: 0x04000F7D RID: 3965
		[SerializeField]
		private TextAsset inkJSONAsset;

		// Token: 0x04000F7E RID: 3966
		private Story _story;

		// Token: 0x04000F7F RID: 3967
		private CancellationTokenSource _cancellationTokenSource;

		// Token: 0x04000F80 RID: 3968
		private Task _loadingTask;
	}
}
