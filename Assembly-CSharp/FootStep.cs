using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class FootStep : MonoBehaviour
{
	// Token: 0x0600021D RID: 541 RVA: 0x0000C698 File Offset: 0x0000A898
	private void Awake()
	{
		FootStep.instance = this;
		this.MatToClipDict = new Dictionary<FootStep.FootstepMaterial, AudioClip[]>
		{
			{
				FootStep.FootstepMaterial.ASPHALT,
				this.AsphaltClips
			},
			{
				FootStep.FootstepMaterial.CARPET,
				this.CarpetClips
			},
			{
				FootStep.FootstepMaterial.CRAWLSPACE,
				this.CrawlClips
			},
			{
				FootStep.FootstepMaterial.GRASS,
				this.GrassClips
			},
			{
				FootStep.FootstepMaterial.WOOD_STAIRS,
				this.WoodStairClips
			},
			{
				FootStep.FootstepMaterial.WOOD,
				this.WoodClips
			},
			{
				FootStep.FootstepMaterial.TILE,
				this.TileClips
			}
		};
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0000C714 File Offset: 0x0000A914
	public void step()
	{
		this.stepnoise.clip = this.MatToClipDict[this.currentMatType][Random.Range(0, this.MatToClipDict[this.currentMatType].Length)];
		this.stepnoise.pitch = Random.Range(1f - this.pitchrange, 1f + this.pitchrange);
		this.stepnoise.Play();
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000C78A File Offset: 0x0000A98A
	public void ChangeFoostepCategory(FootStep.FootstepMaterial mat)
	{
		if (mat != this.currentMatType)
		{
			this.currentMatType = mat;
		}
	}

	// Token: 0x040002FB RID: 763
	public static FootStep instance;

	// Token: 0x040002FC RID: 764
	public AudioSource stepnoise;

	// Token: 0x040002FD RID: 765
	public float pitchrange = 0.05f;

	// Token: 0x040002FE RID: 766
	public FootStep.FootstepMaterial currentMatType = FootStep.FootstepMaterial.WOOD;

	// Token: 0x040002FF RID: 767
	private Dictionary<FootStep.FootstepMaterial, AudioClip[]> MatToClipDict;

	// Token: 0x04000300 RID: 768
	[SerializeField]
	private AudioClip[] AsphaltClips;

	// Token: 0x04000301 RID: 769
	[SerializeField]
	private AudioClip[] CarpetClips;

	// Token: 0x04000302 RID: 770
	[SerializeField]
	private AudioClip[] CrawlClips;

	// Token: 0x04000303 RID: 771
	[SerializeField]
	private AudioClip[] GrassClips;

	// Token: 0x04000304 RID: 772
	[SerializeField]
	private AudioClip[] WoodStairClips;

	// Token: 0x04000305 RID: 773
	[SerializeField]
	private AudioClip[] WoodClips;

	// Token: 0x04000306 RID: 774
	[SerializeField]
	private AudioClip[] TileClips;

	// Token: 0x020002B2 RID: 690
	public enum FootstepMaterial
	{
		// Token: 0x040010B9 RID: 4281
		CARPET,
		// Token: 0x040010BA RID: 4282
		ASPHALT,
		// Token: 0x040010BB RID: 4283
		CRAWLSPACE,
		// Token: 0x040010BC RID: 4284
		GRASS,
		// Token: 0x040010BD RID: 4285
		WOOD_STAIRS,
		// Token: 0x040010BE RID: 4286
		WOOD,
		// Token: 0x040010BF RID: 4287
		TILE
	}
}
