Windows Store Native (v1.15)
----------------------------

Thank you for downloading Windows Store Native we hope you enjoy using it!

A demo scene is included that shows how to use most features
Full documentation can be found on the website or if you need additional support please contact us at the address below
If you would like to see any features added please get in touch

Support Website: http://www.claytoninds.com/
Support Email: clayton.inds+support@gmail.com

------------------------------------------------------------------------------------------------------------------------
Basics - all functions listed below have detailed comments in the code

***All the functions below except the serialisation functions will only work once you build a Windows Store Solution - however they are safe to use in the editor***
***Banner, interstitial and mediator ads as well as maps, Facebook and Feedback Hub require additional setup - please see the website for details***

Show a dialog:
WSANativeDialog.ShowDialog

Show a popup menu:
WSANativePopupMenu.ShowPopupMenu

Serialise to and from xml:
WSANativeSerialisation.SerialiseToXML
WSANativeSerialisation.DeserialiseXML

Access storage:
WSANativeStorage.SaveFile
WSANativeStorage.LoadFile
WSANativeStorage.DeleteFile
WSANativeStorage.DoesFileExist
WSANativeStorage.GetAllFiles

Interface with the Windows Store:
WSANativeStore.EnableTestMode
WSANativeStore.EnableTestMode
WSANativeStore.GetProductListings
WSANativeStore.PurchaseProduct
WSANativeStore.ReportConsumableProductFulfillment
WSANativeStore.GetUnfulfilledConsumableProducts
WSANativeStore.PurchaseApp
WSANativeStore.GetLicenseForApp
WSANativeStore.GetLicenseForProduct

File Picker:
WSANativeFilePicker.ShowFileOpenPicker
WSANativeFilePicker.ShowFileSavePicker

Advertising:
Banner:
WSANativeBannerAd.CreatAd
WSANativeBannerAd.SetVisibility
WSANativeBannerAd.DestroyAd
WSANativeBannerAd.HasAd
Interstitial:
WSANativeInterstitialAd.Initialise
WSANativeInterstitialAd.RequestAd
WSANativeInterstitialAd.ShowAd
WSANativeInterstitialAd.CloseAd
Mediator:
WSANativeMediatorAd.CreatAd
WSANativeMediatorAd.SetVisibility
WSANativeMediatorAd.DestroyAd

Web:
WSANativeWeb.GetString
WSANativeWeb.GetBytes
WSANativeWeb.PostReturnString
WSANativeWeb.PostReturnBytes

Notification:
WSANativeNotification.ShowToastNotification
WSANativeNotification.CreatePushNotificationChannel
WSANativeNotification.SendPushNotificationUriToServer

Mapping:
WSANativeMap.CreateMap
WSANativeMap.CenterMap
WSANativeMap.AddMapElement
WSANativeMap.ClearMap
WSANativeMap.DestroyMap

Geolocation:
WSANativeGeolocation.GetUsersLocation

Social:
WSANativeSocial.ShowAppStoreDescriptionPage
WSANativeSocial.ShowAppStoreReviewPage
WSANativeSocial.ComposeEmail
WSANativeSocial.Share

Device:
WSANativeDevice.EnableFlashlight
WSANativeDevice.DisableFlashlight
WSANativeDevice.CaptureScreenshot

Security:
WSANativeSecurity.SymmetricEncrypt
WSANativeSecurity.SymmetricDecrypt
WSANativeSecurity.EncodeBase64
WSANativeSecurity.DecodeBase64

Facebook:
WSANativeFacebook.Initialise
WSANativeFacebook.Login
WSANativeFacebook.Logout
WSANativeFacebook.GetUserDetails
WSANativeFacebook.HasUserLikedPage
WSANativeFacebook.GraphApiRead
WSANativeFacebook.ShowFeedDialog
WSANativeFacebook.ShowRequestDialog
WSANativeFacebook.ShowSendDialog

Engagement:
WSANativeEngagement.IsFeedbackHubSupported
WSANativeEngagement.ShowFeedbackHub

------------------------------------------------------------------------------------------------------------------------
Windows Store - Configuring test products

When in test mode products and info about your app are defined in an xml file called WindowsStoreProxy.xml which can be found at C:\Users\<username>\AppData\Local\Packages\<app package folder>\LocalState\Microsoft\Windows Store\ApiData\WindowsStoreProxy.xml. If the file isn't there you can copy one from the website.
Once you have build your windows store solution you can copy the file to the Assets folder (this is the Assets folder in the solution that is created when you do a build from Unity).
Then call WSANativeStore.ReloadSimulator when you app starts up to configure the simulated store.

------------------------------------------------------------------------------------------------------------------------
WSANative makes use of JSON.NET and is used under the following license

The MIT License (MIT)

Copyright (c) 2007 James Newton-King

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.