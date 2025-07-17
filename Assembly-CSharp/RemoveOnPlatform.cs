using System;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000172 RID: 370
public class RemoveOnPlatform : MonoBehaviour
{
	// Token: 0x06000D22 RID: 3362 RVA: 0x0004B688 File Offset: 0x00049888
	private void Awake()
	{
		if (this._gameObjectToRemove == null)
		{
			this._gameObjectToRemove = base.gameObject;
		}
		if (!RemoveOnPlatform.ShouldRemoveOnCurrentPlatform(this._platformsToRemoveOn))
		{
			base.enabled = false;
			return;
		}
		this.ReconstructNavigation();
		switch (this._removalMethod)
		{
		default:
			Object.Destroy(this._gameObjectToRemove);
			return;
		case RemoveOnPlatform.RemovalMethod.DisableGameObject:
			this._gameObjectToRemove.SetActive(false);
			return;
		case RemoveOnPlatform.RemovalMethod.EnableGameObject:
			this._gameObjectToRemove.SetActive(true);
			return;
		}
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x0004B708 File Offset: 0x00049908
	public static bool ShouldRemoveOnCurrentPlatform(RemoveOnPlatform.Platform currentPlatform)
	{
		return (currentPlatform.HasFlag(RemoveOnPlatform.Platform.Steam_BigScreen) && SteamUtils.IsSteamInBigPictureMode()) || (currentPlatform.HasFlag(RemoveOnPlatform.Platform.SteamDeck) && SteamUtils.IsSteamRunningOnSteamDeck()) || currentPlatform.HasFlag(RemoveOnPlatform.Platform.Steam_Windows);
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x0004B764 File Offset: 0x00049964
	private void ReconstructNavigation()
	{
		Selectable[] componentsInChildren = this._gameObjectToRemove.GetComponentsInChildren<Selectable>();
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			Selectable selectable = componentsInChildren[i];
			Navigation navigation = componentsInChildren[i].navigation;
			Selectable selectOnUp = navigation.selectOnUp;
			Selectable selectOnDown = navigation.selectOnDown;
			Selectable selectOnLeft = navigation.selectOnLeft;
			Selectable selectOnRight = navigation.selectOnRight;
			if (selectOnLeft && selectOnLeft.navigation.selectOnRight == selectable)
			{
				Navigation navigation2 = selectOnLeft.navigation;
				navigation2.selectOnRight = navigation.selectOnRight;
				selectOnLeft.navigation = navigation2;
			}
			if (selectOnRight && selectOnRight.navigation.selectOnLeft == selectable)
			{
				Navigation navigation3 = selectOnRight.navigation;
				navigation3.selectOnLeft = navigation.selectOnLeft;
				selectOnRight.navigation = navigation3;
			}
			if (selectOnUp && selectOnUp.navigation.selectOnDown == selectable)
			{
				Navigation navigation4 = selectOnUp.navigation;
				navigation4.selectOnDown = navigation.selectOnDown;
				selectOnUp.navigation = navigation4;
			}
			if (selectOnDown && selectOnDown.navigation.selectOnUp == selectable)
			{
				Navigation navigation5 = selectOnDown.navigation;
				navigation5.selectOnUp = navigation.selectOnUp;
				selectOnDown.navigation = navigation5;
			}
			i++;
		}
	}

	// Token: 0x04000BDD RID: 3037
	[SerializeField]
	protected RemoveOnPlatform.Platform _platformsToRemoveOn = RemoveOnPlatform.Platform.Console;

	// Token: 0x04000BDE RID: 3038
	[SerializeField]
	protected RemoveOnPlatform.RemovalMethod _removalMethod;

	// Token: 0x04000BDF RID: 3039
	[Tooltip("If set to None then destroy/disable/enable the game object this component is attached to. If set, then destroy/disable that gameobject")]
	[SerializeField]
	protected GameObject _gameObjectToRemove;

	// Token: 0x02000366 RID: 870
	[Flags]
	public enum Platform : byte
	{
		// Token: 0x0400135C RID: 4956
		Steam_Windows = 1,
		// Token: 0x0400135D RID: 4957
		GameCore_Windows = 4,
		// Token: 0x0400135E RID: 4958
		GameCore_Xbox = 8,
		// Token: 0x0400135F RID: 4959
		PS5 = 16,
		// Token: 0x04001360 RID: 4960
		Switch = 32,
		// Token: 0x04001361 RID: 4961
		Steam_BigScreen = 64,
		// Token: 0x04001362 RID: 4962
		SteamDeck = 128,
		// Token: 0x04001363 RID: 4963
		Console = 56,
		// Token: 0x04001364 RID: 4964
		GameCore = 12,
		// Token: 0x04001365 RID: 4965
		PC = 5
	}

	// Token: 0x02000367 RID: 871
	public enum RemovalMethod
	{
		// Token: 0x04001367 RID: 4967
		DestroyGameObject,
		// Token: 0x04001368 RID: 4968
		DisableGameObject,
		// Token: 0x04001369 RID: 4969
		EnableGameObject
	}
}
