using UnityEngine;
using UnityEditor;
using BlendModes;
using System;

public class BMMaterialEditor : MaterialEditor
{
	private BlendMode selectedBlendMode;

	public override void OnEnable ()
	{
		base.OnEnable();

		BlendMode currentBlendMode = BlendMode.Normal;

		foreach (var keyword in ((Material)target).shaderKeywords)
		{
			if (keyword.StartsWith("BM"))
			{
				currentBlendMode = (BlendMode)Enum.Parse(typeof(BlendMode), keyword.Replace("BM", string.Empty), true);
				break;
			}
		}

		selectedBlendMode = currentBlendMode;
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();
		if (!isVisible) return;

		EditorGUILayout.Separator();

		var targetMaterial = target as Material;

		EditorGUI.BeginChangeCheck();
		selectedBlendMode = (BlendMode)EditorGUILayout.EnumPopup("Blend Mode", selectedBlendMode);
		if (EditorGUI.EndChangeCheck())
		{
			for (int i = 0; i < targetMaterial.shaderKeywords.Length; i++)
				if (targetMaterial.shaderKeywords[i].StartsWith("BM"))
					targetMaterial.DisableKeyword(targetMaterial.shaderKeywords[i]);

			targetMaterial.EnableKeyword("BM" + selectedBlendMode);

			EditorUtility.SetDirty(targetMaterial);
		}
	}
}

