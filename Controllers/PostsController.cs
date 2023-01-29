﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscussionApp.Data;
using DiscussionApp.Models;


namespace DiscussionApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public static string? currentGenre;
        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Posts

        [HttpGet]
        public async Task<IActionResult> Index(string Genre)
        {
            ViewBag.Genre = Genre;
            currentGenre = Genre;
            try
            {
                return View("Index", await _context.Post.Where(a => a.Genre.Equals(Genre)).ToListAsync());
            }
            catch(NullReferenceException ex)
            {
                return View("Index", await _context.SaveChangesAsync());
            }
            
        }

        /*
        public async Task<IActionResult> Index()
        {

            return View("Index", await _context.Post.ToListAsync());
        }
        */
        /*
        public void OnPostDislikes()
        {
            Console.WriteLine("Hello");
        }
        <form method="post" asp-page-handler="Dislikes" >
                <button>Dislike</button>
            </form>
        */

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["Genre"] = currentGenre;
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,SubTitle,Content")] Post post)
        {
            if (ModelState.IsValid)
            {

                post.Genre = currentGenre;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Posts", new { genre = currentGenre });
            }
            return View(post);
        }

       

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UserId,Likes,Dislikes")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Posts", new { genre = currentGenre });
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Post == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Post'  is null.");
            }
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Posts", new { genre = currentGenre });
        }

        private bool PostExists(int id)
        {
          return _context.Post.Any(e => e.Id == id);
        }
    }
}
