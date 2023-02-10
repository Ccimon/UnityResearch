using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Scripts
{
	// Generate Id:53bcfd90-310b-4a6b-9a8f-ceb715e79480
	public partial class Panel
	{
		public const string Name = "Panel";
		
		[SerializeField]
		public UnityEngine.UI.Image ImgBg;
		[SerializeField]
		public RectTransform PanelTop;
		[SerializeField]
		public RectTransform PanelContent;
		[SerializeField]
		public RectTransform PanelBottom;
		
		private PanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ImgBg = null;
			PanelTop = null;
			PanelContent = null;
			PanelBottom = null;
			
			mData = null;
		}
		
		public PanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		PanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
