/****************************************************************************
 * Copyright (c) 2017 snowcold
 * Copyright (c) 2015 - 2022 liangxiegame UNDER MIT License
 * 
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

namespace QFramework
{
    using System;
    using System.Collections.Generic;
    
    public class EnumEventSystem 
    {
        public static readonly EnumEventSystem Global = new EnumEventSystem();
        
        private readonly Dictionary<int, IEasyEvent> mEvents = new Dictionary<int, IEasyEvent>(50);
        
        protected EnumEventSystem(){}

        #region 功能函数

        public IUnRegister Register<T>(T key, Action<int,object[]> onEvent) where T : IntegerContertable
        {
            var kv = key.ToInt32();

            if (mEvents.TryGetValue(kv, out var e))
            {
                var easyEvent = e.As<EasyEvent<int,object[]>>();
                return easyEvent.Register(onEvent);
            }
            else
            {
                var easyEvent = new EasyEvent<int,object[]>();
                mEvents.Add(kv, easyEvent);
                return easyEvent.Register(onEvent);
            }
        }
        
        public IUnRegister Register(int key, Action<int,object[]> onEvent)
        {
            var kv = key;

            if (mEvents.TryGetValue(kv, out var e))
            {
                var easyEvent = e.As<EasyEvent<int,object[]>>();
                return easyEvent.Register(onEvent);
            }
            else
            {
                var easyEvent = new EasyEvent<int,object[]>();
                mEvents.Add(kv, easyEvent);
                return easyEvent.Register(onEvent);
            }
        }

        public void UnRegister<T>(T key, Action<int,object[]> onEvent) where T : IntegerContertable
        {
            var kv = key.ToInt32();

            if (mEvents.TryGetValue(kv, out var e))
            {
                e.As<EasyEvent<int,object[]>>()?.UnRegister(onEvent);
            }
        }
        
        public void UnRegister<T>(T key) where T : IntegerContertable
        {
            var kv = key.ToInt32();

            if (mEvents.ContainsKey(kv))
            {
                mEvents.Remove(kv);
            }
        }
        
        public void UnRegister(int key)
        {
            if (mEvents.ContainsKey(key))
            {
                mEvents.Remove(key);
            }
        }
        
        public void UnRegister(int key,Action<int,object[]> onEvent)
        {
            if (mEvents.TryGetValue(key, out var e))
            {
                e.As<EasyEvent<int,object[]>>()?.UnRegister(onEvent);
            }
        }

        public void UnRegisterAll()
        {
            mEvents.Clear();
        }

        public void Send<T>(T key, params object[] args) where T : IntegerContertable
        {
            var kv = key.ToInt32();

            if (mEvents.TryGetValue(kv, out var e))
            {
                e.As<EasyEvent<int,object[]>>().Trigger(kv,args);
            }
        }
        
        public void  Send(int key, params object[] args)
        {
            var kv = key;

            if (mEvents.TryGetValue(kv, out var e))
            {
                e.As<EasyEvent<int,object[]>>().Trigger(kv,args);
            }
        }
        
        #endregion
        
    }

    [Obsolete("请使用 EnumEventSystem,Please use EnumEventSystem instead", APIVersion.Force)]
    public class QEventSystem : EnumEventSystem
    {
        protected QEventSystem() : base()
        {
            
        }
        [Obsolete("请使用 Global,Please use Global instead", APIVersion.Force)]
        public static EnumEventSystem Instance => Global;
        
        
        [Obsolete("请使用 Global.Send,Please use Global.Send instead", APIVersion.Force)]
        public static void SendEvent<T>(T key, params object[] param) where T : IntegerContertable
        {
            Global.Send(key, param);
        }

        [Obsolete("请使用 Global.Register,Please use Global.Register instead", APIVersion.Force)]
        public static void RegisterEvent<T>(T key, Action<int,object[]> fun) where T : IntegerContertable
        {
            Global.Register(key, fun);
        }

        [Obsolete("请使用 Global.UnRegister,Please use Global.UnRegister instead", APIVersion.Force)]
        public static void UnRegisterEvent<T>(T key, Action<int,object[]> fun) where T : IntegerContertable
        {
            Global.UnRegister(key, fun);
        }
    }
}