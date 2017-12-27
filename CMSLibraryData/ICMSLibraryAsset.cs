using System;
using System.Collections.Generic;
using System.Text;
using CMSLibraryData.DBModels;

namespace CMSLibraryData
{
    public interface ICMSLibraryAsset
    {
        IEnumerable<CMSLibraryAsset> GetAll();
        CMSLibraryAsset GetById(int id);

        void Add(CMSLibraryAsset newAsset);
        string GetAuthorOrDirectorOrPublisher(int id);
        string GetIndex(int id);
        string GetType(int id);
        string GetTitle(int id);
        string GetIsbn(int id);

        CMSLibraryBranch GetCurrentLocation(int id);
    }
}
