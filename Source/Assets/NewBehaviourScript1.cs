using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WSANativeDialogs;
using WSANativeIAPStore;
using WSANativeFileStorage;

public class NewBehaviourScript1 : MonoBehaviour
{
    public Button button;

    public void Start()
    {
        WSANativeStore.EnableTestMode();

        WSANativeStore.ReloadSimulator();
    }

    public void Clicked()
    {
        //WSANativeDialog.ShowDialogWithOptions("This is a title", "This is a message", new List<WSADialogCommand>() { new WSADialogCommand("Yes"), new WSADialogCommand("No"), new WSADialogCommand("Cancel") }, 0, 2, (WSADialogResult result) =>
        //{
        //    if (result.ButtonPressed == "Yes")
        //    {
        //        WSANativeDialog.ShowDialog("Yes Pressed", "Yes was pressed!");
        //    }
        //    else if (result.ButtonPressed == "No")
        //    {
        //        WSANativeDialog.ShowDialog("No Pressed", "No was pressed!");
        //    }
        //    else if (result.ButtonPressed == "Cancel")
        //    {
        //        WSANativeDialog.ShowDialog("Cancel Pressed", "Cancel was pressed!");
        //    }
        //});

        //WSANativePopupMenu.ShowPopupMenu(Screen.width / 2, Screen.height / 2, new List<WSADialogCommand>() { new WSADialogCommand("Option 1"), new WSADialogCommand("Option 2"), new WSADialogCommand("Option 3"), new WSADialogCommand("Option 4"), new WSADialogCommand("Option 5"), new WSADialogCommand("Option 6") }, WSAPopupMenuPlacement.Above, (WSADialogResult result) =>
        //{
        //    if (result.ButtonPressed == "Yes")
        //    {
        //        WSANativeDialog.ShowDialog("Yes Pressed", "Yes was pressed!");
        //    }
        //    else if (result.ButtonPressed == "No")
        //    {
        //        WSANativeDialog.ShowDialog("No Pressed", "No was pressed!");
        //    }
        //    else if (result.ButtonPressed == "Cancel")
        //    {
        //        WSANativeDialog.ShowDialog("Cancel Pressed", "Cancel was pressed!");
        //    }
        //});

        //WSANativePopupMenu.ShowPopupMenu(0, 0, new List<WSADialogCommand>() { new WSADialogCommand("Button 1"), new WSADialogCommand("Button 2"), new WSADialogCommand("Button 3") }, WSAPopupMenuPlacement.Above, (WSADialogResult result) =>
        //{
        //});

        //WSAProductLicense license1 = WSANativeStore.GetLicenseForApp();
        //WSAProductLicense license2 = WSANativeStore.GetLicenseForProduct("1");

        //WSANativeStore.ReportConsumableProductFulfillment("2", new System.Guid("00000000-0000-0000-0000-000000000000"), (response) =>
        //{
        //    WSANativeStore.GetUnfulfilledConsumableProducts((response2) =>
        //    {
        //    });
        //});

        //WSANativeStore.GetUnfulfilledConsumableProducts((response) =>
        //{
        //});

        //WSAProductLicense license = WSANativeStore.GetLicenseForProduct("1");

        //WSANativeStore.GetProductListings((List<WSAProduct> products) =>
        //{
        //    WSANativeStore.PurchaseProduct(products[0].Id, (WSAPurchaseResult result) =>
        //    {
        //        if (result.Status == WSAPurchaseResultStatus.Succeeded)
        //        {
        //            WSANativeDialog.ShowDialog("Title", "YAY");
        //        }
        //        else
        //        {
        //            WSANativeDialog.ShowDialog("Title", "NAY");
        //        }
        //    });
        //});

        //WSANativeStorage.SaveFile("Test.txt", "Hello World");
        //WSANativeStorage.SaveFile("Test2.txt", "Hello World");
        //WSANativeStorage.SaveFile("Test3.txt", "Hello World");

        //if (WSANativeStorage.DoesFileExist("Test.txt"))
        //{
        //    List<string> files = WSANativeStorage.GetAllFiles();

        //    WSANativeStorage.DeleteFile("Test.txt");

        //    WSANativeDialog.ShowDialog("Title", string.Join(";", files));

        //    try
        //    {
        //        string result = WSANativeStorage.LoadFile("Test.txt");
        //    }
        //    catch
        //    {
        //    }
        //}

        string result = WSANativeSerialisers.WSANativeSerialisation.SerialiseToXML(new Test());
        Test test = WSANativeSerialisers.WSANativeSerialisation.DeserialiseXML<Test>(result);
    }
}

public class Test
{
    public int x = 10;
    public float y = 20.56f;
    public string s = "Hello World";
}