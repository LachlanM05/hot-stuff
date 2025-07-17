using System;
using Rewired;
using Team17.Input;

namespace T17.Input
{
	// Token: 0x02000246 RID: 582
	public class RewiredDeviceIDProviderNull : IDeviceIdProvider<Controller, NullDeviceId>
	{
		// Token: 0x0600130D RID: 4877 RVA: 0x0005B7D4 File Offset: 0x000599D4
		public Controller GetDeviceFromId(NullDeviceId deviceId)
		{
			int num = 0;
			int controllerCount = ReInput.controllers.controllerCount;
			while (num != controllerCount)
			{
				Controller controller = ReInput.controllers.Controllers[num];
				if (controller.deviceInstanceGuid.GetHashCode() == deviceId.Id)
				{
					return controller;
				}
				num++;
			}
			return null;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0005B828 File Offset: 0x00059A28
		public NullDeviceId GetDeviceId(Controller device)
		{
			if (device == null)
			{
				return RewiredDeviceIDProviderNull.InvalidDeviceId;
			}
			return new NullDeviceId(device.deviceInstanceGuid.GetHashCode());
		}

		// Token: 0x04000EEC RID: 3820
		private static readonly NullDeviceId InvalidDeviceId = new NullDeviceId(-1);
	}
}
