using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class SquirrelCamera : MonoBehaviour
{
	// Token: 0x06000019 RID: 25 RVA: 0x000027E4 File Offset: 0x000009E4
	private void Update()
	{
		Vector3 vector = this.target.position + this.offset;
		Vector3 vector2 = Vector3.Lerp(base.transform.position, vector, this.SmoothSpeed * Time.deltaTime);
		base.transform.position = vector2;
		base.transform.LookAt(this.target);
	}

	// Token: 0x04000017 RID: 23
	public Vector3 offset;

	// Token: 0x04000018 RID: 24
	public Transform target;

	// Token: 0x04000019 RID: 25
	public float SmoothSpeed = 1f;
}
