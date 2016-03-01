﻿using System.Collections.Generic;
using UnityEngine;
using WSANativeDialogs;
using WSANativeFileStorage;
using WSANativeIAPStore;
using WSANativeSerialisers;

public class ExampleSceneManagerController : MonoBehaviour
{
    public void Start()
    {
        WSANativeStore.EnableTestMode();

        WSANativeStore.ReloadSimulator();
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

    public void SaveFile()
    {
        string result = WSANativeSerialisation.SerialiseToXML(new Test());
        WSANativeStorage.SaveFile("Test.txt", result);
    }

    public void LoadFile()
    {
        if (WSANativeStorage.DoesFileExist("Test.txt"))
        {
            string result = WSANativeStorage.LoadFile("Test.txt");
            WSANativeSerialisation.DeserialiseXML<Test>(result);
        }
    }

    public void DeleteFile()
    {
        if (WSANativeStorage.DoesFileExist("Test.txt"))
        {
            WSANativeStorage.DeleteFile("Test.txt");
        }
    }

    public void PurchaseProduct()
    {
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
}

public class Test
{
    public int x = 10;
    public float y = 20.56f;
    public string s = "Hello World";
}