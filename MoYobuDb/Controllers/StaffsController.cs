using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Controllers
{
    public class StaffsController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("staffId,birthPlace,birthday,name,nameNative,surname,surnameNative")]
            Staff staff)
        {
            StaffDao staffDao = new StaffDao();
            try
            {
                staffDao.Save(staff);
            }
            catch (OracleException e)
            {
                Debug.WriteLine("Chyba uložení do databáze\n{0}", e);
                return View(staff);
            }

            return RedirectToAction("Index", "Administration");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, string name)
        {
            StaffDao staffDao = new StaffDao();

            Staff staff;

            try
            {
                staff = staffDao.FindById(id);
            }
            catch (Exception)
            {
                return NotFound();
            }


            Debug.WriteLine("itemhere");
            return View(staff);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("staffId,staffImg,name,nameNative,surname,surnameNative")]
            Staff staff)
        {
            StaffDao staffDao = new StaffDao();

            staffDao.Edit(staff);
            return RedirectToAction("Index", "Administration");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            StaffDao staffDao = new StaffDao();

            return View(staffDao.FindById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, Staff staff)
        {
            Debug.WriteLine("dlt-id");
            Debug.WriteLine(id);
            Debug.WriteLine("dlt-id2");
            Debug.WriteLine(staff.staffId);
            StaffDao staffDao = new StaffDao();
            staffDao.Delete(staff);

            return RedirectToAction("Index", "Administration");
        }
    }
}