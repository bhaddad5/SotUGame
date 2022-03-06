using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public class DialogDisplayer : MonoBehaviour
	{
		[SerializeField] private TMP_Text Text;

		public void DisplayDialog(string dialog)
		{
			Text.text = dialog;
		}

		public void QuickComplete()
		{
		}
	}
}