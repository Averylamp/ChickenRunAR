using UnityEngine;
using System.Collections;

namespace Unitycoding{
	public class DontDestroyOnLoad : MonoBehaviour {
		private void Awake(){
			DontDestroyOnLoad (gameObject);
		}
	}
}