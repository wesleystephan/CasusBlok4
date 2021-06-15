using CasusBlok4.Controllers;
using CasusBlok4.Models.Entity;
using CasusBlok4.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MockQueryable.Moq;

namespace CasusBlok4.Test
{
    public class MyTransactionControllerTest
    {
        [Fact]
        public async Task Index()
        {
            // act
            Mock<DataContext> mock = new Mock<DataContext>();
            mock.SetupGet(q => q.Transactions).Returns(new[]
            {
                new Transaction()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    StartTime = DateTimeOffset.Now,
                    CustomerId = 3,
                    EmployeeId = 1,
                },
                new Transaction()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    StartTime = DateTimeOffset.Now.AddMinutes(-201),
                    EndTime = DateTimeOffset.Now.AddMinutes(-180),
                    CustomerId = 4,
                    EmployeeId = 1,
                }
            }.AsQueryable().BuildMockDbSet().Object);
            MyTransactionsController controller = new MyTransactionsController(mock.Object);

            // arrange
            IActionResult result = await controller.Index();

            // assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Transaction>>(viewResult.ViewData.Model);
            Assert.True(model.Any());
        }


        [Fact]
        public async Task Details()
        {
            string transactionToCheckId = Guid.NewGuid().ToString();

            // act
            Mock<DataContext> mock = new Mock<DataContext>();
            mock.SetupGet(q => q.Transactions).Returns(new[]
            {
                new Transaction()
                {
                    TransactionId = transactionToCheckId,
                    StartTime = DateTimeOffset.Now,
                    CustomerId = 3,
                    EmployeeId = 1,
                },
                new Transaction()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    StartTime = DateTimeOffset.Now.AddMinutes(-201),
                    EndTime = DateTimeOffset.Now.AddMinutes(-180),
                    CustomerId = 4,
                    EmployeeId = 1,
                }
            }.AsQueryable().BuildMockDbSet().Object);
            MyTransactionsController controller = new MyTransactionsController(mock.Object);

            // arrange
            IActionResult result = await controller.Details(transactionToCheckId);

            // assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Transaction>(viewResult.ViewData.Model);
            Assert.Equal(transactionToCheckId, model.TransactionId);
        }
    }
}
