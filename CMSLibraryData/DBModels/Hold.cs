using System;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity Hold 
    /// Describing the relationship between a CMSLibraryCard 
    /// and CMSLibraryAsset when it is been placed on Hold by a Subscriber
    /// </summary>
    public class Hold
    {
        public int Id { get; set; }
        public virtual CMSLibraryAsset LibraryAsset { get; set; }
        public virtual CMSLibraryCard LibraryCard { get; set; }
        public DateTime HoldPlaced { get; set; }
    }
}
