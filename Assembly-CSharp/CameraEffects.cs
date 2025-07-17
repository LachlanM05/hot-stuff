using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[ExecuteAlways]
public class CameraEffects : MonoBehaviour
{
	// Token: 0x06000124 RID: 292 RVA: 0x00008D04 File Offset: 0x00006F04
	private void LateUpdate()
	{
		Vector3 normalized = (this.Player.transform.position - base.transform.position).normalized;
		RaycastHit raycastHit;
		if (Application.isPlaying && Physics.Raycast(base.transform.position, normalized, out raycastHit, Vector3.Distance(base.transform.position, this.Player.transform.position), this.lm, QueryTriggerInteraction.Ignore))
		{
			this.cutout = Mathf.Lerp(this.cutout, this.circlesize.Evaluate(Vector3.Distance(this.Player.transform.position, base.transform.position)), 0.25f);
			this.lasthit = raycastHit.point;
		}
		else
		{
			this.cutout = Mathf.Lerp(this.cutout, 0f, 0.25f);
		}
		Shader.SetGlobalVector("_PlayerPosition", this.Player.transform.position);
		Shader.SetGlobalVector("_PlayerScreenPosition", Camera.main.WorldToScreenPoint(this.Player.transform.position));
		Shader.SetGlobalVector("_CameraPosition", base.transform.position);
		Shader.SetGlobalFloat("_CutoutSize", this.cutout);
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00008E62 File Offset: 0x00007062
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(this.lasthit, 0.5f);
	}

	// Token: 0x04000131 RID: 305
	public GameObject Player;

	// Token: 0x04000132 RID: 306
	private Vector3 lasthit;

	// Token: 0x04000133 RID: 307
	public LayerMask lm;

	// Token: 0x04000134 RID: 308
	public AnimationCurve circlesize;

	// Token: 0x04000135 RID: 309
	private float cutout;
}
