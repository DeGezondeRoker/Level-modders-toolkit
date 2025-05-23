using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ToolkitDefs.Global
{
	class ForceUnstick : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private readonly Sprite[] sprites = new Sprite[1];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("Toolkit/SonLVL.gif");
			sprites[0] = new Sprite(sheet.GetSection(1, 18, 16, 14), -8, -7);			// X pos, Y pos,  w, h,  X Offset, Y Offset
			
						
			properties[0] = new PropertySpec("Width", typeof(int), "Extended",
				"How wide the object will be.", null,
				(obj) => (obj.PropertyValue >> 4) + 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xf0) | (Math.Min(Math.Max((int)value - 1, 0), 15) << 4)));
			
			properties[1] = new PropertySpec("Height", typeof(int), "Extended",
				"How tall the object will be.", null,
				(obj) => (obj.PropertyValue & 0x0f) + 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x0f) | Math.Min(Math.Max((int)value - 1, 0), 15)));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0x11}); }
		}
		
		public override bool Debug
		{
			get { return true; }
		}
		
		public override byte DefaultSubtype
		{
			get { return 0x11; }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return ((subtype >> 4) + 1) + "x" + ((subtype & 0x0f) + 1) + " tiles";
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[0];
		}

		public override Rectangle GetBounds(ObjectEntry obj)
		{
			int w = 16;
			int h = 16;
			return new Rectangle(obj.X - (w / 2), obj.Y - (h / 2), w, h);
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			int w = ((obj.PropertyValue >> 4) + 1) * 16;
			int h = ((obj.PropertyValue & 0x0f) + 1) * 16;
			
			BitmapBits bmp = new BitmapBits(w, h);
			bmp.DrawRectangle(6, 0, 0, w - 1, h - 1);			// Colour white

			return new Sprite(new Sprite(bmp, -(w / 2), -(h / 2)) , sprites[0]);
		}
	}
}