using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class LocationNpcEntryBindings : MonoBehaviour
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private TMP_Text Title;
		[SerializeField] private Image NpcPic;
		[SerializeField] private GameObject NewIndicator;

		[HideInInspector]
		public Npc npc;
		private MainGameManager mgm;
		private LocationScreenBindings deptUi;

		public void Setup(Npc npc, LocationScreenBindings deptUi, MainGameManager mgm)
		{
			this.npc = npc;
			this.mgm = mgm;
			this.deptUi = deptUi;
			transform.position = npc.UiPosition;
		}

		public void OpenNpc()
		{
			deptUi.ShowNpc(npc, mgm);
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Name.text = $"{npc.FirstName} {npc.LastName}";
			Title.text = $"{npc.Title}";
			Title.gameObject.SetActive(!string.IsNullOrWhiteSpace(npc.Title));
			NpcPic.sprite = npc.Image;
			gameObject.SetActive(npc.IsVisible(mgm));
			NewIndicator.SetActive(npc.HasNewInteractions(mgm));
		}
	}
}