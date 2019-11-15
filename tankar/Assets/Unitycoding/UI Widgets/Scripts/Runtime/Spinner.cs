using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace Unitycoding.UIWidgets{
	public class Spinner : MonoBehaviour {
		public float changeDelay = 0.1f;
		[SerializeField]
		private float m_Current;
		public float current{
			get{
				return this.m_Current;
			}
			set{
				this.m_Current=value;
			}
		}
		public float step = 1.0f;
		public float min;
		public float max;
		public SpinnerEvent onChange=new SpinnerEvent();
		public SpinnerTextEvent m_OnChange=new SpinnerTextEvent();

		private IEnumerator coroutine;

		private void Start(){
			onChange.AddListener (delegate(float value) {
				m_OnChange.Invoke(Mathf.RoundToInt(value).ToString());
			});
		}


		public void StartIncrease(){
			Stop ();
			coroutine = Increase ();
			StartCoroutine (coroutine);
		}

		public void StartDecrease(){
			Stop ();
			coroutine = Decrease ();
			StartCoroutine (coroutine);
		}

		public void Stop(){
			if (coroutine != null) {
				StopCoroutine(coroutine);
			}
		}

		public IEnumerator Increase(){
			while (true) {
				current += step;
				current = Mathf.Clamp (current, min, max);
				onChange.Invoke(current);
				yield return new WaitForSeconds(changeDelay);
			}
		}

		public IEnumerator Decrease(){
			while (true) {
				current -= step;
				current = Mathf.Clamp (current, min, max);
				onChange.Invoke(current);
				yield return new WaitForSeconds(changeDelay);
			}
		}

		[System.Serializable]
		public class SpinnerEvent : UnityEvent<float>{} 
		
		[System.Serializable]
		public class SpinnerTextEvent : UnityEvent<string>{} 
	}
}