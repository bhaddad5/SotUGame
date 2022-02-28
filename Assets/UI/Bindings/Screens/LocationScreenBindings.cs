using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class LocationScreenBindings : MonoBehaviour
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private TMP_Text Description;
		[SerializeField] private Image BackgroundImage;
		[SerializeField] private Transform NpcOptionsParent;

		[SerializeField] private PoliciesPopupBindings PoliciesPopupPrefab;
		
		[SerializeField] private LocationNpcEntryBindings _npcButtonPrefab;
		[SerializeField] private NpcScreenBindings _npcUiPrefab;
		
		private Location loc;
		public bool IsAccessible(MainGameManager mgm) => loc.IsAccessible(mgm);

		private MainGameManager mgm;
		private Action onClose;
		public void Setup(Location loc, MainGameManager mgm, Action onClose)
		{
			this.loc = loc;
			this.mgm = mgm;
			this.onClose = onClose;

			foreach (Npc npc in loc.Npcs)
			{
				var f = Instantiate(_npcButtonPrefab);
				f.Setup(npc, this, mgm);
				f.transform.SetParent(NpcOptionsParent);
			}
		}

		public void CloseCurrentLocation()
		{
			GameObject.Destroy(gameObject);
			onClose();
		}

		void OnDestroy()
		{
			if(currNpc != null)
				GameObject.Destroy(currNpc);
		}

		public void OpenPolicies()
		{
			var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
			var policiesPopup = GameObject.Instantiate(PoliciesPopupPrefab, popupParent.transform);
			policiesPopup.Setup(loc, mgm);
		}

		private GameObject currNpc;
		public void ShowNpc(Npc npc, MainGameManager mgm)
		{
			currNpc = Instantiate(_npcUiPrefab).gameObject;
			currNpc.GetComponent<NpcScreenBindings>().Setup(npc, mgm, () => RefreshUiDisplay(mgm));

		}

		public void CloseCurrentNpc()
		{
			if (currNpc != null)
			{
				GameObject.Destroy(currNpc);
				RefreshUiDisplay(mgm);
			}
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			BackgroundImage.sprite = loc.BackgroundImage;
			Name.text = loc.Name;
			Description.text = loc.Description;
			if (loc.Controlled)
				Name.text += $" (Controlled)";

			foreach (var npc in NpcOptionsParent.GetComponentsInChildren<LocationNpcEntryBindings>(true))
			{
				//Were they just moved/removed?
				if(!loc.Npcs.Contains(npc.npc))
					GameObject.Destroy(npc.gameObject);
				else
					npc.RefreshUiDisplay(mgm);
			}
		}
	}
}