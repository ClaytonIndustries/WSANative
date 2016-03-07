////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Dialogs
{
    public class WSADialogCommand
    {
        public string ButtonName
        {
            get;
            private set;
        }

        /// <summary>
        /// Adds a button to the dialog
        /// </summary>
        /// <param name="buttonName">Name of the button</param>
        public WSADialogCommand(string buttonName)
        {
            ButtonName = buttonName;
        }
    }
}