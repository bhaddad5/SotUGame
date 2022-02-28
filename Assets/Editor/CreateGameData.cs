using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public static class CreateGameData
{
	[MenuItem("Tools/Create Game Data Asset", false, 100)]
	public static void CleanNullsAndMarkDirty()
	{
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GameData>(), $"Assets/Data/GameData.asset");
		AssetDatabase.SaveAssets();
	}
}
