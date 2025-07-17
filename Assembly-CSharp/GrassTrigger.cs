using System;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class GrassTrigger : MonoBehaviour
{
	// Token: 0x06000256 RID: 598 RVA: 0x0000DB00 File Offset: 0x0000BD00
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player") && !this.consumedCollision && BetterPlayerControl.Instance.IsPlayerFloor(base.gameObject))
		{
			Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.GLITCH_CITY);
			BetterPlayerControl.Instance.SetSpeed(30f);
			Dresser.Instance.Outside();
			this.consumedCollision = true;
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000DB68 File Offset: 0x0000BD68
	private void OnCollisionStay(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.CompareTag("Player") && !this.consumedCollision && !this.consumedCollision && BetterPlayerControl.Instance.IsPlayerFloor(base.gameObject))
		{
			Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.GLITCH_CITY);
			BetterPlayerControl.Instance.SetSpeed(30f);
			Dresser.Instance.Outside();
			this.consumedCollision = true;
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000DBD6 File Offset: 0x0000BDD6
	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.consumedCollision = false;
		}
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000DBF4 File Offset: 0x0000BDF4
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("Player") && !this.consumedCollision && !this.consumedCollision && BetterPlayerControl.Instance.IsPlayerFloor(base.gameObject))
		{
			Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.GLITCH_CITY);
			BetterPlayerControl.Instance.SetSpeed(30f);
			Dresser.Instance.Outside();
			this.consumedCollision = true;
		}
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000DC64 File Offset: 0x0000BE64
	private void OnTriggerStay(Collider collisionInfo)
	{
		if (collisionInfo.gameObject.CompareTag("Player") && !this.consumedCollision && !this.consumedCollision && BetterPlayerControl.Instance.IsPlayerFloor(base.gameObject))
		{
			Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.GLITCH_CITY);
			BetterPlayerControl.Instance.SetSpeed(30f);
			Dresser.Instance.Outside();
			this.consumedCollision = true;
		}
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000DCD2 File Offset: 0x0000BED2
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.consumedCollision = false;
		}
	}

	// Token: 0x0400032B RID: 811
	[SerializeField]
	private bool consumedCollision;
}
