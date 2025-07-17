using System;
using System.Collections.Generic;
using System.Reflection;

namespace CowberryStudios.ProjectAI
{
	// Token: 0x02000263 RID: 611
	public class InkBinding
	{
		// Token: 0x060013D2 RID: 5074 RVA: 0x0005F5DD File Offset: 0x0005D7DD
		public InkBinding()
		{
			this._commands = new Dictionary<CommandID, InkBinding.CommandData>();
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0005F5F0 File Offset: 0x0005D7F0
		public void Bind(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			foreach (MethodInfo methodInfo in instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				this.TryBindingMethod(instance, methodInfo);
			}
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0005F634 File Offset: 0x0005D834
		public void Unbind(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			foreach (MethodInfo methodInfo in instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				this.TryUnbindingMethod(instance, methodInfo);
			}
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0005F678 File Offset: 0x0005D878
		private void TryUnbindingMethod(object instance, MethodInfo method)
		{
			Stack<CommandID> stack = new Stack<CommandID>();
			using (IEnumerator<CommandAttribute> enumerator = method.GetCustomAttributes<CommandAttribute>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CommandAttribute commandAttribute = enumerator.Current;
					if (this._commands.ContainsKey(commandAttribute.Command))
					{
						stack.Push(commandAttribute.Command);
					}
				}
				goto IL_005B;
			}
			IL_004E:
			this.Unbind(stack.Pop());
			IL_005B:
			if (stack.Count <= 0)
			{
				return;
			}
			goto IL_004E;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0005F6FC File Offset: 0x0005D8FC
		public bool Unbind(CommandID command)
		{
			return this._commands.Remove(command);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0005F70C File Offset: 0x0005D90C
		public void Invoke(InkCommand command)
		{
			InkBinding.CommandData commandData;
			if (this._commands.TryGetValue(command.Id, out commandData))
			{
				commandData.Invoke(command);
			}
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0005F738 File Offset: 0x0005D938
		public void Invoke(CommandID id, string[] arguments)
		{
			InkCommand inkCommand = new InkCommand(id, arguments);
			this.Invoke(inkCommand);
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0005F754 File Offset: 0x0005D954
		private void TryBindingMethod(object instance, MethodInfo method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			foreach (CommandAttribute commandAttribute in method.GetCustomAttributes<CommandAttribute>())
			{
				if (parameters.Length == 1 && !(parameters[0].ParameterType != typeof(InkCommand)) && !this._commands.ContainsKey(commandAttribute.Command))
				{
					this._commands.Add(commandAttribute.Command, new InkBinding.CommandData(instance, method, commandAttribute));
				}
			}
		}

		// Token: 0x04000F5E RID: 3934
		private Dictionary<CommandID, InkBinding.CommandData> _commands;

		// Token: 0x020003D1 RID: 977
		private class CommandData
		{
			// Token: 0x060018A1 RID: 6305 RVA: 0x0007134A File Offset: 0x0006F54A
			public CommandData(CommandHandler handler)
			{
				this._handler = handler;
			}

			// Token: 0x060018A2 RID: 6306 RVA: 0x00071384 File Offset: 0x0006F584
			public CommandData(object instance, MethodInfo method, CommandAttribute attr)
			{
				this._exactly = attr.Exactly;
				this._not = attr.Not;
				this._greaterThan = attr.GreaterThan;
				this._lessThan = attr.LessThan;
				this._greaterThanOrEqual = attr.GreaterThanOrEqual;
				this._lessThanOrEqual = attr.LessThanOrEqual;
				this._handler = delegate(InkCommand command)
				{
					method.Invoke(instance, new object[] { command });
				};
			}

			// Token: 0x060018A3 RID: 6307 RVA: 0x00071430 File Offset: 0x0006F630
			public void Invoke(InkCommand command)
			{
				if (this.Validate(command))
				{
					this._handler(command);
					return;
				}
				int num = 0;
				foreach (string text in command)
				{
					num++;
				}
			}

			// Token: 0x060018A4 RID: 6308 RVA: 0x00071490 File Offset: 0x0006F690
			private bool Validate(InkCommand command)
			{
				int count = command.Count;
				return (this._exactly == -1 || count == this._exactly) && (this._not == -1 || count != this._not) && (this._greaterThan == -1 || count > this._greaterThan) && (this._greaterThanOrEqual == -1 || count >= this._greaterThanOrEqual) && (this._lessThan == -1 || count < this._lessThan) && (this._lessThanOrEqual == -1 || count <= this._lessThanOrEqual);
			}

			// Token: 0x0400150A RID: 5386
			private CommandHandler _handler;

			// Token: 0x0400150B RID: 5387
			private int _exactly = -1;

			// Token: 0x0400150C RID: 5388
			private int _not = -1;

			// Token: 0x0400150D RID: 5389
			private int _greaterThan = -1;

			// Token: 0x0400150E RID: 5390
			private int _lessThan = -1;

			// Token: 0x0400150F RID: 5391
			private int _greaterThanOrEqual = -1;

			// Token: 0x04001510 RID: 5392
			private int _lessThanOrEqual = -1;
		}
	}
}
