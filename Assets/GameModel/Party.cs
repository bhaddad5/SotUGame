using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class Party : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;
		[TextArea(15, 20)]
		public string Description;
		public Color color;

	}
}