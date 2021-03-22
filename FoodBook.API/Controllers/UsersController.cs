using System.Threading.Tasks;
using FoodBook.Business.Repositories.IRepositories;
using FoodBook.Business.Validators;
using FoodBook.Business.Validators.UserValidators;
using FoodBook.Common.DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Variables
        private readonly IUserRepo _userRepo;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public UsersController(IUserRepo userRepo, ILogger<UsersController> logger)
        {
            _logger = logger;
            _userRepo = userRepo;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Register user
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequestDTO registerUserRequestDTO)
        {
            RegisterUserValidator validator = new RegisterUserValidator();
            var validationResult = validator.Validate(registerUserRequestDTO);

            if(!validationResult.IsValid)
            {
                _logger.LogInformation("Not valid input.");
                ValidatorError validatorError = new ValidatorError();
                return BadRequest(new {
                    message = validatorError.GetErrorMessagesAsString(validationResult.Errors)
                });
            }

            var user = await _userRepo.CreateUser(registerUserRequestDTO.User, registerUserRequestDTO.Password);
            
            if(user == null)
            {
                return BadRequest(new {
                    message = "User with given email already exists."
                });
            }

            return Ok(user);
        }

        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequestDTO loginUserRequestDTO)
        {
            LoginUserValidator validator = new LoginUserValidator();
            var validationResult = validator.Validate(loginUserRequestDTO);

            if(!validationResult.IsValid)
            {
                _logger.LogInformation("Not valid input.");
                ValidatorError validatorError = new ValidatorError();
                return BadRequest(new {
                    message = validatorError.GetErrorMessagesAsString(validationResult.Errors)
                });
            }

            var user = await _userRepo.LoginUser(loginUserRequestDTO.Email, loginUserRequestDTO.Password);
            
            if(user == null)
            {
                return BadRequest(new {
                    message = "Invalid email or passowrd."
                });
            }

            return Ok(user);
        }



        #endregion

        //TODO:
        //-Endpoints for login and registry
        //-Read about global exception controller
        //-
    }
}