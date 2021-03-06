using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel.UiDisplayers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class DialogPopupBindings : MonoBehaviour
	{
		[SerializeField] private Transform DialogTextParent;
		[SerializeField] private Image Picture;
		[SerializeField] private DialogDisplayer PlayerDialogPrefab;
		[SerializeField] private DialogDisplayer NarratorDialogPrefab;
		[SerializeField] private DialogDisplayer NpcDialogPrefab;

		private DialogDisplayer currDialog = null;

		private Action onClose;
		public void Setup(Npc npc, InteractionResult res, MainGameManager mgm, Action onClose)
		{
			this.onClose = onClose;
			Picture.sprite = npc.Image;

			StartCoroutine(DisplayAllDialogs(res, mgm));
		}

		private IEnumerator DisplayAllDialogs(InteractionResult res, MainGameManager mgm)
		{
			foreach (var dialog in res.Dialogs)
			{
				if (dialog.CurrSpeaker == DialogEntry.Speaker.Player)
					currDialog = Instantiate(PlayerDialogPrefab, DialogTextParent);
				else if (dialog.CurrSpeaker == DialogEntry.Speaker.Narrator)
					currDialog = Instantiate(NarratorDialogPrefab, DialogTextParent);
				else if (dialog.CurrSpeaker == DialogEntry.Speaker.Npc)
					currDialog = Instantiate(NpcDialogPrefab, DialogTextParent);

				yield return currDialog.DisplayDialog(dialog.Text, mgm);
			}
		}

		public void QuickCompleteCurrentDialog()
		{
			currDialog?.QuickComplete();
		}

		public void CloseDialogWindow()
		{
			GameObject.Destroy(gameObject.transform.parent.gameObject);
			onClose();
		}
	}
}