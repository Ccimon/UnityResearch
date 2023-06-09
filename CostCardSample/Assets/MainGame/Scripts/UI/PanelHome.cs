using MainGame.Events;
using MainGame.Scripts;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Scripts
{
	public class GameStartCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			GridModel gridModel = this.GetModel<GridModel>();
			
		}
	}
	public class PanelHomeData : UIPanelData
	{
	}
	public partial class PanelHome : UIPanel,IController
	{
		// private ResLoader _loader = ResLoader.Allocate();
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as PanelHomeData ?? new PanelHomeData();
			
			InitListener();
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
		
		#region 事件监听

		private void InitListener()
		{
			BtnGameStart.onClick.AddListener(OnBtnPlayClick);
			// this.RegisterEvent<Game_Event_Board_Init>(GameBoardInit);
		}
		
		#endregion

		#region 事件
		
		private void OnBtnPlayClick()
		{
			TypeEventSystem.Global.Send<Game_Event_Start>();
			BtnGameStart.Hide();
		}
		
		#endregion

		public IArchitecture GetArchitecture()
		{
			return MainArchitecture.Interface;
		}
	}
}
