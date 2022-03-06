using System;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreateNpcWindow : EditorWindow
{
	private static CreateNpcWindow window;

	[MenuItem("Selectacorp/Create NPC")] 

	static void Init()
	{
		window = (CreateNpcWindow)EditorWindow.GetWindow(typeof(CreateNpcWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;

	private string title;
	private string firstName;
	private string lastName;

	private LocationPicker locPicker = new LocationPicker();

	private string errorMsg;

	void OnGUI()
	{
		title = EditorGUILayout.TextField("Title:", title);
		firstName = EditorGUILayout.TextField("First Name:", firstName);
		lastName = EditorGUILayout.TextField("Last Name:", lastName);

		locPicker.DrawLocationDropdown(data);

		GUILayout.Space(10);

		GUILayout.Label($"Finish:", EditorStyles.boldLabel);

		if (GUILayout.Button("Create NPC"))
		{
			var loc = locPicker.Location as Location;

			errorMsg = "";

			if (firstName == null || lastName == null)
			{
				errorMsg = $"ERROR: NPC not fully named!";
			}
			else if (loc == null)
			{
				errorMsg = $"ERROR: YOU MUST SELECT A LOCATION";
			}
			else
			{
				CreateNpc(loc);

				window.Close();
			}
		}

		if(!String.IsNullOrEmpty(errorMsg))
			GUILayout.Label(errorMsg, EditorStyles.boldLabel);
	}
	
	private void CreateNpc(Location loc)
	{
		Npc npc = ScriptableObject.CreateInstance<Npc>();
		npc.Title = title;
		npc.FirstName = firstName;
		npc.LastName = lastName;
		npc.Id = Guid.NewGuid().ToString();

		loc.Npcs.Add(npc);
		EditorUtility.SetDirty(loc);

		var locFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(loc));

		AssetDatabase.CreateFolder($"{locFolder}", npc.NpcFileName().ToFolderName());
		string npcFolder = Path.Combine(locFolder, npc.NpcFileName().ToFolderName());
		AssetDatabase.CreateFolder(npcFolder, "Interactions");

		AssetDatabase.CreateAsset(npc, $"{npcFolder}/{npc.NpcFileName().ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}
}