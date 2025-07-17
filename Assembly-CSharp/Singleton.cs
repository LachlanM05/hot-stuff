using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000D7 RID: 215
public class Singleton<TYPE> : MonoBehaviour, iDuplicateEvent where TYPE : MonoBehaviour, iDuplicateEvent, new()
{
	// Token: 0x0600070B RID: 1803 RVA: 0x00027CDC File Offset: 0x00025EDC
	public virtual void Awake()
	{
		if (Singleton<TYPE>._instance != null && Singleton<TYPE>._instance != this)
		{
			Object.Destroy(base.gameObject);
			Singleton<TYPE>._instance.OnDuplicateDestroyed();
			return;
		}
		Singleton<TYPE>._instance = base.GetComponent<TYPE>();
		this.AwakeSingleton();
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00027D39 File Offset: 0x00025F39
	public virtual void AwakeSingleton()
	{
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x00027D3B File Offset: 0x00025F3B
	public virtual void OnDuplicateDestroyed()
	{
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00027D3D File Offset: 0x00025F3D
	public void SetStart()
	{
		this.isStarted = true;
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x0600070F RID: 1807 RVA: 0x00027D48 File Offset: 0x00025F48
	public static TYPE Instance
	{
		get
		{
			if (Singleton<TYPE>._instance == null)
			{
				Singleton<TYPE>._instance = Object.FindObjectOfType<TYPE>(true);
				if (Singleton<TYPE>._instance == null)
				{
					typeof(TYPE).ToString();
					int buildIndex = SceneManager.GetActiveScene().buildIndex;
				}
			}
			return Singleton<TYPE>._instance;
		}
	}

	// Token: 0x0400066F RID: 1647
	protected static TYPE _instance;

	// Token: 0x04000670 RID: 1648
	protected bool isStarted;
}
