using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Runtime.CompilerServices;
using System.Net;

namespace ShoelaceMVC.Membership
{
    public static class CodeFirstSecurity
    {

        public static Guid CurrentUserId
        {
            get { return GetUserId(CurrentUserName); }
        }

        public static string CurrentUserName
        {
            get { return Context.User.Identity.Name; }
        }

        public static bool HasUserId
        {
            get { return !(CurrentUserId == Guid.Empty); }
        }

        public static bool IsAuthenticated
        {
            get { return Request.IsAuthenticated; }
        }

        private static HttpContextBase Context
        {
            get { return new HttpContextWrapper(HttpContext.Current); }
        }

        private static HttpRequestBase Request
        {
            get { return Context.Request; }
        }

        private static HttpResponseBase Response
        {
            get { return Context.Response; }
        }

        private static CodeFirstExtendedProvider VerifyProvider()
        {
            CodeFirstExtendedProvider provider = System.Web.Security.Membership.Provider as CodeFirstExtendedProvider;
            if (provider == null)
            {
                throw new InvalidOperationException("Provider Is Not ExtendedMembershipProvider");
            }
            return provider;
        }

        public static bool Login(string userNameOrEmail, string password, bool persistCookie = false)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            dynamic success = provider.ExtendedValidateUser(userNameOrEmail, password);
            if (!(string.IsNullOrEmpty(success)))
            {
                FormsAuthentication.SetAuthCookie(success, persistCookie);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Logout()
        {
            VerifyProvider();
            FormsAuthentication.SignOut();
        }

        public static bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            VerifyProvider();
            bool success = false;
            try
            {
                dynamic currentUser = System.Web.Security.Membership.GetUser(userName, true);
                success = currentUser.ChangePassword(currentPassword, newPassword);
            }
            catch (ArgumentException generatedExceptionName)
            {
               
            }
            return success;
        }

        public static bool ConfirmAccount(string accountConfirmationToken)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.ConfirmAccount(accountConfirmationToken);
        }

        public static string CreateAccount(string userName, string password, string email, bool requireConfirmationToken = false)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.CreateAccount(userName, password, email, requireConfirmationToken);
        }

        public static string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.GeneratePasswordResetToken(userName, tokenExpirationInMinutesFromNow);
        }

        public static bool UserExists(string userName)
        {
            VerifyProvider();
            return System.Web.Security.Membership.GetUser(userName) != null;
        }

        public static Guid GetUserId(string userName)
        {
            VerifyProvider();
            MembershipUser user = System.Web.Security.Membership.GetUser(userName);
            if (user == null)
            {
                return Guid.Empty;
            }
            return Guid.Parse(user.ProviderUserKey.ToString());
        }

        public static Guid GetUserIdFromPasswordResetToken(string token)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.GetUserIdFromPasswordResetToken(token);
        }

        public static bool IsCurrentUser(string userName)
        {
            VerifyProvider();
            return string.Equals(CurrentUserName, userName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsConfirmed(string userName)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.IsConfirmed(userName);
        }

        private static bool IsUserLoggedOn(Guid userId)
        {
            VerifyProvider();
            return CurrentUserId == userId;
        }

        public static void RequireAuthenticatedUser()
        {
            VerifyProvider();
            dynamic user = Context.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                Response.SetStatus(HttpStatusCode.Unauthorized);
            }
        }

        public static void RequireUser(Guid userId)
        {
            VerifyProvider();
            if (!IsUserLoggedOn(userId))
            {
                Response.SetStatus(HttpStatusCode.Unauthorized);
            }
        }

        public static void RequireUser(string userName)
        {
            VerifyProvider();
            if (!string.Equals(CurrentUserName, userName, StringComparison.OrdinalIgnoreCase))
            {
                Response.SetStatus(HttpStatusCode.Unauthorized);
            }
        }

        public static void RequireRoles(params string[] ArrayOfRoles)
        {
            VerifyProvider();
            foreach (string role in ArrayOfRoles)
            {
                if (!Roles.IsUserInRole(CurrentUserName, role))
                {
                    Response.SetStatus(HttpStatusCode.Unauthorized);
                    return;
                }
            }
        }

        public static bool ResetPassword(string passwordResetToken, string newPassword)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.ResetPasswordWithToken(passwordResetToken, newPassword);
        }

        public static bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, int intervalInSeconds)
        {
            VerifyProvider();
            return IsAccountLockedOut(userName, allowedPasswordAttempts, TimeSpan.FromSeconds(intervalInSeconds));
        }

        public static bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, TimeSpan interval)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return IsAccountLockedOutInternal(provider, userName, allowedPasswordAttempts, interval);
        }

        private static bool IsAccountLockedOutInternal(CodeFirstExtendedProvider provider, string userName, int allowedPasswordAttempts, TimeSpan interval)
        {
            return (provider.GetUser(userName, false) != null && provider.GetPasswordFailuresSinceLastSuccess(userName) > allowedPasswordAttempts && provider.GetLastPasswordFailureDate(userName).Add(interval) > DateTime.UtcNow);
        }

        public static int GetPasswordFailuresSinceLastSuccess(string userName)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.GetPasswordFailuresSinceLastSuccess(userName);
        }

        public static DateTime GetCreateDate(string userName)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.GetCreateDate(userName);
        }

        public static DateTime GetPasswordChangedDate(string userName)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.GetPasswordChangedDate(userName);
        }

        public static DateTime GetLastPasswordFailureDate(string userName)
        {
            CodeFirstExtendedProvider provider = VerifyProvider();
            return provider.GetLastPasswordFailureDate(userName);
        }

        public static void SetStatus(this HttpResponseBase response, HttpStatusCode httpStatusCode)
        {
            SetStatus(response, (int)httpStatusCode);
        }

        public static void SetStatus(this HttpResponseBase response, int httpStatusCode)
        {
            response.StatusCode = httpStatusCode;
            response.End();
        }
        public static bool IsEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}