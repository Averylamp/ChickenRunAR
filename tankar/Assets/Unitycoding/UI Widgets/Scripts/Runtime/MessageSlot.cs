using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Unitycoding.UIWidgets{
	public class MessageSlot : UISlot<MessageOptions> {
		public Text text;
		public Image icon;
		
		protected override void Start ()
		{
			base.Start ();
			transform.SetAsLastSibling ();
		}
		
		public override void UpdateSlot (MessageOptions item)
		{
			if (item != null) {
				if (text != null) {
					text.text = UIUtility.ColorString (item.text, item.color);
					DelayCrossFade (text, item);
				}
			
				if (icon != null) {
					icon.gameObject.SetActive (item.icon != null);
					if (item.icon != null) {
						icon.sprite = item.icon;
						DelayCrossFade (icon, item);
					}
				}
			}
		}
		
		private void DelayCrossFade(Graphic graphic, MessageOptions options){
			StartCoroutine (DelayCrossFade (graphic, options.delay, options.duration, options.ignoreTimeScale));
		}
		
		private IEnumerator DelayCrossFade(Graphic graphic, float delay,float duration,bool ignoreTimeScale){
			yield return new WaitForSeconds(delay);
			if ((container as MessageContainer).fadeMessage)
				graphic.CrossFadeAlpha(0f,duration,ignoreTimeScale);
		}
	}
}