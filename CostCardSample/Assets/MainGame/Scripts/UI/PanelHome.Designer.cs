using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Scripts
{
	// Generate Id:41a8538c-b377-4a2c-819c-19cdb9c0b048
	public partial class PanelHome
	{
		public const string Name = "PanelHome";
		
		[SerializeField]
		public RectTransform Panel;
		[SerializeField]
		public UnityEngine.UI.Image ImgBg;
		[SerializeField]
		public RectTransform PanelTop;
		[SerializeField]
		public RectTransform PanelContent;
		[SerializeField]
		public UnityEngine.UI.Text TextGameStart;
		[SerializeField]
		public RectTransform GameContent;
		[SerializeField]
		public RectTransform PanelBottom;
		
		private PanelHomeData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Panel = null;
			ImgBg = null;
			PanelTop = null;
			PanelContent = null;
			TextGameStart = null;
			GameContent = null;
			PanelBottom = null;
			
			mData = null;
		}
		
		public PanelHomeData Data
		{
			get
			{
				return mData;
			}
		}
		
		PanelHomeData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PanelHomeData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
