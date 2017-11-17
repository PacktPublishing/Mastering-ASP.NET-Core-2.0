using BookShop.DomainModel;
using BookShop.Web.Controllers;
using BookShop.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace BookShop.UnitTests
{
    public class ControllerTests
    {
        [Fact]
        public void CanExecuteIndex()
        {
            var stringLocalizerMock = new Mock<IStringLocalizer<HomeController>>();
            stringLocalizerMock
                .Setup(x => x[It.IsAny<string>()])
                .Returns(new LocalizedString("Hello", "Hello, World!"));

            var bookServiceMock = new Mock<IBookService>();
            bookServiceMock
                .Setup(x => x.GetTopBooks())
                .Returns(new Book[] { new Book { Title = "Test", Author = new Author { Name = "Test" } } });

            var controller = new HomeController(bookServiceMock.Object, stringLocalizerMock.Object);
            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}
