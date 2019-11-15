using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CoolDown : MonoBehaviour,IPointerClickHandler {
	public Image overlay;
	private bool isCoolDown;
	public bool IsCoolDown{
		get{
			return isCoolDown;
		}
	}
	private float coolDownDuration;
	private float coolDownInitTime;
	public void Update ()
	{
		if (overlay != null) {
			if (Time.time - coolDownInitTime < coolDownDuration) {
				overlay.fillAmount = Mathf.Clamp01 (1 - ((Time.time - coolDownInitTime) / coolDownDuration));
			} else {
				overlay.fillAmount = 0;
			}
			isCoolDown = overlay.fillAmount > 0;
		}
	}

	public void OnPointerClick(PointerEventData eventData){
		Cooldown (3f, 1.5f);
	}

	public void Cooldown(float coolDown, float globalCoolDown){
		if (!isCoolDown) {
			coolDownDuration = coolDown;
			coolDownInitTime = Time.time;
			isCoolDown = true;
			transform.root.BroadcastMessage ("GlobalCooldown", globalCoolDown, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	private void GlobalCooldown(float coolDown){
		if (((Time.time + coolDownInitTime * coolDownDuration) < (Time.time + coolDownInitTime * coolDown)) || !isCoolDown) {
			coolDownDuration = coolDown;
			coolDownInitTime = Time.time;
		}
	}
}
