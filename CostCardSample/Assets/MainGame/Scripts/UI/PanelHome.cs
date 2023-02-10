using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Scripts
{
	public class PanelHomeData : UIPanelData
	{
	}
	public partial class PanelHome : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as PanelHomeData ?? new PanelHomeData();
			
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
