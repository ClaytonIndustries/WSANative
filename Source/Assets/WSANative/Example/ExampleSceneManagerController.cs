using System.Collections.Generic;
using CI.WSANative.Advertising;
using CI.WSANative.Device;
using CI.WSANative.Dialogs;
using CI.WSANative.Facebook;
using CI.WSANative.FileStorage;
using CI.WSANative.IAPStore;
using CI.WSANative.Mapping;
using CI.WSANative.Notification;
using CI.WSANative.Pickers;
using CI.WSANative.Security;
using CI.WSANative.Serialisers;
using CI.WSANative.Twitter;
using UnityEngine;

public class ExampleSceneManagerController : MonoBehaviour
{
    public void Start()
    {
        // Uncomment these lines when testing in app purchases 
        // ReloadSimulator will throw an exception if it is not correctly configured - see website for details

        // A new store implementation is now available - see the online docs for details

        //WSANativeStore.EnableTestMode();

        //WSANativeStore.ReloadSimulator();
    }

    public void CreateDialog()
    {
        WSANativeDialog.ShowDialogWithOptions("This is a title", "This is a message", new List<WSADialogCommand>() { new WSADialogCommand("Yes"), new WSADialogCommand("No"), new WSADialogCommand("Cancel") }, 0, 2, (WSADialogResult result) =>
        {
            if (result.ButtonPressed == "Yes")
            {
                WSANativeDialog.ShowDialog("Yes Pressed", "Yes was pressed!");
            }
            else if (result.ButtonPressed == "No")
            {
                WSANativeDialog.ShowDialog("No Pressed", "No was pressed!");
            }
            else if (result.ButtonPressed == "Cancel")
            {
                WSANativeDialog.ShowDialog("Cancel Pressed", "Cancel was pressed!");
            }
        });
    }

    public void CreatePopupMenu()
    {
        WSANativePopupMenu.ShowPopupMenu(Screen.width / 2, Screen.height / 2, new List<WSADialogCommand>() { new WSADialogCommand("Option 1"), new WSADialogCommand("Option 2"), new WSADialogCommand("Option 3"), new WSADialogCommand("Option 4"), new WSADialogCommand("Option 5"), new WSADialogCommand("Option 6") }, WSAPopupMenuPlacement.Above, (WSADialogResult result) =>
        {
            if (result.ButtonPressed == "Yes")
            {
                WSANativeDialog.ShowDialog("Yes Pressed", "Yes was pressed!");
            }
            else if (result.ButtonPressed == "No")
            {
                WSANativeDialog.ShowDialog("No Pressed", "No was pressed!");
            }
            else if (result.ButtonPressed == "Cancel")
            {
                WSANativeDialog.ShowDialog("Cancel Pressed", "Cancel was pressed!");
            }
        });
    }

    public void CreateToastNotification()
    {
        WSANativeNotification.ShowToastNotification("This is a title", "This is a description, This is a description, This is a description", null);
    }

    public void SaveFile()
    {
        var file = WSANativeStorageLibrary.CreateFile(WSAStorageLibrary.Local, "Test.txt");

        string result = WSANativeSerialisation.SerialiseToXML(new Test());

        file.WriteText(result);
    }

    public void LoadFile()
    {
        if (WSANativeStorageLibrary.DoesFileExist(WSAStorageLibrary.Local, "Test.txt"))
        {
            WSANativeStorageLibrary.GetFile(WSAStorageLibrary.Local, "Test.txt", result =>
            {
                WSANativeSerialisation.DeserialiseXML<Test>(result.ReadText());
            });
        }
    }

    public void DeleteFile()
    {
        if (WSANativeStorageLibrary.DoesFileExist(WSAStorageLibrary.Local, "Test.txt"))
        {
            WSANativeStorageLibrary.DeleteFile(WSAStorageLibrary.Local, "Test.txt");
        }
    }

    public void PurchaseProduct()
    {
        // A new store implementation is now available - see the online docs for details

        WSANativeStore.GetProductListings((List<WSAProduct> products) =>
        {
            if (products != null && products.Count > 0)
            {
                WSANativeStore.PurchaseProduct(products[0].Id, (WSAPurchaseResult result) =>
                {
                    if (result.Status == WSAPurchaseResultStatus.Succeeded)
                    {
                        WSANativeDialog.ShowDialog("Purchased", "YAY");
                    }
                    else
                    {
                        WSANativeDialog.ShowDialog("Not Purchased", "NAY");
                    }
                });
            }
        });
    }

    public void PurchaseApp()
    {
        // A new store implementation is now available - see the online docs for details

        WSANativeStore.PurchaseApp((string response) =>
        {
            if (!string.IsNullOrEmpty(response))
            {
                WSANativeDialog.ShowDialog("Purchased", response);
            }
            else
            {
                WSANativeDialog.ShowDialog("Not Purchased", "NAY");
            }
        });
    }

    public void ShowFileOpenPicker()
    {
        WSANativeFilePicker.PickSingleFile("Select", WSAPickerViewMode.Thumbnail, WSAPickerLocationId.PicturesLibrary, new[] { ".png", ".jpg" }, result =>
        {
            if (result != null)
            {
#pragma warning disable 0219
                byte[] fileBytes = result.ReadBytes();
                string fileString = result.ReadText();
#pragma warning restore 0219
            }
        });
    }

    public void ShowFileSavePicker()
    {
        WSANativeFilePicker.PickSaveFile("Save", ".txt", "Test Text File", WSAPickerLocationId.DocumentsLibrary, new List<KeyValuePair<string, IList<string>>>() { new KeyValuePair<string, IList<string>>("Text Files", new List<string>() { ".txt" }) }, result =>
        {
            if (result != null)
            {
                result.WriteBytes(new byte[2]);
                result.WriteText("Hello World");
            }
        });
    }

    public void ShowFolderPicker()
    {
        WSANativeFolderPicker.PickSingleFolder("Ok", WSAPickerViewMode.List, WSAPickerLocationId.DocumentsLibrary, null, result =>
        {
        });
    }

    public void CreateInterstitialAd()
    {
        WSANativeInterstitialAd.Initialise(WSAInterstitialAdType.Microsoft, "d25517cb-12d4-4699-8bdc-52040c712cab", "11389925");
        WSANativeInterstitialAd.AdReady += (adType) =>
        {
            if (adType == WSAInterstitialAdType.Microsoft)
            {
                WSANativeInterstitialAd.ShowAd(WSAInterstitialAdType.Microsoft);
            }
        };
        WSANativeInterstitialAd.RequestAd(WSAInterstitialAdType.Microsoft, WSAInterstitialAdVariant.Video);
    }

    public void CreateBannerAd()
    {
        WSANativeBannerAd.Initialise(WSABannerAdType.Microsoft, "3f83fe91-d6be-434d-a0ae-7351c5a997f1", "10865270");
        WSANativeBannerAd.CreatAd(WSABannerAdType.Microsoft, 728, 90, WSAAdVerticalPlacement.Top, WSAAdHorizontalPlacement.Centre);
    }

    public void LaunchMapsApp()
    {
        WSANativeMap.LaunchMapsApp("collection=point.40.726966_-74.006076_Some Business");
    }

    public void CreateMap()
    {
        int xPos = (Screen.width / 2) - 350;
        int yPos = (Screen.height / 2) - 350;

        WSANativeMap.CreateMap(string.Empty, 700, 700, new WSAPosition() { X = xPos, Y = yPos }, new WSAGeoPoint() { Latitude = 50, Longitude = 0 }, 6, WSAMapInteractionMode.GestureAndControl);
    }

    public void DestroyMap()
    {
        WSANativeMap.DestroyMap();
    }

    public void AddPOI()
    {
        WSANativeMap.AddMapElement("You are here", new WSAGeoPoint() { Latitude = 52, Longitude = 5 });
    }

    public void ClearMap()
    {
        WSANativeMap.ClearMap();
    }

    public void CenterMap()
    {
        WSANativeMap.CenterMap(new WSAGeoPoint() { Latitude = 52, Longitude = 5 });
    }

    public void EnableFlashlight()
    {
        WSANativeDevice.EnableFlashlight(new WSANativeColour() { Red = 0, Green = 0, Blue = 255 });
    }

    public void DisableFlashlight()
    {
        WSANativeDevice.DisableFlashlight();
    }

    public void CameraCapture()
    {
        WSANativeDevice.CapturePicture(512, 512, result =>
        {
        });
    }

    public void EncryptDecrypt()
    {
        string encrypted = WSANativeSecurity.SymmetricEncrypt("ffffffffffffffffffffffffffffffff", "aaaaaaaaaaaaaaaa", "Tesing123");

        WSANativeSecurity.SymmetricDecrypt("ffffffffffffffffffffffffffffffff", "aaaaaaaaaaaaaaaa", encrypted);
    }

    public void FacebookLogin()
    {
        WSANativeFacebook.Initialise("facebookId", "packageSID");
        WSANativeFacebook.Login(new List<string>() { "public_profile", "email", "user_birthday" }, result =>
        {
            if (result.Success)
            {
                WSANativeFacebook.GetUserDetails(response =>
                {
                    if (response.Success)
                    {
#pragma warning disable 0219
                        WSAFacebookUser user = response.Data;
#pragma warning restore 0219
                    }
                });
            }
        });
    }

    public void TwitterLogin()
    {
        WSANativeTwitter.Initialise("consumerKey", "consumerSecret", "https://www.twitter.com");
        WSANativeTwitter.Login(result =>
        {
            if (result.Success)
            {
                WSANativeTwitter.GetUserDetails(true, response =>
                {
                    if (response.Success)
                    {
#pragma warning disable 0219
                        string user = response.Data;
#pragma warning restore 0219
                    }
                });
            }
        });
    }
}

public class Test
{
    public int x = 10;
    public float y = 20.56f;
    public string s = "Hello World";
}