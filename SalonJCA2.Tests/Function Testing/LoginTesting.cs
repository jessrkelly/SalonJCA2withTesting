using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using SalonJCA2.Areas.Identity.Pages.Account;
using SalonJCA2.Models;
//This testing will focus on how the system works if the user tries to loggin with the incorrect info. 

namespace SalonJCA2.Tests.FunctionTesting
{
    internal class LoginTestingSuite
    {
        public class LoginTesting
        {
            private readonly SignInManager<AppUsers> _signInManager;
            private readonly ILogger<LoginModel> _logger;

            public LoginTesting(SignInManager<AppUsers> signInManager, ILogger<LoginModel> logger)
            {
                //Set up instances of Loggin
                _signInManager = signInManager;
                _logger = logger;
            }

            //This is my Test method:
            [Fact]
            //This is my incorrect loggin Test
            public async Task Login_UserNotIdentified_ReturnsErrorMessage()
            {
                //Arrange - Set up instances for the loggin
                var loginModel = new LoginModel(_signInManager, _logger);
                loginModel.Input = new LoginModel.InputModel
                {
                    //Invalis email / Password
                    Email = "wrongEmail@example.com",
                    Password = "testpassword"
                };

                //Act - Set loggin attempt 
                var result = await loginModel.OnPostAsync();

                //Assert - Check the oage result, Model and Error message
                Assert.IsType<PageResult>(result);
                Assert.False(loginModel.ModelState.IsValid);
                Assert.Contains("Invalid login attempt.", loginModel.ModelState[""].Errors.Select(e => e.ErrorMessage));
            }
        }

        //Mocked main test class to support testing
        public class LoginTestingTests
        {
            private readonly Mock<SignInManager<AppUsers>> _signInManagerMock;
            private readonly Mock<ILogger<LoginModel>> _loggerMock;
            private readonly LoginTesting _loginTesting;

            public LoginTestingTests()
            {
                var store = new Mock<IUserStore<AppUsers>>();
                _signInManagerMock = new Mock<SignInManager<AppUsers>>(store.Object, null, null, null, null, null, null);
                _loggerMock = new Mock<ILogger<LoginModel>>();

                _loginTesting = new LoginTesting(_signInManagerMock.Object, _loggerMock.Object);
            }
        }
    }
}