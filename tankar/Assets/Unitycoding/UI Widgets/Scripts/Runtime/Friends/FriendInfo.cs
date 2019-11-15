using UnityEngine;
using System.Collections;
using System.Linq;

namespace Unitycoding
{
	public class FriendInfo
	{
		
		protected string m_Name;

		public string Name {
			get{ return this.m_Name; }
		}

		protected bool m_Online;

		public bool Online {
			get{ return this.m_Online; }
			set{ this.m_Online = value; }
		}

		public FriendInfo (string name)
		{
			this.m_Name = name;	
		}
	}
}