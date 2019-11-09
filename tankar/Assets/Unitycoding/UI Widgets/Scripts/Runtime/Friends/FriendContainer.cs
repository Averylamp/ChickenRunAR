
using UnityEngine;
using System.Collections;
using Unitycoding.UIWidgets;
using System.Collections.Generic;
using System.Linq;

namespace Unitycoding
{
	public class FriendContainer :  UIContainer<FriendInfo>
	{

		protected override void OnStart ()
		{
			Load ();
		}

		public virtual bool Add (FriendInfo item, bool save)
		{
			if (item != null && Items.Find (x => x != null && x.Name == item.Name) == null && base.Add (item)) {
				if (save) {
					Save ();
				}
				return true;
			}
			return false;
			
		}

		public override bool Add (FriendInfo item)
		{
			return Add (item, true);
		}

		public void Save ()
		{
			List<string> friends = this.Items.Select (x => x.Name).ToList ();
			string data = MiniJSON.Serialize (friends);
			string playerName = PlayerPrefs.GetString ("Player");
			PlayerPrefs.SetString (playerName + ".Friends", data);
		}

		public void Load ()
		{
			string playerName = PlayerPrefs.GetString ("Player");
			string data = PlayerPrefs.GetString (playerName + ".Friends");
			if (!string.IsNullOrEmpty (data)) {
				List<string> friendNames = ((List<object>)MiniJSON.Deserialize (data)).OfType<string> ().ToList ();
				for (int i = 0; i < friendNames.Count; i++) {
					FriendInfo info = new FriendInfo (friendNames [i]);
					Add (info, false);
				}
			}
		}
	}
}
