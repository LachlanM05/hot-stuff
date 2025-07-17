using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class MessyModelsTool : MonoBehaviour
{
	// Token: 0x06000350 RID: 848 RVA: 0x00015A6D File Offset: 0x00013C6D
	private void Update()
	{
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00015A6F File Offset: 0x00013C6F
	private void Start()
	{
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00015A74 File Offset: 0x00013C74
	private void InitInteractablesArray()
	{
		InteractableObj[] array = Object.FindObjectsOfType<InteractableObj>();
		this.InteractableObjs = new List<InteractableObj>();
		foreach (InteractableObj interactableObj in array)
		{
			if (interactableObj.inkFileName != "")
			{
				this.InteractableObjs.Add(interactableObj);
			}
		}
		this.InteractableObjs = (from x in this.InteractableObjs
			group x by x.inkFileName into x
			select x.First<InteractableObj>()).ToList<InteractableObj>();
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00015B1B File Offset: 0x00013D1B
	protected void OnGUI()
	{
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00015B20 File Offset: 0x00013D20
	private void DoMyWindow(int windowID)
	{
		GUILayout.Label("Select a Messy Model:", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		for (int i = 0; i < this.CharacterNode.Count; i += 22)
		{
			this.DoMyWindowVertical(i);
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00015B6C File Offset: 0x00013D6C
	private void DoMyWindowVertical(int startIndex)
	{
		int num = 0;
		int num2 = startIndex;
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		while (num2 < this.CharacterNode.Count && num < 22)
		{
			if (GUILayout.Button(this.CharacterNode[num2], Array.Empty<GUILayoutOption>()))
			{
				GameObject gameObjectByInteractableObj = DateADex.Instance.GetGameObjectByInteractableObj(this.CharacterNode[num2]);
				if (gameObjectByInteractableObj != null)
				{
					AssetCleaner component = gameObjectByInteractableObj.GetComponent<AssetCleaner>();
					if (component != null)
					{
						component.ToggleClean();
					}
				}
			}
			num2++;
			num++;
		}
		GUILayout.EndVertical();
	}

	// Token: 0x04000356 RID: 854
	protected List<InteractableObj> InteractableObjs;

	// Token: 0x04000357 RID: 855
	protected List<string> CharacterNode = new List<string>
	{
		"lyric_literature", "kristof_crosstrainer", "stella_staircase", "teddy_teddybear", "abel_table", "maggie_mglass", "amir_mirror", "arma_alarm", "artt_art", "barry_beauty",
		"betty_bed", "benhwa_sextoys", "bathsheba_bath", "beau_box", "beverly_beverage", "bobby_bobbypin", "bodhi_capsule", "cabrizzio_cabinet", "daemon_bug", "cam_trashcan",
		"mitchell_food", "chairemi_chair", "chance_d20", "curtrod_curtain", "daisuke_dishes", "dante_fireplace", "sassy_chap", "diana_diary", "dishy_dishwasher", "dirk_laundry",
		"dolly_dust", "dorian_door", "doug_dread", "drysdale_dryer", "dasha_desk", "esther_boredom", "fantina_fan", "farya_firstaid", "cf_cf.cf_ceilingsorter", "cf_cf.cf_floorsorter",
		"freddy_fridge", "friar_airfryer", "hanks_hangers", "harper_hamper", "gaia_globe", "hector_hvac", "hoove_vacuum", "hime_anime", "holly_holidays", "ironaldini_iron",
		"jacques_ship", "jeanloo_toilet", "jerry_junkdrawer", "keyes_piano", "keith_key", "koa_couch", "kopi_coffee", "luke_microwave", "lux_lamp", "mac_computer",
		"mateo_blanket", "memoria_memories", "monique_money", "mikey_micro", "eddie_electro", "miranda_toaster", "penelope_stationery", "phoenicia_phone", "prissy_plant", "rainey_record",
		"rebel_duck", "reggie_rejection", "ricardo_sport", "rongomaiwhenua_geode", "scandalabra_candelabra", "johnny_shower", "connie_console", "sam_date", "sinclaire_sink", "skylar_specs",
		"sophia_safe", "stefan_stove", "shelley_shelf", "stepford_trophy", "tbc_ui", "telly_tv", "tina_triangle", "tim_clock", "parker_boardgames", "tony_toolbox",
		"tydus_tide", "tyrell_towel", "tableaux_painting", "vaughn_mousetrap", "wallace_wall", "willi_work", "winnifred_waterheater", "washford_washer", "shadow_darkness", "wyndolyn_window",
		"zoey_ghost", "airyn_air", "nightmare_dreams"
	};

	// Token: 0x04000358 RID: 856
	private List<string> updatedNodeList = new List<string>();

	// Token: 0x04000359 RID: 857
	private bool toolEnabled;
}
