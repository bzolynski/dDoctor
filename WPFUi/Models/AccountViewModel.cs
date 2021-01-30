using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.ViewModels;

namespace WPFUi.Models
{
    public class AccountViewModel : ObservableObject
    {
        public readonly Account account;
        public int Id => account.Id;
        public string Email => account.Email;
        public string Username => account.Username;
        public AccountType AccountType => account.AccountType;
        public string FirstName
        {
            get
            {
                if (account.Doctor != null)
                    return account.Doctor.FirstName;
                else if (account.Registrant != null)
                    return account.Registrant.FirstName;
                else
                    return "admin";
            }
        }
        public string LastName
        {
            get
            {
                if (account.Doctor != null)
                    return account.Doctor.LastName;
                else if (account.Registrant != null)
                    return account.Registrant.LastName;
                else
                    return "admin";
            }
        }

        public AccountViewModel(Account account)
        {
            this.account = account;
        }
    }
}
