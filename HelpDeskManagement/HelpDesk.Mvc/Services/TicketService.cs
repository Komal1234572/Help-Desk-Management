using System.Net.Http.Json;
using HelpDesk.Mvc.Models;

namespace HelpDesk.Mvc.Services
{
    // Service Layer that consumes the HelpDesk.Api Web API via HttpClient.
    public class TicketService : ITicketService
    {
        private readonly HttpClient _httpClient;

        public TicketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            var response = await _httpClient.GetAsync("api/Ticket/All");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Ticket>>() ?? new List<Ticket>();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Ticket/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<Ticket>();
        }

        public async Task<bool> CreateTicketAsync(Ticket ticket)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Ticket", ticket);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTicketAsync(Ticket ticket)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Ticket/{ticket.Id}", ticket);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Ticket/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Ticket>> GetTicketsByStatusAsync(string status)
        {
            var response = await _httpClient.GetAsync($"api/Ticket/Status/{status}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Ticket>>() ?? new List<Ticket>();
        }
    }
}
