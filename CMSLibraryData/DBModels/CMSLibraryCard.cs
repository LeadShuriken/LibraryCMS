using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// DB Entity CMSLibraryCard 
/// Creating the relationship between a Subscriber 
/// Checkout, CheckoutHistory and Hold
/// </summary>
namespace CMSLibraryData.DBModels
{
    public class CMSLibraryCard
    {
        public int Id { get; set; }

        [Display(Name ="Overdue Fees")]
        public decimal Fees { get; set; }

        [Display(Name="Card Issued Date")]
        public DateTime Created { get; set; }

        [Display(Name="Materials on Loan")]
        public virtual IEnumerable<Checkout> Checkouts { get; set; }
    }
}
