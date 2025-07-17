using System;
using System.Collections.Generic;
using Team17.Common;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class Rat : MonoBehaviour, IReloadHandler
{
	// Token: 0x060004F3 RID: 1267 RVA: 0x0001DA40 File Offset: 0x0001BC40
	private void Start()
	{
		if (this.InkVar == null)
		{
			string text = "MovingDateable ink variable not declared in ";
			GameObject gameObject = base.gameObject;
			T17Debug.LogError(text + ((gameObject != null) ? gameObject.ToString() : null));
			return;
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0001DA6C File Offset: 0x0001BC6C
	public void MoveDateable(string newPos)
	{
		foreach (MovingObject movingObject in this.RatState1)
		{
			if (movingObject.Key == newPos)
			{
				movingObject.Object.gameObject.SetActive(true);
				for (int i = 0; i < movingObject.Object.gameObject.transform.childCount; i++)
				{
					GameObject gameObject = movingObject.Object.gameObject.transform.GetChild(i).gameObject;
					gameObject.SetActive(true);
					for (int j = 0; j < gameObject.transform.childCount; j++)
					{
						gameObject.transform.GetChild(j).gameObject.SetActive(true);
					}
				}
			}
			else
			{
				movingObject.Object.gameObject.SetActive(false);
			}
		}
		foreach (MovingObject movingObject2 in this.RatState2)
		{
			if (movingObject2.Key == newPos)
			{
				movingObject2.Object.gameObject.SetActive(true);
				for (int k = 0; k < movingObject2.Object.gameObject.transform.childCount; k++)
				{
					GameObject gameObject2 = movingObject2.Object.gameObject.transform.GetChild(k).gameObject;
					gameObject2.SetActive(true);
					for (int l = 0; l < gameObject2.transform.childCount; l++)
					{
						gameObject2.transform.GetChild(l).gameObject.SetActive(true);
					}
				}
			}
			else
			{
				movingObject2.Object.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x0001DC64 File Offset: 0x0001BE64
	public void LoadState()
	{
		bool flag = false;
		foreach (MovingObject movingObject in this.RatState1)
		{
			bool interactableState = Singleton<Save>.Instance.GetInteractableState(this.InkVar + "_position1_" + movingObject.Key);
			movingObject.Object.SetActive(interactableState);
			flag = flag || interactableState;
		}
		if (!flag)
		{
			this.RatState1[0].Object.SetActive(true);
		}
		flag = false;
		foreach (MovingObject movingObject2 in this.RatState2)
		{
			bool interactableState2 = Singleton<Save>.Instance.GetInteractableState(this.InkVar + "_position2_" + movingObject2.Key);
			movingObject2.Object.SetActive(interactableState2);
			flag = flag || interactableState2;
		}
		if (!flag)
		{
			this.RatState2[0].Object.SetActive(true);
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x0001DD8C File Offset: 0x0001BF8C
	public void SaveState()
	{
		foreach (MovingObject movingObject in this.RatState1)
		{
			Singleton<Save>.Instance.SetInteractableState(this.InkVar + "_position1_" + movingObject.Key, movingObject.Object.activeInHierarchy);
		}
		foreach (MovingObject movingObject2 in this.RatState2)
		{
			Singleton<Save>.Instance.SetInteractableState(this.InkVar + "_position2_" + movingObject2.Key, movingObject2.Object.activeInHierarchy);
		}
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x0001DE6C File Offset: 0x0001C06C
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x040004E9 RID: 1257
	public string InkVar;

	// Token: 0x040004EA RID: 1258
	public List<MovingObject> RatState1;

	// Token: 0x040004EB RID: 1259
	public List<MovingObject> RatState2;
}
