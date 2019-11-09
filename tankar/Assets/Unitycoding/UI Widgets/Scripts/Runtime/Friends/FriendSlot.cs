
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unitycoding.UIWidgets;

namespace Unitycoding
{
	public class FriendSlot : UISlot<FriendInfo>
	{
		[SerializeField]
		protected Text playerName;
		[SerializeField]
		protected Image status;
		[SerializeField]
		protected Sprite online;
		[SerializeField]
		protected Sprite offline;
		[SerializeField]
		protected Button message;


		public override void UpdateSlot (FriendInfo item)
		{
			if (item != null) {
				if (this.playerName != null) {
					this.playerName.text = item.Name;
				}
				if (this.status != null) {
					this.status.sprite = item.Online ? online : offline;
				}
			}
		}
	}
}
