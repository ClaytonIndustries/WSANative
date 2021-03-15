////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

#if ENABLE_WINMD_SUPPORT
using System.Linq;
using Windows.ApplicationModel.Contacts;
using CI.WSANative.Common;
#endif

namespace CI.WSANative.Pickers
{
    public class WSANativeContactPicker
    {
        /// <summary>
        /// Launches a picker which allows the user to choose a contact
        /// </summary>
        /// <param name="response">Contains the chosen contact or null if nothing was selected</param>
        public static void PickContact(Action<WSAContact> response)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(async () =>
            {
                ContactPicker contactPicker = new ContactPicker();

                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Address);

                Contact contact = await contactPicker.PickContactAsync();

                ThreadRunner.RunOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(contact != null ? MapContactToWSAContact(contact) : null);
                    }
                }, true);
            });
#endif
        }

        /// <summary>
        /// Launches a picker which allows the user to choose multiple contacts
        /// </summary>
        /// <param name="response">Contains the chosen contacts or null if nothing was selected</param>
        public static void PickContacts(Action<IEnumerable<WSAContact>> response)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(async () =>
            {
                ContactPicker contactPicker = new ContactPicker();

                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Address);

                IList<Contact> contacts = await contactPicker.PickContactsAsync();

                ThreadRunner.RunOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(contacts != null && contacts.Any() ? contacts.Select(x => MapContactToWSAContact(x)) : null);
                    }
                }, true);
            });
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private static WSAContact MapContactToWSAContact(Contact contact)
        {
            return new WSAContact()
            {
                DisplayName = contact.DisplayName,
                FullName = contact.FullName,
                FirstName = contact.FirstName,
                MiddleName = contact.MiddleName,
                LastName = contact.LastName,
                Nickname = contact.Nickname,
                Emails = contact.Emails.Select(x => x.Address).ToList(),
                Phones = contact.Phones.Select(x => x.Number).ToList(),
                OriginalContact = contact
            };
        }
#endif
    }
}