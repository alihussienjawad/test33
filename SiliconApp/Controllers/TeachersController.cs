using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SiliconApp.Attributes;
using SiliconApp.Data;
using SiliconApp.Models;

namespace SiliconApp.Controllers
{
    [BreadcrumbActionFilter]
    public class TeachersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeachersController(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return View("Error");
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                if (teacher.ImageFile != null)
                {
                    fileName = "/courses/teachers/images/" + teacher.ImageFile.FileName;
                    if (!Directory.Exists(_webHostEnvironment.WebRootPath + "/courses/teachers/images"))
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "/courses/teachers/images/");
                    }



                    using FileStream filestream = System.IO.File.Create(_webHostEnvironment.WebRootPath + fileName);
                    teacher.ImageFile.CopyTo(filestream);
                    filestream.Flush();
                    teacher.ImagePath = fileName;
                }
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return View("Error");
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = "";
                    if (teacher.ImageFile != null)
                    {
                        fileName = "/courses/teachers/images/" + teacher.ImageFile.FileName;
                        if (!Directory.Exists(_webHostEnvironment.WebRootPath + "/courses/teachers/images"))
                        {
                            Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "/courses/teachers/images/");
                        }



                        using FileStream filestream = System.IO.File.Create(_webHostEnvironment.WebRootPath + fileName);
                        teacher.ImageFile.CopyTo(filestream);
                        filestream.Flush();
                        teacher.ImagePath = fileName;
                    }
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
                    {
                        return View("Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return View("Error");
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
