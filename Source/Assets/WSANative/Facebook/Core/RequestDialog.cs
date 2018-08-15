////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;

namespace CI.WSANative.Facebook.Core
{
    public sealed class RequestDialog : FacebookDialogBase
    {
        public RequestDialog(int screenWidth, int screenHeight)
            : base(screenWidth, screenHeight)
        {
        }

        public void Show(string uri, Dictionary<string, string> parameters, string responseUri, Grid parent, Action<IEnumerable<string>> closed)
        {
            _iFrame.NavigationStarting += (s, e) =>
            {
                if (e.Uri.AbsolutePath == responseUri)
                {
                    try
                    {
                        IList<string> userIds = new List<string>();

                        MatchCollection matches = Regex.Matches(e.Uri.Query, "&to\\[\\d+\\]=(\\d+)");

                        for(int i = 0; i < matches.Count; i++)
                        {
                            if(matches[i].Success)
                            {
                                userIds.Add(matches[i].Groups[1].Value);
                            }
                        }

                        Close(closed, userIds, parent);
                    }
                    catch
                    {
                        Close(closed, new List<string>(), parent);
                    }
                }
            };

            _closeButton.Click += (s, e) =>
            {
                Close(closed, new List<string>(), parent);
            };

            Show(uri, parameters, parent);
        }

        private void Close(Action<IEnumerable<string>> closed, IEnumerable<string> result, Grid parent)
        {
            parent.Children.Remove(this);

            if (closed != null)
            {
                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    closed(result);
                }, false);
            }
        }
    }
}
#endif