using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Product;
using LY.Report.Core.Model.Product.Enum;
using LY.Report.Core.Repository.Product.Abnormals;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Product.Abnormals.Input;
using LY.Report.Core.Service.Product.Abnormals.Output;
using System; 
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Product.Abnormals
{
    public class ProductAbnormalService : BaseService, IProductAbnormalService
    {
        private readonly IProductAbnormalRepository _productAbnormalRepository;
        private readonly IProductAbnormalPersonRepository _productAbnormalPersonRepository;

        public ProductAbnormalService(IProductAbnormalRepository repository, IProductAbnormalPersonRepository productAbnormalPersonRepository)
        {
            _productAbnormalRepository = repository;
            _productAbnormalPersonRepository = productAbnormalPersonRepository;
        }

        #region 新增
        public async Task<IResponseOutput> AddAsync(ProductAbnormalAddInput addInput)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录！");
            }
            var entity = Mapper.Map<Abnormal>(addInput);
            string abnormalNo = string.Empty;
            var pago = (await GetCurrDayFirstAbnormalInfo(DateTime.Now.Date)).GetData<ProductAbnormalGetOutput>();
            if (pago == null) //当天第一条
            {
                //生成异常单号
                abnormalNo = string.Format(@"AB{0}000{1}", DateTime.Now.ToString("yyyyMMdd"), 1);
            } 
            else
            {
                string tempAbnormalNo = pago.AbnormalNo;
                string[] arr = tempAbnormalNo.Split("AB");
                if (tempAbnormalNo.Length != 14)
                {
                    return ResponseOutput.NotOk("最新异常单号错误或者已经被修改！请查看！");
                }
                if (arr.Length == 2)
                {
                    long nNum = Convert.ToInt64(arr[1]);
                    nNum++;
                    //生成异常单号
                    abnormalNo = string.Format(@"AB{0}", nNum);
                } 
                else
                {
                    return ResponseOutput.NotOk("最新异常单号错误或者已经被修改！请查看！");
                }
            }
            if (!abnormalNo.IsNotNull())
            {
                return ResponseOutput.NotOk("异常单号生成失败！");
            }
            entity.AbnormalNo = abnormalNo;
            entity.EndTime = null;
            var id = (await _productAbnormalRepository.InsertAsync(entity)).Id;
            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _productAbnormalRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _productAbnormalRepository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(ProductAbnormalDeleteInput deleteInput)
        {
            var result = false;
            if (deleteInput.AbnormalNo.IsNotNull())
            {
                result = (await _productAbnormalRepository.SoftDeleteAsync(t => t.AbnormalNo== deleteInput.AbnormalNo));
            }

            return ResponseOutput.Result(result);
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetListAsync(ProductAbnormalGetInput input)
        {
            var whereSelect = _productAbnormalRepository.Select
            .WhereIf(input.AbnormalNo.IsNotNull(), t => t.AbnormalNo == input.AbnormalNo)
            .WhereIf(input.ProName.IsNotNull() && input.ProName != "0", t => t.ProjectNo == input.ProName)
            .WhereIf(input.LineName.IsNotNull() && input.LineName != "0", t => t.LineName == input.LineName)
            .WhereIf(input.AbnomalStatus > 0, t => t.Status == input.AbnomalStatus)
            .WhereIf(input.ResponDepart > 0, t => t.ResponDepart == input.ResponDepart)
            .WhereIf(input.ResponBy.IsNotNull(), t => t.ResponBy.Contains(input.ResponBy))
            .WhereIf(input.StartDate != null && input.EndDate != null, t => t.CreateDate >= input.StartDate && t.CreateDate < input.EndDate);
            var data = await _productAbnormalRepository.GetListAsync<ProductAbnormalListOutput>(whereSelect);
            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            return await GetOneAsync(new ProductAbnormalGetInput { AbnormalNo = id });
        }
        public async Task<IResponseOutput> GetOneAsync(ProductAbnormalGetInput input)
        {
            var whereSelect = _productAbnormalRepository.Select
            .WhereIf(input.AbnormalNo.IsNotNull(), t => t.AbnormalNo == input.AbnormalNo)
            .WhereIf(input.ProName.IsNotNull() && input.ProName != "0", t => t.ProjectNo == input.ProName)
            .WhereIf(input.LineName.IsNotNull() && input.LineName != "0", t => t.LineName == input.LineName)
            .WhereIf(input.AbnomalStatus > 0, t => t.Status == input.AbnomalStatus)
            .WhereIf(input.ResponDepart > 0, t => t.ResponDepart == input.ResponDepart)
            .WhereIf(input.ResponBy.IsNotNull(), t => t.ResponBy.Contains(input.ResponBy))
            .WhereIf(input.StartDate != null && input.EndDate != null, t => t.CreateDate >= input.StartDate && t.CreateDate < input.EndDate);
            var result = await _productAbnormalRepository.GetOneAsync<ProductAbnormalGetOutput>(whereSelect);
            if (result == null)
            {
                return ResponseOutput.NotOk("记录不存在！");
            }
            var res = await _productAbnormalPersonRepository.GetOneAsync<AbnormalPerson>(result.ResponBy);
            var res2 = new ProductAbnormalGetOutput
            {
                Id = result.Id,
                AbnormalNo = result.AbnormalNo,
                ProjectNo = result.ProjectNo,
                LineName = result.LineName,
                ClassAB = result.ClassAB,
                FProcess = result.FProcess,
                Type = result.Type,
                ItemType = result.ItemType,
                Description = result.Description,
                BeginTime = result.BeginTime,
                EndTime = result.EndTime,
                ResponBy = result.ResponBy,
                ResponName = res.Name,
                ResponDepart = result.ResponDepart,
                Status = result.Status,
                Reason = result.Reason,
                TempMeasures = result.TempMeasures,
                FundaMeasures = result.FundaMeasures
            };
            return ResponseOutput.Data(res2);
        }
        public async Task<IResponseOutput> GetPageListAsync(PageInput<ProductAbnormalGetInput> pageInput)
        {
            var input = pageInput.GetFilter();

            var list = await _productAbnormalRepository.Orm.Select<Abnormal, AbnormalPerson>()
                .LeftJoin((a, ap)=> a.ResponBy== ap.PersonLiableId)
                .WhereIf(input.AbnormalNo.IsNotNull(), (a, ap) => a.AbnormalNo == input.AbnormalNo)
                .WhereIf(input.ProName.IsNotNull() && input.ProName != "0", (a, ap) => a.ProjectNo == input.ProName)
                .WhereIf(input.LineName.IsNotNull() && input.LineName != "0", (a, ap) => a.LineName == input.LineName)
                .WhereIf(input.AbnomalStatus > 0, (a, ap) => a.Status == input.AbnomalStatus)
                .WhereIf(input.ResponDepart > 0, (a, ap) => a.ResponDepart == input.ResponDepart)
                .WhereIf(input.ResponBy.IsNotNull(), (a, ap) => a.ResponBy.Contains(input.ResponBy))
                .WhereIf(input.StartDate != null && input.EndDate != null, (a, ap) => a.CreateDate >= input.StartDate && a.CreateDate < input.EndDate)
                .Count(out var total)
                .OrderByDescending((a, ap) => new { CreateDate = a.CreateDate })
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .ToListAsync((a, ap) => new ProductAbnormalListOutput {
                    Id = a.Id,
                    AbnormalNo = a.AbnormalNo,
                    ProjectNo = a.ProjectNo,
                    LineName = a.LineName,
                    ClassAB = a.ClassAB,
                    FProcess = a.FProcess,
                    Type = a.Type,
                    ItemType = a.ItemType,
                    Description = a.Description,
                    BeginTime = a.BeginTime,
                    EndTime = a.EndTime,
                    ResponBy = a.ResponBy,
                    ResponName =ap.Name,
                    ResponDepart = a.ResponDepart,
                    Status = a.Status,
                    Reason = a.Reason,
                    TempMeasures = a.TempMeasures,
                    FundaMeasures = a.FundaMeasures
                });

            var data = new PageOutput<ProductAbnormalListOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetCurrDayFirstAbnormalInfo(DateTime dt)
        {
            DateTime dt2 = dt.AddDays(1);
            var whereSelect = _productAbnormalRepository.Select.Where(t => t.CreateDate >= dt && t.CreateDate < dt2).OrderByDescending(t => t.CreateDate);
            var result = await _productAbnormalRepository.GetOneAsync<ProductAbnormalGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetAbnormalPerson(string name, ResponDepart responDepart)
        {
            var whereSelect = _productAbnormalPersonRepository.Select
            .WhereIf(name.IsNotNull(), t => t.Name.Contains(name) || t.Phone == name || t.DumpPhone == name);
            var data = await _productAbnormalPersonRepository.GetListAsync<ProductAbnormalPersonListOutput>(whereSelect);
            if (data != null)
            {
                data = data.FindAll(t => t.Department == responDepart);
            }
            return ResponseOutput.Data(data);
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(ProductAbnormalUpdateInput input)
        {
            if (input.AbnormalNo.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }
            int res = await _productAbnormalRepository.UpdateDiyAsync
            //.SetIf(input.ProjectNo.IsNotNull(), t => t.ProjectNo, input.ProjectNo)
            //.SetIf(input.LineName.IsNotNull(), t => t.LineName, input.LineName)
            .SetIf(input.ClassAB.IsNotNull(), t => t.ClassAB, input.ClassAB)
            .SetIf(input.FProcess.IsNotNull(), t => t.FProcess, input.FProcess)
            .SetIf(input.Type > 0, t => t.Type, input.Type)
            .SetIf(input.ItemType > 0, t => t.ItemType, input.ItemType)
            .SetIf(input.Description.IsNotNull(), t => t.Description, input.Description)
            //.SetIf(input.ResponBy.IsNotNull(), t => t.ResponBy, input.ResponBy)
            //.SetIf(input.ResponDepart > 0, t => t.ResponDepart, input.ResponDepart)
            .Set(t => t.BeginTime, input.BeginTime)
            //.Set(t => t.UpdateDate, DateTime.Now)
            .Where(t => t.AbnormalNo == input.AbnormalNo)
            .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }

        public async Task<IResponseOutput> UpdateEntityAsync(ProductAbnormalUpdateInput input)
        {
            var entity = await _productAbnormalRepository.GetOneAsync(t => t.AbnormalNo == input.AbnormalNo);
            if (string.IsNullOrEmpty(entity.AbnormalNo))
            {
                return ResponseOutput.NotOk("数据不存在！");
            }

            Mapper.Map(input, entity);
            int res = await _productAbnormalRepository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }

        public async Task<IResponseOutput> UpdateAbnormalHandle(ProAbnHandleUpdateInput input)
        {
            input.Status = AbnormalStatus.Processed;
            var entity = await _productAbnormalRepository.GetOneAsync(t => t.AbnormalNo == input.AbnormalNo);
            if (string.IsNullOrEmpty(entity.AbnormalNo))
            {
                return ResponseOutput.NotOk("异常记录数据不存在！");
            }
            Mapper.Map(input, entity);
            int res = await _productAbnormalRepository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("异常处理失败");
            }
            return ResponseOutput.Ok("异常处理成功");
        }
        #endregion
    }
}
