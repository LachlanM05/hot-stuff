using System;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using T17.UI;
using Team17.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000E5 RID: 229
public class CanvasUIManager : Singleton<CanvasUIManager>
{
	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000788 RID: 1928 RVA: 0x0002A606 File Offset: 0x00028806
	// (set) Token: 0x06000789 RID: 1929 RVA: 0x0002A60E File Offset: 0x0002880E
	public bool isInGame
	{
		get
		{
			return this._IsInGame;
		}
		set
		{
			if (value != this._IsInGame)
			{
				this._IsInGame = value;
				this._menuHistor.Clear();
			}
		}
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0002A62C File Offset: 0x0002882C
	public void Start()
	{
		this.isInGame = false;
		this._menuComponentList = base.GetComponentsInChildren<MenuComponent>(true);
		for (int i = 0; i < this._menuComponentList.Length; i++)
		{
			this._menuComponentList[i].gameObject.SetActive(false);
		}
		this.SwitchMenu(this.DefaultEnabledMenu);
		this.TemporaryHackSoWecanClosetheHousePicture();
		this.settingsMenu.SetSettings();
		Object.DontDestroyOnLoad(this);
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x0002A696 File Offset: 0x00028896
	public override void AwakeSingleton()
	{
		PlayerPauser._freeze += this.Pause;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x0002A6AC File Offset: 0x000288AC
	private void Update()
	{
		if (this.pause)
		{
			return;
		}
		if (this.player == null && ReInput.players != null)
		{
			this.player = ReInput.players.GetPlayer(0);
			return;
		}
		if (this.player != null && this.player.GetButtonDown("Pause") && this.PausableMenus.Contains(this._activeMenu) && Singleton<GameController>.Instance != null && Singleton<GameController>.Instance.viewState != VIEW_STATE.TALKING && !PlayerPauser.IsPaused() && !BetterPlayerControl.Instance.isGameEndingOn && !Singleton<PhoneManager>.Instance.BlockPhoneOpening)
		{
			this.SwitchMenu(this._pauseRef);
		}
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0002A754 File Offset: 0x00028954
	public MenuComponent FindMenuComponent(string name)
	{
		return this._menuComponentList.FirstOrDefault((MenuComponent x) => x.name == name);
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0002A788 File Offset: 0x00028988
	public void SwitchMainMenu(MenuComponent desiredMenu)
	{
		if (desiredMenu != null && desiredMenu.gameObject != null)
		{
			desiredMenu.gameObject.SetActive(true);
			this._activeMenu = desiredMenu;
			if (ValidateQuestions.Instance)
			{
				if (ValidateQuestions.Instance.gameObject.activeInHierarchy)
				{
					ValidateQuestions.Instance.visible = true;
					if (ValidateQuestions.Instance.nameTextField != null)
					{
						ValidateQuestions.Instance.nameTextField.GetComponent<NameEntrySounds>().Initialize();
						ValidateQuestions.Instance.townTextField.GetComponent<NameEntrySounds>().Initialize();
						return;
					}
				}
				else
				{
					ValidateQuestions.Instance.visible = false;
				}
			}
		}
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x0002A834 File Offset: 0x00028A34
	public void SwitchMenu(MenuComponent _type)
	{
		if (this._activeMenu != null)
		{
			this._activeMenu.gameObject.SetActive(false);
			this.PushToHistory(this._activeMenu);
		}
		MenuComponent menuComponent = this._menuComponentList.FirstOrDefault((MenuComponent x) => x.name == _type.MenuObjectName);
		if (menuComponent != null)
		{
			menuComponent.gameObject.SetActive(true);
			this._activeMenu = menuComponent;
		}
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0002A8B0 File Offset: 0x00028AB0
	public void InactivateMainMenu()
	{
		foreach (MenuComponent menuComponent in this._menuHistor.ToList<MenuComponent>())
		{
			if (menuComponent.name == "MainMenu")
			{
				menuComponent.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0002A920 File Offset: 0x00028B20
	public static void SwitchMenu(string name)
	{
		if (name == "HUD")
		{
			Singleton<CanvasUIManager>.Instance.InactivateMainMenu();
		}
		Singleton<CanvasUIManager>.Instance.SwitchMenu(Singleton<CanvasUIManager>.Instance.FindMenuComponent(name));
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0002A94E File Offset: 0x00028B4E
	public static bool IsActiveMenu(string name)
	{
		return Singleton<CanvasUIManager>.Instance._activeMenu.gameObject.name == name;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x0002A96F File Offset: 0x00028B6F
	public void backtomainmenu()
	{
		this.SwitchMenu(this.DefaultEnabledMenu);
		SceneManager.LoadScene(0);
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x0002A983 File Offset: 0x00028B83
	public void BackAndUnpause()
	{
		this.Back();
		PlayerPauser.Unpause();
		CursorLocker.Lock();
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0002A995 File Offset: 0x00028B95
	public void Back()
	{
		this.BackWithReturn();
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x0002A9A0 File Offset: 0x00028BA0
	public void SetActiveMenuInactive()
	{
		if (this._menuHistor.Count > 0)
		{
			this._activeMenu.gameObject.SetActive(false);
			MenuComponent menuComponent = this.PopFromHistory();
			menuComponent.gameObject.SetActive(true);
			this._activeMenu = menuComponent;
		}
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0002A9E6 File Offset: 0x00028BE6
	private void ResetAnimation()
	{
		this.inAnimation = false;
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x0002A9F0 File Offset: 0x00028BF0
	public bool BackWithReturn()
	{
		if (this.inAnimation)
		{
			return true;
		}
		if (this._menuHistor.Count != 0)
		{
			if (Singleton<PhoneManager>.Instance.IsPhoneAppOpened())
			{
				OverrideClosePhone component = Singleton<PhoneManager>.Instance.GetCurrentApp().GetComponent<OverrideClosePhone>();
				if (component == null || !component.InvokeClose())
				{
					Singleton<PhoneManager>.Instance.ReturnToMainPhoneScreen();
				}
				return false;
			}
			if (this._activeMenu.gameObject.ToString() != "Phone Menu (UnityEngine.GameObject)")
			{
				this.SetActiveMenuInactive();
			}
			return true;
		}
		else
		{
			if (!this.isInGame)
			{
				T17Debug.LogError("No last active canvas to go back to. You must goto a menu via SwitchMenu() before Back() will work correctly.");
				return false;
			}
			return true;
		}
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0002AA86 File Offset: 0x00028C86
	public void Hide()
	{
		if (this._activeMenu is PauseScreen && this._activeMenu.gameObject.activeSelf)
		{
			T17Debug.LogError("[PHONE] Dont directly hide the phone. You must use ClosePhoneAsync to prevent issues.");
		}
		this._activeMenu.gameObject.SetActive(false);
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0002AAC2 File Offset: 0x00028CC2
	public void Show()
	{
		this._activeMenu.gameObject.SetActive(true);
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0002AAD5 File Offset: 0x00028CD5
	private void Pause(bool _pause)
	{
		this.pause = _pause;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0002AAE0 File Offset: 0x00028CE0
	private void TemporaryHackSoWecanClosetheHousePicture()
	{
		Computer[] componentsInChildren = base.transform.GetComponentsInChildren<Computer>(true);
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			if (componentsInChildren[i].name == "Close Icon" && componentsInChildren[i].transform.parent.parent.parent.parent.name == "HouseWindow")
			{
				if (componentsInChildren[i].GetComponent<CancelButtonTrigger>() == null)
				{
					componentsInChildren[i].gameObject.AddComponent<CancelButtonTrigger>();
				}
				if (componentsInChildren[i].GetComponent<SelectOnEnabled>() == null)
				{
					componentsInChildren[i].gameObject.AddComponent<SelectOnEnabled>();
				}
			}
			i++;
		}
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0002AB90 File Offset: 0x00028D90
	private MenuComponent PopFromHistory()
	{
		int count = this._menuHistor.Count;
		MenuComponent menuComponent = null;
		if (count > 0)
		{
			menuComponent = this._menuHistor[count - 1];
			this._menuHistor.RemoveAt(count - 1);
		}
		return menuComponent;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0002ABCD File Offset: 0x00028DCD
	private void PushToHistory(MenuComponent newItem)
	{
		this._menuHistor.Add(newItem);
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x0002ABDC File Offset: 0x00028DDC
	public bool RemoveFromHistory(MenuComponent removeItem)
	{
		bool flag = false;
		for (int i = this._menuHistor.Count - 1; i >= 0; i--)
		{
			if (this._menuHistor[i] == removeItem)
			{
				this._menuHistor.RemoveAt(i);
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0002AC21 File Offset: 0x00028E21
	public void PhoneClosed(MenuComponent phoneItem)
	{
		this.RemoveFromHistory(phoneItem);
		if (this._activeMenu == phoneItem)
		{
			this.SetActiveMenuInactive();
		}
	}

	// Token: 0x040006CC RID: 1740
	[Tooltip("The menu set here is the first the player sees.")]
	public MenuComponent DefaultEnabledMenu;

	// Token: 0x040006CD RID: 1741
	[SerializeField]
	public MenuComponent[] _menuComponentList;

	// Token: 0x040006CE RID: 1742
	public List<MenuComponent> BaseMenus;

	// Token: 0x040006CF RID: 1743
	public List<MenuComponent> PausableMenus;

	// Token: 0x040006D0 RID: 1744
	[SerializeField]
	public MenuComponent _workspaceRef;

	// Token: 0x040006D1 RID: 1745
	[SerializeField]
	public MenuComponent _pauseRef;

	// Token: 0x040006D2 RID: 1746
	[SerializeField]
	public MenuComponent _mainMenuRef;

	// Token: 0x040006D3 RID: 1747
	[SerializeField]
	public MenuComponent _activeMenu;

	// Token: 0x040006D4 RID: 1748
	private List<MenuComponent> _menuHistor = new List<MenuComponent>();

	// Token: 0x040006D5 RID: 1749
	private Player player;

	// Token: 0x040006D6 RID: 1750
	public bool pause;

	// Token: 0x040006D7 RID: 1751
	private bool inAnimation;

	// Token: 0x040006D8 RID: 1752
	private bool _IsInGame;

	// Token: 0x040006D9 RID: 1753
	public bool shouldUseLastCursorState;

	// Token: 0x040006DA RID: 1754
	[SerializeField]
	private SettingsMenu settingsMenu;
}
