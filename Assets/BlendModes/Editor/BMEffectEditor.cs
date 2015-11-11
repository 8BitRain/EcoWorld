using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace BlendModes
{
	[InitializeOnLoad, CustomEditor(typeof(BlendModeEffect)), CanEditMultipleObjects]
	public class BMEffectEditor : Editor
	{
		private bool showEditor, showMaterialParams, showFramebufferInfo, showUnityProWarning;

		private GUIContent blendModeContent = new GUIContent("Blend Mode", "Blend mode of the object.");
		private GUIContent renderModeContent = new GUIContent("Render Mode",
			"Render mode to use for blending.\n\nGrab will execute grab pass for each object with blend mode effect, which may be too heavy for mobile devices.\n\nUnified Grab will work much faster in case there are multiple objects with blend mode effect, but these objects will use single shared grab texture (won't blend with each other).\n\nFramebuffer will not use grab pass at all (which is extremely faster and will work smoothly on mobile devices), but it is supported by iOS 6+ and Nvidia Tegra devices only.");
		private GUIContent meshTextureContent = new GUIContent("Texture", "Texture of the object.");
		private GUIContent meshTintColorContent = new GUIContent("Tint Color", "Tint color of the object.");

		private SerializedProperty blendMode;
		private SerializedProperty renderMode;
		private SerializedProperty texture;
		private SerializedProperty tintColor;

		private static Dictionary<GameObject, bool> affectedObjects = new Dictionary<GameObject, bool>();

		static BMEffectEditor ()
		{
			EditorApplication.update += Update;
		}

		// Hack to control prefab objects material when adding/removing BlendModeEffect component. 
		// If only prefabs would fire OnEnable/Disable events...
		static void Update ()
		{
			foreach (var selectedGO in Selection.gameObjects)
			{
				if (PrefabUtility.GetPrefabType(selectedGO) != PrefabType.Prefab) continue;

				var blendEffect = selectedGO.GetComponent<BlendModeEffect>();
				if (!blendEffect)
				{
					// User removed BlendModeEffect.
					if (affectedObjects.ContainsKey(selectedGO))
					{
						var tempBE = selectedGO.AddComponent<BlendModeEffect>();
						tempBE.OnEnable();
						tempBE.OnDisable();
						DestroyImmediate(tempBE, true);
						affectedObjects.Remove(selectedGO);
						EditorUtility.SetDirty(selectedGO);
					}
					continue;
				}
				else
				{
					// First launch or user added BlendModeEffect.
					if (!affectedObjects.ContainsKey(selectedGO))
					{
						blendEffect.OnEnable();
						affectedObjects.Add(selectedGO, true);
						EditorUtility.SetDirty(selectedGO);
					}

					// User enabled/disabled BlendModeEffect.
					if (affectedObjects[selectedGO] != blendEffect.enabled)
					{
						if (blendEffect.enabled) blendEffect.OnEnable();
						else blendEffect.OnDisable();
						affectedObjects[selectedGO] = blendEffect.enabled;
						EditorUtility.SetDirty(selectedGO);
					}
				}
			}
		}

		private void OnEnable ()
		{
			blendMode = serializedObject.FindProperty("_blendMode");
			renderMode = serializedObject.FindProperty("_renderMode");
			texture = serializedObject.FindProperty("_texture");
			tintColor = serializedObject.FindProperty("_tintColor");

			Undo.undoRedoPerformed += SyncParameters;
		}

		private void OnDisable ()
		{
			Undo.undoRedoPerformed -= SyncParameters;
		}

		public override void OnInspectorGUI ()
		{
			if (!Selection.activeGameObject) return;

			var blendEffect = Selection.activeGameObject.GetComponent<BlendModeEffect>();
			if (!blendEffect) return;

			if (Event.current.type == EventType.Layout)
			{
				showEditor = blendEffect.ObjectType != ObjectType.Unknown;
				showMaterialParams = blendEffect.ObjectType == ObjectType.MeshDefault || 
					blendEffect.ObjectType == ObjectType.ParticleDefault;
				showFramebufferInfo = blendEffect.RenderMode == RenderMode.Framebuffer;
				showUnityProWarning = ShowUnityProWarning();
			}

			if (showEditor)
			{
				serializedObject.Update();

				EditorGUILayout.PropertyField(blendMode, blendModeContent);
				EditorGUILayout.PropertyField(renderMode, renderModeContent);

				if (showMaterialParams)
				{
					EditorGUILayout.PropertyField(texture, meshTextureContent);
					EditorGUILayout.PropertyField(tintColor, meshTintColorContent);
				}

				serializedObject.ApplyModifiedProperties();

				if (GUI.changed) SyncParameters();

				if (showFramebufferInfo) 
					EditorGUILayout.HelpBox("Framebuffer mode is supported by iOS ver. 6 (or newer) and Nvidia Tegra devices only. While in editor, Grab mode will be used for preview.", MessageType.Info);

				if (showUnityProWarning)
					EditorGUILayout.HelpBox("Grab and Unified Grab modes require Unity 4 Pro license to work correctly.", MessageType.Warning);
			}
			else EditorGUILayout.HelpBox("Can't find any UI graphic or compatible renderer component to apply blend mode effect.", MessageType.Warning);
		}

		private void SyncParameters ()
		{
			foreach (var selectedGO in Selection.gameObjects)
			{
				var blendEffect = selectedGO.GetComponent<BlendModeEffect>();
				if (!blendEffect) continue;

				blendEffect.SetBlendMode(blendEffect.BlendMode, blendEffect.RenderMode);
				EditorUtility.SetDirty(selectedGO);
			}
		}

		private bool ShowUnityProWarning ()
		{
			if (Application.unityVersion[0] != '4') return false;

			if (!Application.HasProLicense()) return true;

			#if UNITY_IOS 
			if (!InternalEditorUtility.GetLicenseInfo().Contains("iPhone Pro")) return true;
			#endif

			#if UNITY_ANDROID
			if (!InternalEditorUtility.GetLicenseInfo().Contains("Android Pro")) return true;
			#endif

			return false;
		}
	}
}
