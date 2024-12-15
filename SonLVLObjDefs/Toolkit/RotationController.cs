using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ToolkitDefs.Global
{
	class RotationController : ObjectDefinition
	{
		private readonly Sprite[] sprites = new Sprite[2];
		private PropertySpec[] properties = new PropertySpec[4];
		
		public override void Init(ObjectData data)
		{
			sprites[0] = new Sprite(LevelData.GetSpriteSheet("Toolkit/SonLVL.gif").GetSection(18, 1, 32, 32), -16, -16);
			sprites[1] = new Sprite(sprites[0], true, false); // Flip X

			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"The direction to move the objects in.", null, new Dictionary<string, int>
				{
					{ "Clockwise", 0 },
					{ "Anti-clockwise", 1 }
				},
				(obj) => (int)(((V4ObjectEntry)obj).Direction),
				(obj, value) => ((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)value);
			
			properties[1] = new PropertySpec("Count", typeof(int), "Extended",
				"The amount of objects to rotate (+ 1)", null,
				(obj) => (obj.PropertyValue & 0x0F),
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x0F) | ((int)value)));
			
			properties[2] = new PropertySpec("Speed", typeof(int), "Extended",
				"The speed to rotate the objects with.", null,
				(obj) => (obj.PropertyValue & 0xF0) >> 4,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xF0) | (((int)value) << 4)));
			
			properties[3] = new PropertySpec("Radius", typeof(int), "Extended",
				"The radius.", 20,
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
			
			double angle = 0;
			double angle_offset = Math.PI / (double)count * 2;
			
			int radius = ((int)((V4ObjectEntry)obj).Scale);
			radius *= 4;

			while (i < count) {
				if (index >= LevelData.Objects.Count)
					break;

				short X = (short)(obj.X + Math.Cos(angle) * radius);
				LevelData.Objects[index].X = X;

				short Y = (short)(obj.Y + Math.Sin(angle) * radius);
				LevelData.Objects[index].Y = Y;

				LevelData.Objects[index].UpdateSprite();

				angle += angle_offset;
				index++;
				i++;
			}
			return sprites[(int)(((V4ObjectEntry)obj).Direction)];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			int radius = ((int)((V4ObjectEntry)obj).Scale);
			radius *= 4;
			BitmapBits overlay = new BitmapBits(radius * 2 + 1, radius * 2 + 1);
			overlay.DrawCircle(6, radius, radius, radius); // LevelData.ColorWhite
			Sprite debug = new Sprite(overlay, -radius, -radius);
			return debug;
		}
	}
}