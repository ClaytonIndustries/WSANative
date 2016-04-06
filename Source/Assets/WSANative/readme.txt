WSA Native

Thanks for downloading WSA Native we hope you enjoy using it!

A demo scene is included that shows how to use most features
Full documentation can be found on the website or if you need additional support please contact us at the address below
If you would like to see any features added please get in touch

Support Website: http://claytonindustries.16mb.com/
Support Email: clayton.inds+support@gmail.com

------------------------------------------------------------------------------------------------------------------------
Basics - all functions listed below have detailed comments in the code

***All the functions below except the serialisation functions will only work once you build a windows store solution - however they are safe to use in the editor***
***Both banner and interstitial ads require additional setup once you have build your windows store solution using unity - please see the website for details***

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
WSANativeStore.ShowAppStorePage

File Picker:
WSANativeFilePicker.ShowFileOpenPicker
WSANativeFilePicker.ShowFileSavePicker

Advertising:
Banner:
CreatAd
DestroyAd
Interstitial:
WSANativeInterstitialAd.Initialise
WSANativeInterstitialAd.RequestAd
WSANativeInterstitialAd.ShowAd
WSANativeInterstitialAd.CloseAd


------------------------------------------------------------------------------------------------------------------------
Windows Store - Configuring test products

When in test mode products and info about your app are defined in an xml file called WindowsStoreProxy.xml which can be found at C:\Users\<username>\AppData\Local\Packages\<app package folder>\LocalState\Microsoft\Windows Store\ApiData\WindowsStoreProxy.xml.
Once you have build your windows store solution you can copy the file to the Assets folder (this is the Assets folder in the solution that is created when you do a build from Unity) and call reload simulator (see below) to configure the simulated store.
Then call WSANativeStore.ReloadSimulator when you app starts up.