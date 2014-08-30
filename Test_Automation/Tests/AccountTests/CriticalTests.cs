using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Types;
using Xero.Api.Infrastructure.Exceptions;

namespace Test_Automation.Tests.AccountTests
{
    [TestFixture]
    public class CriticalTests
    {
        private AutomationAPI_Tester API_Tester;
        private string accountCode;
        private string accountName;
        private string accountBankNumber;
        private AccountType accountType;
        private XeroTestCoreApi api;
        private static Random rand = new Random(DateTime.Now.Millisecond);

        [TestFixtureSetUp]
        public void ResetScenario()
        {
            api = new XeroTestCoreApi();
            API_Tester = new AutomationAPI_Tester(api);
        }

        /// <summary>
        /// Positive scenario.
        /// Check valid account can be created.
        /// </summary>
        [Test]
        public void CreateSimpleAccount()
        {
            Account newAccount = null;

            Assert.DoesNotThrow(() =>
                    newAccount = CreateUniqueAccount()
                );

            Assert.IsTrue(newAccount.Id != Guid.Empty);
        }

        /// <summary>
        /// Positive scenario.
        /// Create an account, check account retrieval by bank account number.
        /// </summary>
        [Test]
        public void GetAccountByBankAccountNumber_PositiveScenario()
        {

            Account newAccount = null;

            CreateUniqueAccount();

            Assert.DoesNotThrow(() =>
                    newAccount = API_Tester.GetAccountByAccountBankNumber(accountBankNumber).First()
                 );

            Assert.AreEqual(newAccount.BankAccountNumber, accountBankNumber);
        }

        /// <summary>
        /// Negative scenario.
        /// Check that duplicated account cannot be created.
        /// </summary>
        [Test]
        public void CreateAccount_CreateDuplicateAccount_NegativeScenario()
        {
            Account firstAccount = CreateUniqueAccount();

            Assert.True(firstAccount.Id != Guid.Empty);

            Assert.Throws<ValidationException>(() => CreateAnAccount());

        }

        /// <summary>
        /// Negative scenario.
        /// Check that account cannot be created without name.
        /// </summary>
        [Test]
        public void CreateAccount_WithoutName_NegativeScenario()
        {
            accountCode = Guid.NewGuid().ToString().Substring(0, 6);
            accountType = AccountType.Bank;
            accountName = string.Empty;
            accountBankNumber = Guid.NewGuid().ToString().Substring(0, 12);

            Assert.Throws<ValidationException>(() => CreateAnAccount());
        }

        /// <summary>
        /// Negative scenario.
        /// Check that account cannot be created without account type.
        /// </summary>
        [Test]
        public void CreateAccount_WithoutType_NegativeScenario()
        {
            accountCode = Guid.NewGuid().ToString().Substring(0, 6);
            accountName = "Account - " + rand.Next().ToString();
            accountBankNumber = Guid.NewGuid().ToString().Substring(0, 12);
            accountType = AccountType.None;

            Assert.Throws<ValidationException>(() => CreateAnAccount());
        }

        /// <summary>
        /// Negative scenario.
        /// Check that account cannot be created without bank number.
        /// </summary>
        [Test]
        public void CreateAccount_WithoutBankNumber_NegativeScenario()
        {
            accountCode = Guid.NewGuid().ToString().Substring(0, 6);
            accountName = "Account - " + rand.Next().ToString();
            accountType = AccountType.Bank;
            accountBankNumber = string.Empty;

            Assert.Throws<ValidationException>(() => CreateAnAccount());
        }

        /// <summary>
        /// Create an account.
        /// </summary>
        /// <returns></returns>
        private Account CreateAnAccount()
        {
            return API_Tester.CreateAccount(accountCode, accountName, accountType, accountBankNumber);
        }

        /// <summary>
        /// Create a unique account.
        /// </summary>
        /// <returns></returns>
        private Account CreateUniqueAccount()
        {
            accountCode = Guid.NewGuid().ToString().Substring(0, 6);
            accountName = "Account - " + rand.Next().ToString();
            accountType = AccountType.Bank;
            accountBankNumber = Guid.NewGuid().ToString().Substring(0, 12);

            return API_Tester.CreateAccount(accountCode, accountName, accountType, accountBankNumber);
        }
    }
}
