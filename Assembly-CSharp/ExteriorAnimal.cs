using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000051 RID: 81
public class ExteriorAnimal : MonoBehaviour
{
	// Token: 0x0600020F RID: 527 RVA: 0x0000C378 File Offset: 0x0000A578
	private void Awake()
	{
		this.animClips = this.animator.runtimeAnimatorController.animationClips;
		this.numclips = Mathf.Min(this.animClips.Length, this.numclips);
		this.animator_object.SetActive(false);
		Singleton<DayNightCycle>.Instance.DayPhaseUpdated.AddListener(new UnityAction(this.Initialize));
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000C3DB File Offset: 0x0000A5DB
	public void Initialize()
	{
		this.EndCurrent();
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000C3E4 File Offset: 0x0000A5E4
	public void EndCurrent()
	{
		if (this.numclips == 0)
		{
			return;
		}
		this.previousclip = this.currentclip;
		this.currentclip = Random.Range(0, this.numclips);
		this.current_timer = Random.Range(this.timer_minimum, this.timer_maximum);
		this.animator_object.SetActive(false);
		base.StartCoroutine(this.PlayNext());
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000C448 File Offset: 0x0000A648
	private IEnumerator PlayNext()
	{
		yield return new WaitForSeconds(this.current_timer);
		this.Play();
		yield break;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000C457 File Offset: 0x0000A657
	public void Play()
	{
		base.StopAllCoroutines();
		this.animator_object.SetActive(true);
		this.animator.Play(this.animClips[this.currentclip].name);
	}

	// Token: 0x040002E9 RID: 745
	[SerializeField]
	private float timer_minimum = 20f;

	// Token: 0x040002EA RID: 746
	[SerializeField]
	private float timer_maximum = 110f;

	// Token: 0x040002EB RID: 747
	[SerializeField]
	private float current_timer = 110f;

	// Token: 0x040002EC RID: 748
	[SerializeField]
	private int currentclip;

	// Token: 0x040002ED RID: 749
	[SerializeField]
	private int previousclip;

	// Token: 0x040002EE RID: 750
	[SerializeField]
	private int numclips;

	// Token: 0x040002EF RID: 751
	[SerializeField]
	private GameObject animator_object;

	// Token: 0x040002F0 RID: 752
	[SerializeField]
	private Animator animator;

	// Token: 0x040002F1 RID: 753
	private AnimationClip[] animClips;
}
