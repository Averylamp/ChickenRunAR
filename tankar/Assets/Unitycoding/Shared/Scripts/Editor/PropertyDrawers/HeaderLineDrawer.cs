using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Unitycoding{
	[CustomPropertyDrawer(typeof(HeaderLineAttribute))]
	public class HeaderLineDrawer : DecoratorDrawer
	{
		public HeaderLineDrawer()
		{
		}
		
		public override float GetHeight()
		{
			return 34f;
		}
		
		public override void OnGUI(Rect position)
		{
			position.y = position.y + 25f;
			position = EditorGUI.IndentedRect(position);
			GUI.Label (position,(base.attribute as HeaderLineAttribute).header, Line);
		}

		private GUIStyle line;
		private GUIStyle Line{
			get{
				if(line == null){
					line= new GUIStyle("ShurikenLine");
					line.fontSize=14;
					line.fontStyle=FontStyle.Bold;
					line.normal.textColor=((GUIStyle)"label").normal.textColor;
					line.contentOffset=new Vector2(3,-2);
				}
				return line;
			}
		}
	}
}