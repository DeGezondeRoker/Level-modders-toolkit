using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ToolkitDefs.Global
{
	class UpDownController : ObjectDefinition
	{
		private readonly Sprite[] sprites = new Sprite[2];
		private PropertySpec[] properties = new PropertySpec[4];
		
		public override void Init(ObjectData data)
		{
			sprites[0] = new Sprite(LevelData.GetSpriteSheet("Toolkit/SonLVL.gif").GetSection(84, 1, 32, 32), -16, -16);
			sprites[1] = new Sprite(sprites[0], true, false); // Flip X

			properties[0] = new PropertySpec("Starting direction", typeof(int), "Extended",
				"The direction to first move the objects in.", null, new Dictionary<string, int>
				{
					{ "Up", 0 },
					{ "Down", 1 }
				},
				(obj) => (int)(((V4ObjectEntry)obj).Direction),
				(obj, value) => ((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)value);
			
			properties[1] = new PropertySpec("Count", typeof(int), "Extended",
				"The amount of objects to move (+ 1)", null,
				(obj) => (obj.PropertyValue & 0x0F),
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x0F) | ((int)value)));
			
			properties[2] = new PropertySpec("Speed", typeof(int), "Extended",
				"The speed to move the objects with.", null,
				(obj) => (obj.PropertyValue & 0xF0) >> 4,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xF0) | (((int)value) << 4)));
			
			properties[3] = new PropertySpec("Range", typeof(int), "Extended",
				"The range.", 20,
				(obj) => (int)(((V4ObjectEntry)obj).Scale),
				(obj, value) => ((V4ObjectEntry)obj).Scale = ((int)value));
		}
		
		public override byte DefaultSubtype
		{
			get { return 0; }
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[0]); }
		}
		
		public override string SubtypeName(byte subtype)
		{
			return null;
		}

		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[0];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			int count = (int)(obj.PropertyValue & 0x0F) + 1;
			int i = 0;
			int index = LevelData.Objects.IndexOf(obj) + 1;
			
			int range = ((int)((V4ObjectEntry)obj).Scale);
			range *= 4;
			if ((int)(((V4ObjectEntry)obj).Direction) == 1) {
				range *= -1;
			}

			while (i < count) {
				if (index >= LevelData.Objects.Count)
					break;

				short X = (short)(obj.X);
				LevelData.Objects[index].X = X;

				short Y = (short)(obj.Y + range + (i * 24) - ((count - 1) * 12));
				LevelData.Objects[index].Y = Y;

				LevelData.Objects[index].UpdateSprite();

				index++;
				i++;
			}
			return sprites[(int)(((V4ObjectEntry)obj).Direction)];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			int range = ((int)((V4ObjectEntry)obj).Scale);
			range *= 4;
			BitmapBits overlay = new BitmapBits(1, range * 2 + 1);
			overlay.DrawLine(6, 0, 0, 0, range * 2);
			Sprite debug = new Sprite(overlay, 0, -range);
			return debug;
		}
	}
}