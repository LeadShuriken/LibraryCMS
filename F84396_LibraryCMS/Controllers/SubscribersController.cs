using CMSLibraryData;
using CMSLibraryData.DBModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using F84396_LibraryCMS.Models.Subscribers;

namespace F84396_LibraryCMS.Controllers
{
    public class SubscribersController : Controller
    {
        private ISubscriber _subscriber;

        public SubscribersController(ISubscriber subscriber)
        {
            _subscriber = subscriber;
        }

        public IActionResult Index()
        {
            IEnumerable<Subscriber> allSubscribers = _subscriber.GetAll();

            List<SubscribersDetailModel> model = allSubscribers.Select(p => new SubscribersDetailModel
            {
                Id = p.Id,
                LastName = p.LastName,
                FirstName = p.FirstName,
                LibraryCardId = p.LibraryCard.Id,
                OverdueFees = p.LibraryCard.Fees,
                HomeLibrary = p.HomeLibraryBranch.Name
            }).ToList();

            return View(new SubscribersIndexModel()
            {
                Subscribers = model
            });
        }

        public IActionResult Detail(int id)
        {
            Subscriber subscriber = _subscriber.Get(id);

            SubscribersDetailModel model = new SubscribersDetailModel
            {
                LastName = subscriber.LastName,
                FirstName = subscriber.FirstName,
                Address = subscriber.Address,
                Gender = subscriber.Gender,
                HomeLibrary = subscriber.HomeLibraryBranch.Name,
                MemberSince = subscriber.LibraryCard.Created,
                OverdueFees = subscriber.LibraryCard.Fees,
                LibraryCardId = subscriber.LibraryCard.Id,
                Telephone = subscriber.PhoneNumber,
                AssetsCheckedOut = _subscriber.GetCheckouts(id).ToList() ?? new List<Checkout>(),
                CheckoutHistory = _subscriber.GetCheckoutHistory(id),
                Holds = _subscriber.GetHolds(id)
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}