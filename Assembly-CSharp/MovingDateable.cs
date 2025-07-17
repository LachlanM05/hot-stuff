using System;
using System.Collections.Generic;
using Team17.Common;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class MovingDateable : MonoBehaviour, IReloadHandler
{
	// Token: 0x060005A5 RID: 1445 RVA: 0x000203BF File Offset: 0x0001E5BF
	private void Start()
	{
		string inkVar = this.InkVar;
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x000203C8 File Offset: 0x0001E5C8
	public GameObject GetMovableObject(string key)
	{
		foreach (MovingObject movingObject in this.Locations)
		{
			if (movingObject.Key == key)
			{
				return movingObject.Object;
			}
		}
		return null;
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00020430 File Offset: 0x0001E630
	public static MovingDateable GetMovingDateable(string tag)
	{
		GameObject gameObject = GameObject.FindWithTag(tag);
		if (gameObject != null)
		{
			MovingDateable component = gameObject.GetComponent<MovingDateable>();
			if (component == null)
			{
				T17Debug.LogError("Missing component for " + tag);
			}
			return component;
		}
		T17Debug.LogError("Missing object for " + tag);
		return null;
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00020480 File Offset: 0x0001E680
	public static void MoveDateable(string tag, string key, bool playVFX = true)
	{
		GameObject gameObject = GameObject.FindWithTag(tag);
		if (gameObject != null)
		{
			MovingDateable component = gameObject.GetComponent<MovingDateable>();
			if (component == null)
			{
				T17Debug.LogError("Missing component for " + tag + " " + key);
			}
			component.MoveDateable(key, playVFX);
			return;
		}
		T17Debug.LogError("Missing object for " + tag + " " + key);
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x000204E0 File Offset: 0x0001E6E0
	public static void ForceMoveDateable(string tag, string key, bool isActive)
	{
		GameObject gameObject = GameObject.FindWithTag(tag);
		if (gameObject != null)
		{
			gameObject.GetComponent<MovingDateable>().ForceMoveDateable(key, isActive);
		}
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0002050C File Offset: 0x0001E70C
	public static void MoveDateableEnableAllKeys(string tag)
	{
		GameObject gameObject = GameObject.FindWithTag(tag);
		if (gameObject != null)
		{
			foreach (MovingObject movingObject in gameObject.GetComponent<MovingDateable>().Locations)
			{
				if (movingObject.Object != null)
				{
					movingObject.Object.SetActive(true);
				}
			}
		}
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00020588 File Offset: 0x0001E788
	public static GameObject GetDateable(string tag, string key)
	{
		GameObject gameObject = GameObject.FindWithTag(tag);
		if (gameObject != null)
		{
			return gameObject.GetComponent<MovingDateable>().GetMovableObject(key);
		}
		return null;
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x000205B4 File Offset: 0x0001E7B4
	public void MoveDateable(string newPos, bool playVFX = false)
	{
		foreach (MovingObject movingObject in this.Locations)
		{
			if (movingObject.Object == null)
			{
				T17Debug.LogError("Missing object for " + base.name + " " + movingObject.Key);
			}
			else if (movingObject.Key == newPos)
			{
				if (!this.InvertActiveRule)
				{
					if (playVFX && this.ShouldPlayVFX)
					{
						VFXEvents.Instance.SpawnVisualEffect(movingObject.Object.transform.position, VFXEvents.VisualEffectType.EVENT);
					}
					movingObject.Object.SetActive(true);
				}
				else
				{
					movingObject.Object.SetActive(false);
				}
			}
			else if (!this.KeepActive)
			{
				movingObject.Object.SetActive(false);
			}
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x000206A4 File Offset: 0x0001E8A4
	public void ForceMoveDateable(string newPos, bool isActive)
	{
		foreach (MovingObject movingObject in this.Locations)
		{
			if (movingObject.Object == null)
			{
				T17Debug.LogError("Missing object for " + base.name + " " + movingObject.Key);
			}
			else if (movingObject.Key == newPos)
			{
				movingObject.Object.SetActive(isActive);
			}
		}
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0002073C File Offset: 0x0001E93C
	public void LoadState()
	{
		bool flag = false;
		foreach (MovingObject movingObject in this.Locations)
		{
			if (movingObject != null && !(movingObject.Object == null))
			{
				if (!Singleton<Save>.Instance.HasInteractableState(this.InkVar + "_position_" + movingObject.Key))
				{
					if (movingObject.Object == null)
					{
						T17Debug.LogError("Missing object for " + base.name + " " + movingObject.Key);
					}
					if (this.InvertActiveRule)
					{
						if (movingObject.Object != null)
						{
							movingObject.Object.SetActive(true);
						}
					}
					else if (movingObject.Object != null)
					{
						movingObject.Object.SetActive(false);
					}
				}
				else
				{
					bool interactableState = Singleton<Save>.Instance.GetInteractableState(this.InkVar + "_position_" + movingObject.Key);
					if (movingObject.Object == null)
					{
						T17Debug.LogError("Missing object for " + base.name + " " + movingObject.Key);
					}
					if (this.InvertActiveRule)
					{
						if (movingObject.Object != null)
						{
							movingObject.Object.SetActive(!interactableState);
						}
					}
					else if (movingObject.Object != null)
					{
						movingObject.Object.SetActive(interactableState);
					}
					flag = flag || interactableState;
				}
			}
		}
		if (!flag && !this.InvertActiveRule && !this.KeepActive)
		{
			foreach (MovingObject movingObject2 in this.Locations)
			{
				if (movingObject2.Object == null)
				{
					T17Debug.LogError("Missing object for " + base.name + " " + movingObject2.Key);
				}
				if (movingObject2.Object != null)
				{
					movingObject2.Object.SetActive(false);
				}
			}
			if (this.Locations[0].Object != null)
			{
				this.Locations[0].Object.SetActive(true);
			}
		}
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x000209C4 File Offset: 0x0001EBC4
	public void SaveState()
	{
		foreach (MovingObject movingObject in this.Locations)
		{
			if (movingObject.Object == null)
			{
				T17Debug.LogError(base.name + " " + movingObject.Key + " object is missing. Cannot save state.");
			}
			else if (this.InvertActiveRule)
			{
				Singleton<Save>.Instance.SetInteractableState(this.InkVar + "_position_" + movingObject.Key, !movingObject.Object.activeInHierarchy);
			}
			else
			{
				Singleton<Save>.Instance.SetInteractableState(this.InkVar + "_position_" + movingObject.Key, movingObject.Object.activeInHierarchy);
			}
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00020AAC File Offset: 0x0001ECAC
	public int Priority()
	{
		return 500;
	}

	// Token: 0x0400056C RID: 1388
	public string InkVar;

	// Token: 0x0400056D RID: 1389
	public List<MovingObject> Locations;

	// Token: 0x0400056E RID: 1390
	public bool KeepActive;

	// Token: 0x0400056F RID: 1391
	public bool InvertActiveRule;

	// Token: 0x04000570 RID: 1392
	public bool ShouldPlayVFX = true;
}
