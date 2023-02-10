using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Scripts
{
	public class PanelData : UIPanelData
	{
	}
	public partial class Panel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as PanelData ?? new PanelData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
