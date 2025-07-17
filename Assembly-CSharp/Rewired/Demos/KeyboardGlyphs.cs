using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000266 RID: 614
	public class KeyboardGlyphs : MonoBehaviour
	{
		// Token: 0x060013E6 RID: 5094 RVA: 0x0005F9AC File Offset: 0x0005DBAC
		private void Awake()
		{
			this._keysDict = new Dictionary<int, KeyboardGlyphs.KeyGlyphEntry>();
			foreach (KeyboardGlyphs.KeyGlyphEntry keyGlyphEntry in this.keys)
			{
				this._keysDict.Add((int)keyGlyphEntry.keyCode, keyGlyphEntry);
			}
			this._modifierKeysDict = new Dictionary<int, KeyboardGlyphs.ModifierKeyGlyphEntry>();
			foreach (KeyboardGlyphs.ModifierKeyGlyphEntry modifierKeyGlyphEntry in this.modifierKeys)
			{
				this._modifierKeysDict.Add((int)modifierKeyGlyphEntry.modifierKey, modifierKeyGlyphEntry);
			}
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0005FA70 File Offset: 0x0005DC70
		public int GetGlyphs(ActionElementMap actionElementMap, List<Sprite> results)
		{
			if (actionElementMap == null)
			{
				throw new NullReferenceException("actionElementMap");
			}
			if (actionElementMap.controllerMap.controllerType != ControllerType.Keyboard)
			{
				return 0;
			}
			return this.GetGlyphs(actionElementMap.keyboardKeyCode, actionElementMap.modifierKey1, actionElementMap.modifierKey2, actionElementMap.modifierKey3, results);
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0005FAB0 File Offset: 0x0005DCB0
		public int GetGlyphs(KeyboardKeyCode keyCode, ModifierKey modifierKey1, ModifierKey modifierKey2, ModifierKey modifierKey3, List<Sprite> results)
		{
			if (KeyboardGlyphs.Instance == null)
			{
				return 0;
			}
			if (KeyboardGlyphs.Instance.keys == null)
			{
				return 0;
			}
			if (KeyboardGlyphs.Instance.modifierKeys == null)
			{
				return 0;
			}
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			KeyboardGlyphs.ModifierKeyGlyphEntry modifierKeyGlyphEntry;
			if (KeyboardGlyphs.Instance._modifierKeysDict.TryGetValue((int)modifierKey1, out modifierKeyGlyphEntry) && modifierKeyGlyphEntry.glyph != null)
			{
				results.Add(modifierKeyGlyphEntry.glyph);
			}
			if (KeyboardGlyphs.Instance._modifierKeysDict.TryGetValue((int)modifierKey2, out modifierKeyGlyphEntry) && modifierKeyGlyphEntry.glyph != null)
			{
				results.Add(modifierKeyGlyphEntry.glyph);
			}
			if (KeyboardGlyphs.Instance._modifierKeysDict.TryGetValue((int)modifierKey3, out modifierKeyGlyphEntry) && modifierKeyGlyphEntry.glyph != null)
			{
				results.Add(modifierKeyGlyphEntry.glyph);
			}
			KeyboardGlyphs.KeyGlyphEntry keyGlyphEntry;
			if (KeyboardGlyphs.Instance._keysDict.TryGetValue((int)keyCode, out keyGlyphEntry) && keyGlyphEntry.glyph != null)
			{
				results.Add(keyGlyphEntry.glyph);
			}
			return results.Count;
		}

		// Token: 0x04000F63 RID: 3939
		[SerializeField]
		private List<KeyboardGlyphs.KeyGlyphEntry> keys = new List<KeyboardGlyphs.KeyGlyphEntry>
		{
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Backspace),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Tab),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Clear),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Return),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Pause),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Escape),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Space),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Exclaim),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.DoubleQuote),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Hash),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Dollar),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Ampersand),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Quote),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.LeftParen),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.RightParen),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Asterisk),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Plus),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Comma),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Minus),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Period),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Slash),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha0),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha1),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha2),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha3),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha4),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha5),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha6),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha7),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha8),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Alpha9),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Colon),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Semicolon),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Less),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Equals),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Greater),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Question),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.At),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.LeftBracket),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Backslash),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.RightBracket),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Caret),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Underscore),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.BackQuote),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.A),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.B),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.C),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.D),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.E),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.G),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.H),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.I),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.J),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.K),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.L),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.M),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.N),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.O),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.P),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Q),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.R),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.S),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.T),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.U),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.V),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.W),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.X),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Y),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Z),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Delete),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad0),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad1),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad2),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad3),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad4),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad5),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad6),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad7),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad8),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Keypad9),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.KeypadPeriod),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.KeypadDivide),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.KeypadMultiply),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.KeypadMinus),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.KeypadPlus),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.KeypadEnter),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.KeypadEquals),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.UpArrow),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.DownArrow),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.RightArrow),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.LeftArrow),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Insert),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Home),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.End),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.PageUp),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.PageDown),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F1),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F2),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F3),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F4),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F5),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F6),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F7),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F8),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F9),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F10),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F11),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F12),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F13),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F14),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.F15),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Numlock),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.CapsLock),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.ScrollLock),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.RightShift),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.LeftShift),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.RightControl),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.LeftControl),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.RightAlt),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.LeftAlt),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.RightCommand),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.LeftCommand),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.LeftWindows),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.RightWindows),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.AltGr),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Help),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Print),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.SysReq),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Break),
			new KeyboardGlyphs.KeyGlyphEntry(KeyboardKeyCode.Menu)
		};

		// Token: 0x04000F64 RID: 3940
		[SerializeField]
		private List<KeyboardGlyphs.ModifierKeyGlyphEntry> modifierKeys = new List<KeyboardGlyphs.ModifierKeyGlyphEntry>
		{
			new KeyboardGlyphs.ModifierKeyGlyphEntry(ModifierKey.Control),
			new KeyboardGlyphs.ModifierKeyGlyphEntry(ModifierKey.Alt),
			new KeyboardGlyphs.ModifierKeyGlyphEntry(ModifierKey.Shift),
			new KeyboardGlyphs.ModifierKeyGlyphEntry(ModifierKey.Command)
		};

		// Token: 0x04000F65 RID: 3941
		[NonSerialized]
		private Dictionary<int, KeyboardGlyphs.KeyGlyphEntry> _keysDict;

		// Token: 0x04000F66 RID: 3942
		[NonSerialized]
		private Dictionary<int, KeyboardGlyphs.ModifierKeyGlyphEntry> _modifierKeysDict;

		// Token: 0x04000F67 RID: 3943
		private static KeyboardGlyphs Instance;

		// Token: 0x020003D4 RID: 980
		[Serializable]
		private class KeyGlyphEntry
		{
			// Token: 0x060018A9 RID: 6313 RVA: 0x000715E5 File Offset: 0x0006F7E5
			public KeyGlyphEntry()
			{
			}

			// Token: 0x060018AA RID: 6314 RVA: 0x000715ED File Offset: 0x0006F7ED
			public KeyGlyphEntry(KeyboardKeyCode keyCode)
			{
				this.keyCode = keyCode;
			}

			// Token: 0x04001518 RID: 5400
			public KeyboardKeyCode keyCode;

			// Token: 0x04001519 RID: 5401
			public Sprite glyph;
		}

		// Token: 0x020003D5 RID: 981
		[Serializable]
		private class ModifierKeyGlyphEntry
		{
			// Token: 0x060018AB RID: 6315 RVA: 0x000715FC File Offset: 0x0006F7FC
			public ModifierKeyGlyphEntry()
			{
			}

			// Token: 0x060018AC RID: 6316 RVA: 0x00071604 File Offset: 0x0006F804
			public ModifierKeyGlyphEntry(ModifierKey modifierKey)
			{
				this.modifierKey = modifierKey;
			}

			// Token: 0x0400151A RID: 5402
			public ModifierKey modifierKey;

			// Token: 0x0400151B RID: 5403
			public Sprite glyph;
		}
	}
}
