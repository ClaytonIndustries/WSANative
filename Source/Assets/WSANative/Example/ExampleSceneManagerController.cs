using System.Collections.Generic;
using CI.WSANative.Advertising;
using CI.WSANative.Device;
using CI.WSANative.Dialogs;
using CI.WSANative.Facebook;
using CI.WSANative.FileStorage;
using CI.WSANative.Store;
using CI.WSANative.Mapping;
using CI.WSANative.Notification;
using CI.WSANative.Pickers;
using CI.WSANative.Security;
using CI.WSANative.Serialisers;
using CI.WSANative.Twitter;
using UnityEngine;
using CI.WSANative.Common;
using System.Linq;
using CI.WSANative.Web;
using System;

public class ExampleSceneManagerController : MonoBehaviour
{
    public void Awake()
    {
        // Call this once when your app starts up to configure the library
        WSANativeCore.Initialise();
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
        WSANativeStore.GetAddOns(products =>
        {
            if (products.Products != null && products.Products.Count > 0)
            {
                WSANativeStore.RequestPurchase(products.Products.Keys.First(), result =>
                {
                    if (result.Status == WSAStorePurchaseStatus.Succeeded)
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

    public void ShowFileOpenPicker()
    {
        WSANativeFilePicker.PickSingleFile("Select", WSAPickerViewMode.Thumbnail, WSAPickerLocationId.PicturesLibrary, new[] { ".png", ".jpg" }, result =>
        {
        });
    }

    public void ShowFileSavePicker()
    {
        WSANativeFilePicker.PickSaveFile("Save", ".txt", "Test Text File", WSAPickerLocationId.DocumentsLibrary, new List<KeyValuePair<string, IList<string>>>() { new KeyValuePair<string, IList<string>>("Text Files", new List<string>() { ".txt" }) }, result =>
        {
        });
    }

    public void ShowFolderPicker()
    {
        WSANativeFolderPicker.PickSingleFolder("Ok", WSAPickerViewMode.List, WSAPickerLocationId.DocumentsLibrary, null, result =>
        {
        });
    }

    public void ShowContactPicker()
    {
        WSANativeContactPicker.PickContact(result =>
        {
        });
    }

    public void CreateInterstitialAd()
    {
        WSANativeInterstitialAd.Initialise(WSAInterstitialAdType.Vungle, "Your app id");
        WSANativeInterstitialAd.AdReady += (adType, adUnitOrPlacementId) =>
        {
            if (adType == WSAInterstitialAdType.Vungle)
            {
                WSANativeInterstitialAd.ShowAd(WSAInterstitialAdType.Vungle, adUnitOrPlacementId);
            }
        };
        WSANativeInterstitialAd.RequestAd(WSAInterstitialAdType.Vungle, "Your ad unit or placement id");
    }

    public void CreateBannerAd()
    {
        WSANativeBannerAd.Initialise(WSABannerAdType.AdDuplex, "Your app id", "Your ad unit or placement id");
        WSANativeBannerAd.CreatAd(WSABannerAdType.AdDuplex, 728, 90, WSAVerticalPlacement.Top, WSAHorizontalPlacement.Centre);
    }

    public void LaunchMapsApp()
    {
        WSANativeMap.LaunchMapsApp("collection=point.40.726966_-74.006076_Some Business");
    }

    public void CreateMap()
    {
        WSANativeMap.CreateMap(new WSAMapSettings()
        {
            MapServiceToken = "",
            HorizontalPlacement = WSAHorizontalPlacement.Centre,
            VerticalPlacement = WSAVerticalPlacement.Centre,
            Centre = new WSAGeoPoint() { Latitude = 50, Longitude = 0 },
            ZoomLevel = 6,
            Height = 700,
            Width = 700,
            InteractionMode = WSAMapInteractionMode.GestureAndControl,
            OffsetX = 0,
            OffsetY = 0
        });
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

    public void CreateBrowser()
    {
        WSANativeWebView.Create(new WSAWebViewSettings()
        {
            HorizontalPlacement = WSAHorizontalPlacement.Centre,
            VerticalPlacement = WSAVerticalPlacement.Centre,
            Height = 700,
            Width = 700,
            Uri = new Uri("https://google.co.uk"),
            OffsetX = 0,
            OffsetY = 0
        });
    }

    public void DestroyBrowser()
    {
        WSANativeWebView.Destroy();
    }

    public void CreateSpinner()
    {
        WSANativeDevice.CreateProgressRing(new WSAProgressControlSettings()
        {
            HorizontalPlacement = WSAHorizontalPlacement.Centre,
            VerticalPlacement = WSAVerticalPlacement.Centre,
            Height = 45,
            Width = 45,
            Colour = new Color32(255, 20, 147, 255),
            OffsetX = 0,
            OffsetY = 0
        });
    }

    public void DestroySpinner()
    {
        WSANativeDevice.DestroyProgressRing();
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
        WSANativeFacebook.Initialise("facebookId");
        WSANativeFacebook.Login(null, result =>
        {
            if (result.Success)
            {
            }
        });
    }

    public void TwitterLogin()
    {
        WSANativeTwitter.Initialise("consumerKey", "consumerSecret", "https://www.twitter.com/");
        WSANativeTwitter.Login(true, result =>
        {
            if (result.Success)
            {
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