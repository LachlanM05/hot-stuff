using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class InitControlMenu : MonoBehaviour
{
	// Token: 0x06000957 RID: 2391 RVA: 0x000360E2 File Offset: 0x000342E2
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		this._objects = new List<GameObject>();
		this.UpdateControlMenu();
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x00036108 File Offset: 0x00034308
	public void UpdateControlMenu()
	{
		this.DestroyPrevious();
		this._maps = new List<ControllerMap>();
		this.player.controllers.maps.GetAllMaps(this._maps);
		for (int i = 0; i < this._maps.Count; i++)
		{
			for (int j = 0; j < this._maps[i].AllMaps.Count; j++)
			{
				ActionElementMap actionElementMap = this._maps[i].AllMaps[j];
				if (actionElementMap != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.ControlPrefab, base.gameObject.transform);
					this._objects.Add(gameObject);
					gameObject.GetComponent<InitControlItem>().Initialize(ReInput.mapping.GetAction(actionElementMap.actionId).name, actionElementMap.elementIdentifierName);
				}
			}
		}
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x000361E8 File Offset: 0x000343E8
	private void DestroyPrevious()
	{
		if (this._objects != null)
		{
			foreach (GameObject gameObject in this._objects)
			{
				Object.DestroyImmediate(gameObject);
			}
		}
	}

	// Token: 0x04000892 RID: 2194
	private Player player;

	// Token: 0x04000893 RID: 2195
	public GameObject ControlPrefab;

	// Token: 0x04000894 RID: 2196
	private List<ControllerMap> _maps;

	// Token: 0x04000895 RID: 2197
	private List<GameObject> _objects;
}
