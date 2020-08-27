using System.Collections.Generic;
using Pluto.BlogCore.Domain.SeedWork;

namespace Pluto.BlogCore.Domain.DomainModels.Blog
{
    public class ContentPlatformInfo:ValueObject
    {
        /// <summary>
        /// 平台
        /// </summary>
        public EnumPlatform Platform { get; set; }

        /// <summary>
        /// 格式类型
        /// </summary>
        public EnumContentFormat Format { get; set; }

        /// <summary>
        /// 平台id
        /// </summary>
        public string PlatformId { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Platform;
            yield return Format;
            yield return PlatformId;
        }
    }
}