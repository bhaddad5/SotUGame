using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreatePartyWindow : EditorWindow
{
	private static CreatePartyWindow window;

	[MenuItem("Selectacorp/Create Party")]

	static void Init()
	{
		window = (CreatePartyWindow)EditorWindow.GetWindow(typeof(CreatePartyWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string partyName;

	void OnGUI()
	{
		partyName = EditorGUILayout.TextField("Name:", partyName);

		if (GUILayout.Button("Create!"))
		{
			Create();

			window.Close();
		}
	}

	private void Create()
	{
		Party party = ScriptableObject.CreateInstance<Party>();
		party.Name = partyName;
		party.Id = Guid.NewGuid().ToString();
		
		data.Parties.Add(party);
		EditorUtility.SetDirty(data);

		var dataFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(data));

		AssetDatabase.CreateAsset(party, $"{dataFolder}/_Parties/{party.Name}.asset");
		AssetDatabase.SaveAssets();
	}

}