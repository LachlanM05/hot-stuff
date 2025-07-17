using System;
using T17.Services;
using Team17.Platform.User;
using TMPro;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class UserIdDisplayer : MonoBehaviour
{
	// Token: 0x06000D3C RID: 3388 RVA: 0x0004BE20 File Offset: 0x0004A020
	private void Start()
	{
		Services.EngagementService.OnPrimaryUserEngagedEvent += this.SetUserIdText;
		Services.EngagementService.OnPrimaryUserDisengagedEvent += this.SetUserIdText;
		Services.UserService.OnUsernameChangedEvent += this.OnUsernameChangedEvent;
		if (this._DisplayText == null)
		{
			this._DisplayText = base.gameObject.GetComponentInChildren<TMP_Text>();
		}
		this.SetUserIdText();
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x0004BE94 File Offset: 0x0004A094
	private void OnDestroy()
	{
		Services.EngagementService.OnPrimaryUserEngagedEvent -= this.SetUserIdText;
		Services.EngagementService.OnPrimaryUserDisengagedEvent -= this.SetUserIdText;
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x0004BEC4 File Offset: 0x0004A0C4
	private void SetUserIdText()
	{
		if (this._DisplayText == null)
		{
			return;
		}
		if (Services.UserService == null)
		{
			this._DisplayText.enabled = false;
			return;
		}
		IUser user;
		if (Services.UserService.TryGetPrimaryUser(out user))
		{
			this._DisplayText.enabled = true;
			this._DisplayText.text = user.DisplayName;
			return;
		}
		this._DisplayText.enabled = false;
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x0004BF2C File Offset: 0x0004A12C
	private void OnUsernameChangedEvent(IUser user)
	{
		this.SetUserIdText();
	}

	// Token: 0x04000BFC RID: 3068
	[SerializeField]
	private TMP_Text _DisplayText;
}
