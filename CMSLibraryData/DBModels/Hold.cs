using System;

namespace CMSLibraryData.DBModels
{
    public class Hold
    {
        public int Id { get; set; }
        public virtual CMSLibraryAsset LibraryAsset { get; set; }
        public virtual CMSLibraryCard LibraryCard { get; set; }
        public DateTime HoldPlaced { get; set; }
    }
}
