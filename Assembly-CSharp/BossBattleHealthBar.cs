using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class BossBattleHealthBar : Singleton<BossBattleHealthBar>
{
	// Token: 0x06000110 RID: 272 RVA: 0x00008680 File Offset: 0x00006880
	public override void AwakeSingleton()
	{
		base.AwakeSingleton();
		this.playerHealthBar.gameObject.SetActive(false);
		this.bossHealthBar.gameObject.SetActive(false);
		this.chanceHealthBar.gameObject.SetActive(false);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000086BB File Offset: 0x000068BB
	public void DisableBars()
	{
		this.playerHealthBar.gameObject.SetActive(false);
		this.bossHealthBar.gameObject.SetActive(false);
		this.chanceHealthBar.gameObject.SetActive(false);
		base.StopAllCoroutines();
	}

	// Token: 0x06000112 RID: 274 RVA: 0x000086F8 File Offset: 0x000068F8
	public void StartBattle(bool isDishyBattle, bool isChanceBattle)
	{
		this.isDishyBattle = isDishyBattle;
		this.isChanceBattle = isChanceBattle;
		if (!isChanceBattle)
		{
			this.playerHealthBar.gameObject.SetActive(true);
		}
		else
		{
			this.chanceHealthBar.gameObject.SetActive(true);
		}
		this.currentPlayerHealth = 0;
		this.currentDishyHealth = 0;
		if (!isDishyBattle)
		{
			this.currentPlayerHealth = int.Parse(Singleton<InkController>.Instance.GetVariable("chance_health"));
			this.chanceHealthBarText.SetText(this.currentPlayerHealth.ToString(), true);
		}
		else
		{
			this.bossHealthBar.gameObject.SetActive(true);
			this.currentPlayerHealth = int.Parse(Singleton<InkController>.Instance.GetVariable("player_hp"));
			this.currentDishyHealth = int.Parse(Singleton<InkController>.Instance.GetVariable("dishy_hp"));
			this.playerHealthBarText.SetText(this.currentPlayerHealth.ToString(), true);
			this.bossHealthBarText.SetText(this.currentDishyHealth.ToString(), true);
			this.StartDishyHealthBar();
		}
		if (!isChanceBattle)
		{
			this.StartPlayerHealthBar();
			return;
		}
		this.StartChanceHealthBar();
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00008807 File Offset: 0x00006A07
	public void EndBattle(bool isDishyBattle, bool isChanceBattle)
	{
		this.isDishyBattle = isDishyBattle;
		this.isChanceBattle = isChanceBattle;
		if (!isChanceBattle)
		{
			this.HideDishyHealthBar();
			this.HidePlayerHealthBar();
			return;
		}
		this.HideChanceHealthBar();
	}

	// Token: 0x06000114 RID: 276 RVA: 0x0000882D File Offset: 0x00006A2D
	private void StartChanceHealthBar()
	{
		this.chanceHealthBar.gameObject.SetActive(true);
		this.UpdatePlayerHealth();
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00008846 File Offset: 0x00006A46
	private void StartPlayerHealthBar()
	{
		this.playerHealthBar.gameObject.SetActive(true);
		this.UpdatePlayerHealth();
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000885F File Offset: 0x00006A5F
	private void StartDishyHealthBar()
	{
		this.bossHealthBar.gameObject.SetActive(true);
		this.UpdateDishyHealth();
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00008878 File Offset: 0x00006A78
	private void HideChanceHealthBar()
	{
		this.chanceHealthBar.SetTrigger("Hide");
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000888A File Offset: 0x00006A8A
	public void RevivePlayer()
	{
		this.playerHealthBar.gameObject.SetActive(true);
		this.playerHealthBar.SetTrigger("Revive");
	}

	// Token: 0x06000119 RID: 281 RVA: 0x000088AD File Offset: 0x00006AAD
	private void HidePlayerHealthBar()
	{
		this.playerHealthBar.SetTrigger("Hide");
	}

	// Token: 0x0600011A RID: 282 RVA: 0x000088BF File Offset: 0x00006ABF
	private void HideDishyHealthBar()
	{
		this.bossHealthBar.SetTrigger("Hide");
	}

	// Token: 0x0600011B RID: 283 RVA: 0x000088D4 File Offset: 0x00006AD4
	public void UpdatePlayerHealth()
	{
		int num;
		if (!this.isChanceBattle)
		{
			num = int.Parse(Singleton<InkController>.Instance.GetVariable("player_hp"));
		}
		else
		{
			num = int.Parse(Singleton<InkController>.Instance.GetVariable("chance_health"));
		}
		if (num < 0)
		{
			num = 0;
		}
		if (num == this.currentPlayerHealth)
		{
			return;
		}
		if (!this.isChanceBattle)
		{
			this.playerHealthBar.SetInteger("hp", num);
		}
		else
		{
			this.chanceHealthBar.SetInteger("hp", num);
		}
		if (num > this.currentPlayerHealth)
		{
			if (!this.isChanceBattle)
			{
				this.playerHealthBar.SetTrigger("Recover");
			}
			else
			{
				this.chanceHealthBar.SetTrigger("Recover");
			}
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_player_health_recovered.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_player_health_recovered, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		else
		{
			if (!this.isChanceBattle)
			{
				this.playerHealthBar.SetTrigger("Damaged");
			}
			else
			{
				this.chanceHealthBar.SetTrigger("Damaged");
			}
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_player_health_damaged.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_player_health_damaged, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		if (num <= 0)
		{
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.stinger_ending_hate.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_hate, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.STINGER, false);
		}
		base.StartCoroutine(this.AnimatePlayerHpUpdate(this.currentPlayerHealth, num));
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00008A96 File Offset: 0x00006C96
	private IEnumerator AnimatePlayerHpUpdate(int currentHp, int newHp)
	{
		int tempHp = currentHp;
		float animationTime = 0.3f / (float)Math.Abs(currentHp - newHp);
		if (tempHp > newHp)
		{
			while (tempHp > newHp)
			{
				int num = tempHp;
				tempHp = num - 1;
				if (!this.isChanceBattle)
				{
					this.playerHealthBarText.SetText(tempHp.ToString(), true);
				}
				else
				{
					this.chanceHealthBarText.SetText(tempHp.ToString(), true);
				}
				yield return new WaitForSeconds(animationTime);
			}
		}
		else
		{
			while (tempHp < newHp)
			{
				int num = tempHp;
				tempHp = num + 1;
				if (!this.isChanceBattle)
				{
					this.playerHealthBarText.SetText(tempHp.ToString(), true);
				}
				else
				{
					this.chanceHealthBarText.SetText(tempHp.ToString(), true);
				}
				yield return new WaitForSeconds(animationTime);
			}
		}
		this.currentPlayerHealth = newHp;
		yield break;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00008AB3 File Offset: 0x00006CB3
	private IEnumerator AnimateDishyHpUpdate(int currentHp, int newHp)
	{
		int tempHp = currentHp;
		float animationTime = 0.3f / (float)Math.Abs(currentHp - newHp);
		if (tempHp > newHp)
		{
			while (tempHp > newHp)
			{
				int num = tempHp;
				tempHp = num - 1;
				this.bossHealthBarText.SetText(tempHp.ToString(), true);
				yield return new WaitForSeconds(animationTime);
			}
		}
		else
		{
			while (tempHp < newHp)
			{
				int num = tempHp;
				tempHp = num + 1;
				this.bossHealthBarText.SetText(tempHp.ToString(), true);
				yield return new WaitForSeconds(animationTime);
			}
		}
		this.currentDishyHealth = newHp;
		yield break;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00008AD0 File Offset: 0x00006CD0
	public void UpdateDishyHealth()
	{
		int num = int.Parse(Singleton<InkController>.Instance.GetVariable("dishy_hp"));
		if (num < 0)
		{
			num = 0;
		}
		if (num == this.currentDishyHealth)
		{
			return;
		}
		this.bossHealthBar.SetInteger("hp", num);
		if (num > this.currentDishyHealth)
		{
			this.bossHealthBar.SetTrigger("Recovered");
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_boss_health_recovered.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_boss_health_recovered, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		else
		{
			this.bossHealthBar.SetTrigger("Damaged");
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_boss_health_damaged.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_boss_health_damaged, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		if (num <= 0)
		{
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.stinger_ending_realized.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_realized, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.STINGER, false);
		}
		base.StartCoroutine(this.AnimateDishyHpUpdate(this.currentDishyHealth, num));
	}

	// Token: 0x04000125 RID: 293
	[SerializeField]
	private Animator playerHealthBar;

	// Token: 0x04000126 RID: 294
	[SerializeField]
	private Animator bossHealthBar;

	// Token: 0x04000127 RID: 295
	[SerializeField]
	private Animator chanceHealthBar;

	// Token: 0x04000128 RID: 296
	[SerializeField]
	private TextMeshProUGUI chanceHealthBarText;

	// Token: 0x04000129 RID: 297
	[SerializeField]
	private TextMeshProUGUI playerHealthBarText;

	// Token: 0x0400012A RID: 298
	[SerializeField]
	private TextMeshProUGUI bossHealthBarText;

	// Token: 0x0400012B RID: 299
	private int currentPlayerHealth;

	// Token: 0x0400012C RID: 300
	private int currentDishyHealth;

	// Token: 0x0400012D RID: 301
	private bool isChanceBattle;

	// Token: 0x0400012E RID: 302
	private bool isDishyBattle;
}
