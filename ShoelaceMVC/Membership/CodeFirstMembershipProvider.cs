using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using $safeprojectname$.Entities;

namespace $safeprojectname$.Membership
{
    public class CodeFirstMembershipProvider : CodeFirstExtendedProvider
            {

                private string _ApplicationName;
                public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
                {
                    if (config == null)
                    {
                        throw new ArgumentNullException("config");
                    }
                    if (string.IsNullOrEmpty(name))
                    {
                        name = "CodeFirstMembershipProvider";
                    }
                    if (string.IsNullOrEmpty(config["description"]))
                    {
                        config.Remove("description");
                        config.Add("description", "Code-First Membership Provider");
                    }

                    base.Initialize(name, config);

                    _ApplicationName = config["applicationName"];
                }

                #region "Main Functions"

                public override string CreateAccount(string userName, string password, string email, bool requireConfirmationToken)
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
                    }
                    string hashedPassword = CodeFirstCrypto.HashPassword(password);
                    if (hashedPassword.Length > 128)
                    {
                        throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
                    }
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw new MembershipCreateUserException(MembershipCreateStatus.InvalidUserName);
                    }
                    if (string.IsNullOrEmpty(email))
                    {
                        throw new MembershipCreateUserException(MembershipCreateStatus.InvalidEmail);
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        dynamic emailuser = context.Users.FirstOrDefault(Usr => Usr.Email == email);
                        if (user != null)
                        {
                            throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);
                        }
                        if (emailuser != null)
                        {
                            throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);
                        }
                        string token = null;
                        if (requireConfirmationToken)
                        {
                            token = CodeFirstCrypto.GenerateToken();
                        }
                        int defaultNumPasswordFailures = 0;
                        User NewUser = new User
                        {
                            UserId = Guid.NewGuid(),
                            Username = userName,
                            Password = hashedPassword,
                            IsConfirmed = !requireConfirmationToken,
                            Email = email,
                            ConfirmationToken = token,
                            CreateDate = DateTime.UtcNow,
                            PasswordChangedDate = DateTime.UtcNow,
                            PasswordFailuresSinceLastSuccess = defaultNumPasswordFailures,
                            LastPasswordFailureDate = DateTime.UtcNow
                        };

                        context.Users.Add(NewUser);
                        context.SaveChanges();
                        return token;
                    }
                }

                public override bool ConfirmAccount(string accountConfirmationToken)
                {
                    if (string.IsNullOrEmpty(accountConfirmationToken))
                    {
                        throw CreateArgumentNullOrEmptyException("accountConfirmationToken");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic row = context.Users.FirstOrDefault(Usr => Usr.ConfirmationToken == accountConfirmationToken);
                        if (row != null)
                        {
                            row.IsConfirmed = true;
                            context.SaveChanges();
                            return true;
                        }
                        return false;
                    }
                }

                public override bool ChangePassword(string userName, string oldPassword, string newPassword)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    if (string.IsNullOrEmpty(oldPassword))
                    {
                        throw CreateArgumentNullOrEmptyException("oldPassword");
                    }
                    if (string.IsNullOrEmpty(newPassword))
                    {
                        throw CreateArgumentNullOrEmptyException("newPassword");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            return false;
                        }
                        dynamic hashedPassword = user.Password;
                        bool verificationSucceeded = (hashedPassword != null && CodeFirstCrypto.VerifyHashedPassword(hashedPassword, oldPassword));
                        if (verificationSucceeded)
                        {
                            user.PasswordFailuresSinceLastSuccess = 0;
                        }
                        else
                        {
                            int failures = user.PasswordFailuresSinceLastSuccess;
                            if (failures != -1)
                            {
                                user.PasswordFailuresSinceLastSuccess += 1;
                                user.LastPasswordFailureDate = DateTime.UtcNow;
                            }
                            context.SaveChanges();
                            return false;
                        }
                        dynamic newhashedPassword = CodeFirstCrypto.HashPassword(newPassword);
                        if (newhashedPassword.Length > 128)
                        {
                            throw new ArgumentException("Password too long");
                        }
                        user.Password = newhashedPassword;
                        user.PasswordChangedDate = DateTime.UtcNow;
                        context.SaveChanges();
                        return true;
                    }
                }

                public override bool DeleteAccount(string userName)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            return false;
                        }
                        context.Users.Remove(user);
                        context.SaveChanges();
                        return true;
                    }
                }

                public override bool IsConfirmed(string userName)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            return false;
                        }
                        if (user.IsConfirmed)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                public override string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                        }
                        if (!user.IsConfirmed)
                        {
                            throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                        }
                        string token = null;
                        if (user.PasswordVerificationTokenExpirationDate > DateTime.UtcNow)
                        {
                            token = user.PasswordVerificationToken;
                        }
                        else
                        {
                            token = CodeFirstCrypto.GenerateToken();
                        }
                        user.PasswordVerificationToken = token;
                        user.PasswordVerificationTokenExpirationDate = DateTime.UtcNow.AddMinutes(tokenExpirationInMinutesFromNow);
                        context.SaveChanges();
                        return token;
                    }
                }

                public override bool ResetPasswordWithToken(string token, string newPassword)
                {
                    if (string.IsNullOrEmpty(newPassword))
                    {
                        throw CreateArgumentNullOrEmptyException("newPassword");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.PasswordVerificationToken == token && Usr.PasswordVerificationTokenExpirationDate > DateTime.UtcNow);
                        if (user != null)
                        {
                            dynamic newhashedPassword = CodeFirstCrypto.HashPassword(newPassword);
                            if (newhashedPassword.Length > 128)
                            {
                                throw new ArgumentException("Password too long");
                            }
                            user.Password = newhashedPassword;
                            user.PasswordChangedDate = DateTime.UtcNow;
                            user.PasswordVerificationToken = null;
                            user.PasswordVerificationTokenExpirationDate = null;
                            context.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                public override string ExtendedValidateUser(string userNameOrEmail, string password)
                {
                    if (string.IsNullOrEmpty(userNameOrEmail))
                    {
                        throw CreateArgumentNullOrEmptyException("userNameOrEmail");
                    }
                    if (string.IsNullOrEmpty(password))
                    {
                        throw CreateArgumentNullOrEmptyException("password");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        User user = null;
                        user = context.Users.FirstOrDefault(Usr => Usr.Username == userNameOrEmail);
                        if (user == null)
                        {
                            user = context.Users.FirstOrDefault(Usr => Usr.Email == userNameOrEmail);
                        }
                        if (user == null)
                        {
                            return string.Empty;
                        }
                        if (!user.IsConfirmed)
                        {
                            return string.Empty;
                        }
                        dynamic hashedPassword = user.Password;
                        bool verificationSucceeded = (hashedPassword != null && CodeFirstCrypto.VerifyHashedPassword(hashedPassword, password));
                        if (verificationSucceeded)
                        {
                            user.PasswordFailuresSinceLastSuccess = 0;
                        }
                        else
                        {
                            int failures = user.PasswordFailuresSinceLastSuccess;
                            if (failures != -1)
                            {
                                user.PasswordFailuresSinceLastSuccess += 1;
                                user.LastPasswordFailureDate = DateTime.UtcNow;
                            }
                        }
                        context.SaveChanges();
                        if (verificationSucceeded)
                        {
                            return user.Username;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }

                private ArgumentException CreateArgumentNullOrEmptyException(string paramName)
                {
                    return new ArgumentException(string.Format("Argument cannot be null or empty: {0}", paramName));
                }

                #endregion

                #region "Get Functions"

                public override System.DateTime GetPasswordChangedDate(string userName)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                        }
                        return user.PasswordChangedDate;
                    }
                }

                public override System.DateTime GetCreateDate(string userName)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                        }
                        return user.CreateDate;
                    }
                }

                public override int GetPasswordFailuresSinceLastSuccess(string userName)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                        }
                        return user.PasswordFailuresSinceLastSuccess;
                    }
                }

                public override System.Web.Security.MembershipUser GetUser(string userName, bool userIsOnline)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            return null;
                        }
                        return new MembershipUser(System.Web.Security.Membership.Provider.Name, userName, user.UserId, user.Email, null, null, true, false, user.CreateDate, DateTime.MinValue,
                        DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                    }
                }

                public override System.Guid GetUserIdFromPasswordResetToken(string token)
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        throw CreateArgumentNullOrEmptyException("token");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic result = context.Users.FirstOrDefault(Usr => Usr.PasswordVerificationToken == token);
                        if (result != null)
                        {
                            return result.UserId;
                        }
                        return Guid.Empty;
                    }
                }

                public override System.DateTime GetLastPasswordFailureDate(string userName)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    using ($safeprojectname$Context context = new $safeprojectname$Context())
                    {
                        dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                        }
                        return user.LastPasswordFailureDate;
                    }
                }

                #endregion

                #region "Properties"
                public override string ApplicationName
                {
                    get { return _ApplicationName; }
                    set { _ApplicationName = value; }
                }
                public override bool EnablePasswordReset
                {
                    get { return false; }
                }
                public override bool EnablePasswordRetrieval
                {
                    get { return false; }
                }
                public override int MaxInvalidPasswordAttempts
                {
                    get { return int.MaxValue; }
                }
                public override int MinRequiredNonAlphanumericCharacters
                {
                    get { return 0; }
                }
                public override int MinRequiredPasswordLength
                {
                    get { return 0; }
                }
                public override int PasswordAttemptWindow
                {
                    get { return int.MaxValue; }
                }
                public override System.Web.Security.MembershipPasswordFormat PasswordFormat
                {
                    get { return MembershipPasswordFormat.Hashed; }
                }
                public override string PasswordStrengthRegularExpression
                {
                    get { return string.Empty; }
                }
                public override bool RequiresQuestionAndAnswer
                {
                    get { return false; }
                }
                public override bool RequiresUniqueEmail
                {
                    get { return true; }
                }
                #endregion

                #region "Not Needed"
                public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
                {
                    throw new NotSupportedException();
                }

                public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
                {
                    throw new NotSupportedException();
                }

                public override bool DeleteUser(string username, bool deleteAllRelatedData)
                {
                    throw new NotImplementedException();
                }

                public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
                {
                    throw new NotSupportedException();
                }

                public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
                {
                    throw new NotSupportedException();
                }

                public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
                {
                    throw new NotSupportedException();
                }

                public override int GetNumberOfUsersOnline()
                {
                    throw new NotSupportedException();
                }

                public override string GetPassword(string username, string answer)
                {
                    throw new NotSupportedException();
                }

                public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
                {
                    throw new NotSupportedException();
                }

                public override string GetUserNameByEmail(string email)
                {
                    throw new NotSupportedException();
                }

                public override string ResetPassword(string username, string answer)
                {
                    throw new NotSupportedException();
                }

                public override bool UnlockUser(string userName)
                {
                    throw new NotSupportedException();
                }

                public override void UpdateUser(MembershipUser user)
                {
                    throw new NotSupportedException();
                }

                public override bool ValidateUser(string username, string password)
                {
                    throw new NotSupportedException();
                }
                #endregion


            }
}