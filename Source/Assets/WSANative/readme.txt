Windows Store Native (v1.32)
----------------------------

Thank you for downloading Windows Store Native we hope you enjoy using it!

A demo scene is included that shows how to use most features
Full documentation can be found on the website or if you need additional support please contact us at the address below
If you would like to see any features added please get in touch

Support Website: http://www.claytoninds.com/
Support Email: clayton.inds+support@gmail.com

------------------------------------------------------------------------------------------------------------------------
Why not check out our other plugins:
- Http Client (Http requests made easy)
- Quick Save (Saving made easy)
- Task Parallel (Threading made easy)

------------------------------------------------------------------------------------------------------------------------
Basics - all functions listed below have detailed comments in the code

***All the functions below except the Dispatcher, Image and Serialisation functions will only work once you build a Windows Store Solution - however they are safe to use in the editor***
***Banner and Interstitial Ads as well as Maps, Facebook, Feedback Hub, Input and Twitter require additional setup - please see the website for details***

------------------------------------------------------------------------------------------------------------------------
Windows Store - Configuring test products

When in test mode products and info about your app are defined in an xml file called WindowsStoreProxy.xml which can be found at C:\Users\<username>\AppData\Local\Packages\<app package folder>\LocalState\Microsoft\Windows Store\ApiData\WindowsStoreProxy.xml. If the file isn't there you can copy one from the website.
Once you have build your windows store solution you can copy the file to the Assets folder (this is the Assets folder in the solution that is created when you do a build from Unity).
Then call WSANativeStore.ReloadSimulator when your app starts up to configure the simulated store.