using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.Pocos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCloud.WebAPI.Controllers
{
    [Route("api/careercloud/applicant/v1")]
    [ApiController]
    public class ApplicantWorkHistoryController : ControllerBase
    {
        private readonly ApplicantWorkHistoryLogic _logic;

        public ApplicantWorkHistoryController()
        {
            var repo = new EFGenericRepository<ApplicantWorkHistoryPoco>();
            _logic = new ApplicantWorkHistoryLogic(repo);
        }

        [HttpGet]
        [Route("workhistory/{id}")]
        public ActionResult GetApplicantWorkHistory(Guid id)
        {
            ApplicantWorkHistoryPoco poco = _logic.Get(id);
            if (poco == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(poco);
            }
        }

        [HttpGet]
        [Route("workhistory")]
        public ActionResult GetApplicantWorkHistory()
        {
            var poco = _logic.GetAll();
            if (poco == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(poco);
            }
        }

        [HttpPost]
        [Route("workhistory")]
        public ActionResult PostApplicantWorkHistory([FromBody]ApplicantWorkHistoryPoco[] pocos)
        {
            _logic.Add(pocos);
            return Ok();
        }

        [HttpPut]
        [Route("workhistory")]
        public ActionResult PutApplicantWorkHistory([FromBody]ApplicantWorkHistoryPoco[] pocos)
        {
            _logic.Update(pocos);
            return Ok();
        }

        [HttpDelete]
        [Route("workhistory")]
        public ActionResult DeleteApplicantWorkHistory([FromBody]ApplicantWorkHistoryPoco[] pocos)
        {
            _logic.Delete(pocos);
            return Ok();
        }
    }
}