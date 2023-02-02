using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using swf.Configurations;
using swf.Libraries;
using swf.Models.AuthResult;
using swf.Models.DTOsUsers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace swf.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Route("api/[controller]")]  //api/authentication
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        //private readonly JwtConfig _jwtConfig;
        public AuthenticationController( UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            //_jwtConfig = jwtConfig;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDTO userRegistrationDTO)
        {
            //validate the incoming requests
            if (ModelState.IsValid)
            {
                //verify if the meail exists
                var user_exist = await _userManager.FindByEmailAsync(userRegistrationDTO.Email);
                if (user_exist != null)
                {
                    //return badrequest cause user already exists
                    return BadRequest(new AuthResult()
                    {
                        Result=false,
                        Errors=new List<string>()
                        {
                            "Email already exist"
                        }
                    });
                }

                //continue in case the user does not exist. creating the user
                var new_user = new IdentityUser()
                {
                    Email = userRegistrationDTO.Email,
                    UserName = userRegistrationDTO.Name,
                };
                var is_created = await _userManager.CreateAsync(new_user, userRegistrationDTO.Password);
                //check if successfully was created
                if (is_created.Succeeded)
                {
                    //generate the token based on credentials
                    var token=GenerateJwtToken.ReturnJwtToken(new_user, _configuration);
                    return Ok(new AuthResult()
                    {
                        Token = token,
                        Result = true
                    }) ;

                }
                //if not succeeded return an error
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Server Error"
                    },
                    Result = false
                }) ;
            }
           return BadRequest();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO loginRequest)
        {
            if (ModelState.IsValid)
            {
                //first stept check if the user exists - the email exists
                var existing_user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (existing_user == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors=new List<string>()
                        {
                            "invalid payload"
                        },
                        Result=false
                        
                    });
                }
                var isCorrect =await _userManager.CheckPasswordAsync(existing_user, loginRequest.Password);
                if (!isCorrect)
                {
                    return BadRequest(new AuthResult()
                    {
                        Result=false,
                        Errors=new List<string>()
                        {
                            "invalid password"
                        }
                    });
                }
                //if isCorrect we generate a jwtToken
                var jwtToken = GenerateJwtToken.ReturnJwtToken(existing_user, _configuration);
                return Ok(new AuthResult()
                {
                    Token=jwtToken
                });

            }
            return BadRequest(new AuthResult()
            {
                Result=false,
                Errors= new List<string>()
                {
                    "Invalid payload"
                }
            });
        }

    }
}
