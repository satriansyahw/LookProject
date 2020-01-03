﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenHelper;
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
    public class DtCertificationController : Controller
    {
        GeneralController general = new GeneralController();

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
            var hasil = await general.HapusActiveBoolAsync<DtCertification>(deleteByIdList.IdentityId, deleteByIdList.UserByName);
            return Ok(hasil);
        }
        [Route("SimpanListAsync")]
        [HttpPost("{listEntity}")]
        public async Task<IActionResult> SimpanListAsync([FromBody]List<DtCertification> listEntity)
        {
            var hasil = await general.SimpanAsync(listEntity);
            return Ok(hasil);
        }
        [Route("UpdateListAsync")]
        [HttpPost("{listEntity}")]
        public async Task<IActionResult> UpdateListAsync([FromBody]List<DtCertification> listEntity)
        {
            var hasil = await general.UbahAsync(listEntity);
            return Ok(hasil);
        }
        [Route("ListDataAsync")]
        [HttpPost("{searchParam}")]
        public async Task<IActionResult> ListDataAsync([FromBody]SearchParameter searchParam)
        {
            var hasil = await general.ListDataAsync<DtCertification, VW_DtCertification>(searchParam.SearchFieldList, searchParam.SortColumn, searchParam.IsAscending, searchParam.TopTake);
            return Ok(hasil);
        }
        [Route("GetSearchByIdAsync")]
        [HttpGet]
        public async Task<IActionResult> GetSearchByIdAsync(int id)
        {
            var hasil = await general.SearchByIdAsync<DtCertification>(id);
            return Ok(hasil);
        }
    }
}