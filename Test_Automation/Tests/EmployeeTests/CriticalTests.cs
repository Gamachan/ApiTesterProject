using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Xero.Api.Core.Model;
using Xero.Api.Infrastructure.Exceptions;

namespace Test_Automation.Tests.EmployeeTests
{
    [TestFixture]
    public class CriticalTests
    {
        private AutomationAPI_Tester API_Tester;
        private string _firstName;
        private string _lastName;

        [TestFixtureSetUp]
        public void ResetScenario()
        {
            XeroTestCoreApi api = new XeroTestCoreApi();
            API_Tester = new AutomationAPI_Tester(api);
        }

        /// <summary>
        /// Positive scenario.
        /// Check employee can be created.
        /// </summary>
        [Test]
        public void CreateEmployee()
        {
            Employee newEmployee = null;

            Assert.DoesNotThrow(() =>
                    newEmployee = CreateUniqueEmployee()
                );

            if (newEmployee != null)
            {
                Assert.IsTrue(newEmployee.Id != Guid.Empty);
            }
        }

        /// <summary>
        /// Positive scenario.
        /// Get existing employee.
        /// </summary>
        [Test]
        public void Get_Existing_Employee_PositiveScenario()
        {
            Employee employee = null;

            CreateUniqueEmployee();

            Assert.DoesNotThrow(() =>
                   employee = API_Tester.GetEmployeesByFirstName(_firstName).First()
                );

            if (employee != null)
            {
                Assert.AreEqual(employee.FirstName, _firstName);
            }
        }

        /// <summary>
        /// Negative scenario.
        /// Check that duplicated employee cannot be created. 
        /// </summary>
        [Test]
        public void CreateEmployee_CreateDuplicatedEmployee_NegativeScenario()
        {
            Employee firstEmployee = CreateUniqueEmployee();

            Assert.True(firstEmployee.Id != Guid.Empty);

            Assert.Throws<ValidationException>(() => CreateSpecifiedEmployee(_firstName, _lastName));

        }

        /// <summary>
        /// Negative scenario.
        /// Check that employee without first name cannot be created. 
        /// </summary>
        [Test]
        public void CreateEmployee_WithoutFirstName_NegativeScenario()
        {
            string firstName = Guid.NewGuid().ToString().Substring(0, 10);
            Assert.Throws<ValidationException>(() => CreateSpecifiedEmployee(firstName, string.Empty));
        }

        /// <summary>
        /// Negative scenario.
        /// Check that employee without last name cannot be created. 
        /// </summary>
        [Test]
        public void CreateEmployee_WithoutLastName_NegativeScenario()
        {
            string lastName = Guid.NewGuid().ToString().Substring(0, 10);
            Assert.Throws<ValidationException>(() => CreateSpecifiedEmployee(string.Empty, lastName));
        }

        /// <summary>
        /// Positive scenario.
        /// Get a random employee change first and last name and attempt to update.
        /// Retrieve the employee by first name, check the return employee is the original employee.
        /// </summary>
        [Test]
        public void UpdateEmployee_PositiveScenario()
        {
            Employee employee = API_Tester.GetAllEmployees().First();

            _firstName = Guid.NewGuid().ToString().Substring(0, 10);
            _lastName = Guid.NewGuid().ToString().Substring(0, 10);

            employee.FirstName = _firstName;
            employee.LastName = _lastName;

            employee = API_Tester.UpdateEmployee(employee);

            Employee updatedEmployee = null;

            Assert.DoesNotThrow(() =>
                 updatedEmployee = API_Tester.GetEmployeesByFirstName(_firstName).First()
                    );

            if (updatedEmployee == null)
            {
                Assert.Fail();
                return;
            }

            Assert.AreEqual(employee.Id, updatedEmployee.Id);
        }

        /// <summary>
        /// Negative scenario.
        /// Get a random employee change last name to an empty string and attempt to update.
        /// </summary>
        [Test]
        public void UpdateEmployee_UpdateEmptyMandatoryProperties_NegativeScenario()
        {
            Employee employee = API_Tester.GetAllEmployees().First();

            _firstName = Guid.NewGuid().ToString().Substring(0, 10);
            _lastName = string.Empty;

            employee.FirstName = _firstName;
            employee.LastName = _lastName;

            Assert.Throws<ValidationException>(() =>
                    API_Tester.UpdateEmployee(employee));
        }

        /// <summary>
        /// Create unique employee.
        /// </summary>
        /// <returns></returns>
        private Employee CreateUniqueEmployee()
        {
            _firstName = Guid.NewGuid().ToString().Substring(0, 10);
            _lastName = Guid.NewGuid().ToString().Substring(0, 10);

            return API_Tester.CreateEmployee(_firstName, _lastName);
        }

        private Employee CreateSpecifiedEmployee(string firstName, string lastName)
        {
            return API_Tester.CreateEmployee(firstName, lastName);
        }

    }
}
