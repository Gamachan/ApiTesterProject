using Xero.Api.Core;
using System.Collections.Generic;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Types;
using Xero.Api.Core.Model.Status;


namespace Test_Automation
{
    internal class AutomationAPI_Tester
    {
        private readonly XeroCoreApi _api;


        public AutomationAPI_Tester(XeroCoreApi api)
        {
            _api = api;
        }


        #region Invoices Methods

        /// <summary>
        /// Get all organization employees.
        /// </summary>
        /// <returns>Collection of all the employees in the organization.</returns>
        public IEnumerable<Invoice> GetAllInvoice()
        {
            return _api.Invoices.Find();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvoiceNumber"></param>
        /// <returns></returns>
        public IEnumerable<Invoice> GetInvoiceByInvoiceNumber(string invoiceNumber)
        {
            return _api.Invoices.Where("invoiceNumber == \"" + invoiceNumber + "\"").Find();
        }

        /// <summary>
        /// Create a new invoice in the organization.
        /// </summary>
        /// <param name="newInvoice">First name.</param>
        /// <returns>A new employee with an unique ID.</returns>
        public Invoice SubmitAnInvoice(Invoice newInvoice)
        {
            return _api.Create(newInvoice);
        }

        #endregion

        #region Employee Methods

        /// <summary>
        /// Get all organization employees.
        /// </summary>
        /// <returns>Collection of all the employees in the organization.</returns>
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _api.Employees.Find();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetEmployeesByFirstName(string firstName)
        {
            return _api.Employees.Where("FirstName == \"" + firstName + "\"").Find();
        }

        /// <summary>
        /// Create a new unique employee in the organization.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <returns>A new employee with an unique ID.</returns>
        public Employee CreateEmployee(string firstName, string lastName)
        {
            var emp = _api.Create(new Employee
            {
                FirstName = firstName,
                LastName = lastName,
            });

            return emp;
        }

        /// <summary>
        /// Update an existing employee in the organization.
        /// </summary>
        /// <param name="employee">Employee to be updated.</param>
        /// <returns>An updated employee.</returns>
        public Employee UpdateEmployee(Employee employee)
        {
            return _api.Update(employee);
        }

        #endregion

        #region Account Methods

        /// <summary>
        /// Get all the organization accounts.
        /// </summary>
        /// <returns>Collection of all the accounts associated to the organization.</returns>
        public IEnumerable<Account> GetAllAccounts()
        {
            return _api.Accounts.Find();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IEnumerable<Account> GetAccountByAccountBankNumber(string accountBankNumber)
        {
            return _api.Accounts.Where("BankAccountNumber == \"" + accountBankNumber + "\"").Find();
        }

        /// <summary>
        /// Create a new unique account for the organization.
        /// </summary>
        /// <param name="code">Account code.</param>
        /// <param name="Name">Account name.</param>
        /// <param name="accountType">Account type.</param>
        /// <param name="bankAccountNumber">Account bank number.</param>
        /// <returns>A new account with an unique ID.</returns>
        public Account CreateAccount(string code, string name, AccountType accountType, string bankAccountNumber)
        {
            var account = _api.Create(new Account
            {
                Code = code,
                Name = name,
                Type = accountType,
                BankAccountNumber = bankAccountNumber
            });

            return account;
        }

        #endregion

    }
}
