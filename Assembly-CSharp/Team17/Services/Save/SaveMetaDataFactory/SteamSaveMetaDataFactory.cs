using System;
using Team17.Platform.SaveGame;

namespace Team17.Services.Save.SaveMetaDataFactory
{
	// Token: 0x020001DE RID: 478
	public class SteamSaveMetaDataFactory : SaveMetaDataFactory
	{
		// Token: 0x06001027 RID: 4135 RVA: 0x00055464 File Offset: 0x00053664
		public override SaveMetaData Create()
		{
			SaveMetaData saveMetaData = base.Create();
			saveMetaData.m_AppId = new AppIdDefinition().TitleId.ToString();
			return saveMetaData;
		}
	}
}
