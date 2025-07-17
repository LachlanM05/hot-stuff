using System;
using T17.Services;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class ArtGalleryTool : MonoBehaviour
{
	// Token: 0x060000B0 RID: 176 RVA: 0x00004744 File Offset: 0x00002944
	private void OnEnable()
	{
		Object.DontDestroyOnLoad(this);
		if (!Debug.isDebugBuild)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x0000475E File Offset: 0x0000295E
	private void Start()
	{
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00004760 File Offset: 0x00002960
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F3))
		{
			this._charArtImgs = Services.AssetProviderService.LoadResourceAssets<Texture2D>("");
			this._index = 0;
			this._active = !this._active;
			if (this._active)
			{
				CursorLocker.Unlock();
				return;
			}
			CursorLocker.Lock();
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000047B8 File Offset: 0x000029B8
	private void OnGUI()
	{
		if (!this._active)
		{
			return;
		}
		GUI.Box(new Rect(10f, 10f, (float)(Screen.width - 20), (float)(Screen.height - 20)), "Art Gallery");
		if (GUI.Button(new Rect(20f, (float)(Screen.height / 2), 80f, 20f), "Back"))
		{
			this._index--;
			if (this._index < 0)
			{
				this._index = this._charArtImgs.Length - 1;
			}
		}
		if (GUI.Button(new Rect((float)(Screen.width - 100), (float)(Screen.height / 2), 80f, 20f), "Next"))
		{
			this._index++;
			if (this._index > this._charArtImgs.Length - 1)
			{
				this._index = 0;
			}
		}
		GUI.DrawTexture(new Rect(90f, 30f, (float)(Screen.width - 150), (float)(Screen.height - 80)), this._charArtImgs[this._index], ScaleMode.ScaleToFit);
		GUI.Box(new Rect(20f, (float)(Screen.height - 30), 320f, 20f), this._charArtImgs[this._index].name);
		GUI.Box(new Rect(20f, (float)(Screen.height - 60), 80f, 20f), this._index.ToString() + "/" + this._charArtImgs.Length.ToString());
	}

	// Token: 0x040000AA RID: 170
	private Texture2D[] _charArtImgs;

	// Token: 0x040000AB RID: 171
	private int _index;

	// Token: 0x040000AC RID: 172
	private bool _active;
}
