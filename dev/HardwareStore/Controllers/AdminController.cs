using HardwareStore.Data;
using HardwareStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;

namespace HardwareStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;


        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ShowRoles()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([Bind("Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        /// <summary>
        /// Excel 
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult ExcelWork(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string filePath = LoadFile(uploadedFile);
                ParseDocument(filePath);
                DeleteFile(filePath);

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public string LoadFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "documents/admin");
                string filePath = Path.Combine(uploadsFolder, uploadedFile.FileName);

                using (var fstr = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(fstr);
                }
                
                return filePath;
            }
            return null;
        }

        public void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine("Deleted");
                }
                catch
                {
                    Console.WriteLine("АШИБКА!");
                }
            }
        }

        public void ParseDocument(string filePath)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                string entityName = "";

                var things = _context.Thing.ToList();

                for (int i = 2; i < int.MaxValue; i++)
                {
                    var id = worksheet.Cells[i, 5].Value;
                    var name = worksheet.Cells[i, 7].Value;
                    var price = worksheet.Cells[i, 10].Value;
                    var amount = worksheet.Cells[i, 11].Value;

                    if (worksheet.Cells[i, 1].Value == null)
                    {
                        break;
                    }

                    if (name == null)
                    {
                        entityName = worksheet.Cells[i, 1].Value.ToString();
                        entityName = entityName.Substring(0, entityName.LastIndexOf("CATA")).Trim();
                    }
                    else
                    {
                        // Update
                        if (things.Any(x => x.Id == (string)id))
                        {
                            var thing = things.Where(x => x.Id == (string)id).First();
                            thing.Price = Convert.ToInt32(price);
                            thing.Name = (string)name;

                            if (Convert.ToString(amount) != "0") thing.Existence = true;
                            else thing.Existence = false;

                            _context.Update(thing);
                        }
                        // Add
                        else
                        {
                            Thing thing = new Thing {
                                Id = (string)id,
                                Name = (string)name,
                                Price = Convert.ToInt32(price)
                            };

                            if (Convert.ToString(amount) != "0") thing.Existence = true;
                            else thing.Existence = false;

                            if(_context.Entity.Any(x => x.Name == entityName))
                            {
                                var entity = _context.Entity.Include(x => x.Categories).Where(x => x.Name == entityName).First();
                                int categoryId = entity.Categories.Where(x => x.Name == "Void").First().Id;

                                thing.CategoryId = categoryId;
                                _context.Add(thing);
                            }
                        }
                    }
                }
            }
            _context.SaveChanges();
        }
    }
}
