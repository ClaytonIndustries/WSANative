////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using CI.WSANative.Dispatchers.Core;
using UnityEngine;

namespace CI.WSANative.Dispatchers
{
    public static class WSANativeDispatcher
    {
        private static WSADispatcher _dispatcher;

        public static void Initialise()
        {
            if (_dispatcher == null)
            {
                _dispatcher = new GameObject("WSANativeDispatcher").AddComponent<WSADispatcher>();
            }
        }

        public static void Invoke(Action action)
        {
            if (_dispatcher != null)
            {
                _dispatcher.Enqueue(action);
            }
        }
    }
}