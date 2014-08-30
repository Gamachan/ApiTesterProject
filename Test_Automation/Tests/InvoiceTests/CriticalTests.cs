using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Types;
using Xero.Api.Infrastructure.Exceptions;

namespace Test_Automation.Tests.InvoiceTests
{
    [TestFixture]
    public class CriticalTests
    {
        private AutomationAPI_Tester API_Tester;
        private string invoiceNumber;
        private XeroTestCoreApi api;

        [TestFixtureSetUp]
        public void ResetScenario()
        {
            api = new XeroTestCoreApi();
            API_Tester = new AutomationAPI_Tester(api);

        }

        /// <summary>
        /// Positive scenario.
        /// Check valid invoice can be created.
        /// </summary>
        [Test]
        public void CreateInvoice_ThreeItemLines_PositiveScenario()
        {
            invoiceNumber = Guid.NewGuid().ToString();

            Invoice invoice = SubmitNewInvoice();

            Assert.True(invoice.Id != Guid.Empty);
            Assert.AreEqual(InvoiceType.AccountsReceivable, invoice.Type);
            Assert.AreEqual(3, invoice.Items.Count());
        }

        /// <summary>
        /// Positive scenario.
        /// Create an invoice, check invoice retrieval by invoice number.
        /// </summary>
        [Test]
        public void GetInvoiceByInvoiceNumber_PositiveScenario()
        {
            invoiceNumber = Guid.NewGuid().ToString();

            Invoice invoice = SubmitNewInvoice();

            List<Invoice> result = API_Tester.GetInvoiceByInvoiceNumber(invoiceNumber).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 1);

            if (result.Count > 0)
            {
                Assert.AreEqual(result[0].Number, invoiceNumber);
            }
        }

        /// <summary>
        /// Positive scenario.
        /// Create an invoice with lines, retrieve the invoice by number and check the invoice lines aggregate sum equals to the invoice header total sum.
        /// </summary>
        [Test]
        public void GetInvoiceByInvoiceNumber_CheckLinesSumMatchHeaderSum_PositiveScenario()
        {
            invoiceNumber = Guid.NewGuid().ToString();

            Invoice invoice = SubmitNewInvoice();

            Invoice result = API_Tester.GetInvoiceByInvoiceNumber(invoiceNumber).First();

            decimal linesAggregatedSum = 0;

            foreach (LineItem lineItem in result.Items)
            {
                if (lineItem.LineAmount.HasValue)
                {
                    linesAggregatedSum += lineItem.LineAmount.Value;
                }
            }

            Assert.AreEqual(result.Total, linesAggregatedSum);
        }


        [Test]
        public void CreateInvoice_CreateDuplicateInvoice_NegativeScenario()
        {
            invoiceNumber = Guid.NewGuid().ToString();

            Invoice firstInvoice = SubmitNewInvoice();

            Assert.True(firstInvoice.Id != Guid.Empty);

            Assert.Throws<ValidationException>(() => SubmitNewInvoice());

        }

        [Test]
        public void CreateInvoice_WithoutLines_NegativeScenario()
        {
            invoiceNumber = Guid.NewGuid().ToString();

            Invoice invoice = new Invoice
            {
                Contact = new Contact { Name = "AM Inc." },
                Type = InvoiceType.AccountsReceivable,
                Number = invoiceNumber
            };

            Assert.Throws<ValidationException>(() => API_Tester.SubmitAnInvoice(invoice));
        }

        private Invoice SubmitNewInvoice()
        {
            return API_Tester.SubmitAnInvoice(new Invoice
            {
                Contact = new Contact { Name = "AM Inc." },
                Type = InvoiceType.AccountsReceivable,
                Number = invoiceNumber,
                Items = new List<LineItem>
                {
                    new LineItem
                    {
                        AccountCode = "315",
                        Description = "Amazing product",
                        UnitAmount = 0.6m,
                        Quantity = 1.6m
                    },
                    new LineItem
                    {
                        AccountCode = "315",
                        Description = "More amazing product",
                        UnitAmount = 1258.65m,
                        Quantity = 45.5m
                    },
                    new LineItem
                    {
                        AccountCode = "315",
                        Description = "Even more amazing product",
                        UnitAmount = 12548.65m,
                        Quantity = 435.5m
                    }

                }
            });
        }
    }
}
