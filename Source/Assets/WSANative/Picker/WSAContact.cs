////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

#if ENABLE_WINMD_SUPPORT
using Windows.ApplicationModel.Contacts;
#endif

namespace CI.WSANative.Pickers
{
    public class WSAContact
    {
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public IEnumerable<string> Emails { get; set; }
        public IEnumerable<string> Phones { get; set; }

#if ENABLE_WINMD_SUPPORT
        /// <summary>
        /// The original contact selected by the user - any calls to this must be wrapped in a NETFX_CORE block
        /// </summary>
        public Contact OriginalContact { get; set; }
#endif
    }
}