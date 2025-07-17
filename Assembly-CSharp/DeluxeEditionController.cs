using System;
using T17.Services;
using Team17.Platform.Entitlements;
using Team17.Scripts.Services.Entitlement;

// Token: 0x02000048 RID: 72
public class DeluxeEditionController : Singleton<DeluxeEditionController>, IEntitlementListener
{
	// Token: 0x060001D7 RID: 471 RVA: 0x0000B850 File Offset: 0x00009A50
	private void Start()
	{
		this.ReadEntitlements();
		this.UpdateEntitlements();
		Services.Entitlements.RegisterListener(this);
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000B86C File Offset: 0x00009A6C
	private void OnDestroy()
	{
		IMirandaEntitlementService entitlements = Services.Entitlements;
		if (entitlements != null)
		{
			entitlements.UnregisterListener(this);
		}
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000B889 File Offset: 0x00009A89
	public void OnEntitlementsChanged()
	{
		this.ReadEntitlements();
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000B894 File Offset: 0x00009A94
	private void ReadEntitlements()
	{
		IMirandaEntitlementService entitlements = Services.Entitlements;
		if (entitlements != null)
		{
			this.isDeluxEdition = entitlements.IsProductOwned(this.DeluxeDefinitions);
			this.isEarlyBirdEdition = entitlements.IsProductOwned(this.EarlyBirdDefinitions);
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000B8CE File Offset: 0x00009ACE
	public void UpdateEntitlements()
	{
		this.IS_EARLY_BIRD_EDITION = this.isEarlyBirdEdition;
		this.IS_DELUXE_EDITION = this.isDeluxEdition;
	}

	// Token: 0x040002B6 RID: 694
	public static bool IS_DEMO_EDITION = false;

	// Token: 0x040002B7 RID: 695
	public static int DEMO_LIMIT_MINUTES = 10;

	// Token: 0x040002B8 RID: 696
	public static int NUMBER_OF_BASEGAME_CHARACTERS = 100;

	// Token: 0x040002B9 RID: 697
	public bool IS_DELUXE_EDITION = true;

	// Token: 0x040002BA RID: 698
	public bool IS_EARLY_BIRD_EDITION = true;

	// Token: 0x040002BB RID: 699
	private bool isDeluxEdition;

	// Token: 0x040002BC RID: 700
	private bool isEarlyBirdEdition;

	// Token: 0x040002BD RID: 701
	public static string DELUXE_CHARACTER_1 = "mikey";

	// Token: 0x040002BE RID: 702
	public static string DELUXE_CHARACTER_2 = "lucinda";

	// Token: 0x040002BF RID: 703
	public static int NUMBER_OF_DELUXE_CHARACTERS = 2;

	// Token: 0x040002C0 RID: 704
	public static string STEAM_DELUXE_PRODUCT_ID = "3043470";

	// Token: 0x040002C1 RID: 705
	public static string STEAM_EARLY_BIRD_PRODUCT_ID = "3043460";

	// Token: 0x040002C2 RID: 706
	public static string PS5_DELUXE_PRODUCT_ID = "EP4064-PPSA22788_00-DATEEVERYTHING02";

	// Token: 0x040002C3 RID: 707
	public static string PS5_EARLY_BIRD_PRODUCT_ID = "EP4064-PPSA22788_00-DATEEVERYTHING01";

	// Token: 0x040002C4 RID: 708
	public static string XBX_DELUXE_PRODUCT_ID = "S-1-15-2-2249851129-3132134204-3176013266-926687722-4259669241-905134543-3470718609";

	// Token: 0x040002C5 RID: 709
	public static string XBX_EARLY_BIRD_PRODUCT_ID = "S-1-15-2-1206457686-2522453926-707746695-895433487-567713200-3245403621-4036051802";

	// Token: 0x040002C6 RID: 710
	public static string SWITCH_DELUXE_PRODUCT_ID = "DateEverythingDLC-Deluxe";

	// Token: 0x040002C7 RID: 711
	public static string SWITCH_EARLY_BIRD_PRODUCT_ID = "DateEverythingDLC-Preorder";

	// Token: 0x040002C8 RID: 712
	public ProductDefinition DeluxeDefinitions;

	// Token: 0x040002C9 RID: 713
	public ProductDefinition EarlyBirdDefinitions;
}
