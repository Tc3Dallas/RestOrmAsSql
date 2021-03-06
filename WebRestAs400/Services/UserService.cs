using NLog;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using WebRestAs400.Models;
using AsOrm3;

namespace WebRestAs400.Services
{
    public class WebAccessoriUserService : BaseService
    {
        public bool Validation(string user, string password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain,"zegna.com"))
            {
                var ret = pc.ValidateCredentials(user, password);
                return ret;
            }
        }

        public WebAccessoriUser GetAccessoriUserByADUser(string ADUser)
        {
            var Db = new DbContext();
            var ritorno = Db.ExecuteReader<WebAccessoriUser>($"GRUTAD = '{ADUser}'");
            if (ritorno.Count == 1)
                return ritorno.FirstOrDefault();
            else
            {
                if (ritorno.Count > 1)
                {
                    Logger.Error($"Trovate troppe occorrenze per l'utente {ADUser}");
                }
                else
                {
                    Logger.Error($"Nessuna occorrenza trovata per l'utente {ADUser}");
                }

                return null;
            }
        }

        public bool SetUltimoAccesso(WebAccessoriUser accessoriUser)
        {
            var Db = new DbContext();
            var now = DateTime.Now;
            accessoriUser.GRDULI = now.ToIntDate();
            accessoriUser.GROULI = now.ToIntTime();
            var ret = Db.SaveChange<WebAccessoriUser>(accessoriUser);
            if (ret == 1)
                return true;
            else
                return false;
        }

        //TODO da provare
        public bool SetPassword(string userName, string oldPassword, string newpassword)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
                {
                    user.ChangePassword(oldPassword, newpassword);
                    user.Save();
                }
            }
            return Validation(userName, newpassword);
        }
    }
}