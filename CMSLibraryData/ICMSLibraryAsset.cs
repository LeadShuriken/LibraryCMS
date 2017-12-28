using CMSLibraryData.DBModels;
using System.Collections.Generic;

namespace CMSLibraryData
{
    public interface ICMSLibraryAsset : IAssetsBase
    {
        IEnumerable<CMSLibraryAsset> GetAll();
        CMSLibraryAsset GetById(int id);

        void Add(CMSLibraryAsset newAsset);
        string GetSource(int id);
        string GetIndex(int id);
        string GetTitle(int id);
        string GetIsbn(int id);

        CMSLibraryBranch GetCurrentLocation(int id);
    }
}
