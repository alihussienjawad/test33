using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Packaging.Licenses;
using SiliconApp.Attributes;
using SiliconApp.Data;
using SiliconApp.Models;
using SiliconApp.Models.VM;

namespace SiliconApp.Controllers
{
    [BreadcrumbActionFilter]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
     
 

        public CoursesController(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
    
        
        }

        // GET: Courses
        public async Task<IActionResult> Index(int CourseCategory,string? SearchCourse)
        {
           IEnumerable<Course> courses=await GetCourses();
        
            return View( await GetData(courses,CourseCategory, SearchCourse));
        }

        private  async Task<IEnumerable<Course>> GetCourses()
        {
            IEnumerable<Course> Courses=[];
         
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7136/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync("api/Courses");
                if (response.IsSuccessStatusCode)
                {
                    Courses = await response.Content.ReadAsAsync<IEnumerable<Course>>();
                    return Courses;
                }
            
            }
            return Courses;
        }

        [HttpGet]
        public async Task<IActionResult> _indexCourseUser(int CourseCategory, string? SearchCourse)
        {
            IEnumerable<Course> courses = await GetCourses();
            return PartialView( await GetData(courses,CourseCategory, SearchCourse));
        }
        [HttpGet]
        public async Task<IActionResult> GetSavedCourseUser()
        {
            IEnumerable<Course> courses = await GetCourses();
            IEnumerable<Course>? CourseList =  await GetData(courses,0, null);
            if(CourseList != null)
            {
                CourseList= CourseList.Where(i=>i.IsSaved).ToList();
            }
            return PartialView(CourseList);
        }     
        [HttpGet]
        public async Task<IActionResult> DeleteAllSavedItem()
        {
            IEnumerable<Course> courses = await GetCourses();
            IEnumerable<Course>? CourseList = await  GetData(courses,0, null);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            if (CourseList != null)
            {
                CourseList= CourseList.Where(i=>i.IsSaved).ToList();
            }
            if (CourseList!=null)
            {
                foreach (var item in CourseList)
                {
                    List<UserSavedItem>? userSavedItem = _context.UserSavedItems.Where(i => i.CourseId == item.CourseId && i.ApplicationUserId == userId).ToList();
                    if (userSavedItem != null)
                    {
                        foreach(var item2 in userSavedItem)
                        {
                            _context.UserSavedItems.Remove(item2);
                            await _context.SaveChangesAsync();
                        }
                    }
                    
                
                }
            }
              CourseList = await  GetData(courses,0, null);
           
            if (CourseList != null)
            {
                CourseList = CourseList.Where(i => i.IsSaved).ToList();
            }
            return PartialView("GetSavedCourseUser",CourseList);
        }
        [HttpGet]
        public async Task<IActionResult> RemoveSaveCourse(int id)
        {
            IEnumerable<Course> courses = await GetCourses();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
          
           
                UserSavedItem? userSavedItem = _context.UserSavedItems.Where(i => i.CourseId == id && i.ApplicationUserId == userId).FirstOrDefault();
            if (userSavedItem != null)
            {
                _context.UserSavedItems.Remove(userSavedItem);
                await _context.SaveChangesAsync();
                
            }

            IEnumerable<Course>? CourseList = await  GetData(courses,0, null);
            if (CourseList != null)
            {
                CourseList = CourseList.Where(i => i.IsSaved).ToList();
            }

            return PartialView("GetSavedCourseUser", CourseList);
        }
        private async Task<IEnumerable<Course>?>  GetData(IEnumerable<Course> Courses, int CourseCategory, string? SearchCourse)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)??"";
           

            foreach(Course item in Courses)
            {
                item.IsSaved = _context.UserSavedItems.Any(i => i.CourseId == item.CourseId && i.ApplicationUserId == userId && i.Is_Saved);
                item.IsLiked = _context.UserSavedItems.Any(i => i.CourseId == item.CourseId && i.ApplicationUserId == userId && i.Is_Liked);
            }

            if (CourseCategory > 0)
                Courses = Courses.Where(i => i.CategoryId == CourseCategory).ToList();

            if (!string.IsNullOrEmpty(SearchCourse))
            {
                Courses = Courses
                   .Where(i => i.CourseName.Contains(SearchCourse))
                   .ToList();
                ViewData["SearchCourse"] = SearchCourse;
            }


            ViewData["categories"] = new SelectList(_context.Catogoris, "CategoryId", "CategoryName", CourseCategory);
            return  Courses;
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Course course = new();
            if (id == null)
            {
                return PartialView(NotFound());
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7136/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync($"api/Courses/{id}");
                if (response.IsSuccessStatusCode)
                {
                    course = await response.Content.ReadAsAsync<Course>();
                    return View(course);
                }

            }

           
                return PartialView(NotFound());
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Catogoris, "CategoryId", "CategoryName");
 
            Course course = new() { CourseLearns = [], CourseDetails = [], TeacherCourses = [], selectListItems = [] };
            course.CourseLearns.Add(new() { Id = 1 });
            course.CourseDetails.Add(new() { Id = 1 });
            course.TeacherCourses.Add(new() { Id = 1 });
            course.selectListItems.Add(new SelectList(_context.Teachers, "Id", "TeacherName"));

            return View(course);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                if (course.ImageFile != null)
                {
                    fileName = "/courses/images/" + course.ImageFile.FileName;
                    if (!Directory.Exists(_webHostEnvironment.WebRootPath + "/courses/images"))
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "/courses/images/");
                    }



                    using FileStream filestream = System.IO.File.Create(_webHostEnvironment.WebRootPath + fileName);
                    course.ImageFile.CopyTo(filestream);
                    filestream.Flush();
                    course.ImagePath = fileName;
                }

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Catogoris, "CategoryId", "CategoryName", course.CategoryId);


            List<int> TeachersId = course.TeacherCourses.Select(i => i.CourseId).ToList();

            if (TeachersId.Any())
            {
                foreach (var i in TeachersId)
                {
                    course.selectListItems.Add(new SelectList(_context.Teachers, "Id", "TeacherName", i));
                }
            }
            else
            {
                course.selectListItems.Add(new SelectList(_context.Teachers, "Id", "TeacherName"));

            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var course = await _context.Courses
                .Where(i=>i.CourseId==id)
                .Include(i=>i.CourseLearns)
                .Include(i=>i.TeacherCourses)
                .Include(i=>i.CourseDetails)
                .FirstOrDefaultAsync();
                
            if (course == null)
            {
                return View("Error");
            }
 
            List<int> TeachersId=course.TeacherCourses.Select(i=>i.CourseId).ToList();

            if (TeachersId.Any())
            {
                foreach (var i in TeachersId)
                {
                    course.selectListItems.Add(new SelectList(_context.Teachers, "Id", "TeacherName", i));
                }
            }
            else
            {
                course.selectListItems.Add(new SelectList(_context.Teachers, "Id", "TeacherName"));

            }

            if (string.IsNullOrEmpty(course.ImagePath)){
                course.ImagePath = "/Img/upload.png";
            }
            ViewData["CategoryId"] = new SelectList(_context.Catogoris, "CategoryId", "CategoryName", course.CategoryId);


            if (course.CourseLearns.Count==0)
                course.CourseLearns.Add(new() { Id = 1 }); 

            if (course.TeacherCourses.Count==0)
                course.TeacherCourses.Add(new() { Id = 1 }); 
            
            if (course.CourseDetails.Count==0)
                course.CourseDetails.Add(new() { Id = 1 });



            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = "";
                    if (course.ImageFile != null)
                    {
                        fileName = "/courses/images/" + course.ImageFile.FileName;
                        if (!Directory.Exists(_webHostEnvironment.WebRootPath + "/courses/images"))
                        {
                            Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "/courses/images/");
                        }
                        using FileStream filestream = System.IO.File.Create(_webHostEnvironment.WebRootPath + fileName);
                        course.ImageFile.CopyTo(filestream);
                        filestream.Flush();
                        course.ImagePath = fileName;
                    }
                    List<CourseLearn> courseLearns = [.. _context.CourseLearns.Where(l => l.CourseId == course.CourseId)];
                    List<TeacherCourse> teacherCourses = [.. _context.TeacherCourses.Where(l => l.CourseId == course.CourseId)];
                    List<CourseDetails> courseDetails = [.. _context.CourseDetails.Where(l => l.CourseId == course.CourseId)];

                    _context.CourseLearns.RemoveRange(courseLearns);
                    _context.TeacherCourses.RemoveRange(teacherCourses);
                    _context.CourseDetails.RemoveRange(courseDetails);
                    await _context.SaveChangesAsync();
                    
                    _context.Attach(course);
                    _context.Entry(course).State=EntityState.Modified;
                   
                    _context.CourseLearns.AddRange(course.CourseLearns);
                    _context.TeacherCourses.AddRange(course.TeacherCourses);
                    _context.CourseDetails.AddRange(course.CourseDetails);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            List<int> TeachersId = course.TeacherCourses.Select(i => i.CourseId).ToList();

            foreach (var i in TeachersId)
            {
                course.selectListItems.Add(new SelectList(_context.Teachers, "Id", "TeacherName", i));
            }
            ViewData["CategoryId"] = new SelectList(_context.Catogoris, "CategoryId", "CategoryName", course.CategoryId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var course = await _context.Courses
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return View("Error");
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
        [HttpGet]
        public async Task<JsonResult> SaveCourse(int CourseId,int IsSaved)
        {
            string Message;
             
                string? userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(userid != null)
                {
                if(IsSaved==1) 
                { 
                    //Save and Un Save Course
                    UserSavedItem? userSavedItem = _context.UserSavedItems.Where(i => i.CourseId == CourseId && i.ApplicationUserId == userid).FirstOrDefault();
                    if (userSavedItem != null)
                    {
                        if (userSavedItem.Is_Saved)
                        {
                            userSavedItem.Is_Saved = false;
                            Message = "The Course  Was UnSaved";
                        }
                        else
                        {
                            userSavedItem.Is_Saved = true;
                            Message = "The Course  Was Saved";
                        }
                        _context.UserSavedItems.Update(userSavedItem);
                        await _context.SaveChangesAsync();
                        return Json(new { status = userSavedItem.Is_Saved, message = Message });
                    }
                    else
                    {
                        Message = "The Course  Was Saved";
                        userSavedItem = new() { ApplicationUserId = userid, CourseId = CourseId, Is_Saved = true, Is_Liked = false};
                        _context.UserSavedItems.Add(userSavedItem);
                        await _context.SaveChangesAsync();
                        return Json(new { status = userSavedItem.Is_Saved, message = Message });
                    }
                }
                else
                {
                    // Like And dislike Course
                    UserSavedItem? userSavedItem = _context.UserSavedItems.Where(i => i.CourseId == CourseId && i.ApplicationUserId == userid).FirstOrDefault();
                    if (userSavedItem != null)
                    {
                        if (userSavedItem.Is_Liked)
                        {
                            Course? course1=await _context.Courses.Where(i => i.CourseId== CourseId).FirstOrDefaultAsync();
                            if(course1 != null) {
                                course1.NumberOfLikes = course1.NumberOfLikes - 1;
                                if (course1.NumberOfLikes < 0)
                                      course1.NumberOfLikes = 0;
                                _context.Courses.Update(course1);
                                await _context.SaveChangesAsync();
                            }
                            userSavedItem.Is_Liked = false;
                            Message = "You  disLike This Course";
                        }
                        else
                        {
                            Course? course1 = await _context.Courses.Where(i => i.CourseId == CourseId).FirstOrDefaultAsync();
                            if (course1 != null)
                            {
                                if (course1.NumberOfLikes < 0)
                                    course1.NumberOfLikes = 0;
                                course1.NumberOfLikes = course1.NumberOfLikes + 1;
                                _context.Courses.Update(course1);
                                await _context.SaveChangesAsync();
                            }
                            userSavedItem.Is_Liked = true;
                            Message = "You  Like This Course";
                        }
                        _context.UserSavedItems.Update(userSavedItem);
                        await _context.SaveChangesAsync();
                        return Json(new { status = userSavedItem.Is_Liked, message = Message });
                    }
                    else
                    {
                        Message = "You  Like This Course";
                        userSavedItem = new() { ApplicationUserId = userid, CourseId = CourseId, Is_Saved = false, Is_Liked = true };
                        _context.UserSavedItems.Add(userSavedItem);
                        await _context.SaveChangesAsync();

                        Course? course1 = await _context.Courses.Where(i => i.CourseId == CourseId).FirstOrDefaultAsync();
                        if (course1 != null)
                        {
                            if (course1.NumberOfLikes < 0)
                                course1.NumberOfLikes = 0;
                            course1.NumberOfLikes = course1.NumberOfLikes + 1;
                            _context.Courses.Update(course1);
                            await _context.SaveChangesAsync();
                        }
                        return Json(new { status = userSavedItem.Is_Liked, message = Message });
                    }

                }
               
              
                 
                }
                return Json(new {status=false, message = "Un Error " });
      
        }
    }
}
