using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Helpers;
using LY.Report.Core.Db;
using Newtonsoft.Json;

namespace LY.Report.Core.Helper
{
    /// <summary>
    /// Api帮助类
    /// </summary>
    [SingleInstance]
    public class ApiHelper
    {
        private List<ApiHelperDto> _apis;
        private static readonly object _lockObject = new object();

        public List<ApiHelperDto> GetApis()
        {
            if (_apis != null && _apis.Any())
                return _apis;

            lock (_lockObject)
            {
                if (_apis != null && _apis.Any())
                    return _apis;

                _apis = new List<ApiHelperDto>();
                var filePath = Path.Combine(AppContext.BaseDirectory, "Db/Data/data.json").ToPath();
                var jsonData = FileHelper.ReadFile(filePath);
                var apis = JsonConvert.DeserializeObject<Data>(jsonData).Apis;
                foreach (var api in apis)
                {
                    var parentLabel = apis.FirstOrDefault(a => a.Id == api.ParentId)?.Label;

                    _apis.Add(new ApiHelperDto
                    {
                        Label = parentLabel.IsNotNull() ? $"{parentLabel} / {api.Label}" : api.Label,
                        Path = api.Path?.ToLower().Trim('/')
                    });
                }

                return _apis;
            }
        }
    }

    public class ApiHelperDto
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string Path { get; set; }
    }
}
