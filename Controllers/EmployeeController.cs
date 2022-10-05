using EmployeeCRUDCoreWebMVCEF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;

namespace EmployeeCRUDCoreWebMVCEF.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
        }
        public IActionResult Employee()
        {
            return View();
        }

        #region CRUD
        [HttpGet]
        public async Task<JsonResult> GetDepartments()
        {
            try
            {
                IEnumerable<Department> deptList = await _context.Departments.ToListAsync();

                if (deptList != null && deptList.Count() > 0)
                {
                    return JsonOK(deptList);
                }
            }
            catch (Exception ex)
            {
                return JsonExceptionOccurred(ex.Message);
            }
            return JsonNotFound();
        }
        [HttpGet]
        public async Task<JsonResult> GetEmployeesByDepartment(long departmentID)
        {
            try
            {
                IEnumerable<Employee> empList = await _context.Employees.Where(x => x.DepartmentId == departmentID).ToListAsync();

                if (empList != null && empList.Count() > 0)
                {
                    return JsonOK(empList);
                }
            }
            catch (Exception ex)
            {
                return JsonExceptionOccurred(ex.Message);
            }
            return JsonNotFound();
        }
        [HttpPut]
        public async Task<JsonResult> Put(string empObjIdToUpdate, string cellToUpdate, string cellValueToUpdate)
        {
            try
            {
                if (!string.IsNullOrEmpty(empObjIdToUpdate) && !string.IsNullOrEmpty(cellToUpdate) && !string.IsNullOrEmpty(cellValueToUpdate))
                {
                    long empId = Convert.ToInt64(empObjIdToUpdate);
                   
                    if (empId == 0) { return JsonNotFound(); }

                    var empExistingObjtoChange = await _context.Employees.FindAsync(empId);

                    if (empExistingObjtoChange == null) { return JsonNotFound(); }

                    switch (cellToUpdate)
                    {
                        case "Name":
                            empExistingObjtoChange.Name = cellValueToUpdate;
                            break;
                        case "Email":
                            empExistingObjtoChange.Email = cellValueToUpdate;
                            break;
                        case "Address":
                            empExistingObjtoChange.Address = cellValueToUpdate;
                            break;
                        case "Position":
                            empExistingObjtoChange.Position = cellValueToUpdate;
                            break;
                        case "ContactNo":
                            empExistingObjtoChange.ContactNo = Convert.ToInt64(cellValueToUpdate);
                            break;
                    }

                    await _context.SaveChangesAsync();
                    return JsonActionOK();
                }
            }
            catch (Exception ex)
            {
                return JsonExceptionOccurred(ex.Message);
            }
            return JsonNotSave();
        }
        [HttpDelete]
        public async Task<JsonResult> Delete(string empObjIdToUpdate)
        {
            try
            {
                if (!string.IsNullOrEmpty(empObjIdToUpdate))
                {
                    long empId = Convert.ToInt64(empObjIdToUpdate);

                    if (empId < 1) { return JsonBadRequest(); }

                    var emp = await _context.Employees.FindAsync(empId);
                    if (emp == null) { return JsonNotFound(); }

                    _context.Employees.Remove(emp);
                    await _context.SaveChangesAsync();
                    return JsonActionOK();
                }
            }
            catch (Exception ex)
            {
                return JsonExceptionOccurred(ex.Message);
            }

            return JsonBadRequest();
        }
        [HttpPost]
        public async Task<JsonResult> Create(string empObjStr)
        {
            try
            {
                if(!string.IsNullOrEmpty(empObjStr))
                {
                    Employee emp = JsonConvert.DeserializeObject<Employee>(empObjStr);

                    if (emp != null)
                    {
                        _context.Add(emp);
                        await _context.SaveChangesAsync();
                        return JsonActionOK(emp.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonExceptionOccurred(ex.Message);
            }
            return JsonBadRequest();
        }
        #endregion

        #region JSON RETURN 
        JsonResult JsonNotFound()
        {
            return Json(new
            {
                returnStatus = "Fail",
                returnMsg = "No data found. Please try again later.",
                data = string.Empty
            });
        }
        JsonResult JsonExceptionOccurred(string msg)
        {
            return Json(new
            {
                returnStatus = "ErrorOccurred",
                returnMsg = "Error occurred." + msg,
                data = string.Empty
            });
        }
        JsonResult JsonOK(IEnumerable<object> objList)
        {
            return Json(new
            {
                returnStatus = "OK",
                returnMsg = string.Empty,
                data = JsonConvert.SerializeObject(objList)
            });
        }
        JsonResult JsonActionOK()
        {
            return Json(new
            {
                returnStatus = "OK",
                returnMsg = string.Empty
            });
        }
        JsonResult JsonActionOK(long objId)
        {
            return Json(new
            {
                returnStatus = "OK",
                returnMsg = string.Empty,
                Id = objId
            });
        }
        JsonResult JsonBadRequest()
        {
            return Json(new
            {
                returnStatus = "Not OK",
                returnMsg = "Please try again.."
            });
        }
        JsonResult JsonNotSave()
        {
            return Json(new
            {
                returnStatus = "Fail",
                returnMsg = "No data saved. Please try again later.",
                data = string.Empty
            });
        }
        #endregion
    }
}
