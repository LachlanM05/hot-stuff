using System;
using Team17.Input;

namespace Team17.Scripts.Input.Rumble
{
	// Token: 0x0200023D RID: 573
	public interface IControllerRumbleFactory
	{
		// Token: 0x0600127F RID: 4735
		ControllerRumble RequestControllerRumble();

		// Token: 0x06001280 RID: 4736
		void ReturnControllerRumble(ControllerRumble rumble);
	}
}
