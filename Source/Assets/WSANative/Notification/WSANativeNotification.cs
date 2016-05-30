#if NETFX_CORE
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
#endif

namespace CI.WSANative.Notification
{
    public static class WSANativeNotification
    {
        /// <summary>
        /// Shows a toast notfication with the specified title and text
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public static void ShowToastNotification(string title, string text)
        {
#if NETFX_CORE
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");

            if(stringElements != null && stringElements.Length >= 2)
            {
                stringElements[0].AppendChild(toastXml.CreateTextNode(title));
                stringElements[1].AppendChild(toastXml.CreateTextNode(text));

                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
#endif
        }
    }
}