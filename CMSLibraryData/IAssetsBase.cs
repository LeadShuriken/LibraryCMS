using System;
using System.Collections.Generic;
using System.Text;

namespace CMSLibraryData
{
    /// <summary>
    /// IAssetsBase: ase methods interface to be used across interfaces currently:
    /// ICheckout, ICMSLibraryAsset
    /// </summary>
    public interface IAssetsBase
    {
        string GetType(int id);
        string GetAssetStatus(int id);
        bool IsCheckedOut(int id);
    }
}
