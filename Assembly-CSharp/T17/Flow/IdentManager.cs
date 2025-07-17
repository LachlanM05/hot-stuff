using System;
using System.Collections;
using T17.Services;
using UnityEngine;

namespace T17.Flow
{
	// Token: 0x02000251 RID: 593
	public class IdentManager : MonoBehaviour
	{
		// Token: 0x06001353 RID: 4947 RVA: 0x0005C7BA File Offset: 0x0005A9BA
		private bool CanStart()
		{
			return Services.PlatformService != null && Services.PlatformService.IsInitialised();
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0005C7CF File Offset: 0x0005A9CF
		private void Start()
		{
			this.RemoveCursor();
			this.InitialiseElements();
			if (this.autoStart)
			{
				base.StartCoroutine(this.StartIdentsWhendReady());
			}
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0005C7F2 File Offset: 0x0005A9F2
		private IEnumerator StartIdentsWhendReady()
		{
			yield return null;
			while (!this.CanStart())
			{
				yield return null;
			}
			this.StartIdents();
			yield break;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0005C801 File Offset: 0x0005AA01
		private void Update()
		{
			if (this.currentElementIndex > -1)
			{
				this.UpdateIdents(Time.deltaTime);
			}
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0005C817 File Offset: 0x0005AA17
		private void UpdateIdents(float deltaTime)
		{
			this.Elements[this.currentElementIndex].UpdateElement(deltaTime);
			if (this.Elements[this.currentElementIndex].Finished)
			{
				this.NextElement();
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0005C848 File Offset: 0x0005AA48
		private void InitialiseElements()
		{
			int i = 0;
			int num = this.Elements.Length;
			while (i < num)
			{
				if (this.Elements[i] != null)
				{
					this.Elements[i].InitialiseElement();
				}
				i++;
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0005C888 File Offset: 0x0005AA88
		public void StartIdents()
		{
			if (Services.PlatformService.ShouldSkipBootFlow())
			{
				this.skipIdents = true;
			}
			if (this.currentElementIndex == -1)
			{
				this.NextElement();
			}
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0005C8AC File Offset: 0x0005AAAC
		private void NextElement()
		{
			if (this.currentElementIndex > -1)
			{
				this.Elements[this.currentElementIndex].StopElement();
			}
			do
			{
				int num = this.currentElementIndex + 1;
				this.currentElementIndex = num;
				if (num >= this.Elements.Length)
				{
					goto IL_009D;
				}
			}
			while (!(this.Elements[this.currentElementIndex] != null) || !this.Elements[this.currentElementIndex].Valid || ((this.skipIdents || IdentManager.forceSkip) && this.Elements[this.currentElementIndex].IsSkipable(IdentManager.forceSkip)));
			this.Elements[this.currentElementIndex].StartElement();
			IL_009D:
			if (this.currentElementIndex >= this.Elements.Length)
			{
				this.currentElementIndex = -1;
			}
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0005C96D File Offset: 0x0005AB6D
		public static void EnableForceSkip()
		{
			IdentManager.forceSkip = true;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0005C975 File Offset: 0x0005AB75
		private void RemoveCursor()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Token: 0x04000F17 RID: 3863
		[SerializeField]
		private IdentElement[] Elements = new IdentElement[0];

		// Token: 0x04000F18 RID: 3864
		[SerializeField]
		private bool autoStart = true;

		// Token: 0x04000F19 RID: 3865
		private int currentElementIndex = -1;

		// Token: 0x04000F1A RID: 3866
		private bool skipIdents;

		// Token: 0x04000F1B RID: 3867
		private static bool forceSkip;
	}
}
