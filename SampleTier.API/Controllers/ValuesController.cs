using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SampleTier.DataAccess.Uow;
using SampleTier.API.Models;
using SampleTier.API.Utiliy;
using SampleTier.Models;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace SampleTier.API.Controllers
{

    [Route("api/[controller]")]
    public class ValuesController : Controller

    {
        private readonly IUnitOfWorkBase _uow;
        int Firstrow = 0;
        int pageSize = 3;
        //IRepository<Staff> repository; 
        public ValuesController(IUnitOfWorkBase uowProvider)
        {
            _uow = uowProvider;
        }
        
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out Firstrow);
                int.TryParse(vals[1], out pageSize);
            }

            int currentPage = (Firstrow / pageSize)+1;
            int currentPageSize = pageSize;
            var totalSchedules = _uow.StaffRepository.Count();
            var totalPages = (int)Math.Ceiling((double)totalSchedules / pageSize);
            IEnumerable<Staff> staff = _uow.StaffRepository.GetAll().Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize).ToList();
         
            if (staff != null)
            {
                Response.AddPagination(currentPage, pageSize, totalSchedules, totalPages);
                return new OkObjectResult(staff);
            }
            else
            {
                return NotFound();
            }                     
           
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public IActionResult add([FromBody]Staff value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (value != null)
                {
                    // create a new Item with the client-sent json data
                    var item = new Staff();
                    item.Id = value.Id;
                    item.Famil = value.Famil;
                    item.Name = value.Name;
                    item.Code = value.Code;
                 // add the new item
                    _uow.StaffRepository.Add(item);
                // persist the changes into the Database.
                    _uow.Save();
                return new OkResult();
                }
                // return a generic HTTP Status 500 (Not Found) if the client   payload is invalid.
                return new StatusCodeResult(500);
            
        }

        // PUT api/values/5
        [HttpPut]
        public IActionResult Put([FromBody]staffViewModel value)
        {
              if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            
                
                Staff staff = _uow.StaffRepository.GetById(s => s.Id == value.id);
                if (staff == null)
                {
                    return NotFound();
                }
                else
                {
                    staff.Name = value.name;
                    staff.Famil = value.famil;
                    staff.Code = value.code;
                    _uow.Save();
                }
                
            return new NoContentResult();
        }

      
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            
            Staff staff = _uow.StaffRepository.GetById(s => s.Id == id);
            if (staff != null)
            {
                _uow.StaffRepository.Remove(staff);
                _uow.Save();
                return new OkResult();
            }
                // return a HTTP Status 404 (Not Found) if we couldn't find a suitable item.
                return NotFound(new { Error = String.Format("Item ID {0} has not been found", id) });
            }
        public IActionResult callspsample1(int sId)
        {

            IEnumerable<Staff> staffs =
               _uow.StaffRepository.ExecQueryStoreProcedure(
               "spGetProducts @bigCategoryId",
               new SqlParameter("bigCategoryId", SqlDbType.BigInt) { Value = sId }
        );
            return Json(staffs);
        }
        public IActionResult callspsample2(int sId)
        {

            IEnumerable<Staff> staffs =
               _uow.StaffRepository.ExecQueryStoreProcedure(
               "spGetProducts @bigStaffId",
               new SqlParameter("bigStaffId", SqlDbType.BigInt) { Value = sId }
        );
            // _uow.StaffRepository.ExecQueryStoreProcedure("EXECUTE dbo.GetMostPopularStaff {0}", sId);
            // _uow.StaffRepository.ExecQueryStoreProcedure(SELECT* FROM dbo.SearchStaff {0}, sId);
            return Json(staffs);
        }
    }
    }

