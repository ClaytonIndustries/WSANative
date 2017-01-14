using System;
using CI.WSANative.Dispatchers.Core;
using UnityEngine;

namespace CI.WSANative.Dispatchers
{
    public static class WSANativeDispatcher
    {
        private static WSADispatcher _dispatcher;

        static WSANativeDispatcher()
        {
            if(_dispatcher == null)
            {
                _dispatcher = new GameObject("WSANativeDispatcher").AddComponent<WSADispatcher>();
            }
        }

        public static void Invoke(Action action)
        {
            _dispatcher.Enqueue(action);
        }
    }
}