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
                    TransactionId = 1,
                    Date = DateTimeOffset.Now,
                    ProfileId = 3,
                },
                new Transaction()
                {
                    TransactionId = 2,
                    Date = DateTimeOffset.Now.AddMinutes(-180),
                    ProfileId = 4,
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
            int transactionToCheckId = 2;

            // act
            Mock<DataContext> mock = new Mock<DataContext>();
            mock.SetupGet(q => q.Transactions).Returns(new[]
            {
                new Transaction()
                {
                    TransactionId = 1,
                    Date = DateTimeOffset.Now,
                    ProfileId = 3,
                },
                new Transaction()
                {
                    TransactionId = 2,
                    Date = DateTimeOffset.Now.AddMinutes(-201),
                    ProfileId = 4,
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
