using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class CarCollision : MonoBehaviour
{
	// Token: 0x06000134 RID: 308 RVA: 0x000091B0 File Offset: 0x000073B0
	private void OnCollisionEnter(Collision collision)
	{
		if (this.used)
		{
			return;
		}
		if (collision.gameObject.CompareTag("Player"))
		{
			BetterPlayerControl.Instance.ChangePlayerState(BetterPlayerControl.PlayerState.CantControl);
			GameObject gameObject = Object.Instantiate<GameObject>(this.prefab, BetterPlayerControl.Instance._camera.transform.position, collision.gameObject.transform.rotation);
			this.rb = gameObject.GetComponent<Rigidbody>();
			this.used = true;
			base.Invoke("ResetPlayer", 8f);
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00009238 File Offset: 0x00007438
	private void FixedUpdate()
	{
		if (this.rb != null && !this.forceApplied)
		{
			this.rb.AddForce(Vector3.up + BetterPlayerControl.Instance._camera.transform.forward.normalized * -1f / 2f, ForceMode.Impulse);
			this.rb.AddTorque(BetterPlayerControl.Instance._camera.transform.forward * -1f);
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x000092CC File Offset: 0x000074CC
	private void ResetPlayer()
	{
		BetterPlayerControl.Instance.ChangePlayerState(BetterPlayerControl.PlayerState.CanControl);
		Object.Destroy(this.rb.gameObject);
		this.rb = null;
		this.forceApplied = false;
		this.used = false;
		Dresser.Instance.Inside();
		this.trigger.TriggerAchievement();
		KillPlane.Instance.ResetPlayer();
	}

	// Token: 0x04000142 RID: 322
	private Rigidbody rb;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	private GameObject prefab;

	// Token: 0x04000144 RID: 324
	private bool used;

	// Token: 0x04000145 RID: 325
	private bool forceApplied;

	// Token: 0x04000146 RID: 326
	[SerializeField]
	private AchievementTrigger trigger;
}
