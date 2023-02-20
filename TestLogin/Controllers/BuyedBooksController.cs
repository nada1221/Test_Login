using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestLogin.Models;

namespace TestLogin.Controllers
{
    public class BuyedBooksController : Controller
    {
        private readonly TestDBContext _context;

        public BuyedBooksController(TestDBContext context)
        {
            _context = context;
        }
		#region Index
		// GET: BuyedBooks
		public async Task<IActionResult> Index()
        {
            var testDBContext = _context.BuyedBook.Include(b => b.Book).Include(b => b.User);
            var list = await testDBContext.ToListAsync();
			return View(list);
        }
		#endregion

		#region Details
		// GET: BuyedBooks/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BuyedBook == null)
            {
                return NotFound();
            }
            var buyedBook = await _context.BuyedBook
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Nid == id);
            if (buyedBook == null)
            {
                return NotFound();
            }

            return View(buyedBook);
        }
		#endregion

		#region Create
		// GET: BuyedBooks/Create
		public async Task<IActionResult> Create()
        {
            var userList = await _context.User.ToListAsync();
            var bookList = await _context.Book.ToListAsync();
            var userSelectList = new SelectList(userList, "Id", "Id");
            var bookSelectList = new SelectList(bookList, "BookId", "BookId");

            ViewData["UserList"] = userSelectList;
            ViewData["BookList"] = bookSelectList;
			return View();
        }

        // POST: BuyedBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, Book book)
        {
            var buy = new BuyedBook();
            buy.UserId = user.Id;
            buy.BookId = book.BookId;
            var buyedBookEntity = await _context.BuyedBook.AddAsync(buy);
            var count = await _context.SaveChangesAsync();
            var nid = buyedBookEntity.Entity.Nid; // 부여받은 nid값 확인 용

			// TODO: Redirect to Index / Redirect to Edit 
			return RedirectToAction("Index", "BuyedBooks");
		}
		#endregion

		#region Edit
		// GET: BuyedBooks/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BuyedBook == null)
            {
                return NotFound();
            }

            var buyedBook = await _context.BuyedBook.FindAsync(id);
            if (buyedBook == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "BookId", buyedBook.BookId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", buyedBook.UserId);
            return View(buyedBook);
        }

        // POST: BuyedBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nid,UserId,BookId")] BuyedBook buyedBook)
        {
            if (id != buyedBook.Nid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buyedBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuyedBookExists(buyedBook.Nid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "BookId", buyedBook.BookId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", buyedBook.UserId);
            return View(buyedBook);
        }
		#endregion

		#region Delete
		// GET: BuyedBooks/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BuyedBook == null)
            {
                return NotFound();
            }

            var buyedBook = await _context.BuyedBook
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Nid == id);
            if (buyedBook == null)
            {
                return NotFound();
            }

            return View(buyedBook);
        }

        // POST: BuyedBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BuyedBook == null)
            {
                return Problem("Entity set 'TestDBContext.BuyedBook'  is null.");
            }
            var buyedBook = await _context.BuyedBook.FindAsync(id);
            if (buyedBook != null)
            {
                _context.BuyedBook.Remove(buyedBook);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		#endregion
		private bool BuyedBookExists(int id)
        {
          return (_context.BuyedBook?.Any(e => e.Nid == id)).GetValueOrDefault();
        }
    }
}
