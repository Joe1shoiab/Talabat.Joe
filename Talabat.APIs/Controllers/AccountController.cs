using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Error;
using Talabat.APIs.Extentions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)  
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.tokenService = tokenService;
            _mapper = mapper;
        }


       

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new ApiResponse(401));

            var resault = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (resault.Succeeded is false)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Token  = await tokenService.CreateTokenAsync(user, _userManager),
                Email = user.Email

            });

        }

        

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email is already in use"));
            }
            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email, 
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await tokenService.CreateTokenAsync(user, _userManager),
                Email = user.Email
            };
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);

            var  CurrentUser = new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await tokenService.CreateTokenAsync(user, _userManager),
                Email = user.Email
            };
            return Ok(CurrentUser);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            //return Ok(user?.Address); // cycle reference shit 
            var addressDto = _mapper.Map<Address, AddressDto>(user.Address!);
            return Ok(addressDto);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto updatedAddressDto)
        {
            var currentuser = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<AddressDto, Address>(updatedAddressDto);
            MappedAddress.Id = currentuser.Address.Id;  // to update the address instead of creating a new row in DB with new ID
            currentuser.Address = MappedAddress;

            var Result = await _userManager.UpdateAsync(currentuser);
            if (!Result.Succeeded) 
            {
                return BadRequest(new ApiResponse(400));
            }
            return Ok(updatedAddressDto);
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            return user != null;
        }
    }
}
