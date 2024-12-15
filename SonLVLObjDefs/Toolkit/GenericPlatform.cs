using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ToolkitDefs.Global
{
	class GenericPlatform : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private readonly Sprite[] sprites = new Sprite[4];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("Toolkit/Objects.gif");
			sprites[0] = new Sprite(sheet.GetSection(1, 159, 16, 16), -8, -8);
			sprites[1] = new Sprite(sheet.GetSection(17, 159, 16, 16), -8, -8);
			sprites[2] = new Sprite(sheet.GetSection(33, 159, 16, 16), -8, -8);
			
			// object icon, 2x2 box
			sprites[3] = new Sprite(new Sprite(sprites[0], -8, -8), new Sprite(sprites[2],  8, -8));
			
			properties[0] = new PropertySpec("Length", typeof(int), "Extended",
				"The length of the platform. Starts at 32px, increases by 16px.", null,
				(obj) => obj.PropertyValue,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0x11}); }
		}
		
		public override bool Debug
		{
			get { return true; }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return null;
		}

		public override Sprite Image
		{
			get { return sprites[3]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[3];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			int length = (obj.PropertyValue) + 2;
			
			int sx = length * 8 - 8;
			
			int index = 0;
			
			Sprite row = new Sprite();
			for (int i = 0; i < length; i++) {
				if (i > 0) {
					index = 1;
				}
				if (i > obj.PropertyValue) {
					index = 2;
				}
				row = new Sprite(row, new Sprite(sprites[index], -sx + (i * 16), 0));
			}
			return row;
		}
	}
}