using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    public interface IAccountService
    {
        void ChangePassword(UserToChangePassword user);

        void ChangeEmail(UserToChangeEmail user);

        void DeleteAccount(UserToDelete user);

        bool DoesUserExist(string username);
    }
}
