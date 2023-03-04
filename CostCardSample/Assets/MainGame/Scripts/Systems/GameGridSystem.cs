using UnityEngine;
using QFramework;

namespace MainGame.Events.Scripts.Systems
{
    public class GameGridSystem : ISystem
    {
        private IArchitecture _main;
        public IArchitecture GetArchitecture()
        {
            return _main;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            _main = architecture;
        }

        public void Init()
        {
        }
    }
}