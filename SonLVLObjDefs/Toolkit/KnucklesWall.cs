using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ToolkitDefs.Global
{
	class KnucklesWall : ObjectDefinition
	{
		private readonly Sprite[] sprites = new Sprite[2];
		private PropertySpec[] properties = new PropertySpec[1];
		
		public override void Init(ObjectData data)
		{
			sprites[0] = new Sprite(LevelData.GetSpriteSheet("Toolkit/Objects.gif").GetSection(1, 62, 32, 64), -16, -32);
			sprites[1] = new Sprite(LevelData.GetSpriteSheet("Toolkit/Objects.gif").GetSection(1, 62, 32, 96), -16, -48);

			properties[0] = new PropertySpec("Size", typeof(int), "Extended",
				"The size of this wall.", null, new Dictionary<string, int>
				{
					{ "Normal", 0 },
					{ "Large", 1 }
				},
				(obj) => (obj.PropertyValue == 0) ? 0 : 1,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
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
			if (subtype >= 1) {
				return sprites[1];
			}
			return sprites[0];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			if (obj.PropertyValue >= 1) {
				return sprites[1];
			}
			return sprites[0];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return null;
		}
	}
}