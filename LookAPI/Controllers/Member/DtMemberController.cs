using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenHelper;
using LookDAL.Mem.DAL;
using LookDB.Model.Member;
using LookWeb.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LookAPI.Controllers.Member
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("GenProjCORS")]
    public class DtMemberController : Controller
    {
        GeneralController general = new GeneralController();
        DALDtMember dtMember = new DALDtMember();

        [HttpGet]
        public string Get()
        {
            return "OK";
        }
        public IActionResult Index()
        {
            return View();
        }
      
        [Route("DeleteListAsync")]
        [HttpPost("{deleteByIdList}")]
        public async Task<IActionResult> DeleteListAsync([FromBody] DeleteByIDList deleteByIdList)
        {
            var hasil = await general.HapusActiveBoolAsync<DtMember>(deleteByIdList.IdentityId, deleteByIdList.UserByName);
            return Ok(hasil);
        }
        [Route("SimpanListAsync")]
        [HttpPost("{listEntity}")]
        public async Task<IActionResult> SimpanListAsync([FromBody]List<DtMember> listEntity)
        {
            var hasil = await general.SimpanAsync(listEntity);
            return Ok(hasil);
        }
        [Route("UpdateListAsync")]
        [HttpPost("{listEntity}")]
        public async Task<IActionResult> UpdateListAsync([FromBody]List<DtMember> listEntity)
        {
            var hasil = await general.UbahAsync(listEntity);
            return Ok(hasil);
        }
        [Route("ListDataAsync")]
        [HttpPost("{searchParam}")]
        public async Task<IActionResult> ListDataAsync([FromBody]SearchParameter searchParam)
        {
            var hasil = await general.ListDataAsync<DtMember, VW_DtMember>(searchParam.SearchFieldList, searchParam.SortColumn, searchParam.IsAscending, searchParam.TopTake);
            return Ok(hasil);
        }
        [Route("GetSearchByIdAsync")]
        [HttpGet]
        public async Task<IActionResult> GetSearchByIdAsync(int id)
        {
            var hasil = await general.SearchByIdAsync<DtMember>(id);
            return Ok(hasil);
        }
        [Route("GetMemberInfoByIdAsync")]
        [HttpGet]
        public async Task<IActionResult> GetMemberInfoByIdAsync(int id)
        {
            var kembalian = new APIReturn();           
            var helper = new Helper();
            var hasil = await dtMember.SearchMemberAllInfoByMemberId(id);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;    
            var hasil2 = helper.CompressedData(kembalian);
            return Ok(hasil2);
        }
    }
}