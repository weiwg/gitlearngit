

namespace LY.Report.Core.Util.Timer
{
    /// <summary>
    /// 任务执行者接口
    /// </summary>
    public interface IJobExecutor
    {
        /// <summary>
        /// 开始任务
        /// </summary>
        void StartJob();

        /// <summary>
        ///  结束任务
        /// </summary>
        void StopJob();
    }
}
