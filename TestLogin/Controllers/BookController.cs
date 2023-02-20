using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestLogin.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TestLogin.Controllers
{
    [Route("Book/")]
    public class BookController : Controller
    {
        private readonly TestDBContext _context;
        const string ERROR_MSG = "ErrorMessage";

        public BookController(TestDBContext context)
        {
            _context = context;
        }

        #region BookList
        [HttpGet]
        [Route("BookList")]
        public async Task<IActionResult> BookList()
        {
            return _context.Book != null ?
                        View(await _context.Book.ToListAsync()) :
                        Problem("Entity set 'TestDBContext.Book'  is null.");
        }
        #endregion
        #region PurchaseBook
        [HttpGet]
        [Route("PurchaseBook")]
        public IActionResult PurchaseBook()
        {
            return View();
        }
        [HttpPost]
        [Route("PurchaseBook")]
        public async Task<IActionResult> PurchaseBook(Book book)
        {
            if (ModelState.ValidationState == ModelValidationState.Invalid)
            {
                ViewData[ERROR_MSG] = "잘못된 요청입니다.";
                return View();
            }
            if (string.IsNullOrWhiteSpace(book.BookName))
            {
                ViewData[ERROR_MSG] = "책 이름이 공백입니다";
                return View();
            }
            var bookEntry = await _context.Book.AddAsync(book);
            var bookCount = await _context.SaveChangesAsync();

            return RedirectToAction("BookList", "Book");
        }

        #endregion
    }
}
