using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace Unitycoding{
	/// <summary>
	/// Utility editor window.
	/// </summary>
	public class UtilityWindow : EditorWindow {
		public static UtilityWindow instance;
		private Action onClose;
		private Action onGUI;
		private Vector2 scroll;

		public static UtilityWindow ShowWindow(string title, Action onGUI){
			return ShowWindow (title, new Vector2 (227,200), onGUI, null);
		}
		
		public static UtilityWindow ShowWindow(string title,Vector2 size, Action onGUI){
			return ShowWindow (title, size, onGUI, null);
		}

		public static UtilityWindow ShowWindow(string title,Vector2 size, Action onGUI,Action onClose){
			UtilityWindow window = EditorWindow.GetWindow<UtilityWindow>(true,title);
			window.minSize = size;
			window.onGUI = onGUI;
			window.onClose = onClose;
			return window;
		}

		private void OnEnable(){
			instance = this;
		}
		
		public static void CloseWindow(){
			if (UtilityWindow.instance != null) {
				UtilityWindow.instance.Close();
			}
		}
		
		private void OnDestroy(){
			if (onClose != null) {
				onClose.Invoke();
			}
		}

		private void OnGUI(){
			scroll = EditorGUILayout.BeginScrollView (scroll);
			if (onGUI != null) {
				onGUI.Invoke ();
			}
			EditorGUILayout.EndScrollView ();
		}
	}
}