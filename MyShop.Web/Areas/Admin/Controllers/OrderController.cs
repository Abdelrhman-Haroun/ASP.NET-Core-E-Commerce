using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModels;
using MyShop.Utilities;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            IEnumerable<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll(IncludeWord: "ApplicationUser");
            return Json(new { data = orderHeaders });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            OrderVM orderVM = new OrderVM
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstorDefault(x => x.Id == id, IncludeWord: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetails.GetAll(x => x.OrderHeaderId == id, IncludeWord: "Product")
            };
            return View(orderVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails(OrderVM OrderVM)
        {
            var orderfromdb = _unitOfWork.OrderHeader.GetFirstorDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderfromdb.Name = OrderVM.OrderHeader.Name;
            orderfromdb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderfromdb.Address = OrderVM.OrderHeader.Address;
            orderfromdb.City = OrderVM.OrderHeader.City;

            if (OrderVM.OrderHeader.Carrier != null)
            {
                orderfromdb.Carrier = OrderVM.OrderHeader.Carrier;
            }

            if (OrderVM.OrderHeader.TrakingNumber != null)
            {
                orderfromdb.TrakingNumber = OrderVM.OrderHeader.TrakingNumber;
            }

            _unitOfWork.OrderHeader.Update(orderfromdb);
            _unitOfWork.Complete();
            TempData["Update"] = "Order has Updated Successfully";
            return RedirectToAction("Details", "Order", new { orderfromdb.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProccess(OrderVM OrderVM)
        {
            _unitOfWork.OrderHeader.UpdateOredrStatus(OrderVM.OrderHeader.Id, SD.Proccessing, null);
            _unitOfWork.Complete();
            TempData["Update"] = "Order Status has Updated Successfully";
            return RedirectToAction("Details", "Order", new { OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartShipp(OrderVM OrderVM)
        {
            var orderfromdb = _unitOfWork.OrderHeader.GetFirstorDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderfromdb.Carrier = OrderVM.OrderHeader.Carrier;
            orderfromdb.TrakingNumber = OrderVM.OrderHeader.TrakingNumber;
            orderfromdb.ShippingDate = DateTime.Now;
            orderfromdb.OrderStatus = SD.Shipped;
            _unitOfWork.OrderHeader.Update(orderfromdb);
            _unitOfWork.Complete();
            TempData["Update"] = "Order has Shipped Successfully";
            return RedirectToAction("Details", "Order", new { orderfromdb.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder(OrderVM OrderVM)
        {
            var orderfromdb = _unitOfWork.OrderHeader.GetFirstorDefault(u => u.Id == OrderVM.OrderHeader.Id);
            if (orderfromdb.PaymentStatus == SD.Approved)
            {

                //var option = new RefundCreateOptions
                //{
                //    Reason = RefundReasons.RequestedByCustomer,
                //    PaymentIntent = orderfromdb.PaymentIntentId
                //};

                //var service = new RefundService();
                //Refund refund = service.Create(option);


                _unitOfWork.OrderHeader.UpdateOredrStatus(orderfromdb.Id, SD.Cancelled, SD.Refund);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateOredrStatus(orderfromdb.Id, SD.Cancelled, SD.Cancelled);
            }
            _unitOfWork.Complete();

            TempData["Delete"] = "Order has Cancelled Successfully";
            return RedirectToAction("Details", "Order", new { OrderVM.OrderHeader.Id });
        }
    }
}
