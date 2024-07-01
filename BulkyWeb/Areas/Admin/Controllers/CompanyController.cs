using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
           
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
          
            if(id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
               Company companyObj = _unitOfWork.Company.Get(u=>u.Id == id);
                return View(companyObj);

            }

        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
           
            if (ModelState.IsValid)
            {
               
                if(companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                    TempData["success"] = "Company Created Sucessfully";

                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                    TempData["info"] = "Company Updated Sucessfully";

                }
                _unitOfWork.Save();
               
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);
            }
            

        }
       
        
        //[HttpPost]
        //public IActionResult Delete(int? id)
        //{

        //    Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["error"] = "Category Deleted Sucessfully";
        //    return RedirectToAction("Index");

        //}
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompany = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompany });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if(CompanyToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while Deleting"});
            }
           
            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Sucessful" });

        }

        #endregion

    }
}
