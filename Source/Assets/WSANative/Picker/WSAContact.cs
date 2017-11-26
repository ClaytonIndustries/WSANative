using System.Collections.Generic;

#if NETFX_CORE
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

#if NETFX_CORE
        /// <summary>
        /// The original contact selected by the user - any calls to this must be wrapped in a NETFX_CORE block
        /// </summary>
        public Contact OriginalContact { get; set; }
#endif
    }
}