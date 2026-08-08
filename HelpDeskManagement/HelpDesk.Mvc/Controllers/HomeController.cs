using Microsoft.AspNetCore.Mvc;
using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;

namespace HelpDesk.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITicketService _ticketService;

        public HomeController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: / (Dashboard)
        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();

            var dashboard = new DashboardViewModel
            {
                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => t.Status == "Open"),
                InProgressTickets = tickets.Count(t => t.Status == "In Progress"),
                ClosedTickets = tickets.Count(t => t.Status == "Closed")
            };

            return View(dashboard);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
