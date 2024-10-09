using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeesManagement.Context;
using EmployeesManagement.Models;

namespace EmployeesManagement.Views
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string searchString)
        {
            var employees = from e in _context.Employees
                            select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.FirstName.Contains(searchString) ||
                                                 e.MidleName.Contains(searchString) ||
                                                 e.LastName.Contains(searchString) ||
                                                 e.EmailAddress.Contains(searchString) ||
                                                 e.PhoneNumber.ToString().Contains(searchString));
            }

            return View(await employees.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MidleName,LastName,EmailAddress,PhoneNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();

                // log activity: mencatat penambahan karyawan baru
                await LogActivity("Add Employee", $"Added {employee.FirstName} {employee.LastName}");

                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MidleName,LastName,EmailAddress,PhoneNumber")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                    // log activity: buat mencatat perubahan data karyawan
                    await LogActivity("Update Employee", $"Updated {employee.FirstName} {employee.LastName}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);

                // log activity: mencatat penghapusan data karyawan
                await LogActivity("Delete Employee", $"Deleted {employee.FirstName} {employee.LastName}");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // fungsi untuk catat log activity ke dalam tabel ActivityLog
        private async Task LogActivity(string action, string details)
        {
            var log = new ActivityLog
            {
                Action = action,
                Details = details,
                IPAddress = GetUserIpAddress(),  // get IP Address pengguna
                ActionTimes = DateTime.Now 
            };

            _context.ActivityLogs.Add(log); 
            await _context.SaveChangesAsync(); 
        }

        // fungsi buat dapetin IP Address user
        private string GetUserIpAddress()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            return remoteIpAddress != null ? remoteIpAddress.ToString() : "Unknown";
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        // GET: Employees/ActivityLogs
        public async Task<IActionResult> ActivityLogs()
        {
            var activityLogs = await _context.ActivityLogs.ToListAsync();
            return View(activityLogs);
        }
    }
}
