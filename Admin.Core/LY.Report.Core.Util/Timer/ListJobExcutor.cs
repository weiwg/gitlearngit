
using System.Collections.Generic;

namespace LY.Report.Core.Util.Timer
{
    /// <summary>
    /// 通用列表循环任务执行者基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ListJobExcutor<T> : IJobExecutor
    {
        /// <summary>
        ///  运行状态
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        ///   开始任务
        /// </summary>
        public void StartJob()
        {
            //  任务依然在执行中，不需要再次唤起
            if (IsRunning)
                return;

            IsRunning = true;
            IList<T> list = null; // 结清实体list
            do
            {
                for (var i = 0; IsRunning && i < list?.Count; i++)
                {
                    ExcuteItem(list[i], i);
                }

                list = GetExcuteSource();

            } while (IsRunning && list?.Count > 0);

            IsRunning = false;
        }

        public void StopJob()
        {
            IsRunning = false;
        }

        /// <summary>
        ///   获取list数据源
        /// </summary>
        /// <returns></returns>
        protected virtual IList<T> GetExcuteSource()
        {
            return null;
        }

        /// <summary>
        ///  个体任务执行
        /// </summary>
        /// <param name="item">单个实体</param>
        /// <param name="index">在数据源中的索引</param>
        protected virtual void ExcuteItem(T item, int index)
        {
        }
    }
}
