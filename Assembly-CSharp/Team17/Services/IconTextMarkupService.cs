using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Rewired;
using Steamworks;
using T17.Services;
using Team17.Common;
using UnityEngine;

namespace Team17.Services
{
	// Token: 0x020001C6 RID: 454
	public class IconTextMarkupService : MonoBehaviour, IIconTextMarkupService, IService
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000F10 RID: 3856 RVA: 0x000527A4 File Offset: 0x000509A4
		// (remove) Token: 0x06000F11 RID: 3857 RVA: 0x000527DC File Offset: 0x000509DC
		public event Action RefreshedIconsEvent;

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x00052811 File Offset: 0x00050A11
		public bool HasCachedControllerMaps
		{
			get
			{
				return this._HaveCachedControllerMaps;
			}
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0005281C File Offset: 0x00050A1C
		public async void OnStartUp()
		{
			this._HaveCachedControllerMaps = false;
			await this.SetupInputSpriteBindingMarkupObjects();
			await this.CacheControllerMaps();
			Player player = ReInput.players.GetPlayer(0);
			if (player != null)
			{
				player.controllers.ControllerAddedEvent += this.OnControllerAdded;
				player.controllers.ControllerRemovedEvent += this.OnControllerRemoved;
			}
			ReInput.players.GetSystemPlayer().controllers.ControllerAddedEvent += this.OnControllerAdded;
			ReInput.players.GetSystemPlayer().controllers.ControllerRemovedEvent += this.OnControllerRemoved;
			Services.EngagementService.OnPrimaryUserEngagedEvent += this.OnPrimaryUserEngaged;
			Services.EngagementService.OnPrimaryUserDisengagedEvent += this.OnPrimaryUserDisengaged;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00052854 File Offset: 0x00050A54
		public void OnCleanUp()
		{
			if (ReInput.isReady && ReInput.players != null)
			{
				Player player = ReInput.players.GetPlayer(0);
				if (player != null)
				{
					player.controllers.ControllerAddedEvent -= this.OnControllerAdded;
					player.controllers.ControllerRemovedEvent -= this.OnControllerRemoved;
				}
				ReInput.players.GetSystemPlayer().controllers.ControllerAddedEvent -= this.OnControllerAdded;
				ReInput.players.GetSystemPlayer().controllers.ControllerRemovedEvent -= this.OnControllerRemoved;
			}
			Services.EngagementService.OnPrimaryUserEngagedEvent -= this.OnPrimaryUserEngaged;
			Services.EngagementService.OnPrimaryUserDisengagedEvent -= this.OnPrimaryUserDisengaged;
			this.ClearCachedControllerMaps(IconTextMarkupService.ControllerType.All);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00052926 File Offset: 0x00050B26
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00052928 File Offset: 0x00050B28
		public string ReplaceMirandaMarkupWithTMPSpriteMarkup(string mirandaText, out bool stringConverted)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			StringBuilder stringBuilder = new StringBuilder();
			stringConverted = false;
			int i = 0;
			int length = mirandaText.Length;
			while (i < length)
			{
				if (num != 0)
				{
					if (num == 1)
					{
						if (mirandaText[i] == '}')
						{
							int num4 = i - 1;
							stringBuilder.Append(mirandaText, num3, num2 - num3 - 1);
							string text = mirandaText.Substring(num2, num4 - num2 + 1);
							string rewiredActionName = this._InputActionBindings.GetRewiredActionName(text);
							if (!string.IsNullOrEmpty(rewiredActionName))
							{
								string tmpspriteTag = this.GetTMPSpriteTag(rewiredActionName, true);
								stringBuilder.Append(tmpspriteTag);
							}
							i = num4 + 1;
							num3 = num4 + 2;
							num = 0;
						}
					}
				}
				else if (mirandaText[i] == '{')
				{
					num2 = i + 1;
					num = 1;
				}
				i++;
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(mirandaText, num3, mirandaText.Length - num3);
				stringConverted = true;
				return stringBuilder.ToString();
			}
			return mirandaText;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x00052A14 File Offset: 0x00050C14
		public string GetTMPSpriteTag(string inputAction, bool addBrackets = true)
		{
			bool flag = Services.InputService.IsLastActiveInputController();
			if (flag)
			{
				this._CurrentSpriteMarkupMap = this._ControllerSpriteMarkupMap;
			}
			else
			{
				this._CurrentSpriteMarkupMap = this._KeyboardSpriteControllerMarkupMap;
			}
			string text = string.Empty;
			if (!this._HaveCachedControllerMaps)
			{
				T17Debug.LogError("[IconTextMarkupService] GetTMPSpriteTag has been called but the controller maps haven't been loaded/cached yet. Change the calling code to wait for _HaveCachedControllerMaps is true before calling this function");
				return string.Empty;
			}
			IList<ControllerMap> list;
			if (flag)
			{
				list = this._CachedJoystickControllerMaps;
			}
			else
			{
				list = this._CachedKeyboarMouseControllerMaps;
			}
			if (list != null)
			{
				bool flag2 = false;
				foreach (ControllerMap controllerMap in list)
				{
					if (controllerMap != null)
					{
						foreach (ActionElementMap actionElementMap in controllerMap.AllMaps)
						{
							if (actionElementMap != null && (actionElementMap.actionDescriptiveName == inputAction || string.Compare(actionElementMap.elementIdentifierName, inputAction, StringComparison.OrdinalIgnoreCase) == 0))
							{
								text = actionElementMap.elementIdentifierName;
								if (flag)
								{
									InputSpriteBindingMarkupObject.BindingTypeEnum bindingType = this._CurrentSpriteMarkupMap.BindingType;
									if (bindingType != InputSpriteBindingMarkupObject.BindingTypeEnum.GenericElementName)
									{
										if (bindingType == InputSpriteBindingMarkupObject.BindingTypeEnum.ActionName)
										{
											text = inputAction;
										}
									}
									else if (actionElementMap.controllerMap.controller.templateCount > 0 && actionElementMap.controllerMap.controller.Templates[0].GetElementTargets(actionElementMap, this._tempTemplateElementTargets) > 0)
									{
										text = this._tempTemplateElementTargets[0].descriptiveName;
									}
								}
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = inputAction;
			}
			if (this._CurrentSpriteMarkupMap != null)
			{
				string tmpspriteTag = this._CurrentSpriteMarkupMap.GetTMPSpriteTag(text);
				if (!string.IsNullOrEmpty(tmpspriteTag))
				{
					return tmpspriteTag;
				}
			}
			if (addBrackets)
			{
				return "[" + text + "]";
			}
			return text;
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00052C1C File Offset: 0x00050E1C
		private async Task CacheControllerMaps()
		{
			Player player = null;
			if (this._primaryUserHasEngaged)
			{
				player = ReInput.players.GetPlayer(0);
			}
			else
			{
				player = ReInput.players.GetSystemPlayer();
			}
			if (player.controllers.joystickCount > 0)
			{
				Joystick joystick = player.controllers.Joysticks[0];
				this.ClearCachedControllerMaps(IconTextMarkupService.ControllerType.Joystick);
				this._CachedJoystickControllerMaps = player.controllers.maps.GetMaps(joystick.type, joystick.id);
				await this.SetupInputSpriteBindingMarkupObjects();
			}
			if (this._CachedKeyboarMouseControllerMaps == null && (player.controllers.hasKeyboard || player.controllers.hasMouse))
			{
				this._CachedKeyboarMouseControllerMaps = new List<ControllerMap>(player.controllers.maps.GetMaps(player.controllers.Keyboard.type, player.controllers.Keyboard.id));
				IList<ControllerMap> maps = player.controllers.maps.GetMaps(player.controllers.Mouse.type, player.controllers.Mouse.id);
				for (int i = 0; i < maps.Count; i++)
				{
					this._CachedKeyboarMouseControllerMaps.Add(maps[i]);
				}
			}
			this._HaveCachedControllerMaps = this._CachedJoystickControllerMaps != null || this._CachedKeyboarMouseControllerMaps != null;
			Action refreshedIconsEvent = this.RefreshedIconsEvent;
			if (refreshedIconsEvent != null)
			{
				refreshedIconsEvent();
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00052C60 File Offset: 0x00050E60
		public void ClearCachedControllerMaps(IconTextMarkupService.ControllerType controller)
		{
			if (controller.HasFlag(IconTextMarkupService.ControllerType.KeyboardMouse) && this._CachedKeyboarMouseControllerMaps != null)
			{
				this._CachedKeyboarMouseControllerMaps.Clear();
				this._CachedKeyboarMouseControllerMaps = null;
			}
			if (controller.HasFlag(IconTextMarkupService.ControllerType.Joystick))
			{
				this._CachedJoystickControllerMaps = null;
			}
			this._HaveCachedControllerMaps = this._CachedJoystickControllerMaps != null || this._CachedKeyboarMouseControllerMaps != null;
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x00052CD0 File Offset: 0x00050ED0
		private async void OnControllerAdded(ControllerAssignmentChangedEventArgs args)
		{
			await this.SetupInputSpriteBindingMarkupObjects();
			await this.CacheControllerMaps();
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00052D07 File Offset: 0x00050F07
		private void OnControllerRemoved(ControllerAssignmentChangedEventArgs args)
		{
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00052D0C File Offset: 0x00050F0C
		private async void OnPrimaryUserEngaged()
		{
			this._primaryUserHasEngaged = true;
			await this.SetupInputSpriteBindingMarkupObjects();
			await this.CacheControllerMaps();
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00052D43 File Offset: 0x00050F43
		private void OnPrimaryUserDisengaged()
		{
			this._primaryUserHasEngaged = false;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00052D4C File Offset: 0x00050F4C
		private async Task SetupInputSpriteBindingMarkupObjects()
		{
			Player player = null;
			if (this._primaryUserHasEngaged)
			{
				player = ReInput.players.GetPlayer(0);
			}
			else
			{
				player = ReInput.players.GetSystemPlayer();
			}
			this._ControllerSpriteMarkupMap = this._PCControllerSpriteMarkupMaps[0];
			if (player.controllers.joystickCount > 0)
			{
				Joystick joystick = player.controllers.Joysticks[0];
				bool flag = false;
				int i = 0;
				int num = this._PCControllerSpriteMarkupMaps.Length;
				while (i < num)
				{
					if (this._PCControllerSpriteMarkupMaps[i].Matches(joystick.hardwareTypeGuid, joystick.hardwareName, joystick.hardwareIdentifier))
					{
						this._ControllerSpriteMarkupMap = this._PCControllerSpriteMarkupMaps[i];
						flag = true;
						break;
					}
					i++;
				}
				if (!flag)
				{
					this._ControllerSpriteMarkupMap = this._DefaultGenericSpriteControllerMarkupMap;
				}
				await this._ControllerSpriteMarkupMap.CacheTMPAssets();
			}
			if (player.controllers.hasKeyboard && this._KeyboardSpriteControllerMarkupMap != null)
			{
				await this._KeyboardSpriteControllerMarkupMap.CacheTMPAssets();
			}
			if (Services.InputService.IsLastActiveInputController())
			{
				this._CurrentSpriteMarkupMap = this._ControllerSpriteMarkupMap;
			}
			else
			{
				this._CurrentSpriteMarkupMap = this._KeyboardSpriteControllerMarkupMap;
			}
			if (SteamManager.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				InputSpriteBindingMarkupObject steamDeckControllerSpriteMarkupMap = this._SteamDeckControllerSpriteMarkupMap;
				this._CurrentSpriteMarkupMap = steamDeckControllerSpriteMarkupMap;
				this._ControllerSpriteMarkupMap = steamDeckControllerSpriteMarkupMap;
				await this._ControllerSpriteMarkupMap.CacheTMPAssets();
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00052D90 File Offset: 0x00050F90
		[Conditional("DEBUG")]
		private void DumpRewiredDebugInfo()
		{
			Player player = ReInput.players.GetPlayer(0);
			Joystick joystick = null;
			if (player != null && player.controllers.joystickCount > 0)
			{
				joystick = player.controllers.Joysticks[0];
			}
			for (int i = 0; i < 2; i++)
			{
				IList<ControllerMap> list;
				if (i == 0)
				{
					list = this._CachedJoystickControllerMaps;
					if (joystick != null)
					{
						for (int j = 0; j < joystick.buttonCount; j++)
						{
						}
						for (int k = 0; k < joystick.axisCount; k++)
						{
						}
					}
				}
				else
				{
					list = this._CachedKeyboarMouseControllerMaps;
				}
				if (list != null)
				{
					IList<ControllerTemplateElementTarget> list2 = new List<ControllerTemplateElementTarget>(2);
					foreach (ControllerMap controllerMap in list)
					{
						ReInput.mapping.GetMapCategory(controllerMap.categoryId);
						ReInput.mapping.GetJoystickLayout(controllerMap.layoutId);
						foreach (ActionElementMap actionElementMap in controllerMap.AllMaps)
						{
							if (actionElementMap != null)
							{
								ControllerElementTarget controllerElementTarget = new ControllerElementTarget(actionElementMap);
								if (Services.InputService.IsLastActiveInputController() && joystick != null)
								{
									foreach (IControllerTemplate controllerTemplate in joystick.Templates)
									{
										list2.Clear();
										controllerTemplate.GetElementTargets(controllerElementTarget, list2);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000D7F RID: 3455
		private const int kDefaultPlayerIndex = 0;

		// Token: 0x04000D80 RID: 3456
		private const int kDefaultControllerIndex = 0;

		// Token: 0x04000D81 RID: 3457
		[SerializeField]
		private InputActionBindingObject _InputActionBindings;

		// Token: 0x04000D82 RID: 3458
		[SerializeField]
		private InputSpriteBindingMarkupObject[] _PCControllerSpriteMarkupMaps;

		// Token: 0x04000D83 RID: 3459
		[SerializeField]
		private InputSpriteBindingMarkupObject _KeyboardSpriteControllerMarkupMap;

		// Token: 0x04000D84 RID: 3460
		[SerializeField]
		private InputSpriteBindingMarkupObject _DefaultGenericSpriteControllerMarkupMap;

		// Token: 0x04000D85 RID: 3461
		[SerializeField]
		private InputSpriteBindingMarkupObject _PS5ControllerSpriteMarkupMap;

		// Token: 0x04000D86 RID: 3462
		[SerializeField]
		private InputSpriteBindingMarkupObject _XSXControllerSpriteMarkupMap;

		// Token: 0x04000D87 RID: 3463
		[SerializeField]
		private InputSpriteBindingMarkupObject _SwitchControllerSpriteMarkupMap;

		// Token: 0x04000D88 RID: 3464
		[SerializeField]
		private InputSpriteBindingMarkupObject _SteamDeckControllerSpriteMarkupMap;

		// Token: 0x04000D89 RID: 3465
		private IList<ControllerMap> _CachedJoystickControllerMaps;

		// Token: 0x04000D8A RID: 3466
		private IList<ControllerMap> _CachedKeyboarMouseControllerMaps;

		// Token: 0x04000D8B RID: 3467
		private InputSpriteBindingMarkupObject _ControllerSpriteMarkupMap;

		// Token: 0x04000D8C RID: 3468
		private InputSpriteBindingMarkupObject _CurrentSpriteMarkupMap;

		// Token: 0x04000D8E RID: 3470
		private List<ControllerTemplateElementTarget> _tempTemplateElementTargets = new List<ControllerTemplateElementTarget>();

		// Token: 0x04000D8F RID: 3471
		private bool _HaveCachedControllerMaps;

		// Token: 0x04000D90 RID: 3472
		private bool _primaryUserHasEngaged;

		// Token: 0x02000394 RID: 916
		[Flags]
		public enum ControllerType : byte
		{
			// Token: 0x04001413 RID: 5139
			KeyboardMouse = 1,
			// Token: 0x04001414 RID: 5140
			Joystick = 2,
			// Token: 0x04001415 RID: 5141
			All = 255
		}
	}
}
