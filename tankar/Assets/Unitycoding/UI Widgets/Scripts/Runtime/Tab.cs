using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace Unitycoding.UIWidgets{
	[RequireComponent(typeof(Button))]
	public class Tab : MonoBehaviour {

		public TabEvent onSelect=new TabEvent();
		public TabEvent onDeselect = new TabEvent ();

		private Button m_Button;

		// Use this for initialization
		private void Start () {
			m_Button = GetComponent<Button> ();
			m_Button.onClick.AddListener (Select);
		}

		public void Select(){
			m_Button.transform.parent.BroadcastMessage ("Deselect",this,SendMessageOptions.DontRequireReceiver);
			onSelect.Invoke ();
		}

		private void Deselect(Tab exceptTab){
			if (this != exceptTab) {
				onDeselect.Invoke();
			}
		}

		[System.Serializable]
		public class TabEvent:UnityEvent{}
	}
}