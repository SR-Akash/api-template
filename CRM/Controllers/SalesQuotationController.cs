//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using CRM.DTO.AccountInfo;
//using CRM.DTO.SalesQuotationDTO;
//using CRM.Helper;
//using CRM.IRepository;

//using Swashbuckle.AspNetCore.Annotations;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CRM.Controllers
//{
//    [Route("CRM/[controller]")]
//    [ApiController]
//    public class SalesQuotationController : ControllerBase
//    {
//        private readonly ISalesQuotation _IRepository;
//        private readonly AesModel _model;

//        public SalesQuotationController(ISalesQuotation IRepository, AesModel model)
//        {
//            _IRepository = IRepository;
//            _model = model;
//        }


//        [HttpPost]
//        [Route("CreateSalesQuotationHeader")]
//        [SwaggerOperation(Description = "Example {  }")]
//        public async Task<IActionResult> CreateSalesQuotationHeader(SalesQuotationCommonDTO create)
//        {
//            var msg = await _IRepository.CreateSalesQuotationHeader(create);
//            return Ok(JsonConvert.SerializeObject(msg));
//        }

//        [HttpPut]
//        [Route("ApproveSalesQuotation")]
//        [SwaggerOperation(Description = "Example {  }")]
//        public async Task<IActionResult> ApproveSalesQuotation(SalesQuotationCommonDTO approve)
//        {
//            var msg = await _IRepository.ApproveSalesQuotation(approve);
//            return Ok(JsonConvert.SerializeObject(msg));
//        }

//        [HttpPut]
//        [Route("EditSalesQuotation")]
//        [SwaggerOperation(Description = "Example {  }")]
//        public async Task<IActionResult> EditSalesQuotation(SalesQuotationCommonDTO edit)
//        {
//            var msg = await _IRepository.EditSalesQuotation(edit);
//            return Ok(JsonConvert.SerializeObject(msg));
//        }

//        [HttpPut]
//        [Route("DeleteSalesQuotation")]
//        [SwaggerOperation(Description = "Example {  }")]
//        public async Task<IActionResult> DeleteSalesQuotation(long salesQuotationId)
//        {
//            var msg = await _IRepository.DeleteSalesQuotation(salesQuotationId);
//            return Ok(JsonConvert.SerializeObject(msg));
//        }

//        [HttpGet]
//        [Route("GetSalesQuotationLandingPasignation")]
//        [SwaggerOperation(Description = "Example { }")]
//        public async Task<IActionResult> GetSalesQuotationLandingPasignation(string search, long accountId, long branchId, string viewOrder, long pageNo, long pageSize)
//        {

//            var dt = await _IRepository.GetSalesQuotationLandingPasignation(search, accountId, branchId, viewOrder, pageNo, pageSize);
//            if (dt == null)
//            {
//                return NotFound();
//            }
//            return Ok(JsonConvert.SerializeObject(dt));
//        }

//        [HttpGet]
//        [Route("GetSalesQuotationLandingPasignationNew")]
//        [SwaggerOperation(Description = "Example { }")]
//        public async Task<IActionResult> GetSalesQuotationLandingPasignationNew(string search, long accountId, long branchId, DateTime? fromdate, DateTime? toDate, string filterBy, long? supplierId, string viewOrder, long pageNo, long pageSize)
//        {

//            var dt = await _IRepository.GetSalesQuotationLandingPasignationNew(search, accountId, branchId, fromdate, toDate, filterBy, supplierId, viewOrder, pageNo, pageSize);
//            if (dt == null)
//            {
//                return NotFound();
//            }
//            return Ok(JsonConvert.SerializeObject(dt));
//        }

//        [HttpGet]
//        [Route("SalesQuotationDownload")]
//        public async Task<IActionResult> SalesQuotationDownload(string search, long accountId, long branchId, DateTime? fromdate, DateTime? toDate, string filterBy, long? supplierId, string viewOrder, long pageNo, long pageSize)
//        {
//            var data = await _IRepository.GetSalesQuotationLandingPasignationNew(search, accountId, branchId, fromdate, toDate, filterBy, supplierId, viewOrder, pageNo, pageSize);

//            return await DownloadXML.ToObjectDownload<SalesQuotationLandingHeader>("Sales Quotation List Report", data.Data);
//        }


//        [HttpGet]
//        [Route("GetSalesQuotationById")]
//        [SwaggerOperation(Description = "Example { }")]
//        public async Task<IActionResult> GetSalesQuotationById(long salesQuotationId)
//        {

//            var dt = await _IRepository.GetSalesQuotationById(salesQuotationId);
//            if (dt == null)
//            {
//                return NotFound();
//            }
//            return Ok(JsonConvert.SerializeObject(dt));
//        }

//        [HttpGet]
//        [Route("GetSalesQuotationDDL")]
//        [SwaggerOperation(Description = "Example { }")]
//        public async Task<IActionResult> GetSalesQuotationDDL(long accountId, long branchId)
//        {

//            var dt = await _IRepository.GetSalesQuotationDDL(accountId, branchId);
//            if (dt == null)
//            {
//                return NotFound();
//            }
//            return Ok(JsonConvert.SerializeObject(dt));
//        }


//        [HttpGet]
//        [Route("getCalculationForSales")]
//        [SwaggerOperation(Description = "Example { }")]
//        public async Task<IActionResult> getCalculationForSales(decimal totalPrice, decimal discountAmount, decimal vatPercent, decimal sdPercent, bool isInclusive)
//        {

//            var dt = await _IRepository.getCalculationForSales(totalPrice, discountAmount, vatPercent, sdPercent, isInclusive);
//            if (dt == null)
//            {
//                return NotFound();
//            }
//            return Ok(JsonConvert.SerializeObject(dt));
//        }
//    }
//}
