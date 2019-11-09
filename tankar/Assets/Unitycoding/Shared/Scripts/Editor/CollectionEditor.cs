using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Unitycoding{
	/// <summary>
	/// Base class for a collection of items.
	/// </summary>
	public abstract class CollectionEditor<T>: ICollectionEditor {
		private const  float LIST_MIN_WIDTH = 200f;
		private const  float LIST_MAX_WIDTH = 400f;
		private const float LIST_RESIZE_WIDTH = 10f;


		protected Rect sidebarRect = new Rect(0,30,200,1000);
		protected Vector2 scrollPosition;
		protected string searchString="Search...";
		protected Vector2 sidebarScrollPosition;

		protected T selectedItem;
		protected abstract List<T> Items {get;}

		public void OnGUI(Rect position){

			sidebarRect = new Rect (position.x, position.y, sidebarRect.width, position.height);
			GUILayout.BeginArea(sidebarRect,"",Styles.background);

			DoSearchGUI ();

			EditorGUILayout.Space ();
			Color color = GUI.color;
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button ("Create...")) {
				GUI.FocusControl("");
				Create();
				if(Items.Count>0){
					selectedItem=Items[Items.Count-1];
				}
			}
			GUI.backgroundColor = color;
			EditorGUILayout.Space ();
			sidebarScrollPosition = GUILayout.BeginScrollView (sidebarScrollPosition);
			for (int i = 0; i < Items.Count; i++) {
				T currentItem = Items[i];
				if(!MatchesSearch(currentItem,searchString)){
					continue;
				}
				GUILayout.BeginHorizontal();
				color = GUI.backgroundColor;
				GUI.backgroundColor = (selectedItem != null && selectedItem.Equals(currentItem) ? new Color(0, 1.0f, 0, 0.3f) : new Color(0, 0, 0, 0.0f));
				if (GUILayout.Button ("# "+i+"  "+GetSidebarLabel(currentItem), Styles.selectButton,GUILayout.Height(25))) {
					GUI.FocusControl("");
					selectedItem = currentItem;
					Select(selectedItem);
				}

				GUI.backgroundColor=color;
				if(GUILayout.Button("",Styles.minusButton,GUILayout.Width(18))){
					GUI.FocusControl("");
					Remove(currentItem);
				}
				GUILayout.EndHorizontal();
				
			}
			GUILayout.EndScrollView ();
			GUILayout.EndArea ();


			Rect rect = new Rect (sidebarRect.width, sidebarRect.y, position.width - sidebarRect.width, position.height);

			GUILayout.BeginArea (rect, "",Styles.background);
			scrollPosition = GUILayout.BeginScrollView (scrollPosition,EditorStyles.inspectorDefaultMargins);
			if (selectedItem != null && Items.Contains(selectedItem)) {
				DrawItem(selectedItem);
			}
			GUILayout.EndScrollView ();
			GUILayout.EndArea ();
			ResizeSidebar();
		}

		/// <summary>
		/// Select an item.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void Select(T item){}

		/// <summary>
		/// Create an item.
		/// </summary>
		protected virtual void Create(){}

		/// <summary>
		/// Remove the specified item from collection.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void Remove(T item){}

		/// <summary>
		/// Draws the item properties.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void DrawItem(T item){}

		/// <summary>
		/// Gets the sidebar label displayed in sidebar.
		/// </summary>
		/// <returns>The sidebar label.</returns>
		/// <param name="item">Item.</param>
		protected abstract string GetSidebarLabel(T item);

		/// <summary>
		/// Checks for search.
		/// </summary>
		/// <returns><c>true</c>, if search was matchesed, <c>false</c> otherwise.</returns>
		/// <param name="item">Item.</param>
		/// <param name="search">Search.</param>
		protected abstract bool MatchesSearch (T item, string search);

		protected virtual void DoSearchGUI(){
			searchString = UnityEditorUtility.SearchField (searchString,GUILayout.Width(sidebarRect.width-20));
		}

		private void ResizeSidebar(){
			Rect rect = new Rect (sidebarRect.width - LIST_RESIZE_WIDTH*0.5f, sidebarRect.y, LIST_RESIZE_WIDTH, sidebarRect.height);
			EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			Event ev = Event.current;
			switch (ev.rawType) {
			case EventType.MouseDown:
				if(rect.Contains(ev.mousePosition)){
					GUIUtility.hotControl = controlID;
					ev.Use();
				}
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID)
				{
					GUIUtility.hotControl = 0;
					ev.Use();
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID)
				{
					sidebarRect.width=ev.mousePosition.x;
					sidebarRect.width=Mathf.Clamp(sidebarRect.width,LIST_MIN_WIDTH,LIST_MAX_WIDTH);
					ev.Use();
				}
				break;
			}
		}

		public static class Styles{
			public static GUIStyle minusButton;
			public static GUIStyle selectButton;
			public static GUIStyle background;

			static Styles(){
				minusButton = new GUIStyle ("OL Minus"){
					margin=new RectOffset(0,0,4,0)
				};
				selectButton = new GUIStyle ("MeTransitionSelectHead"){
					alignment= TextAnchor.MiddleLeft
				};
				background = new GUIStyle("ProgressBarBack");
			}
		}
	}
}