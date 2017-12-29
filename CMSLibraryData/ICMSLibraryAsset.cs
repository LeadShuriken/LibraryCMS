using CMSLibraryData.DBModels;
using System.Collections.Generic;

namespace CMSLibraryData
{
    /// <summary>
    /// ICMSLibraryAsset: spesifying the service methods to be used by the CMSLibraryAsset
    /// </summary>
    public interface ICMSLibraryAsset : IAssetsBase
    {
        IEnumerable<CMSLibraryAsset> GetAll();
        CMSLibraryAsset GetById(int id);

        /// <summary>
        /// GetSource returns aider the: Author, Director, Agency or Publisher of the asset
        /// respectfully for the type of: Book, Video, Magazine or Newspaper 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetSource(int id);
        string GetIndex(int id);
        string GetTitle(int id);
        string GetIsbn(int id);

        CMSLibraryBranch GetCurrentLocation(int id);
    }
}
