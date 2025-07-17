using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000115 RID: 277
public class MainMenuSubScreen : MonoBehaviour
{
	// Token: 0x0600097C RID: 2428 RVA: 0x00036ED3 File Offset: 0x000350D3
	public void OpenSubScreen()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00036EE4 File Offset: 0x000350E4
	public void HideMainMenu()
	{
		this.mainMenu.SetActive(false);
		if (this.newGameButton != null)
		{
			this.UpdateImageColorAlpha(this.newGameButton, 0);
		}
		if (this.loadGameButton != null)
		{
			this.UpdateImageColorAlpha(this.loadGameButton, 0);
		}
		if (this.optionsButton != null)
		{
			this.UpdateImageColorAlpha(this.optionsButton, 0);
		}
		if (this.creditsButton != null)
		{
			this.UpdateImageColorAlpha(this.creditsButton, 0);
		}
		if (this.exitButton != null)
		{
			this.UpdateImageColorAlpha(this.exitButton, 0);
		}
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x00036F84 File Offset: 0x00035184
	private void UpdateImageColorAlpha(GameObject gameObject, int alpha)
	{
		Color color = gameObject.GetComponent<Image>().color;
		Color color2 = new Color(color.r, color.g, color.b, (float)alpha);
		this.newGameButton.GetComponent<Image>().color = color2;
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00036FC9 File Offset: 0x000351C9
	public void ActivateDateADex()
	{
		this.dateADexScreen.SetActive(false);
		this.dateADex.SetActive(true);
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00036FE3 File Offset: 0x000351E3
	public void ReturnToMainMenuScreen()
	{
		if (BetterPlayerControl.Instance)
		{
			BetterPlayerControl.Instance.ChangePlayerState(BetterPlayerControl.PlayerState.CantControl);
			BetterPlayerControl.Instance.StopBeamSounds();
		}
		base.StartCoroutine(this.FadeOut());
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00037013 File Offset: 0x00035213
	private IEnumerator FadeOut()
	{
		yield return new WaitForEndOfFrame();
		this.animations.TriggerAnimationAtIndex(1);
		yield return new WaitUntil(() => !this.animations.IsAnimatingAtIndex(1));
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040008BD RID: 2237
	[SerializeField]
	private GameObject mainMenu;

	// Token: 0x040008BE RID: 2238
	[SerializeField]
	private GameObject dateADex;

	// Token: 0x040008BF RID: 2239
	[SerializeField]
	private GameObject dateADexScreen;

	// Token: 0x040008C0 RID: 2240
	[SerializeField]
	private GameObject newGameButton;

	// Token: 0x040008C1 RID: 2241
	[SerializeField]
	private GameObject loadGameButton;

	// Token: 0x040008C2 RID: 2242
	[SerializeField]
	private GameObject optionsButton;

	// Token: 0x040008C3 RID: 2243
	[SerializeField]
	private GameObject creditsButton;

	// Token: 0x040008C4 RID: 2244
	[SerializeField]
	private GameObject exitButton;

	// Token: 0x040008C5 RID: 2245
	[SerializeField]
	private DoCodeAnimation animations;
}
