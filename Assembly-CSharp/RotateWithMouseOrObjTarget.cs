using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200012E RID: 302
public class RotateWithMouseOrObjTarget : MonoBehaviour
{
	// Token: 0x06000A55 RID: 2645 RVA: 0x0003BA14 File Offset: 0x00039C14
	private void Start()
	{
		this.initialRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x0003BA35 File Offset: 0x00039C35
	private void OnEnable()
	{
		this.running = true;
		this.stop = false;
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x0003BA45 File Offset: 0x00039C45
	private void Update()
	{
		if (!Singleton<PhoneManager>.Instance.phoneOpened)
		{
			this.ResetImmediately();
			return;
		}
		if (Singleton<PhoneManager>.Instance.toggling)
		{
			this.ResetRotation();
			return;
		}
		if (this.running)
		{
			this.lookAtMouse();
		}
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0003BA7B File Offset: 0x00039C7B
	public void StartTracking()
	{
		this.running = true;
		this.stop = false;
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x0003BA8B File Offset: 0x00039C8B
	public void ResetRotation()
	{
		base.transform.DOLocalRotateQuaternion(this.initialRotation, this.resetSpeed);
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0003BAA5 File Offset: 0x00039CA5
	public void ResetRotationQuick()
	{
		this.running = false;
		base.transform.DOLocalRotateQuaternion(this.initialRotation, 0.1f);
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0003BAC5 File Offset: 0x00039CC5
	public void ResetImmediately()
	{
		this.running = false;
		this.stop = true;
		base.transform.rotation = this.initialRotation;
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0003BAE8 File Offset: 0x00039CE8
	private void lookAtMouse()
	{
		Vector3 vector = Vector3.one;
		if (this.centerButton != null && this.centerButton.CompareTag(this.UIButtonElementTag))
		{
			vector = this.centerButton.transform.localPosition;
		}
		Vector3 vector2 = vector;
		if (this.UIButtonElementTag == null || this.UIButtonElementTag == "")
		{
			if (this.cam)
			{
				vector = base.transform.position - this.cam.ScreenToWorldPoint(Input.mousePosition);
			}
		}
		else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.CompareTag(this.UIButtonElementTag))
		{
			vector = base.transform.localPosition - EventSystem.current.currentSelectedGameObject.transform.localPosition;
		}
		if (!this.stop && vector != vector2)
		{
			float num = vector.y * this.manualTiltAmount;
			float num2 = vector.x * this.manualTiltAmount;
			float z = base.transform.eulerAngles.z;
			float num3 = Mathf.LerpAngle(base.transform.eulerAngles.x, num, this.tiltSpeed * Time.deltaTime);
			float num4 = Mathf.LerpAngle(base.transform.eulerAngles.y, num2, this.tiltSpeed * Time.deltaTime);
			float num5 = Mathf.LerpAngle(base.transform.eulerAngles.z, z, this.tiltSpeed / 2f * Time.deltaTime);
			base.transform.eulerAngles = new Vector3(num3, num4, num5);
		}
	}

	// Token: 0x04000977 RID: 2423
	[SerializeField]
	private float resetSpeed = 0.3f;

	// Token: 0x04000978 RID: 2424
	[SerializeField]
	[Range(0.001f, 1f)]
	private float manualTiltAmount = 0.04f;

	// Token: 0x04000979 RID: 2425
	[SerializeField]
	private float tiltSpeed = 20f;

	// Token: 0x0400097A RID: 2426
	[SerializeField]
	private Camera cam;

	// Token: 0x0400097B RID: 2427
	[Header("If Tag follow UI with that tag instead of mouse")]
	[SerializeField]
	private string UIButtonElementTag;

	// Token: 0x0400097C RID: 2428
	[SerializeField]
	private GameObject centerButton;

	// Token: 0x0400097D RID: 2429
	private Quaternion initialRotation;

	// Token: 0x0400097E RID: 2430
	private bool running = true;

	// Token: 0x0400097F RID: 2431
	private bool stop;
}
