using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Microsoft.Win32;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using System.Linq;

namespace Unitycoding{
	public class PlayerPrefsEditor : EditorWindow {
		private List<PlayerPrefValue> playerPrefs;
		private Vector2 scrollPosition;
		private string searchString="Search...";
		private bool IsSearching
		{
			get { return !string.IsNullOrEmpty(searchString) && searchString != "Search..."; }
		}
		private bool isCreateNew = false;
		private PlayerPrefValue newValue;
		[MenuItem("Tools/Unitycoding/PlayerPrefs Editor")]
		private static void ShowWindow() {
			GetWindow<PlayerPrefsEditor>("PlayerPrefs");
		}
		
		
		private void OnGUI() {
			if (playerPrefs == null) {
				RefreshPlayerPrefs ();
			}
			GUIStyle textStyle= new GUIStyle(EditorStyles.textField);
			textStyle.wordWrap=true;
			textStyle.fixedHeight = 0;
			textStyle.stretchHeight = true;
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUIStyle style = new GUIStyle (EditorStyles.boldLabel);
			style.contentOffset = new Vector2 (0,-4);
			GUILayout.Label("PlayerPrefs: " + "unity." + PlayerSettings.companyName + "." + PlayerSettings.productName, style);
			if (GUILayout.Button ("Delete All", EditorStyles.toolbarButton, GUILayout.Width(70))) {
				if(EditorUtility.DisplayDialog("Delete PlayerPrefs","Are you sure you want to delete all player prefs?", "Yes", "No")){
					PlayerPrefs.DeleteAll();
					RefreshPlayerPrefs();
				}
			}
			if (GUILayout.Button ("New", EditorStyles.toolbarButton, GUILayout.Width(40))) {
				isCreateNew=!isCreateNew;
			}
			GUILayout.EndHorizontal ();
			if (isCreateNew) {
				if(newValue == null){
					newValue=new PlayerPrefValue("","string","");
				}
				GUILayout.BeginVertical("box");
				newValue.name = EditorGUILayout.TextField("Key", newValue.name);
				GUILayout.BeginHorizontal();
				GUILayout.Label("Value", GUILayout.Width(146f));
				switch (newValue.prefType) {
				case PlayerPrefValue.PrefType.Int:
					newValue.intValue = EditorGUILayout.IntField( newValue.intValue);
					break;
				case PlayerPrefValue.PrefType.Float:
					newValue.floatValue = EditorGUILayout.FloatField(newValue.floatValue);
					break;
				case PlayerPrefValue.PrefType.String:
					int lines = newValue.stringValue.Split('\n').Length;
					
					newValue.stringValue = EditorGUILayout.TextArea(newValue.stringValue, textStyle, GUILayout.Height(EditorGUIUtility.singleLineHeight*lines));
					break;
				}
				GUILayout.EndHorizontal();
				newValue.prefType = (PlayerPrefValue.PrefType)EditorGUILayout.EnumPopup("Type",newValue.prefType);
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("Cancel",GUILayout.Width(70f))){
					isCreateNew=!isCreateNew;
				}
				if(GUILayout.Button("Add",GUILayout.Width(70f))){
					AddPref(newValue);
					RefreshPlayerPrefs();
				}
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}

			searchString = SearchField (searchString,GUILayout.Width(Screen.width-20));

			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			for (int i = 0; i < playerPrefs.Count; i++) {
				if(!string.IsNullOrEmpty(searchString) && !searchString.Equals("Search...") && !playerPrefs[i].name.Contains(searchString)){
					continue;
				}

				GUILayout.BeginHorizontal();
				GUILayout.Label("# "+i+"  "+playerPrefs[i].name, GUILayout.Width(150f));
				switch (playerPrefs[i].prefType) {
				case PlayerPrefValue.PrefType.Int:
					playerPrefs[i].intValue = EditorGUILayout.IntField( playerPrefs[i].intValue, EditorStyles.textField);
					break;
				case PlayerPrefValue.PrefType.Float:
					playerPrefs[i].floatValue = EditorGUILayout.FloatField(playerPrefs[i].floatValue, EditorStyles.textField);
					break;
				case PlayerPrefValue.PrefType.String:
					int lines = playerPrefs[i].stringValue.Split('\n').Length;
					lines=Mathf.Clamp(lines,1,int.MaxValue);
					playerPrefs[i].stringValue = EditorGUILayout.TextArea(playerPrefs[i].stringValue, textStyle);
					break;
				}

				GUIStyle minusButton = new GUIStyle ("OL Minus"){
					margin=new RectOffset(0,0,4,0)
				};
				if(GUILayout.Button("",minusButton,GUILayout.Width(18f))){
					if(EditorUtility.DisplayDialog("Delete PlayerPrefs entry","Are you sure you want to delete " + playerPrefs[i].name+"?", "Yes", "No")){
						PlayerPrefs.DeleteKey(playerPrefs[i].name);
						playerPrefs=null;
						break;
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}

		private void AddPref(PlayerPrefValue value){
			switch (value.prefType) {
			case PlayerPrefValue.PrefType.Int:
				PlayerPrefs.SetInt(value.name,value.intValue);
				break;
			case PlayerPrefValue.PrefType.Float:
				PlayerPrefs.SetFloat(value.name,value.floatValue);
				break;
			case PlayerPrefValue.PrefType.String:
				PlayerPrefs.SetString(value.name,value.stringValue);
				break;
			}
		}

		private string SearchField(string search,params GUILayoutOption[] options){
			GUILayout.BeginHorizontal ();
			string before = search;
			string after = EditorGUILayout.TextField ("", before, "SearchTextField",options);
			
			if (GUILayout.Button ("", "SearchCancelButton", GUILayout.Width (18f))) {
				after = "Search...";
				GUIUtility.keyboardControl = 0;
			}
			GUILayout.EndHorizontal();
			return after;
		}

		private void RefreshPlayerPrefs() {
			if (playerPrefs != null) 
				playerPrefs.Clear();

			playerPrefs = new List<PlayerPrefValue>();
			if (Application.platform == RuntimePlatform.WindowsEditor) {
				GetPrefKeysWindows();
			} else {
				GetPrefKeysMac();
			}
		}
		
		private void GetPrefKeysWindows() {
			// Unity stores prefs in the registry on Windows. 
			string regKey = @"Software\" + PlayerSettings.companyName + @"\" + PlayerSettings.productName;
			RegistryKey key = Registry.CurrentUser.OpenSubKey(regKey);
			if (key != null) {
				foreach (string subkeyName in key.GetValueNames()) {
					string keyName = subkeyName.Substring (0, subkeyName.LastIndexOf ("_"));
					string val = key.GetValue (subkeyName).ToString ();
					int testInt = -1;
					string newType = "";
					bool couldBeInt = int.TryParse (val, out testInt);
					if (!float.IsNaN (PlayerPrefs.GetFloat (keyName, float.NaN))) {
						val = PlayerPrefs.GetFloat (keyName).ToString ();
						newType = "real";
					} else if (couldBeInt && (PlayerPrefs.GetInt (keyName, testInt - 10) == testInt)) {		
						newType = "integer";		
					} else {
						newType = "string";
					}
					PlayerPrefValue pref = new PlayerPrefValue (keyName, newType, val);
					playerPrefs.Add (pref);
				}
			}
		}
		
		private void GetPrefKeysMac() {
			string homePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string pListPath = homePath +"/Library/Preferences/unity." + PlayerSettings.companyName + "." +
				PlayerSettings.productName + ".plist";
			// Convert from binary plist to xml.
			Process p = new Process();
			ProcessStartInfo psi = new ProcessStartInfo("plutil", "-convert xml1 \"" + pListPath + "\"");
			p.StartInfo = psi;
			p.Start();
			p.WaitForExit();
			
			StreamReader sr = new StreamReader(pListPath);
			string pListData = sr.ReadToEnd();
			
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(pListData);
			
			XmlElement plist = xml["plist"];
			if (plist == null) return;
			XmlNode node = plist["dict"].FirstChild;
			while (node != null) {
				string name = node.InnerText;
				node = node.NextSibling;
				PlayerPrefValue pref = new PlayerPrefValue(name, node.Name, node.InnerText);
				node = node.NextSibling;
				playerPrefs.Add(pref);
			}
			Process.Start("plutil", " -convert binary1 \"" + pListPath + "\"");
		}


		private class PlayerPrefValue {
			public enum PrefType {
				Float, Int, String
			}

			private PrefType m_PrefType;
			public PrefType prefType{
				get{
					return this.m_PrefType;
				}
				set{
					this.m_PrefType=value;
				}
			}

			private string m_Name;
			public string name{
				get{
					return this.m_Name;
				}
				set{
					this.m_Name=value;
				}
			}
			private string m_StringValue;
			public string stringValue{
				get{
					return this.m_StringValue;
				}
				set{
					if(m_StringValue != value){
						this.m_StringValue=value;
						if(PlayerPrefs.HasKey(name))
							PlayerPrefs.SetString(name,this.m_StringValue);
					}
				}
			}
			private int m_IntValue;
			public int intValue{
				get{
					return this.m_IntValue;
				}
				set{
					if(m_IntValue != value){
						this.m_IntValue=value;
						if(PlayerPrefs.HasKey(name))
							PlayerPrefs.SetInt(name,this.m_IntValue);
					}
				}
			}
			private float m_FloatValue;
			public float floatValue{
				get{
					return this.m_FloatValue;
				}
				set{
					if(this.m_FloatValue != value){
						this.m_FloatValue=value;
						if(PlayerPrefs.HasKey(name))
							PlayerPrefs.SetFloat(name,this.m_FloatValue);
					}
				}
			}

			public PlayerPrefValue(string name, string prefType, string value) {
				this.m_Name = name;

				switch (prefType) {
				case "integer":
					this.m_IntValue=int.Parse(value);
					this.m_PrefType = PrefType.Int;
					break;
				case "real":
					this.m_FloatValue = float.Parse(value);
					this.m_PrefType = PrefType.Float;
					break;
				case "string":
					this.m_StringValue = value;
					this.m_PrefType = PrefType.String;
					break;
				}
			}
		}
		
	}
}


