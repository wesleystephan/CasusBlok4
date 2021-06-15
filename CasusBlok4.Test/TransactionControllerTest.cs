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
using CasusBlok4.Models;

namespace CasusBlok4.Test
{
    public class TransactionControllerTest
    {
        [Fact]
        public void IndexNoActiveTransaction()
        {
            // act
            Mock<DataContext> dbMock = new Mock<DataContext>();
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.Index();

            // assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void IndexActiveTransaction()
        {
            // act
            Mock<DataContext> dbMock = new Mock<DataContext>();
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString()
            });
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.Index();

            // assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void IndexPostActiveTransaction()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString()
            });
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.IndexPost(null);

            // assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void IndexPostNoActiveTransaction()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            dbMock.Setup(q => q.Customers).Returns(new []
            {
                new Customer()
                {
                   Id = 1,
                   Name = "Noud Wijngaards",
                   Saldo = 8,
                }
            }.AsQueryable().BuildMockDbSet().Object);
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.IndexPost(new TransactionIndexViewModel() {
                CustomerId = 1,
            });

            // assert
            Assert.IsType<RedirectToActionResult>(result);
            transactionManagerMock.Verify(q => q.StartTransaction(It.IsAny<Customer>()), Times.Once());
        }

        [Fact]
        public void AddProductActiveTransaction()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            dbMock.Setup(q => q.Products).Returns(new[]
            {
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Poster van Marcel van de Beek",
                    PointsWorth = 7,
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Noud zijn t-shirt",
                    PointsWorth = 10,
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "CD van Nick en Simon",
                    PointsWorth = 1,
                }
            }.AsQueryable().BuildMockDbSet().Object);
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString()
            });
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.AddProduct();

            // assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<TransactionAddProductViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, viewModel.Products.Count());
        }

        [Fact]
        public void AddProductNoActiveTransaction()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.AddProduct();

            // assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void AddProductPostNoActiveTransaction()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.AddProductPost(null);

            // assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void AddProductPostNoProductMatch()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            dbMock.SetupGet(q => q.Products).Returns(new[]
            {
                new Product()
                {
                    Id = "12345678",
                    Name = "Poster van Vincent van der Meer",
                    PointsWorth = 4,
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Arne zijn sneakers",
                    PointsWorth = 7,
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "CD van Rammstein",
                    PointsWorth = 3,
                }
            }.AsQueryable().BuildMockDbSet().Object);

            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString()
            });
            transactionManagerMock.Setup(q => q.AddProductForSellToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()));
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.AddProductPost(new TransactionAddProductViewModel()
            {
                IsForSell = true,
                Points = 3,
                NumberOfProducts = 1,
                SelectedProductId = 563465,
            });

            // assert
            Assert.IsType<ViewResult>(result);
            transactionManagerMock.Verify(q => q.AddProductForSellToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()), Times.Never());
            transactionManagerMock.Verify(q => q.AddProductToBuyToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()), Times.Never());

        }

        [Fact]
        public void AddProductPostForSell()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            dbMock.SetupGet(q => q.Products).Returns(new[]
            {
                new Product()
                {
                    Id = "12345678",
                    Name = "Poster van Vincent van der Meer",
                    PointsWorth = 4,
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Arne zijn sneakers",
                    PointsWorth = 7,
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "CD van Rammstein",
                    PointsWorth = 3,
                }
            }.AsQueryable().BuildMockDbSet().Object);

            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString()
            });
            transactionManagerMock.Setup(q => q.AddProductForSellToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()));
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.AddProductPost(new TransactionAddProductViewModel() {
                IsForSell = true,
                Points = 3,
                NumberOfProducts = 1,
                SelectedProductId = 12345678,
            });

            // assert
            Assert.IsType<RedirectToActionResult>(result);
            transactionManagerMock.Verify(q => q.AddProductForSellToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()), Times.Once());
            transactionManagerMock.Verify(q => q.AddProductToBuyToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()), Times.Never());
        }

        [Fact]
        public void AddProductPostToBuy()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            dbMock.SetupGet(q => q.Products).Returns(new[]
            {
                new Product()
                {
                    Id = "12345678",
                    Name = "Poster van Chris Kockelkorn",
                    PointsWorth = 5,
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Damian zijn ketting",
                    PointsWorth = 3,
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "CD van Katy Pery",
                    PointsWorth = 1,
                }
            }.AsQueryable().BuildMockDbSet().Object);

            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString()
            });
            transactionManagerMock.Setup(q => q.AddProductForSellToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()));
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.AddProductPost(new TransactionAddProductViewModel()
            {
                IsForSell = false,
                Points = 3,
                NumberOfProducts = 1,
                SelectedProductId = 12345678,
            });

            // assert
            Assert.IsType<RedirectToActionResult>(result);
            transactionManagerMock.Verify(q => q.AddProductForSellToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()), Times.Never());
            transactionManagerMock.Verify(q => q.AddProductToBuyToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()), Times.Once());
        }

        [Fact]
        public void ListNoActiveTransaction()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.List();

            // assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ListActiveTransaction()
        {
            Transaction transaction = new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString(),
                StartTime = DateTimeOffset.Now,
                CustomerId = 3,
                EmployeeId = 40,
            };

            Mock<DataContext> dbMock = new Mock<DataContext>();
            dbMock.SetupGet(q => q.TransactionProducts).Returns(new[]
            {
                
                new TransactionProduct()
                {
                    Points = 3,
                    ProductId = "2231",
                    IsForSell = true,
                    NumberOfProduct = 1,
                    TransactionId = transaction.TransactionId,
                    Transaction = transaction
                },
                new TransactionProduct()
                {
                    Points = 4,
                    ProductId = "2091",
                    IsForSell = false,
                    NumberOfProduct = 2,
                    TransactionId = transaction.TransactionId,
                    Transaction = transaction
                }
            }.AsQueryable().BuildMockDbSet().Object);

            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString(),
                Customer = new Customer()
                {
                    Id = 1,
                    Name = "Roel Bindels",
                    Saldo = 105
                },
                CustomerId = 1,
                Employee = new Employee()
                {
                    Id = 40, 
                    Name = "Kees Bekker"
                },
                EmployeeId = 40
            });
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.List();

            // assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<TransactionListViewModel>(viewResult.ViewData.Model);
            Assert.Equal(2, viewModel.TransactionProducts.Count());
        }

        [Fact]
        public void FinishNoActiveTransaction()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.Finish();

            // assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void FinishActiveTransaction()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString()
            });
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.Finish();

            // assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result); 
            transactionManagerMock.Verify(q => q.EndTransaction(), Times.Once());
        }
    }
}
