using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Scripts
{
	// Generate Id:1cb54a7b-ac58-4c82-a792-59d9242a11f8
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
		public RectTransform PanelBottom;
		
		private PanelHomeData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Panel = null;
			ImgBg = null;
			PanelTop = null;
			PanelContent = null;
			TextGameStart = null;
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
