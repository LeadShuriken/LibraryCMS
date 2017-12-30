using CMSLibraryData.DBModels;
using System;
using System.Collections.Generic;

namespace F84396_LibraryCMS.Models.Subscribers
{
    /// <summary>
    /// /Views/Subscribers/Detail
    /// </summary>
    public class SubscribersDetailModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LibraryCardId { get; set; }
        public string Address { get; set; }
        public DateTime MemberSince { get; set; }
        public string Gender { get; set; }
        public string Telephone { get; set; }
        public string HomeLibrary { get; set; }
        public decimal OverdueFees { get; set; }
        public IEnumerable<Checkout> AssetsCheckedOut { get; set; }
        public IEnumerable<CheckoutHistory> CheckoutHistory { get; set; }
        public IEnumerable<Hold> Holds { get; set; }
    }
}
