using System;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x0200008D RID: 141
public class Makeup : MonoBehaviour
{
	// Token: 0x060004E0 RID: 1248 RVA: 0x0001D7D6 File Offset: 0x0001B9D6
	private void Start()
	{
		this.makeup.Stop();
		this.toothpaste.Stop();
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0001D7EE File Offset: 0x0001B9EE
	public void StartMakeup()
	{
		this.makeup.Play();
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0001D7FB File Offset: 0x0001B9FB
	public void StopMakeup()
	{
		this.makeup.Stop();
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0001D808 File Offset: 0x0001BA08
	public void StartToothpaste()
	{
		this.toothpaste.Play();
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0001D815 File Offset: 0x0001BA15
	public void StopToothpaste()
	{
		this.toothpaste.Stop();
	}

	// Token: 0x040004E0 RID: 1248
	[SerializeField]
	private VisualEffect toothpaste;

	// Token: 0x040004E1 RID: 1249
	[SerializeField]
	private VisualEffect makeup;
}
