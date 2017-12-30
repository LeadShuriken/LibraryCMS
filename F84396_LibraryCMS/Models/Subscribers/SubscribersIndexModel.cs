using System.Collections.Generic;

namespace F84396_LibraryCMS.Models.Subscribers
{
    /// <summary>
    /// /Views/Subscribers/Index
    /// </summary>
    public class SubscribersIndexModel
    {
        public IEnumerable<SubscribersDetailModel> Subscribers { get; set; }
    }
}
