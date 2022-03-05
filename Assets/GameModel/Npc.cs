using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.GameModel
{
	[Serializable]
	public class Npc : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public int StartingOpinion;

		public string Title;
		public string FirstName;
		public string LastName;
		public int Age;
		[TextArea(15, 20)]
		public string Description;
		
		public Sprite BackgroundImage;

		public Vector2 UiPosition;

		public ActionRequirements VisibilityRequirements;
		
		[HideInInspector]
		public int Opinion;
		[HideInInspector]
		public bool Controlled;
		[HideInInspector]
		public bool Exists = true;

		public Sprite Image;
		public List<Interaction> Interactions = new List<Interaction>();
		private MainGameManager mgm;

		public void Setup(MainGameManager mgm)
		{
			this.mgm = mgm;

			Controlled = false;
			Exists = true;
			Opinion = StartingOpinion;

			foreach (var ob in Interactions)
				ob.Setup();
		}

		public bool IsVisible(MainGameManager mgm)
		{
			if (!Exists)
				return false;

			return VisibilityRequirements.RequirementsAreMet(mgm);
		}

		public bool HasNewInteractions(MainGameManager mgm)
		{
			return Interactions.Any(i => i.IsNew(mgm));
		}
		
		public override string ToString()
		{
			return $"{FirstName} {LastName}";
		}
	}
}
