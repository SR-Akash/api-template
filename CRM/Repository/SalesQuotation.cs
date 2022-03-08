//using CRM.DbContexts;
//using CRM.DTO.AccountInfo;
//using CRM.DTO.SalesOrder;
//using CRM.DTO.SalesQuotationDTO;
//using CRM.Helper;
//using CRM.IRepository;
//using CRM.Models.Write;
//using CRM.StoredProcedure;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//#pragma warning disable

//namespace CRM.Repository
//{
//    public class SalesQuotation : ISalesQuotation
//    {
//        public readonly ReadDbContext _contextR;
//        public readonly WriteDbContext _contextW;
//        CodeGenerator con = new CodeGenerator();

//        public SalesQuotation(ReadDbContext contextR, WriteDbContext contextW)
//        {
//            _contextR = contextR;
//            _contextW = contextW;
//        }

//        public async Task<MessageHelper> ApproveSalesQuotation(SalesQuotationCommonDTO approve)
//        {
//            try
//            {
//                var header = await Task.FromResult((from a in _contextW.SalesQuotationHeaders
//                                                    where a.SalesQuotationId == approve.header.SalesQuotationId
//                                                    && a.IsActive == true
//                                                    select a).FirstOrDefault());
//                if (header == null)
//                {
//                    throw new Exception("Approve Data Not Found");
//                }

//                header.IsApprove = true;

//                _contextW.SalesQuotationHeaders.Update(header);
//                await _contextW.SaveChangesAsync();

//                var rowData = await Task.FromResult((from a in _contextW.SalesQuotationRows
//                                                     where a.SalesQuotationHeaderId == header.SalesQuotationId
//                                                     select a).ToList());

//                rowData.ForEach(x => { x.ApproveQuantity = x.Quantity; x.ApprovePrice = x.Price; x.ApproveAmount = x.TotalAmount; x.TotalValue = x.TotalValue; });

//                _contextW.SalesQuotationRows.UpdateRange(rowData);
//                await _contextW.SaveChangesAsync();

//                var msg = new MessageHelper
//                {
//                    Message = "Approve Successfully",
//                    statuscode = 200
//                };

//                return msg;

//                //var rowList = new List<Models.Write.SalesQuotationRow>();

//                //foreach (var itm in approve.row)
//                //{
//                //    var rowData = _contextW.SalesQuotationRows.Where(x => x.SalesQuotationHeaderId == header.SalesQuotationId
//                //    && x.ItemId == itm.ItemId && x.IsActive == true).FirstOrDefault();
//                //    if (rowData == null)
//                //    {
//                //        throw new Exception(itm.ItemName + "- This item not found for Update");
//                //    }

//                //    rowData.ApproveAmount = itm.ApproveAmount;
//                //    rowData.ApprovePrice = itm.ApprovePrice;
//                //    rowData.ApproveQuantity = itm.ApproveQuantity;
//                //    rowData.Sdpercentage = itm.SdPercentage;
//                //    rowData.Sdamount = itm.SdAmount;
//                //    rowData.Vatpercentage = itm.VatPercentage;
//                //    rowData.Vatamount = itm.VatAmount;
//                //    rowData.TotalValue = itm.TotalValue;
//                //    rowData.ItemDescription = itm.ItemDescription;

//                //    rowList.Add(rowData);
//                //}

//                //_contextW.SalesQuotationRows.UpdateRange(rowList);
//                //await _contextW.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<MessageHelper> CreateSalesQuotationHeader(SalesQuotationCommonDTO create)
//        {
//            try
//            {
//                if (create.row.Count() <= 0)
//                {
//                    throw new Exception("Add at least One Item");
//                }

//                var header = create.header;
//                var code = con.getCode(header.AccountId, "Sales Quotation", "SQ-", DateTime.Now);

//                var headerData = new Models.Write.SalesQuotationHeader
//                {
//                    SalesQuotationCode = code,
//                    AccountId = header.AccountId,
//                    BranchId = header.BranchId,
//                    PartnerId = header.PartnerId,
//                    PartnerName = header.PartnerName,
//                    QuotationDate = DateTime.Now,
//                    ValidityDate = header.ValidityDate,
//                    Narration = header.Narration,
//                    ActionBy = header.ActionBy,
//                    IsActive = true,
//                    LastActionDateTime = DateTime.Now,
//                    IsApprove = false,
//                    OfficeId = header.OfficeId,
//                    IsVatinclusive = header.IsInclusive
//                };

//                await _contextW.SalesQuotationHeaders.AddAsync(headerData);
//                await _contextW.SaveChangesAsync();

//                var quotationItemList = new List<Models.Write.SalesQuotationRow>();

//                foreach (var itm in create.row)
//                {
//                    var rowData = new Models.Write.SalesQuotationRow
//                    {
//                        SalesQuotationHeaderId = headerData.SalesQuotationId,
//                        ItemId = itm.ItemId,
//                        ItemCode = itm.ItemCode,
//                        ItemName = itm.ItemName,
//                        ItemDescription = itm.ItemDescription,
//                        Quantity = itm.Quantity,
//                        Price = itm.Price,
//                        UomId = itm.UomId,
//                        UomName = itm.UomName,
//                        IsActive = true,
//                        Sdamount = itm.SdAmount,
//                        Sdpercentage = itm.SdPercentage,
//                        Vatamount = itm.VatAmount,
//                        Vatpercentage = itm.VatPercentage,
//                        TotalValue = itm.TotalValue,
//                        ApproveAmount = 0,
//                        ApproveQuantity = 0,
//                        ApprovePrice = 0
//                    };

//                    quotationItemList.Add(rowData);
//                }

//                await _contextW.SalesQuotationRows.AddRangeAsync(quotationItemList);
//                await _contextW.SaveChangesAsync();

//                var msg = new MessageHelper
//                {
//                    Message = headerData.SalesQuotationCode,
//                    statuscode = 200
//                };

//                return msg;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<MessageHelper> DeleteSalesQuotation(long salesQuotationId)
//        {
//            try
//            {
//                var data = await Task.FromResult((from a in _contextW.SalesQuotationHeaders
//                                                  where a.SalesQuotationId == salesQuotationId
//                                                  && a.IsActive == true
//                                                  select a).FirstOrDefault());
//                if (data == null)
//                {
//                    throw new Exception("Delete Data Not Found");
//                }
//                if (data.IsSo == true)
//                {
//                    throw new Exception("You can not delete this SQ because Sales Order Already Created with this SQ");
//                }

//                data.IsActive = false;

//                _contextW.SalesQuotationHeaders.Update(data);
//                await _contextW.SaveChangesAsync();

//                var msg = new MessageHelper
//                {
//                    Message = "Sales Quotation [ " + data.SalesQuotationCode + " ] Deleted",
//                    statuscode = 200
//                };

//                return msg;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<MessageHelper> EditSalesQuotation(SalesQuotationCommonDTO edit)
//        {
//            try
//            {
//                var headerUpdate = await Task.FromResult((from a in _contextW.SalesQuotationHeaders
//                                                          where a.SalesQuotationId == edit.header.SalesQuotationId
//                                                          && a.IsActive == true
//                                                          select a).FirstOrDefault());

//                headerUpdate.Narration = edit.header.Narration;
//                headerUpdate.QuotationDate = DateTime.Now;
//                headerUpdate.ValidityDate = edit.header.ValidityDate;
//                headerUpdate.ActionBy = edit.header.ActionBy;
//                headerUpdate.IsVatinclusive = edit.header.IsInclusive;


//                _contextW.SalesQuotationHeaders.Update(headerUpdate);
//                await _contextW.SaveChangesAsync();

//                var mRowsNew = new List<Models.Write.SalesQuotationRow>(edit.row.Count());
//                var mRowsEdit = new List<Models.Write.SalesQuotationRow>(edit.row.Count());

//                foreach (var itm in edit.row)
//                {
//                    if (itm.SalesQuotationRowId == 0)
//                    {
//                        var rowData = new Models.Write.SalesQuotationRow
//                        {
//                            SalesQuotationHeaderId = headerUpdate.SalesQuotationId,
//                            ItemId = itm.ItemId,
//                            ItemCode = itm.ItemCode,
//                            ItemName = itm.ItemName,
//                            ItemDescription = itm.ItemDescription,
//                            //Narration = itm.Remarks,
//                            Quantity = itm.Quantity,
//                            Price = itm.Price,
//                            UomId = itm.UomId,
//                            UomName = itm.UomName,
//                            IsActive = true,
//                            Sdamount = itm.SdAmount,
//                            Sdpercentage = itm.SdPercentage,
//                            Vatamount = itm.VatAmount,
//                            Vatpercentage = itm.VatPercentage,
//                            TotalValue = itm.TotalValue,
//                            ApproveAmount = 0,
//                            ApprovePrice = 0,
//                            ApproveQuantity = 0
//                        };

//                        mRowsNew.Add(rowData);
//                    }
//                    else
//                    {
//                        var detalisrow = _contextW.SalesQuotationRows.Where(x => x.SalesQuotationRowId == itm.SalesQuotationRowId && x.IsActive == true).FirstOrDefault();

//                        if (detalisrow != null)
//                        {
//                            detalisrow.ItemId = itm.ItemId;
//                            detalisrow.ItemCode = itm.ItemCode;
//                            detalisrow.ItemName = itm.ItemName;
//                            detalisrow.ItemDescription = itm.ItemDescription;
//                            detalisrow.Quantity = itm.Quantity;
//                            detalisrow.Price = itm.Price;
//                            detalisrow.TotalAmount = itm.Quantity * itm.Price;
//                            detalisrow.TotalValue = itm.TotalValue;

//                            mRowsEdit.Add(detalisrow);
//                        }
//                    }

//                    var innerquery = from c in edit.row
//                                     where c.SalesQuotationRowId > 0
//                                     select c.SalesQuotationRowId;

//                    var inactiveItems = (from p in _contextW.SalesQuotationRows
//                                         where p.SalesQuotationHeaderId == headerUpdate.SalesQuotationId && !innerquery.Contains(p.SalesQuotationRowId)
//                                         select p).ToList();

//                    foreach (var a in inactiveItems)
//                    {
//                        a.IsActive = false;
//                    }

//                    _contextW.SalesQuotationRows.UpdateRange(inactiveItems);
//                    await _contextW.SaveChangesAsync();
//                }

//                if (mRowsNew.Count > 0)
//                {
//                    await _contextW.SalesQuotationRows.AddRangeAsync(mRowsNew);
//                    await _contextW.SaveChangesAsync();
//                }
//                if (mRowsEdit.Count > 0)
//                {
//                    _contextW.SalesQuotationRows.UpdateRange(mRowsEdit);
//                    await _contextW.SaveChangesAsync();
//                }

//                var msg = new MessageHelper
//                {
//                    Message = "Update Successfully",
//                    statuscode = 200
//                };

//                return msg;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<CalculationForSalesDTO> getCalculationForSales(decimal totalPrice, decimal discountAmount, decimal vatPercent, decimal sdPercent, bool isInclusive)
//        {
//            try
//            {
//                var data = VATSDCalculationForSales.CalculationForSales(totalPrice, discountAmount, vatPercent, sdPercent, isInclusive);
//                return data;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<SalesQuotationCommonDTO> GetSalesQuotationById(long salesQuotationId)
//        {
//            try
//            {
//                var headerData = await Task.FromResult((from a in _contextR.SalesQuotationHeaders
//                                                        join o in _contextR.BusinessOffices on a.OfficeId equals o.OfficeId
//                                                        where a.SalesQuotationId == salesQuotationId

//                                                        select new SalesQuotationHeaderDTO
//                                                        {
//                                                            SalesQuotationId = a.SalesQuotationId,
//                                                            SalesQuotationCode = a.SalesQuotationCode,
//                                                            AccountId = a.AccountId,
//                                                            BranchId = a.BranchId,
//                                                            PartnerId = a.PartnerId,
//                                                            PartnerName = a.PartnerName,
//                                                            QuotationDate = a.QuotationDate,
//                                                            ValidityDate = a.ValidityDate,
//                                                            Narration = a.Narration,
//                                                            ActionBy = a.ActionBy,
//                                                            IsExpierd = a.IsExpierd,
//                                                            IsSO = a.IsSo,
//                                                            ParnerPhone = (from b in _contextR.Partners
//                                                                           where b.PartnerId == a.PartnerId
//                                                                           select b.MobileNo).FirstOrDefault(),
//                                                            IsApprove = a.IsApprove,
//                                                            IsActive = a.IsActive,
//                                                            OfficeId = a.OfficeId,
//                                                            IsInclusive = a.IsVatinclusive,
//                                                            OfficeName = o.OfficeName,
//                                                            Totalvalue = (from b in _contextR.SalesQuotationRows
//                                                                          where a.SalesQuotationId == b.SalesQuotationHeaderId
//                                                                          && b.IsActive == true
//                                                                          select b.TotalValue).Sum()
//                                                        }).FirstOrDefault());

//                var rowData = await Task.FromResult((from b in _contextR.SalesQuotationRows
//                                                     join a in _contextR.Items on b.ItemId equals a.ItemId into aa
//                                                     from a in aa.DefaultIfEmpty()

//                                                     where b.SalesQuotationHeaderId == salesQuotationId

//                                                     select new SalesQuotationRowDTO
//                                                     {
//                                                         SalesQuotationRowId = b.SalesQuotationRowId,
//                                                         SalesQuotationHeaderId = b.SalesQuotationHeaderId,
//                                                         ItemId = b.ItemId,
//                                                         ItemCode = a.ItemCode,
//                                                         ItemDescription = b.ItemDescription,
//                                                         ItemName = b.ItemName,
//                                                         //Remarks = b.Narration,
//                                                         Quantity = b.Quantity,
//                                                         Price = b.Price,
//                                                         UomId = b.UomId,
//                                                         UomName = b.UomName,
//                                                         TotalAmount = b.TotalAmount,
//                                                         Brand = a.Brand,
//                                                         SdAmount = b.Sdamount,
//                                                         SdPercentage = b.Sdpercentage,
//                                                         VatAmount = b.Vatamount,
//                                                         VatPercentage = b.Vatpercentage,
//                                                         TotalValue = b.TotalValue,
//                                                         ApproveAmount = b.ApproveAmount,
//                                                         ApproveQuantity = b.ApproveQuantity,
//                                                         ApprovePrice = b.ApprovePrice
//                                                     }).ToList());

//                return new SalesQuotationCommonDTO
//                {
//                    header = headerData,
//                    row = rowData
//                };
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<List<SalesQuotationDDL>> GetSalesQuotationDDL(long accountId, long branchId)
//        {
//            try
//            {
//                //var index = data.IndexOf(':');
//                //data = data.Remove(index);

//                var data = await Task.FromResult((from a in _contextR.SalesQuotationHeaders
//                                                  join p in _contextR.Partners on a.PartnerId equals p.PartnerId
//                                                  where a.AccountId == accountId
//                                                  && a.BranchId == branchId
//                                                  && a.IsActive == true
//                                                  && a.ValidityDate.Date >= DateTime.Now.Date

//                                                  && a.IsApprove == true
//                                                  select new SalesQuotationDDL
//                                                  {
//                                                      Value = a.SalesQuotationId,
//                                                      Label = a.SalesQuotationCode,
//                                                      PartnerId = a.PartnerId,
//                                                      PartnerName = a.PartnerName,
//                                                      PartnerMobileNo = p.MobileNo,
//                                                      IsInclusive = a.IsVatinclusive,
//                                                      OfficeId = a.OfficeId,
//                                                      OfficeName = (from b in _contextR.BusinessOffices
//                                                                    where a.OfficeId == b.OfficeId
//                                                                    select b.OfficeName).FirstOrDefault(),
//                                                      itemData = (from b in _contextR.SalesQuotationRows
//                                                                  where b.SalesQuotationHeaderId == a.SalesQuotationId
//                                                                  && b.IsActive == true
//                                                                  select new ItemInfoForSalesOrderDTO
//                                                                  {
//                                                                      ItemId = b.ItemId,
//                                                                      ItemCode = b.ItemCode,
//                                                                      ItemName = b.ItemName,
//                                                                      ItemDescription = b.ItemDescription,
//                                                                      //Narration = b.Narration,
//                                                                      Quantity = b.Quantity,
//                                                                      Price = b.Price,
//                                                                      UomId = b.UomId,
//                                                                      UomName = b.UomName,
//                                                                      TotalAmount = b.TotalAmount ?? 0,
//                                                                      SDAmount = b.Sdamount,
//                                                                      SDPercentage = b.Sdpercentage,
//                                                                      VATAmount = b.Vatamount,
//                                                                      VATPercentage = b.Vatpercentage,
//                                                                      TotalValue = b.TotalValue,
//                                                                      ApproveAmount = b.ApproveAmount,
//                                                                      ApprovePrice = b.ApprovePrice,
//                                                                      ApproveQuantity = b.ApproveQuantity
//                                                                  }).ToList()

//                                                  }).OrderByDescending(x => x.Value).ToList());

//                return data;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<SalesQuotationLandingPasignationDTO> GetSalesQuotationLandingPasignation(string search, long accountId, long branchId, string viewOrder, long pageNo, long pageSize)
//        {
//            try
//            {
//                var data = await Task.FromResult((from i in _contextR.SalesQuotationHeaders
//                                                  where i.AccountId == accountId && i.BranchId == branchId && i.IsActive == true
//                                                  group new { i } by new { i.PartnerId } into gr
//                                                  select new SalesQuotationLandingHeader()
//                                                  {

//                                                      PartnerId = gr.Key.PartnerId,
//                                                      PartnerName = (from a in _contextR.Partners
//                                                                     where a.PartnerId == gr.Key.PartnerId
//                                                                     select a.PartnerName).FirstOrDefault()
//                                                      //don't take partner name from SalesQuotationHeaders this table it will effect in report
//                                                  }));

//                if (search != null)
//                {
//                    search = search.Trim();
//                    search = search.ToLower();

//                    data = data.Where(x => x.PartnerName.Contains(search));
//                }

//                if (data == null)
//                    throw new Exception("Data Not Found.");

//                data = viewOrder.ToUpper() switch
//                {
//                    "ASC" => data.OrderBy(o => o.PartnerId),
//                    "DESC" => data.OrderByDescending(o => o.PartnerId),
//                    _ => data
//                };

//                if (pageNo <= 0)
//                    pageNo = 1;
//                var itemdata = PagingList<SalesQuotationLandingHeader>.CreateAsync(data, pageNo, pageSize);

//                long index = (int)(1 + ((pageNo - 1) * pageSize));
//                foreach (var item in itemdata)
//                {
//                    item.Sl = index++;

//                    item.objRow = (from a in _contextR.SalesQuotationHeaders
//                                   where a.PartnerId == item.PartnerId
//                                   select new SalesQuotationLandingRow
//                                   {
//                                       SalesQuotationId = a.SalesQuotationId,
//                                       SalesQuotationCode = a.SalesQuotationCode,
//                                       QuotationDate = a.QuotationDate,
//                                       ValidityDate = a.ValidityDate,
//                                       Narration = a.Narration,
//                                       TotalQty = (from b in _contextR.SalesQuotationRows
//                                                   where b.SalesQuotationHeaderId == a.SalesQuotationId && b.IsActive == true
//                                                   select b.Quantity).Sum(),
//                                       Totalvalue = (from b in _contextR.SalesQuotationRows
//                                                     where b.SalesQuotationHeaderId == a.SalesQuotationId && b.IsActive == true
//                                                     select b.Quantity * b.Price).Sum(),
//                                       isSO = a.IsSo
//                                   }).ToList();

//                }


//                var itm = new SalesQuotationLandingPasignationDTO
//                {
//                    Data = itemdata,
//                    CurrentPage = pageNo,
//                    TotalCount = data.Count(),
//                    PageSize = pageSize
//                };
//                return itm;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        public static string getFinalStatus(long quotationId, WriteDbContext _contextW)
//        {
//            try
//            {
//                var invRequest = (from a in _contextW.SalesQuotationHeaders
//                                  where a.SalesQuotationId == quotationId

//                                  select a).FirstOrDefault();

//                if (invRequest == null)
//                    return "Deleted";

//                if (invRequest.IsActive == false)
//                    return "Cancelled";

//                if (invRequest.IsApprove == false && invRequest.IsActive == true)
//                    return "Pending";

//                if (invRequest.IsActive == true && invRequest.IsApprove == true)
//                    return "Approved";
//                else
//                    return "";
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<SalesQuotationLandingPasignationDTO> GetSalesQuotationLandingPasignationNew(string search, long accountId, long branchId, DateTime? fromdate, DateTime? toDate, string filterBy, long? supplierId, string viewOrder, long pageNo, long pageSize)
//        {
//            try
//            {
//                var data = await Task.FromResult((from i in _contextR.SalesQuotationHeaders
//                                                  join prt in _contextR.Partners on i.PartnerId equals prt.PartnerId
//                                                  where i.AccountId == accountId
//                                                  && i.BranchId == branchId
//                                                  && i.ValidityDate.Date >= DateTime.Now.Date
//                                                  && (fromdate == null || i.QuotationDate.Date >= fromdate.Value.Date)
//                                                  && (toDate == null || i.QuotationDate.Date <= toDate.Value.Date)
//                                                  && (supplierId == null || supplierId == i.PartnerId)

//                                                  select new SalesQuotationLandingHeader()
//                                                  {

//                                                      PartnerId = i.PartnerId,
//                                                      PartnerName = prt.PartnerName,
//                                                      Mobile = prt.MobileNo,
//                                                      SalesQuotationId = i.SalesQuotationId,
//                                                      SalesQuotationCode = i.SalesQuotationCode,
//                                                      QuotationDate = i.QuotationDate,
//                                                      ValidityDate = i.ValidityDate,
//                                                      Narration = i.Narration,
//                                                      TotalValue = (from b in _contextR.SalesQuotationRows
//                                                                    where b.SalesQuotationHeaderId == i.SalesQuotationId && b.IsActive == true
//                                                                    select b.TotalValue).Sum(),
//                                                      IsInclusive = i.IsVatinclusive,
//                                                      isSO = i.IsSo,
//                                                      IsApprove = i.IsApprove,
//                                                      OfficeId = i.OfficeId,
//                                                      OfficeName = _contextR.BusinessOffices.Where(x => x.OfficeId == x.OfficeId).Select(x => x.OfficeName).FirstOrDefault(),
//                                                      FinalStatus = getFinalStatus(i.SalesQuotationId, _contextW),
//                                                      IsActive = i.IsActive

//                                                  }));

//                if (search != null)
//                {
//                    search = search.Trim();
//                    search = search.ToLower();

//                    data = data.Where(x => x.PartnerName.Contains(search) || x.SalesQuotationCode.Contains(search));
//                }

//                if (search == null && filterBy != null)
//                {
//                    if (filterBy.Trim().ToLower() == "none")
//                        data = data;
//                    else if (filterBy.Trim().ToLower() == "approved")
//                    {
//                        data = data.Where(x => x.IsApprove == true);
//                    }
//                    else if (filterBy.Trim().ToLower() == "pending")
//                    {
//                        data = data.Where(x => x.IsApprove == false);
//                    }
//                    else if (filterBy.Trim() == "soCreated")
//                    {
//                        data = data.Where(x => x.isSO == true);
//                    }
//                    else
//                    {
//                        data = data.Where(x => x.isSO == false);
//                    }
//                }

//                if (data == null)
//                    throw new Exception("Data Not Found.");

//                data = viewOrder.ToUpper() switch
//                {
//                    "ASC" => data.OrderBy(o => o.SalesQuotationId),
//                    "DESC" => data.OrderByDescending(o => o.SalesQuotationId),
//                    _ => data
//                };

//                if (pageNo <= 0)
//                    pageNo = 1;
//                var itemdata = PagingList<SalesQuotationLandingHeader>.CreateAsync(data, pageNo, pageSize);

//                long index = (int)(1 + ((pageNo - 1) * pageSize));
//                foreach (var item in itemdata)
//                {
//                    item.Sl = index++;
//                }

//                var itm = new SalesQuotationLandingPasignationDTO
//                {
//                    Data = itemdata,
//                    CurrentPage = pageNo,
//                    TotalCount = data.Count(),
//                    PageSize = pageSize
//                };
//                return itm;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//    }
//}
