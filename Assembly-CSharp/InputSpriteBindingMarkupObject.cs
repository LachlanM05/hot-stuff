using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rewired.Data.Mapping;
using TMPro;
using UnityEngine;

// Token: 0x02000179 RID: 377
[CreateAssetMenu(menuName = "ScriptableObjects/Team17/Input Binding Markup Definition (Sprite)", fileName = "InputMarkupDefinition")]
public class InputSpriteBindingMarkupObject : ScriptableObject
{
	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000D43 RID: 3395 RVA: 0x0004BF96 File Offset: 0x0004A196
	public InputSpriteBindingMarkupObject.BindingTypeEnum BindingType
	{
		get
		{
			return this.m_BindingType;
		}
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x0004BF9E File Offset: 0x0004A19E
	public string DeviceName(int index)
	{
		if (this.m_DeviceNames != null || (index >= 0 && index < this.m_DeviceNames.Length))
		{
			return this.m_DeviceNames[index];
		}
		return string.Empty;
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x0004BFC8 File Offset: 0x0004A1C8
	public bool Matches(Guid hardwareGuid, string hardwareName, string hardwareIdentifier)
	{
		int i = 0;
		int num = this.m_HardwareJoystickMap.Length;
		while (i < num)
		{
			if (this.m_HardwareJoystickMap[i].Guid == hardwareGuid)
			{
				return true;
			}
			i++;
		}
		int j = 0;
		int num2 = this.m_DeviceNames.Length;
		while (j < num2)
		{
			if (this.m_DeviceNames[j] == hardwareName)
			{
				return true;
			}
			if (this.m_DeviceNames[j] == hardwareIdentifier)
			{
				return true;
			}
			j++;
		}
		return false;
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0004C03C File Offset: 0x0004A23C
	public int GetSpriteIndex(string rewiredElementName)
	{
		int i = 0;
		int num = this.m_DeviceBindingSprites.Length;
		while (i < num)
		{
			if (string.Compare(this.m_DeviceBindingSprites[i].RewiredElementName, rewiredElementName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.m_DeviceBindingSprites[i].spriteId;
			}
			i++;
		}
		if (string.Compare(rewiredElementName, "Engagement Input", StringComparison.OrdinalIgnoreCase) == 0)
		{
			return this.m_EngagementInputSpriteIndex;
		}
		return -1;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x0004C0A0 File Offset: 0x0004A2A0
	public string GetTMPSpriteTag(string rewiredElement)
	{
		int spriteIndex = this.GetSpriteIndex(rewiredElement);
		if (spriteIndex >= 0 && this.m_SpriteAsset != null)
		{
			return string.Format("<sprite=\"{0}\" index={1}>", this.m_SpriteAsset.name, spriteIndex);
		}
		return string.Empty;
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x0004C0E8 File Offset: 0x0004A2E8
	public Task CacheTMPAssets()
	{
		InputSpriteBindingMarkupObject.<CacheTMPAssets>d__16 <CacheTMPAssets>d__;
		<CacheTMPAssets>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CacheTMPAssets>d__.<>4__this = this;
		<CacheTMPAssets>d__.<>1__state = -1;
		<CacheTMPAssets>d__.<>t__builder.Start<InputSpriteBindingMarkupObject.<CacheTMPAssets>d__16>(ref <CacheTMPAssets>d__);
		return <CacheTMPAssets>d__.<>t__builder.Task;
	}

	// Token: 0x04000BFE RID: 3070
	private const string kEngagementInputRewiredElementName = "Engagement Input";

	// Token: 0x04000BFF RID: 3071
	[Tooltip("What the Rewired Element Name in the Device Binding Sprites represents.\n\nControllerElementName: Map specific controller elements to icons e.g. 'Cross' on PS5 => Sprite Id \n\nGenericElementName: Map generic elements to icons e.g. 'Action Button 1' => Sprite Id \n\nActionName: map action to icon e.g. 'Interact' => Sprite Id")]
	[SerializeField]
	private InputSpriteBindingMarkupObject.BindingTypeEnum m_BindingType;

	// Token: 0x04000C00 RID: 3072
	[Space]
	[SerializeField]
	private string[] m_DeviceNames;

	// Token: 0x04000C01 RID: 3073
	[Tooltip("The hardware device to match against. The list of supported hardware devices are in Assets/Plugins/Rewired/Internal/Data/Controllers/HardwareMaps/Joysticks/... ")]
	[SerializeField]
	private HardwareJoystickMap[] m_HardwareJoystickMap;

	// Token: 0x04000C02 RID: 3074
	[SerializeField]
	private AssetReferenceTMP_SpriteAsset m_SpriteAssetRef;

	// Token: 0x04000C03 RID: 3075
	[SerializeField]
	private InputSpriteBindingMarkupObject.RewiredIdSpritePair[] m_DeviceBindingSprites;

	// Token: 0x04000C04 RID: 3076
	[SerializeField]
	private int m_EngagementInputSpriteIndex;

	// Token: 0x04000C05 RID: 3077
	private TMP_SpriteAsset m_SpriteAsset;

	// Token: 0x0200036A RID: 874
	[Serializable]
	internal struct RewiredIdSpritePair
	{
		// Token: 0x04001371 RID: 4977
		public string RewiredElementName;

		// Token: 0x04001372 RID: 4978
		public int spriteId;
	}

	// Token: 0x0200036B RID: 875
	public enum BindingTypeEnum
	{
		// Token: 0x04001374 RID: 4980
		ControllerElementName,
		// Token: 0x04001375 RID: 4981
		GenericElementName,
		// Token: 0x04001376 RID: 4982
		ActionName
	}
}
