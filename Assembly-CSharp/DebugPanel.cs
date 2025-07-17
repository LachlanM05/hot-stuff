using System;
using T17.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000045 RID: 69
public class DebugPanel : MonoBehaviour
{
	// Token: 0x060001C3 RID: 451 RVA: 0x0000B5D0 File Offset: 0x000097D0
	private void Awake()
	{
		this.sensitivitySlider.onValueChanged.AddListener(new UnityAction<float>(this.SetCameraSensitivity));
		this.smoothnessSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetCameraSmoothRate));
		DebugPanel.instance = this;
		this.debugPanel.SetActive(false);
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000B627 File Offset: 0x00009827
	public void ToggleUI()
	{
		if (this.uiShown)
		{
			Singleton<CanvasUIManager>.Instance.Hide();
			return;
		}
		Singleton<CanvasUIManager>.Instance.Show();
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000B646 File Offset: 0x00009846
	public void EnableMouse()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000B654 File Offset: 0x00009854
	public void DisableMouse()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000B662 File Offset: 0x00009862
	public void IncreaseTimeOfDay()
	{
		Singleton<DayNightCycle>.Instance.ForceIncrementTime();
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000B66E File Offset: 0x0000986E
	public void DecreaseTimeOfDay()
	{
		Singleton<DayNightCycle>.Instance.ForceDecrementTime();
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000B67A File Offset: 0x0000987A
	public void SetCameraSensitivity(float val)
	{
		Services.GameSettings.SetFloat("cameraSENSITIVITY", val);
		this.sensitivityValue.text = val.ToString();
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000B69E File Offset: 0x0000989E
	public void SetCameraSmoothRate(float val)
	{
		BetterPlayerControl.Instance.easingTime = val;
		this.smoothnessValue.text = val.ToString();
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000B6BD File Offset: 0x000098BD
	public void ToggleDebugPanel()
	{
		this.debugPanel.SetActive(!this.debugPanel.activeSelf);
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000B6D8 File Offset: 0x000098D8
	public void RestoreDateCharges()
	{
		Singleton<Dateviators>.Instance.ResetCharges();
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000B6E4 File Offset: 0x000098E4
	public void UnlockAttic()
	{
		AtticDoorUnlocker atticDoorUnlocker = Object.FindObjectOfType<AtticDoorUnlocker>(true);
		if (atticDoorUnlocker != null)
		{
			atticDoorUnlocker.gameObject.SetActive(true);
			MovingDateable.MoveDateable("MovingPlants", "attic", true);
			AtticDoorUnlocker.instance.StartSequence();
		}
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000B728 File Offset: 0x00009928
	public void RestartTutorial()
	{
		Singleton<Save>.Instance.SetTutorialFinished(false);
		string playerName = Singleton<Save>.Instance.GetPlayerName();
		string playerTown = Singleton<Save>.Instance.GetPlayerTown();
		Singleton<Save>.Instance.NewGame();
		Singleton<Save>.Instance.SetPlayerName(playerName);
		Singleton<Save>.Instance.SetPlayerPronouns(0);
		Singleton<Save>.Instance.SetPlayerTown(playerTown);
		Singleton<ChatMaster>.Instance.ClearChatHistory();
		MovingDateable.MoveDateable("Computer", "tutorial", true);
		MovingDateable.MoveDateable("MovingOfficeDoor", "locked", true);
		MovingDateable.MoveDateable("MovingOfficeDoorCloset", "locked", true);
		Singleton<TutorialController>.Instance.LoadState();
	}

	// Token: 0x040002AE RID: 686
	public static DebugPanel instance;

	// Token: 0x040002AF RID: 687
	[Header("References")]
	[SerializeField]
	private GameObject debugPanel;

	// Token: 0x040002B0 RID: 688
	[SerializeField]
	private Slider sensitivitySlider;

	// Token: 0x040002B1 RID: 689
	[SerializeField]
	private TextMeshProUGUI sensitivityValue;

	// Token: 0x040002B2 RID: 690
	[SerializeField]
	private Slider smoothnessSlider;

	// Token: 0x040002B3 RID: 691
	[SerializeField]
	private TextMeshProUGUI smoothnessValue;

	// Token: 0x040002B4 RID: 692
	private bool uiShown = true;
}
