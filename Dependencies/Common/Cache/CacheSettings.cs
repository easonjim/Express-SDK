
/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ComLib.Caching
{
    /// <summary>
    /// Cache settings for the Cache instance.
    /// </summary>
    public class CacheSettings
    {
        /// <summary>
        /// Used to prefix all the cache keys.
        /// </summary>
        public string PrefixForCacheKeys = "Jsoft";
        
        
        /// <summary>
        /// 是否使用前缀
        /// </summary>
        public bool UsePrefix = true;


        /// <summary>
        /// Cache 优先级
        /// </summary>
        public CacheItemPriority DefaultCachePriority = CacheItemPriority.Normal;


        /// <summary>
        /// 是否滑动过期
        /// </summary>
        public bool DefaultSlidingExpirationEnabled = false;


        /// <summary>
        /// 默认过期时间 10分钟
        /// </summary>
        public int DefaultTimeToLive = 600;
    }
}
