using CasusBlok4.Controllers;
using CasusBlok4.Services;
using CasusBlok4.Views.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CasusBlok4.Test
{
    public class HomeControllerTest
    {

        [Fact]
        public void Index()
        {
            // act
            HomeController controller = new HomeController();

            // arrange
            IActionResult result = controller.Index();

            // assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public void Privacy()
        {
            // act
            HomeController controller = new HomeController();

            // arrange
            IActionResult result = controller.Privacy();

            // assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public void Error()
        {
            // act
            HomeController controller = new HomeController()
            {
                ControllerContext = new ControllerContext()
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // arrange
            IActionResult result = controller.Error();

            // assert
            Assert.IsAssignableFrom<ViewResult>(result);
            ViewResult view = (ViewResult) result;
            Assert.IsAssignableFrom<ErrorViewModel>(view.Model);
        }
    }
}
