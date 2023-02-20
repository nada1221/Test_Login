using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using TestLogin.Models;

namespace TestLogin.Controllers
{
    [Route("Account/")]
    public class AccountController : Controller 
    {
		private readonly TestDBContext _context;
		const string ERROR_MSG = "ErrorMessage";

		public AccountController(TestDBContext context)
		{
			_context = context;
		}

		#region Login
		[HttpGet]
		[Route("Login")]
		public async Task<IActionResult> Login()
		{
			return View();
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login(
			[Bind(nameof(user.Id), nameof(user.Pw))]
			User user)
		{
			if (ModelState.ValidationState == ModelValidationState.Invalid)
			{
				ViewData[ERROR_MSG] = "잘못된 요청입니다.";
				return View();
			}
			if (string.IsNullOrWhiteSpace(user.Id))
			{
				ViewData[ERROR_MSG] = "아이디가 공백입니다";
				return View();
			}
			if (string.IsNullOrWhiteSpace(user.Pw))
			{
				ViewData[ERROR_MSG] = "패스워드가 공백입니다";
				return View();
			}

			var ExistUser = await this._context.FindAsync<User>(user.Id);

			IQueryable<User> query = from u in _context.User
									 where u.Id == user.Id && u.Pw == user.Pw
									 select u;

			var list = await query.ToListAsync();
			if (list.Count == 0)
			{
				ViewData[ERROR_MSG] = "ID 또는 패스워드를 확인해 주세요.";
				return View();
			}

			var acceptedUser = list.First();
			ViewData[ERROR_MSG] = "로그인 성공!";
			return RedirectToAction("Index", "Users");
		}
		#endregion

		#region SignUp
		[HttpGet]
		[Route("SignUp")]
		public async Task<IActionResult> SignUp()
		{
			return View();
		}

		[HttpPost]
		[Route("SignUp")]
		public async Task<IActionResult> SignUp(User user)
		{
			if (ModelState.ValidationState == ModelValidationState.Invalid)
			{
				ViewData[ERROR_MSG] = "잘못된 요청입니다.";
				return View();
			}
			if (string.IsNullOrWhiteSpace(user.Id))
			{
				ViewData[ERROR_MSG] = "아이디가 공백입니다";
				return View();
			}
			if (string.IsNullOrWhiteSpace(user.Pw))
			{
				ViewData[ERROR_MSG] = "패스워드가 공백입니다";
				return View();
			}

			var entry = await _context.User.AddAsync(user);
			var count = await _context.SaveChangesAsync();

			return RedirectToAction("Login", "Account");
		}
		#endregion


	}
}
