//using CRM.DTO.SalesOrder;
//using CRM.DTO.SalesQuotationDTO;
//using CRM.Helper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CRM.IRepository
//{
//    public interface ISalesQuotation
//    {
//        public Task<MessageHelper> CreateSalesQuotationHeader(SalesQuotationCommonDTO create);
//        public Task<MessageHelper> ApproveSalesQuotation(SalesQuotationCommonDTO approve);
//        public Task<MessageHelper> EditSalesQuotation(SalesQuotationCommonDTO edit);
//        public Task<MessageHelper> DeleteSalesQuotation(long salesQuotationId);
//        public Task<SalesQuotationLandingPasignationDTO> GetSalesQuotationLandingPasignation(string search, long accountId, long branchId, string viewOrder, long pageNo, long pageSize);
//        public Task<SalesQuotationLandingPasignationDTO> GetSalesQuotationLandingPasignationNew(string search, long accountId, long branchId, DateTime? fromdate, DateTime? toDate, string filterBy,long? supplierId, string viewOrder, long pageNo, long pageSize);
//        public Task<SalesQuotationCommonDTO> GetSalesQuotationById(long salesQuotationId);
//        public Task<List<SalesQuotationDDL>> GetSalesQuotationDDL(long accountId, long branchId);

//        public Task<CalculationForSalesDTO> getCalculationForSales(decimal totalPrice, decimal discountAmount, decimal vatPercent, decimal sdPercent, bool isInclusive);
//    }
//}
