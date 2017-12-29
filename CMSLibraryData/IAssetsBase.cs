using System;
using System.Collections.Generic;
using System.Text;

namespace CMSLibraryData
{
    /// <summary>
    /// IAssetsBase: methods interface to be used across interfaces currently:
    /// ICheckout, ICMSLibraryAsset
    /// </summary>
    public interface IAssetsBase
    {
        string GetType(int id);
    }
}
