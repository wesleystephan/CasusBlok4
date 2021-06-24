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
                TransactionId = 1
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
                TransactionId = 1
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
            dbMock.Setup(q => q.ProfileData).Returns(new []
            {
                new ProfileData()
                {
                   Id = 1,
                   FirstName = "Noud",
                   LastName = "Wijngaards",
                   Balans = 8,
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
            transactionManagerMock.Verify(q => q.StartTransaction(It.IsAny<ProfileData>()), Times.Once());
        }

        [Fact]
        public void AddProductActiveTransaction()
        {
            Mock<DataContext> dbMock = new Mock<DataContext>();
            dbMock.Setup(q => q.Products).Returns(new[]
            {
                new Product()
                {
                    Id = 1,
                    Name = "Poster van Marcel van de Beek",
                    PointsWorth = 7,
                },
                new Product()
                {
                    Id = 2,
                    Name = "Noud zijn t-shirt",
                    PointsWorth = 10,
                },
                new Product()
                {
                    Id = 3,
                    Name = "CD van Nick en Simon",
                    PointsWorth = 1,
                }
            }.AsQueryable().BuildMockDbSet().Object);
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = 1
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
                    Id = 1,
                    Name = "Poster van Vincent van der Meer",
                    PointsWorth = 4,
                },
                new Product()
                {
                    Id = 2,
                    Name = "Arne zijn sneakers",
                    PointsWorth = 7,
                },
                new Product()
                {
                    Id = 3,
                    Name = "CD van Rammstein",
                    PointsWorth = 3,
                }
            }.AsQueryable().BuildMockDbSet().Object);

            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = 1
            });
            transactionManagerMock.Setup(q => q.AddProductForSellToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()));
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.AddProductPost(new TransactionAddProductViewModel()
            {
                IsForSell = true,
                Points = 3,
                NumberOfProducts = 1,
                SelectedProductId = 4,
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
                    Id = 1,
                    Name = "Poster van Marcel van den Beek",
                    PointsWorth = 4,
                },
                new Product()
                {
                    Id = 2,
                    Name = "Wesley zijn horloge",
                    PointsWorth = 7,
                },
                new Product()
                {
                    Id = 2,
                    Name = "CD van Imagine dDagons",
                    PointsWorth = 3,
                }
            }.AsQueryable().BuildMockDbSet().Object);

            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = 1
            });
            transactionManagerMock.Setup(q => q.AddProductForSellToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()));
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.AddProductPost(new TransactionAddProductViewModel() {
                IsForSell = true,
                Points = 3,
                NumberOfProducts = 1,
                SelectedProductId = 1,
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
                    Id = 1,
                    Name = "Poster van Chris Kockelkorn",
                    PointsWorth = 5,
                },
                new Product()
                {
                    Id = 2,
                    Name = "Damian zijn ketting",
                    PointsWorth = 3,
                },
                new Product()
                {
                    Id = 3,
                    Name = "CD van Katy Pery",
                    PointsWorth = 1,
                }
            }.AsQueryable().BuildMockDbSet().Object);

            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = 1
            });
            transactionManagerMock.Setup(q => q.AddProductForSellToTransaction(It.IsAny<Product>(), It.IsAny<byte>(), It.IsAny<short?>()));
            TransactionController controller = new TransactionController(dbMock.Object, transactionManagerMock.Object);

            // arrange
            IActionResult result = controller.AddProductPost(new TransactionAddProductViewModel()
            {
                IsForSell = false,
                Points = 3,
                NumberOfProducts = 1,
                SelectedProductId = 1,
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
                TransactionId = 1,
                Date = DateTimeOffset.Now,
                ProfileId = 3,
            };

            Mock<DataContext> dbMock = new Mock<DataContext>();
            dbMock.SetupGet(q => q.TransactionProducts).Returns(new[]
            {
                
                new TransactionProduct()
                {
                    Points = 3,
                    ProductId = 2231,
                    IsForSell = true,
                    NumberOfProduct = 1,
                    TransactionId = transaction.TransactionId,
                    Transaction = transaction
                },
                new TransactionProduct()
                {
                    Points = 4,
                    ProductId = 2091,
                    IsForSell = false,
                    NumberOfProduct = 2,
                    TransactionId = transaction.TransactionId,
                    Transaction = transaction
                }
            }.AsQueryable().BuildMockDbSet().Object);

            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            transactionManagerMock.SetupGet(q => q.ActiveTransaction).Returns(new Transaction()
            {
                TransactionId = 1,
                Customer = new ProfileData()
                {
                    Id = 1,
                    FirstName = "Roel",
                    LastName = "Bindels",
                    Balans = 105
                },
                ProfileId = 1
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
                TransactionId = 1
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
