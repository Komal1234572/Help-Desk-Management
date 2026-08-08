using Microsoft.AspNetCore.Mvc;
using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;

namespace HelpDesk.Mvc.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        private static readonly List<string> PriorityOptions = new() { "Low", "Medium", "High" };
        private static readonly List<string> StatusOptions = new() { "Open", "In Progress", "Closed" };

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: /Ticket
        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return View(tickets);
        }

        // GET: /Ticket/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: /Ticket/Create
        public IActionResult Create()
        {
            ViewBag.PriorityOptions = PriorityOptions;
            var ticket = new Ticket { Status = "Open" };
            return View(ticket);
        }

        // POST: /Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            // Status is hardcoded to Open for newly raised tickets.
            ticket.Status = "Open";
            ModelState.Remove(nameof(Ticket.Status));

            if (!ModelState.IsValid)
            {
                ViewBag.PriorityOptions = PriorityOptions;
                return View(ticket);
            }

            await _ticketService.CreateTicketAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Ticket/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.PriorityOptions = PriorityOptions;
            ViewBag.StatusOptions = StatusOptions;
            return View(ticket);
        }

        // POST: /Ticket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.PriorityOptions = PriorityOptions;
                ViewBag.StatusOptions = StatusOptions;
                return View(ticket);
            }

            await _ticketService.UpdateTicketAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Ticket/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: /Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Ticket/FilterByStatus
        public async Task<IActionResult> FilterByStatus(string status)
        {
            ViewBag.StatusOptions = StatusOptions;
            ViewBag.SelectedStatus = status;

            var tickets = string.IsNullOrEmpty(status)
                ? new List<Ticket>()
                : await _ticketService.GetTicketsByStatusAsync(status);

            return View(tickets);
        }
    }
}
