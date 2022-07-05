using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.User;
using LY.Report.Core.Repository.User;
using LY.Report.Core.Service.User.WeiXinInfo.Input;
using LY.Report.Core.Service.User.WeiXinInfo.Output;

namespace LY.Report.Core.Service.User.WeiXinInfo
{
    public class UserWeiXinInfoService : IUserWeiXinInfoService
    {
        private readonly IMapper _mapper;
        private readonly IUserWeiXinInfoRepository _repository;
        public UserWeiXinInfoService(IMapper mapper, IUserWeiXinInfoRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(UserWeiXinInfoAddInput input)
        {
            var entity = await _repository.GetOneAsync(t=>t.OpenId == input.OpenId);
            if (entity.OpenId.IsNotNull())
            {
                return await UpdateAsync(_mapper.Map<UserWeiXinInfoUpdateInput>(input));
            }

            entity = _mapper.Map<UserWeiXinInfo>(input);
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(UserWeiXinInfoUpdateInput input)
        {
            if (input.OpenId.IsNull())
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _repository.GetOneAsync(t => t.OpenId == input.OpenId);
            if (entity.OpenId.IsNull())
            {
                return await AddAsync(input);
            }

            int res = await _repository.UpdateDiyAsync
                .SetIf(input.NickName.IsNotNull(), t => t.NickName, input.NickName)
                .SetIf(input.Sex > 0, t => t.Sex, input.Sex)
                .SetIf(input.Province.IsNotNull(), t => t.Province, input.Province)
                .SetIf(input.City.IsNotNull(), t => t.City, input.City)
                .SetIf(input.Country.IsNotNull(), t => t.Country, input.Country)
                .SetIf(input.HeadImgUrl.IsNotNull(), t => t.HeadImgUrl, input.HeadImgUrl)
                .SetIf(input.Privilege.IsNotNull(), t => t.Privilege, input.Privilege)
                .SetIf(input.UnionId.IsNotNull(), t => t.UnionId, input.UnionId)
                .Where(t => t.OpenId == input.OpenId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            return ResponseOutput.Ok();
        }

        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string openId)
        {
            var result = await _repository.GetOneAsync<UserWeiXinInfoGetOutput>(openId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(UserWeiXinInfoGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.Id == input.Id);//获取实体
            var result = await _repository.GetOneAsync<UserWeiXinInfoGetOutput>(t => t.OpenId == input.OpenId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<UserWeiXinInfoGetInput> input)
        {
            var id = input.Filter?.Id;

            long total;
            var list = await _repository.GetPageListAsync<UserWeiXinInfoListOutput>(t => t.Id == id, input.CurrentPage,input.PageSize, t => t.Id, out total);

            var data = new PageOutput<UserWeiXinInfoListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

    }
}
