////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Dialogs
{
    public class WSADialogResult
    {
        /// <summary>
        /// The name of the button that was pressed
        /// </summary>
        public string ButtonPressed
        {
            get;
            private set;
        }

        /// <summary>
        /// Indicates the users response to a dialog
        /// </summary>
        /// <param name="buttonPressed">The name of the button that was pressed</param>
        public WSADialogResult(string buttonPressed)
        {
            ButtonPressed = buttonPressed;
        }
    }
}