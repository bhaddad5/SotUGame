using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public class DialogDisplayer : MonoBehaviour
	{
		[SerializeField] private TMP_Text Text;
		
		private string textToShow = "";
		private Coroutine runningCoroutine = null;
		public IEnumerator DisplayDialog(string dialog, MainGameManager mgm)
		{
			textToShow = UiDisplayHelpers.ApplyDynamicValuesToString(dialog, mgm);
			Text.text = "";
			
			foreach (var c in textToShow)
			{
				Text.text += c;
				yield return new WaitForSeconds(.02f);
			}

			runningCoroutine = null;
		}

		public void QuickComplete()
		{
			StopCoroutine(runningCoroutine);
			runningCoroutine = null;
			Text.text = textToShow;
		}
	}
}