using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class NpcInteractionEntryBindings : MonoBehaviour, ITooltipProvider, IPointerEnterHandler
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;
		[SerializeField] private GameObject NewIndicator;

		[SerializeField] private DialogPopupBindings DialogPopupPrefab;

		private Interaction interaction;
		private MainGameManager mgm;
		private Action onDialogClose;
		private Npc npc;
		public void Setup(Npc npc, Interaction interaction, MainGameManager mgm, Action onDialogClose)
		{
			this.interaction = interaction;
			this.mgm = mgm;
			this.npc = npc;
			this.onDialogClose = onDialogClose;

			Text.text = $"{interaction.Name}";

			if (interaction.CanFail)
				Text.text += $" ({(int)((1f - interaction.ProbabilityOfFailureResult) * 100)}% chance)";

			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()))
				Text.text += $" {interaction.Cost.GetCostString()}";
			Button.interactable = interaction.InteractionValid(mgm);
			gameObject.SetActive(interaction.InteractionVisible(mgm));
			NewIndicator.SetActive(interaction.IsNew(mgm));

		}

		public void ExecuteInteraction()
		{
			bool succeeded = interaction.GetInteractionSucceeded();
			var res = interaction.GetInteractionResult(succeeded);
			interaction.Cost.SubtractCost(mgm);

			var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent).transform;
			var dialogPrefab = Instantiate(DialogPopupPrefab, popupParent);
			dialogPrefab.Setup(npc, res, mgm, () =>
			{
				res.Execute(mgm);
				if (succeeded)
					interaction.Completed++;
				onDialogClose?.Invoke();
			});
		}

		public string GetTooltip()
		{
			if (interaction.InteractionValid(mgm))
				return null;
			
			var reqTooltip = interaction.Requirements.GetInvalidTooltip(mgm);
			var costTooltip = interaction.Cost.GetInvalidTooltip(mgm);

			var tooltip = $"{reqTooltip}\n{costTooltip}";

			if (tooltip.StartsWith("\n"))
				tooltip = tooltip.Substring(1);
			if (tooltip.EndsWith("\n"))
				tooltip = tooltip.Substring(0, tooltip.Length - 1);

			return tooltip;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (interaction.InteractionValid(mgm))
			{
				interaction.New = false;
				NewIndicator.SetActive(false);
			}
		}
	}
}