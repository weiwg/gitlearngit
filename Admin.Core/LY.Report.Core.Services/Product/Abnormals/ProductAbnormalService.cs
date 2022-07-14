using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Product;
using LY.Report.Core.Repository.Product.Abnormals;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Product.Abnormals.Input;
using LY.Report.Core.Service.Product.Abnormals.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                if (arr.Length == 2)
                {
                    int nNum = Convert.ToInt32(arr[1]);
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
            .WhereIf(input.ProName.IsNotNull(), t => t.ProjectNo == input.ProName)
            .WhereIf(input.LineName.IsNotNull(), t => t.LineName == input.LineName)
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
            .WhereIf(input.ProName.IsNotNull(), t => t.ProjectNo == input.ProName)
            .WhereIf(input.LineName.IsNotNull(), t => t.LineName == input.LineName)
            .WhereIf(input.AbnomalStatus > 0, t => t.Status == input.AbnomalStatus)
            .WhereIf(input.ResponDepart > 0, t => t.ResponDepart == input.ResponDepart)
            .WhereIf(input.ResponBy.IsNotNull(), t => t.ResponBy.Contains(input.ResponBy))
            .WhereIf(input.StartDate != null && input.EndDate != null, t => t.CreateDate >= input.StartDate && t.CreateDate < input.EndDate);
            var result = await _productAbnormalRepository.GetOneAsync<ProductAbnormalGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }
        public async Task<IResponseOutput> GetPageListAsync(PageInput<ProductAbnormalGetInput> pageInput)
        {
            var input = pageInput.GetFilter();

            var list = await _productAbnormalRepository.Select
                .WhereIf(input.AbnormalNo.IsNotNull(), t => t.AbnormalNo == input.AbnormalNo)
                .WhereIf(input.ProName.IsNotNull(), t => t.ProjectNo == input.ProName)
                .WhereIf(input.LineName.IsNotNull(), t => t.LineName == input.LineName)
                .WhereIf(input.AbnomalStatus > 0, t => t.Status == input.AbnomalStatus)
                .WhereIf(input.ResponDepart > 0, t => t.ResponDepart == input.ResponDepart)
                .WhereIf(input.ResponBy.IsNotNull(), t => t.ResponBy.Contains(input.ResponBy))
                .WhereIf(input.StartDate != null && input.EndDate != null, t => t.CreateDate >= input.StartDate && t.CreateDate < input.EndDate)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .ToListAsync<ProductAbnormalListOutput>();

            var data = new PageOutput<ProductAbnormalListOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetCurrDayFirstAbnormalInfo(DateTime dt)
        {
            var whereSelect = _productAbnormalRepository.Select.Where(t => t.CreateDate >= dt && t.CreateDate < dt.AddDays(1)).OrderByDescending(t => t.CreateDate);
            var result = await _productAbnormalRepository.GetOneAsync<ProductAbnormalGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetAbnormalPerson(string condition)
        {
            var whereSelect = _productAbnormalPersonRepository.Select
            .WhereIf(condition.IsNotNull(), t => t.Name.Contains(condition) || t.Phone == condition || t.DumpPhone == condition);
            var data = await _productAbnormalPersonRepository.GetListAsync<ProductAbnormalPersonListOutput>(whereSelect);
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
            .SetIf(input.ProjectNo.IsNotNull(), t => t.ProjectNo, input.ProjectNo)
            .SetIf(input.LineName.IsNotNull(), t => t.LineName, input.LineName)
            .SetIf(input.ClassAB.IsNotNull(), t => t.ClassAB, input.ClassAB)
            .SetIf(input.FProcess.IsNotNull(), t => t.FProcess, input.FProcess)
            .SetIf(input.Type > 0, t => t.Type, input.Type)
            .SetIf(input.ItemType > 0, t => t.ItemType, input.ItemType)
            .SetIf(input.Description.IsNotNull(), t => t.Description, input.Description)
            .Set(t => t.BeginTime, input.BeginTime)
            .SetIf(input.ResponBy.IsNotNull(), t => t.ResponBy, input.ResponBy)
            .SetIf(input.ResponDepart > 0, t => t.ResponDepart, input.ResponDepart)
            .SetIf(input.UpdateUserId.IsNotNull(), t => t.UpdateUserId, input.UpdateUserId)
            .Set(t => t.UpdateDate, DateTime.Now)
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
        #endregion
    }
}
