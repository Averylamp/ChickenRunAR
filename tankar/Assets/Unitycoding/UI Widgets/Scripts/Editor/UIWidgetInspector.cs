using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor.AnimatedValues;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Unitycoding.UIWidgets{
	[CustomEditor(typeof(UIWidget), true)]
	public class UIWidgetInspector: Editor
	{

		private SerializedProperty m_Script;
		private SerializedProperty m_Name;
		private SerializedProperty m_Focus;
		private SerializedProperty m_EaseType;
		private SerializedProperty m_Duration;
		private SerializedProperty m_ShowSound;
		private SerializedProperty m_CloseSound;
		private SerializedProperty m_DeactivateOnClose;

		protected string[] m_PropertyPathToExcludeForChildClasses;
		protected string[] m_PropertyPathToDrawForBaseClasses;
		
		
		private SerializedProperty m_DynamicContainer;
		private SerializedProperty m_SlotPrefab;
		private SerializedProperty m_Grid;
		private AnimBool m_ShowDynamicContainer;

		private SerializedProperty m_DelegatesProperty;
		private GUIContent m_IconToolbarMinus;
		private GUIContent m_EventIDName;
		private GUIContent[] m_EventTypes;
		private GUIContent m_AddButonContent;

		protected virtual void OnEnable()
		{
			this.m_Script = base.serializedObject.FindProperty("m_Script");
			this.m_Name = base.serializedObject.FindProperty("name");
			this.m_Focus = base.serializedObject.FindProperty ("focus");
			this.m_EaseType = base.serializedObject.FindProperty ("easeType");
			this.m_Duration = base.serializedObject.FindProperty ("duration");
			this.m_ShowSound = base.serializedObject.FindProperty("showSound");
			this.m_CloseSound = base.serializedObject.FindProperty("closeSound");
			this.m_DeactivateOnClose = base.serializedObject.FindProperty("deactivateOnClose");
			this.m_DelegatesProperty = base.serializedObject.FindProperty("m_Delegates");

			this.m_PropertyPathToDrawForBaseClasses=new string[] {this.m_Script.propertyPath, this.m_Name.propertyPath,this.m_Focus.propertyPath, this.m_EaseType.propertyPath, this.m_Duration.propertyPath, this.m_ShowSound.propertyPath, this.m_CloseSound.propertyPath,this.m_DeactivateOnClose.propertyPath };
			this.m_PropertyPathToExcludeForChildClasses=new string[this.m_PropertyPathToDrawForBaseClasses.Length];
			Array.Copy (this.m_PropertyPathToDrawForBaseClasses, this.m_PropertyPathToExcludeForChildClasses, this.m_PropertyPathToDrawForBaseClasses.Length);
			ArrayUtility.Add<string> (ref this.m_PropertyPathToExcludeForChildClasses, this.m_DelegatesProperty.propertyPath);
			
			this.m_DynamicContainer = base.serializedObject.FindProperty ("dynamicContainer");
			
			if (this.m_DynamicContainer != null) {
				this.m_Grid = serializedObject.FindProperty ("grid");
				this.m_SlotPrefab = serializedObject.FindProperty ("slotPrefab");
				if (this.m_Grid.objectReferenceValue == null) {
					GridLayoutGroup group=((MonoBehaviour)target).gameObject.GetComponentInChildren<GridLayoutGroup>();
					if(group != null){
						serializedObject.Update();
						this.m_Grid.objectReferenceValue=group.transform;
						serializedObject.ApplyModifiedProperties();
					}
				}
				this.m_ShowDynamicContainer = new AnimBool (this.m_DynamicContainer.boolValue);
				this.m_ShowDynamicContainer.valueChanged.AddListener (new UnityAction (this.Repaint));
				ArrayUtility.AddRange<string>(ref this.m_PropertyPathToExcludeForChildClasses,new string[]{m_DynamicContainer.propertyPath,this.m_Grid.propertyPath,this.m_SlotPrefab.propertyPath});
			}

			this.m_AddButonContent = new GUIContent("Add New Trigger");
			this.m_EventIDName = new GUIContent(string.Empty);
			this.m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"))
			{
				tooltip = "Remove all events in this list."
			};
			string[] names = Enum.GetNames(typeof(UIWidget.TriggerType));
			this.m_EventTypes = new GUIContent[(int)names.Length];
			for (int i = 0; i < (int)names.Length; i++)
			{
				this.m_EventTypes[i] = new GUIContent(names[i]);
			}
		}
	
		protected virtual void OnDisable(){
			if (this.m_ShowDynamicContainer != null) {
				this.m_ShowDynamicContainer.valueChanged.RemoveListener (new UnityAction (this.Repaint));
			}
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			this.BaseClassPropertiesGUI ();
			this.ChildClassPropertiesGUI ();
			this.TriggerGUI ();
			base.serializedObject.ApplyModifiedProperties();
		}

		private void OnAddNewSelected(object index)
		{
			int num = (int)index;
			SerializedProperty mDelegatesProperty = this.m_DelegatesProperty;
			mDelegatesProperty.arraySize = mDelegatesProperty.arraySize + 1;
			SerializedProperty arrayElementAtIndex = this.m_DelegatesProperty.GetArrayElementAtIndex(this.m_DelegatesProperty.arraySize - 1);
			arrayElementAtIndex.FindPropertyRelative("eventID").enumValueIndex = num;
			base.serializedObject.ApplyModifiedProperties();
		}
		
		private void RemoveEntry(int toBeRemovedEntry)
		{
			this.m_DelegatesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
		}
		
		private void ShowAddTriggermenu()
		{
			GenericMenu genericMenu = new GenericMenu();
			for (int i = 0; i < (int)this.m_EventTypes.Length; i++)
			{
				bool flag = true;
				for (int j = 0; j < this.m_DelegatesProperty.arraySize; j++)
				{
					if (this.m_DelegatesProperty.GetArrayElementAtIndex(j).FindPropertyRelative("eventID").enumValueIndex == i)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					genericMenu.AddDisabledItem(this.m_EventTypes[i]);
				}
				else
				{
					genericMenu.AddItem(this.m_EventTypes[i], false, new GenericMenu.MenuFunction2(this.OnAddNewSelected), i);
				}
			}
			genericMenu.ShowAsContext();
			Event.current.Use();
		}

		protected void BaseClassPropertiesGUI(){

			SerializedProperty iterator = base.serializedObject.GetIterator();
			bool flag = true;
			while (iterator.NextVisible(flag))
			{
				flag = false;
				if (this.m_PropertyPathToDrawForBaseClasses.Contains<string>(iterator.name) )
				{
					EditorGUILayout.PropertyField(iterator, true);
				}
			}
			
			if (this.m_DynamicContainer != null) {
				EditorGUILayout.PropertyField (this.m_DynamicContainer);
				this.m_ShowDynamicContainer.target = this.m_DynamicContainer.boolValue;
				if (EditorGUILayout.BeginFadeGroup (this.m_ShowDynamicContainer.faded)) {
					EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
					EditorGUILayout.PropertyField (this.m_Grid);
					EditorGUILayout.PropertyField (this.m_SlotPrefab);
					EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
				}
				EditorGUILayout.EndFadeGroup ();
			}
		}
		
		protected void ChildClassPropertiesGUI()
		{
			Editor.DrawPropertiesExcluding(base.serializedObject, this.m_PropertyPathToExcludeForChildClasses);
		}

		protected void TriggerGUI(){
			int num = -1;
			EditorGUILayout.Space();
			Vector2 vector2 = GUIStyle.none.CalcSize(this.m_IconToolbarMinus);
			for (int i = 0; i < this.m_DelegatesProperty.arraySize; i++)
			{
				SerializedProperty arrayElementAtIndex = this.m_DelegatesProperty.GetArrayElementAtIndex(i);
				SerializedProperty serializedProperty = arrayElementAtIndex.FindPropertyRelative("eventID");
				SerializedProperty serializedProperty1 = arrayElementAtIndex.FindPropertyRelative("callback");
				this.m_EventIDName.text = serializedProperty.enumDisplayNames[serializedProperty.enumValueIndex];
				EditorGUILayout.PropertyField(serializedProperty1, this.m_EventIDName, new GUILayoutOption[0]);
				Rect lastRect = GUILayoutUtility.GetLastRect();
				Rect rect = new Rect(lastRect.xMax - vector2.x - 8f, lastRect.y + 1f, vector2.x, vector2.y);
				if (GUI.Button(rect, this.m_IconToolbarMinus, GUIStyle.none))
				{
					num = i;
				}
				EditorGUILayout.Space();
			}
			if (num > -1)
			{
				this.RemoveEntry(num);
			}
			Rect rect1 = GUILayoutUtility.GetRect(this.m_AddButonContent, GUI.skin.button);
			rect1.x = rect1.x + (rect1.width - 200f) / 2f;
			rect1.width = 200f;
			if (GUI.Button(rect1, this.m_AddButonContent))
			{
				this.ShowAddTriggermenu();
			}
		}
	}
}