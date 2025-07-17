using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E1 RID: 225
public class AmbientSpace : MonoBehaviour, IReloadHandler
{
	// Token: 0x0600076B RID: 1899 RVA: 0x00029FB4 File Offset: 0x000281B4
	private void Awake()
	{
		if (this.collider == null)
		{
			this.collider = base.GetComponent<Collider>();
		}
		if (this.audioSource == null)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (this.audioSource == null || this.collider == null)
		{
			return;
		}
		this.audioSource.volume = 0f;
		this.audioSource.Stop();
		this._bounds = this.collider.bounds;
		Singleton<DayNightCycle>.Instance.DayPhaseUpdated.AddListener(new UnityAction(this.ResetImmediate));
		Singleton<PhoneManager>.Instance.ReturningToMainMenu.AddListener(new UnityAction(this.ResetToMain));
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x0002A074 File Offset: 0x00028274
	public void ResetFromZero()
	{
		this.audioSource.volume = 0f;
		this.audioSource.Stop();
		this.audioSource.DOKill(false);
		this.audioSource.Play();
		this.audioSource.DOFade(1f, 5f);
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x0002A0CC File Offset: 0x000282CC
	private void OnTriggerEnter(Collider other)
	{
		if (!this.canActivate)
		{
			return;
		}
		if (other.tag != "Player")
		{
			return;
		}
		this.audioSource.DOKill(false);
		this.audioSource.Play();
		this.audioSource.DOFade(1f, 3f);
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x0002A123 File Offset: 0x00028323
	private void OnTriggerStay(Collider other)
	{
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0002A125 File Offset: 0x00028325
	private void OnTriggerExit(Collider other)
	{
		if (other.tag != "Player")
		{
			return;
		}
		this.ResetThis();
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0002A140 File Offset: 0x00028340
	public void ResetTimed(float time)
	{
		this.audioSource.DOKill(false);
		this.audioSource.DOFade(0f, time).OnComplete(delegate
		{
			this.audioSource.Stop();
		});
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0002A172 File Offset: 0x00028372
	public void ResetToMain()
	{
		this.canActivate = false;
		this.audioSource.DOKill(false);
		this.audioSource.DOFade(0f, 0.2f).OnComplete(delegate
		{
			this.audioSource.Stop();
		});
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x0002A1AF File Offset: 0x000283AF
	public void ResetThis()
	{
		this.audioSource.DOKill(false);
		this.audioSource.DOFade(0f, 3f).OnComplete(delegate
		{
			this.audioSource.Stop();
		});
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0002A1E5 File Offset: 0x000283E5
	public void ResetImmediate()
	{
		this.audioSource.DOKill(false);
		this.audioSource.volume = 0f;
		this.audioSource.Stop();
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0002A210 File Offset: 0x00028410
	public static Vector3 ClosestPointOnBounds(Vector3 point, Bounds bounds)
	{
		Plane plane = new Plane(Vector3.forward, bounds.max);
		Plane plane2 = new Plane(Vector3.back, bounds.min);
		Plane plane3 = new Plane(Vector3.right, bounds.max);
		Plane plane4 = new Plane(Vector3.left, bounds.min);
		Vector3 vector = plane.ClosestPointOnPlane(point);
		Vector3 vector2 = plane2.ClosestPointOnPlane(point);
		Vector3 vector3 = plane3.ClosestPointOnPlane(point);
		Vector3 vector4 = plane4.ClosestPointOnPlane(point);
		Vector3 vector5 = point;
		float num = float.MaxValue;
		foreach (Vector3 vector6 in new Vector3[] { vector, vector2, vector4, vector3 })
		{
			float num2 = Vector3.Distance(vector6, point);
			if (num2 < num)
			{
				num = num2;
				vector5 = vector6;
			}
		}
		return vector5;
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0002A2FF File Offset: 0x000284FF
	public int Priority()
	{
		return 500;
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0002A306 File Offset: 0x00028506
	public void LoadState()
	{
		this.audioSource.volume = 0f;
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x0002A318 File Offset: 0x00028518
	public void SaveState()
	{
	}

	// Token: 0x040006BB RID: 1723
	[SerializeField]
	[Tooltip("How close to the edge of the collider the fade begins.")]
	private float falloffStartRange = 0.1f;

	// Token: 0x040006BC RID: 1724
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x040006BD RID: 1725
	[SerializeField]
	public Collider collider;

	// Token: 0x040006BE RID: 1726
	[SerializeField]
	private float playerDistanceFromBounds;

	// Token: 0x040006BF RID: 1727
	[SerializeField]
	private Vector3 closestOnBounds_this_player;

	// Token: 0x040006C0 RID: 1728
	private Bounds _bounds;

	// Token: 0x040006C1 RID: 1729
	private bool canActivate = true;
}
