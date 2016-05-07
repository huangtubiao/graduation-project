using MyUniversity.Models;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IContactService
    {
        List<Contact> getContactsByUserId(long userId);

        bool addNewContact(Contact newContact);

        List<ContactModel> selectContactsModel(List<Contact> contacts);

        Contact getContactByFriendUserId(long userId, long friendUserId);

        ContactModel selectContactModel(User user);

        bool deleteContact(Contact contact);
    }
}
