using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ToolkitDefs.Global
{
	class SlopedBridge : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite sprite;
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(1, 1, 16, 16), -8, -8);
			
			properties[0] = new PropertySpec("Length", typeof(int), "Extended",
				"How long the Bridge should be.", null,
				(obj) => (obj.PropertyValue & ~0xE0),
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & 0xE0) | (int)value));
			
			properties[1] = new PropertySpec("Slope", typeof(int), "Extended",
				"The slope of the Bridge (in pixels per log).", null,
				(obj) => (obj.PropertyValue >> 5),
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & 0x1f) | (int)value << 5));
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0x11}); }
		}
		
		public override byte DefaultSubtype
		{
			get { return 204; }
		}

		public override string SubtypeName(byte subtype)
		{
			return "Sloped Bridge";
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		public override Sprite Image
		{
			get { return sprite; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprite;
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			int length = obj.PropertyValue & 0x1f;
			int slope = obj.PropertyValue >> 5;
			int dir = (int)(((V4ObjectEntry)obj).Direction);
			if (dir == 1)
				slope *= -1;

			if (length <= 1)
				return sprite;
			
			int st = -((length * 16) / 2) + 8;
			int ypos = 0;
			List<Sprite> sprs = new List<Sprite>();
			for (int i = 0; i < length; i++) {
				sprs.Add(new Sprite(sprite, st + (i * 16), ypos));
				ypos += slope;
			}
			return new Sprite(sprs.ToArray());
		}
	}
}