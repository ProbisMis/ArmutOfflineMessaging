using System;
using Xunit;
using MessagingWebApi.Controllers;
using MessagingWebApi.Services;
using MessagingWebApiTest.Services;
using Microsoft.Extensions.Logging;
using MessagingWebApi.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using NLog.Web;
using MessagingWebApi;
using Microsoft.Extensions.Hosting;

namespace MessagingWebApiTest
{
    public class UserControllerTest
    {
        UsersController _controller;
        IUserService _userService;
        ILogger<UsersController> _logger;

        public UserControllerTest()
        {
            _userService = new FakeUserService();
            _controller = new UsersController(_userService, _logger);
        }


        [Fact]
        public async Task Register_ResultOk()
        {
            //Arrange
            User user = new User()
            {
                Username = "test",
                Password = "123",
                CreatedDate = DateTime.Now
            };

            //Act
            var result = await _controller.Create(user) as OkObjectResult;

            //Asset
            Assert.IsType<User>(result.Value);
        }


        [Fact]
        public async Task GetUserById_NotFound()
        {
            //Arrange
            int id = 999;
            //Act
            var result = _userService.GetUserById(id);
            //Asset
            Assert.Null(result.user);
        }

        [Fact]
        public async Task Login_ResultOk()
        {
            //Arrange
            User user = new User()
            {
                Username = "test1",
                Password = "123",
            };
            
            //Act
            var result = _controller.Login(user);
            
            //Asset
            Assert.IsType<User>(result.Result);
        }
    }
}
