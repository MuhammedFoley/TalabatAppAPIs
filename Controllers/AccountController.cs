using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalabatAppAPIs.Dtos;
using TalabatAppAPIs.Erorrs;
using TalabatAppAPIs.Extentions;
using Talabt.Core.Entities.identity;
using Talabt.Core.Services;

namespace TalabatAppAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userMAnager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<AppUser> userMAnager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            _userMAnager = userMAnager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
        {
            var user = await _userMAnager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));


            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = loginDto.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userMAnager)
            });
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new AppUser() {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.Email.Split("@")[0]
            };
            var result= await _userMAnager.CreateAsync(user, registerDto.Password); 
            if (!result.Succeeded) return Unauthorized(new ApiResponse(400));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = registerDto.Email,
                Token =await _tokenService.CreateTokenAsync(user , _userMAnager)
            });
       
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]  
        [HttpGet]
        public async Task<ActionResult<UserDto>> getCurrentLoginedUser()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var user= await _userMAnager.FindByEmailAsync(email);

            return Ok(new UserDto() { 

                DisplayName=user.DisplayName,
                Email=user.Email,
                //genetate token
                Token=await _tokenService.CreateTokenAsync(user ,_userMAnager)
            });
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Adress")]
        public async Task<ActionResult<AdressDTO>> UserAdress()
        {

            var user = await _userMAnager.findUSerAddresAsync(User);

            var addres=_mapper.Map<Adress,AdressDTO>(user.Adress);

            return Ok(addres);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateAdress")]
        public async Task<ActionResult<AdressDTO>> UpdateUserAdress(AdressDTO UpdatedAdress)
        {

            var adress=_mapper.Map<AdressDTO,Adress>(UpdatedAdress);

            var user = await _userMAnager.findUSerAddresAsync(User);

            adress.Id=user.Adress.Id;

            user.Adress=adress;
            var result = await _userMAnager.UpdateAsync(user);
            if(!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedAdress);
        }
    }
}
