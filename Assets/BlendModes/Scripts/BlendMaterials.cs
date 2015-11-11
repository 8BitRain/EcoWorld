using UnityEngine;
using System.Collections.Generic;

namespace BlendModes
{
	/// <summary> 
	/// Manages all the materials used for blending and provides caching.
	/// </summary> 
	public static class BlendMaterials
	{
		private static Dictionary<ObjectType, Dictionary<RenderMode, Dictionary<BlendMode, Material>>> cachedMaterials =
			new Dictionary<ObjectType, Dictionary<RenderMode, Dictionary<BlendMode, Material>>>();

		public static Material GetMaterial (ObjectType objectType, RenderMode renderMode, BlendMode blendMode)
		{
			if (blendMode == BlendMode.Normal)
			{
				if (objectType == ObjectType.MeshDefault)
				{
					var mat = new Material(Shader.Find("Diffuse"));
					mat.hideFlags = HideFlags.HideAndDontSave;
					return mat;
				}
				else if (objectType == ObjectType.SpriteDefault)
				{
					var mat = new Material(Shader.Find("Sprites/Default"));
					mat.hideFlags = HideFlags.HideAndDontSave;
					return mat;
				}
				else if (objectType == ObjectType.ParticleDefault)
				{
					var mat = new Material(Shader.Find("Particles/Additive"));
					mat.hideFlags = HideFlags.HideAndDontSave;
					return mat;
				}
				else return null;
			}

			// Framebuffer won't work in the editor, so fallback to Grab mode.
			if (Application.isEditor && renderMode == RenderMode.Framebuffer) renderMode = RenderMode.Grab;

			// Disable caching for mesh and particle materials, as they are sharing them.
			if (objectType != ObjectType.MeshDefault && objectType != ObjectType.ParticleDefault &&
				cachedMaterials.ContainsKey(objectType) &&
				cachedMaterials[objectType].ContainsKey(renderMode) &&
				cachedMaterials[objectType][renderMode].ContainsKey(blendMode))
				return cachedMaterials[objectType][renderMode][blendMode];
			else
			{
				var mat = new Material(Resources.Load<Shader>(string.Format("BlendModes/{0}/{1}", objectType, renderMode)));
				mat.hideFlags = HideFlags.HideAndDontSave;
				mat.EnableKeyword("BM" + blendMode.ToString());

				if (!cachedMaterials.ContainsKey(objectType)) cachedMaterials.Add(objectType, new Dictionary<RenderMode, Dictionary<BlendMode, Material>>());
				if (!cachedMaterials[objectType].ContainsKey(renderMode)) cachedMaterials[objectType].Add(renderMode, new Dictionary<BlendMode, Material>());
				if (!cachedMaterials[objectType][renderMode].ContainsKey(blendMode)) cachedMaterials[objectType][renderMode].Add(blendMode, mat);

				return mat;
			}
		}
	}
}
