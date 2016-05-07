using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Models.Repositories.Interface;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services
{
    public class ContactService : IContactService
    {
        public IContactRepository _contactRepository { get; private set; }
        private loginUser loginUser { get; set; }

        public ContactService(IContactRepository contactRepository)
        {
            this._contactRepository = contactRepository;
            loginUser = Models.Help.loginUser.getLoginUser();
        }

        #region 条件检索
        public List<Contact> getContactsByUserId(long userId)
        {
            return _contactRepository.Get(o => o.contactUserId == userId || o.contactFriendId == userId).OrderByDescending(o => o.contactTime).ToList();
        }

        public Contact getContactByFriendUserId(long userId, long friendUserId)
        {
            return _contactRepository.Get(o => o.contactUserId == userId && o.contactFriendId == friendUserId ||
                o.contactUserId == friendUserId && o.contactFriendId == userId).FirstOrDefault();
        }
        #endregion

        #region 添加新的联系人
        public bool addNewContact(Contact newContact)
        {
            try
            {
                _contactRepository.Add(newContact);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        #region 实现算法
        public List<ContactModel> selectContactsModel(List<Contact> contacts)
        {
            List<ContactModel> contactModel = new List<ContactModel>();
            foreach (var c in contacts)
            {
                if (c.contactUserId == loginUser.userId)
                {
                    contactModel.Add(new ContactModel
                    {
                        contactFriendId = c.contactFriendId,
                        userImg = c.User.userImg,
                        userName = c.User.userName
                    });
                }
                else
                {
                    contactModel.Add(new ContactModel
                    {
                        contactFriendId = c.contactUserId,
                        userImg = c.User1.userImg,
                        userName = c.User1.userName
                    });
                }
            }
            return contactModel;
        }

        public ContactModel selectContactModel(User user)
        {
            ContactModel contactModel = new ContactModel();
            contactModel.contactFriendId = user.userId;
            contactModel.userImg = user.userImg;
            contactModel.userName = user.userName;
            return contactModel;
        }
        #endregion

        #region 删除联系人
        public bool deleteContact(Contact contact)
        {
            try
            {
                _contactRepository.Delete(contact);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion
    }
}
