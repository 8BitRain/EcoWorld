using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BlendModes
{
	public class DemoPicker : MonoBehaviour
	{
		public BlendModeEffect TargetBlendMode;

		private Text targetText;

		private void Start ()
		{
			targetText = TargetBlendMode.transform.Find("Text Blend Mode").GetComponent<Text>();

			var buttons = new List<Button>(22);
			foreach (Transform element in transform)
				if (element.GetComponent<Button>()) 
					buttons.Add(element.GetComponent<Button>());

			for (int i = 0; i < 22; i++)
			{
				int ic = i;
				buttons[ic].GetComponentInChildren<Text>().text = Regex.Replace(((BlendMode)ic).ToString(), "(\\B[A-Z])", " $1");
				if (TargetBlendMode.BlendMode == (BlendMode)ic)
				{
					targetText.text = Regex.Replace(((BlendMode)ic).ToString(), "(\\B[A-Z])", " $1");
					buttons[ic].GetComponent<Image>().color = Color.green;
				}
				buttons[ic].onClick.RemoveAllListeners();
				buttons[ic].onClick.AddListener(() => {
					TargetBlendMode.BlendMode = (BlendMode)ic;
					targetText.text = Regex.Replace(((BlendMode)ic).ToString(), "(\\B[A-Z])", " $1");
					foreach (var button in buttons) 
						button.GetComponent<Image>().color = button == buttons[ic] ? Color.green : Color.white;
				});
			}
		}
	}
}
