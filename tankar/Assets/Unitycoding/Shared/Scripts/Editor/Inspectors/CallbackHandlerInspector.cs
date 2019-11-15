using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEditorInternal;

namespace Unitycoding{
	[CustomEditor(typeof(CallbackHandler), true)]
	public class CallbackHandlerInspector : Editor {
		private GUIContent addButtonContent;
		private GUIContent[] eventCallbackTypes;
		private SerializedProperty delegatesProperty;
		private GUIContent eventCallbackName;
		private GUIContent iconToolbarMinus;
		
		protected virtual void OnEnable(){
			this.delegatesProperty = base.serializedObject.FindProperty("delegates");
			this.addButtonContent = new GUIContent ("Add Listener");
			this.eventCallbackName = new GUIContent(string.Empty);
			string[] names = (target as CallbackHandler).Callbacks;
			this.eventCallbackTypes = new GUIContent[(int)names.Length];
			for (int i = 0; i < (int)names.Length; i++)
			{
				this.eventCallbackTypes[i] = new GUIContent(names[i]);
			}
			this.iconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"))
			{
				tooltip = "Remove all events in this list."
			};
		}
		
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			base.serializedObject.Update();
			int num = -1;
			EditorGUILayout.Space();
			Vector2 vector2 = GUIStyle.none.CalcSize(this.iconToolbarMinus);
			for (int i = 0; i < this.delegatesProperty.arraySize; i++)
			{
				SerializedProperty arrayElementAtIndex = this.delegatesProperty.GetArrayElementAtIndex(i);
				SerializedProperty serializedProperty = arrayElementAtIndex.FindPropertyRelative("eventID");
				SerializedProperty serializedProperty1 = arrayElementAtIndex.FindPropertyRelative("callback");
				this.eventCallbackName.text = serializedProperty.stringValue;
				
				EditorGUILayout.PropertyField(serializedProperty1, this.eventCallbackName, new GUILayoutOption[0]);
				Rect lastRect = GUILayoutUtility.GetLastRect();
				Rect rect = new Rect(lastRect.xMax - vector2.x - 8f, lastRect.y + 1f, vector2.x, vector2.y);
				if (GUI.Button(rect, this.iconToolbarMinus, GUIStyle.none))
				{
					num = i;
				}
				EditorGUILayout.Space();
			}
			if (num > -1)
			{
				this.RemoveEntry(num);
			}
			Rect rect1 = GUILayoutUtility.GetRect(this.addButtonContent, GUI.skin.button);
			rect1.x = rect1.x + (rect1.width - 200f) / 2f;
			rect1.width = 200f;
			if (GUI.Button(rect1, this.addButtonContent))
			{
				this.ShowAddEventmenu();
			}
			base.serializedObject.ApplyModifiedProperties();
		}
		
		private void ShowAddEventmenu(){
			GenericMenu genericMenu = new GenericMenu();
			for (int i = 0; i < (int)this.eventCallbackTypes.Length; i++)
			{
				bool flag = true;
				for (int j = 0; j < this.delegatesProperty.arraySize; j++)
				{
					if (this.delegatesProperty.GetArrayElementAtIndex(j).FindPropertyRelative("eventID").stringValue == eventCallbackTypes[i].text)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					genericMenu.AddDisabledItem(this.eventCallbackTypes[i]);
				}
				else
				{
					genericMenu.AddItem(this.eventCallbackTypes[i], false, new GenericMenu.MenuFunction2(this.OnAddNewSelected), eventCallbackTypes[i].text);
				}
			}
			genericMenu.ShowAsContext();
			Event.current.Use();
		}
		
		private void OnAddNewSelected(object eventID ){
			string id = (string)eventID;
			SerializedProperty mDelegatesProperty = this.delegatesProperty;
			mDelegatesProperty.arraySize = mDelegatesProperty.arraySize + 1;
			SerializedProperty arrayElementAtIndex = this.delegatesProperty.GetArrayElementAtIndex(this.delegatesProperty.arraySize - 1);
			arrayElementAtIndex.FindPropertyRelative("eventID").stringValue = id;
			base.serializedObject.ApplyModifiedProperties();
		}
		
		private void RemoveEntry(int index)
		{
			this.delegatesProperty.DeleteArrayElementAtIndex(index);
		}
	}
}