using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E7 RID: 231
public class ChangeTextColor : MonoBehaviour
{
	// Token: 0x060007A7 RID: 1959 RVA: 0x0002ACAB File Offset: 0x00028EAB
	public void ResetColor()
	{
		this.ChangeColor(this.defaultColor, this.backingColor);
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0002ACBF File Offset: 0x00028EBF
	public void ChangeColor()
	{
		this.ChangeColor(this.changeColor, this.backingChangeColor);
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x0002ACD4 File Offset: 0x00028ED4
	public void PlaySound(string specs, bool failed = true, bool playSound = true, bool andComplete = false)
	{
		if (playSound)
		{
			if (!failed)
			{
				if (!(specs == "smarts"))
				{
					if (!(specs == "poise"))
					{
						if (!(specs == "empathy"))
						{
							if (!(specs == "charm"))
							{
								if (specs == "sass")
								{
									Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_sass.name, 0f);
									Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_sass, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
								}
							}
							else
							{
								Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_charm.name, 0f);
								Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_charm, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
							}
						}
						else
						{
							Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_empathy.name, 0f);
							Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_empathy, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
						}
					}
					else
					{
						Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_poise.name, 0f);
						Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_poise, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					}
				}
				else
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_smarts.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_smarts, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				}
			}
			else if (!(specs == "smarts"))
			{
				if (!(specs == "poise"))
				{
					if (!(specs == "empathy"))
					{
						if (!(specs == "charm"))
						{
							if (specs == "sass")
							{
								Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_fail_sass.name, 0f);
								Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_fail_sass, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
							}
						}
						else
						{
							Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_fail_charm.name, 0f);
							Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_fail_charm, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
						}
					}
					else
					{
						Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_fail_empathy.name, 0f);
						Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_fail_empathy, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					}
				}
				else
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_fail_poise.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_fail_poise, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				}
			}
			else
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_fail_smarts.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_fail_smarts, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
		}
		if (andComplete)
		{
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_realization_recipe_complete.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_realization_recipe_complete, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0002B0A8 File Offset: 0x000292A8
	private void ChangeColor(Color32 color, Color32 backingColor)
	{
		TMP_Text[] array = this.texts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = color;
		}
		this.images[0].color = color;
		this.images[1].color = backingColor;
	}

	// Token: 0x040006DE RID: 1758
	[SerializeField]
	private Color32 changeColor;

	// Token: 0x040006DF RID: 1759
	[SerializeField]
	private Color32 defaultColor;

	// Token: 0x040006E0 RID: 1760
	[SerializeField]
	private Color32 backingColor;

	// Token: 0x040006E1 RID: 1761
	[SerializeField]
	private Color32 backingChangeColor;

	// Token: 0x040006E2 RID: 1762
	[SerializeField]
	private TMP_Text[] texts;

	// Token: 0x040006E3 RID: 1763
	[SerializeField]
	private Image[] images;
}
