using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000D5 RID: 213
public class SFXBank : MonoBehaviour
{
	// Token: 0x06000705 RID: 1797 RVA: 0x00027C3A File Offset: 0x00025E3A
	private void Awake()
	{
		if (SFXBank.Instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		SFXBank.Instance = this;
		Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00027C61 File Offset: 0x00025E61
	private void Start()
	{
		this.initialized = true;
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00027C6C File Offset: 0x00025E6C
	public void Play(AudioClip clip)
	{
		if (!this.initialized)
		{
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack(clip, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00027CA0 File Offset: 0x00025EA0
	public static void PlayUIExit()
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x04000604 RID: 1540
	public static SFXBank Instance;

	// Token: 0x04000605 RID: 1541
	[Header("UI")]
	[Header("General")]
	public AudioClip start_game;

	// Token: 0x04000606 RID: 1542
	public AudioClip approved_stamp;

	// Token: 0x04000607 RID: 1543
	public AudioClip ui_save_complete;

	// Token: 0x04000608 RID: 1544
	public AudioClip ui_scroll;

	// Token: 0x04000609 RID: 1545
	public AudioClip ui_select;

	// Token: 0x0400060A RID: 1546
	public AudioClip ui_text_writing;

	// Token: 0x0400060B RID: 1547
	public AudioClip ui_collectable_appear;

	// Token: 0x0400060C RID: 1548
	public AudioClip ui_collectable_leave;

	// Token: 0x0400060D RID: 1549
	public AudioClip ui_transition_character;

	// Token: 0x0400060E RID: 1550
	public AudioClip ui_transition_house;

	// Token: 0x0400060F RID: 1551
	public AudioClip ui_dialogue_confirm;

	// Token: 0x04000610 RID: 1552
	public AudioClip ui_dialogue_option_select;

	// Token: 0x04000611 RID: 1553
	public AudioClip ui_dialogue_option_specs_select;

	// Token: 0x04000612 RID: 1554
	public AudioClip ui_dialogue_scroll;

	// Token: 0x04000613 RID: 1555
	public AudioClip ui_dialogue_option_specs_scroll;

	// Token: 0x04000614 RID: 1556
	public AudioClip ui_delete_file;

	// Token: 0x04000615 RID: 1557
	public AudioClip ui_exit;

	// Token: 0x04000616 RID: 1558
	public AudioClip ui_examine;

	// Token: 0x04000617 RID: 1559
	[Header("Dateviators")]
	public AudioClip ui_dateviators_on;

	// Token: 0x04000618 RID: 1560
	public AudioClip ui_dateviators_off;

	// Token: 0x04000619 RID: 1561
	public AudioClip ui_menu_dateviators_open;

	// Token: 0x0400061A RID: 1562
	public AudioClip ui_menu_dateviators_close;

	// Token: 0x0400061B RID: 1563
	[FormerlySerializedAs("dateviator_beam_heart_loop")]
	public AudioClip ui_dateviator_heart_rise;

	// Token: 0x0400061C RID: 1564
	public AudioClip dateviator_beam_cancel;

	// Token: 0x0400061D RID: 1565
	public AudioClip dateviator_beam_cancel_focused;

	// Token: 0x0400061E RID: 1566
	[FormerlySerializedAs("dateviator_beam_awakening_loop")]
	public AudioClip ui_dateviator_character_rise;

	// Token: 0x0400061F RID: 1567
	[FormerlySerializedAs("dateviator_beam_beam")]
	public AudioClip ui_dateviator_beam;

	// Token: 0x04000620 RID: 1568
	[Header("Dateables")]
	public AudioClip ui_dateable_awakened;

	// Token: 0x04000621 RID: 1569
	public AudioClip ui_dateable_enter_fast;

	// Token: 0x04000622 RID: 1570
	public AudioClip ui_dateable_enter_slow;

	// Token: 0x04000623 RID: 1571
	public AudioClip ui_dateable_exit;

	// Token: 0x04000624 RID: 1572
	public AudioClip ui_dateable_switch_pose;

	// Token: 0x04000625 RID: 1573
	public AudioClip ui_dateable_move;

	// Token: 0x04000626 RID: 1574
	[Header("Realization Recipes")]
	public AudioClip ui_realization_recipe_smarts;

	// Token: 0x04000627 RID: 1575
	public AudioClip ui_realization_recipe_poise;

	// Token: 0x04000628 RID: 1576
	public AudioClip ui_realization_recipe_empathy;

	// Token: 0x04000629 RID: 1577
	public AudioClip ui_realization_recipe_charm;

	// Token: 0x0400062A RID: 1578
	public AudioClip ui_realization_recipe_sass;

	// Token: 0x0400062B RID: 1579
	public AudioClip ui_realization_recipe_fail_smarts;

	// Token: 0x0400062C RID: 1580
	public AudioClip ui_realization_recipe_fail_poise;

	// Token: 0x0400062D RID: 1581
	public AudioClip ui_realization_recipe_fail_empathy;

	// Token: 0x0400062E RID: 1582
	public AudioClip ui_realization_recipe_fail_charm;

	// Token: 0x0400062F RID: 1583
	public AudioClip ui_realization_recipe_fail_sass;

	// Token: 0x04000630 RID: 1584
	public AudioClip ui_realization_recipe_complete;

	// Token: 0x04000631 RID: 1585
	[Header("Canopy")]
	public AudioClip ui_canopy_refund;

	// Token: 0x04000632 RID: 1586
	public AudioClip ui_canopy_select;

	// Token: 0x04000633 RID: 1587
	public AudioClip ui_canopy_open;

	// Token: 0x04000634 RID: 1588
	public AudioClip ui_canopy_jobs_complete;

	// Token: 0x04000635 RID: 1589
	public AudioClip ui_canopy_star_increase;

	// Token: 0x04000636 RID: 1590
	public AudioClip ui_canopy_star_decrease;

	// Token: 0x04000637 RID: 1591
	public AudioClip ui_canopy_populate;

	// Token: 0x04000638 RID: 1592
	public AudioClip ui_canopy_newtasks;

	// Token: 0x04000639 RID: 1593
	[Header("Workspace")]
	public AudioClip ui_wkspace_notification;

	// Token: 0x0400063A RID: 1594
	public AudioClip ui_wkspace_messagereceived;

	// Token: 0x0400063B RID: 1595
	[Header("Thiscord")]
	public AudioClip ui_thiscord_notification;

	// Token: 0x0400063C RID: 1596
	public AudioClip ui_thiscord_messagereceived;

	// Token: 0x0400063D RID: 1597
	[FormerlySerializedAs("ui_emote_anger")]
	[Header("Emotes")]
	public AudioClip ui_emote_angry;

	// Token: 0x0400063E RID: 1598
	public AudioClip ui_emote_joy;

	// Token: 0x0400063F RID: 1599
	public AudioClip ui_emote_love1;

	// Token: 0x04000640 RID: 1600
	public AudioClip ui_emote_love2;

	// Token: 0x04000641 RID: 1601
	public AudioClip ui_emote_disgust;

	// Token: 0x04000642 RID: 1602
	public AudioClip ui_emote_good;

	// Token: 0x04000643 RID: 1603
	public AudioClip ui_emote_bad;

	// Token: 0x04000644 RID: 1604
	public AudioClip ui_emote_sweat;

	// Token: 0x04000645 RID: 1605
	public AudioClip ui_emote_violent;

	// Token: 0x04000646 RID: 1606
	public AudioClip ui_emote_sleep;

	// Token: 0x04000647 RID: 1607
	public AudioClip ui_emote_sing;

	// Token: 0x04000648 RID: 1608
	public AudioClip ui_emote_resolve;

	// Token: 0x04000649 RID: 1609
	public AudioClip ui_emote_shake;

	// Token: 0x0400064A RID: 1610
	public AudioClip ui_emote_spotlight;

	// Token: 0x0400064B RID: 1611
	[Header("Phone")]
	public AudioClip ui_phone_menu_scroll;

	// Token: 0x0400064C RID: 1612
	public AudioClip ui_phone_menu_select;

	// Token: 0x0400064D RID: 1613
	public AudioClip ui_phone_menu_landscape_transition;

	// Token: 0x0400064E RID: 1614
	public AudioClip ui_phone_menu_portrait_transition;

	// Token: 0x0400064F RID: 1615
	public AudioClip ui_phone_menu_open;

	// Token: 0x04000650 RID: 1616
	public AudioClip ui_phone_menu_close;

	// Token: 0x04000651 RID: 1617
	public AudioClip ui_phone_menu_exit;

	// Token: 0x04000652 RID: 1618
	[Header("Specs")]
	public AudioClip ui_specs_star_maximum;

	// Token: 0x04000653 RID: 1619
	public AudioClip ui_specs_menu_select;

	// Token: 0x04000654 RID: 1620
	public AudioClip ui_specs_menu_scroll;

	// Token: 0x04000655 RID: 1621
	[Header("Roomer")]
	public AudioClip ui_roomer_menu_open;

	// Token: 0x04000656 RID: 1622
	public AudioClip ui_roomer_added;

	// Token: 0x04000657 RID: 1623
	[Header("Date-a-Dex")]
	public AudioClip ui_date_a_dex_menu_open;

	// Token: 0x04000658 RID: 1624
	public AudioClip ui_date_a_dex_scroll_up;

	// Token: 0x04000659 RID: 1625
	public AudioClip ui_date_a_dex_scroll_down;

	// Token: 0x0400065A RID: 1626
	public AudioClip ui_date_a_dex_added;

	// Token: 0x0400065B RID: 1627
	[Header("Stingers")]
	public AudioClip stinger_sleep;

	// Token: 0x0400065C RID: 1628
	public AudioClip stinger_dateable_awakened;

	// Token: 0x0400065D RID: 1629
	public AudioClip stinger_ending_friend;

	// Token: 0x0400065E RID: 1630
	public AudioClip stinger_ending_love;

	// Token: 0x0400065F RID: 1631
	public AudioClip stinger_ending_hate;

	// Token: 0x04000660 RID: 1632
	public AudioClip stinger_ending_realized;

	// Token: 0x04000661 RID: 1633
	public AudioClip stinger_reggie;

	// Token: 0x04000662 RID: 1634
	[Header("Standard Foley")]
	public AudioClip light_switch_standard;

	// Token: 0x04000663 RID: 1635
	public AudioClip mysterious_box_drone_fly_deliver_sfx;

	// Token: 0x04000664 RID: 1636
	public AudioClip mysterious_box_drone_loop;

	// Token: 0x04000665 RID: 1637
	public AudioClip mysterious_box_drone_leave_sfx;

	// Token: 0x04000666 RID: 1638
	public AudioClip vfx_poof;

	// Token: 0x04000667 RID: 1639
	public AudioClip vfx_sparkle;

	// Token: 0x04000668 RID: 1640
	public AudioClip sfx_fireplace;

	// Token: 0x04000669 RID: 1641
	[Header("Ending")]
	public AudioClip yachtDelivery;

	// Token: 0x0400066A RID: 1642
	[Header("Boss Battle")]
	public AudioClip ui_player_health_damaged;

	// Token: 0x0400066B RID: 1643
	public AudioClip ui_player_health_recovered;

	// Token: 0x0400066C RID: 1644
	public AudioClip ui_boss_health_damaged;

	// Token: 0x0400066D RID: 1645
	public AudioClip ui_boss_health_recovered;

	// Token: 0x0400066E RID: 1646
	private bool initialized;
}
