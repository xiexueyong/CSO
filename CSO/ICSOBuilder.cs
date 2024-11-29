using CSOEngine.Object;
using CSOEngine.Proxy;
using JetBrains.Annotations;

namespace CSOEngine
{
    /// <summary>
    /// cso对象构造器接口
    /// </summary>
    public interface ICSOBuilder
    {
        /// <summary>
        /// CSO 创建component接口
        /// </summary>
        /// <param name="cso"></param>
        /// <param name="args"></param>
        void OnBuild(CObject cso, params object[] args);

        /// <summary>
        /// CSO 更新Component数据接口
        /// </summary>
        /// <param name="cso"></param>
        /// <param name="args"></param>
        void OnUpdate(CSOProxy cso, params object[] args)
        {
        }

        /// <summary>
        /// CSO序列化，导出数据接口
        /// </summary>
        /// <param name="cso"></param>
        /// <param name="args"></param>
        [CanBeNull]
        object OnExport(CSOProxy cso, params object[] args)
        {
            return null;
        }
    }
}