using UnityEngine;
using UnityEngine.UI;

namespace BlendModes
{
	/// <summary>
	/// Adds blend mode effect to the object with an UI graphic or compatible renderer.
	/// </summary>
	[AddComponentMenu("Effects/Blend Mode"), ExecuteInEditMode]
	public class BlendModeEffect : MonoBehaviour
	{
		/// <summary>
		/// Current blend mode of the object.
		/// </summary>
		public BlendMode BlendMode
		{
			get { return _blendMode; }
			set { SetBlendMode(value, RenderMode); }
		}

		/// <summary>
		/// Current render mode used for blending.
		/// </summary>
		public RenderMode RenderMode
		{
			get { return _renderMode; }
			set { SetBlendMode(BlendMode, value); }
		}

		/// <summary>
		/// Texture to use with the object.
		/// Have no effect for GUI and sprite objects.
		/// </summary>
		public Texture2D Texture
		{
			get { return _texture; }
			set 
			{
				if (Material && (ObjectType == ObjectType.MeshDefault || 
					ObjectType == ObjectType.ParticleDefault)) 
					Material.mainTexture = value;
				_texture = value;
			}
		}

		/// <summary>
		/// Tint color of the object.
		/// Have no effect for GUI and sprite objects.
		/// </summary>
		public Color TintColor
		{
			get { return _tintColor; }
			set
			{
				if (Material && (ObjectType == ObjectType.MeshDefault ||
					ObjectType == ObjectType.ParticleDefault)) 
					Material.color = value;
				_tintColor = value;
			}
		}

		/// <summary>
		/// Type of the object in context of applying blend mode effect.
		/// </summary>
		public ObjectType ObjectType
		{
			get 
			{
				if (GetComponent<MaskableGraphic>()) return ObjectType.UIDefault;
				if (GetComponent<Text>()) return ObjectType.UIDefaultFont;
				if (GetComponent<SpriteRenderer>()) return ObjectType.SpriteDefault;
				if (GetComponent<MeshRenderer>()) return ObjectType.MeshDefault;
				if (GetComponent<ParticleSystem>()) return ObjectType.ParticleDefault;

				return ObjectType.Unknown;
			}
		}

		/// <summary>
		/// Material of the object.
		/// </summary>
		public Material Material
		{
			get
			{
				switch (ObjectType)
				{
					case ObjectType.UIDefault:
						return GetComponent<MaskableGraphic>().material;
					case ObjectType.UIDefaultFont:
						return GetComponent<Text>().material;
					case ObjectType.SpriteDefault:
						return GetComponent<SpriteRenderer>().sharedMaterial;
					case ObjectType.MeshDefault:
						return GetComponent<MeshRenderer>().sharedMaterial;
					case ObjectType.ParticleDefault:
						return GetComponent<ParticleSystem>()
							.GetComponent<Renderer>().sharedMaterial;
					default:
						return null;
				}
			}
			set
			{
				switch (ObjectType)
				{
					case ObjectType.UIDefault:
						GetComponent<MaskableGraphic>().material = value;
						break;
					case ObjectType.UIDefaultFont:
						GetComponent<Text>().material = value;
						break;
					case ObjectType.SpriteDefault:
						GetComponent<SpriteRenderer>().sharedMaterial = value;
						break;
					case ObjectType.MeshDefault:
						GetComponent<MeshRenderer>().sharedMaterial = value;
						break;
					case ObjectType.ParticleDefault:
						GetComponent<ParticleSystem>()
							.GetComponent<Renderer>().sharedMaterial = value;
						break;
				}
			}
		}

		[SerializeField]
		private BlendMode _blendMode;
		[SerializeField]
		private RenderMode _renderMode;
		[SerializeField]
		private Texture2D _texture;
		[SerializeField]
		private Color _tintColor = Color.white;

		/// <summary>
		/// Sets specific blend mode to the object.
		/// </summary>
		/// <param name="blendMode">Blend mode.</param>
		/// <param name="renderMode">Render mode to use for blending.</param>
		public void SetBlendMode (BlendMode blendMode, RenderMode renderMode = RenderMode.Grab)
		{
			if (ObjectType == ObjectType.Unknown) return;

			Material = BlendMaterials.GetMaterial(ObjectType, renderMode, blendMode);
			Texture = Texture;
			TintColor = TintColor;

			_blendMode = blendMode;
			_renderMode = renderMode;
		}

		public void OnEnable ()
		{
			if (Material && Material.mainTexture) 
				Texture = (Texture2D)Material.mainTexture;

			SetBlendMode(BlendMode, RenderMode);
		}

		public void OnDisable ()
		{
			Texture2D curTex = Texture;

			Material = BlendMaterials.GetMaterial(ObjectType, RenderMode.Grab, BlendMode.Normal);

			if (Material && curTex) Material.mainTexture = curTex;
		}
	}
}
