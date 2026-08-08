using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Api.Controllers;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;

namespace HelpDesk.Tests
{
    public class TicketControllerTests
    {
        private readonly Mock<ITicketRepository> _mockRepository;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _mockRepository = new Mock<ITicketRepository>();
            _controller = new TicketController(_mockRepository.Object);
        }

        private static Ticket CreateSampleTicket(int id = 1) => new Ticket
        {
            Id = id,
            Title = "Sample Ticket",
            Description = "Sample Description",
            Priority = "High",
            Status = "Open",
            RaisedBy = "John Doe",
            CreatedDate = DateTime.Now
        };

        // ---------- Mandatory Test Cases ----------

        [Fact]
        public async Task GetAllTickets_ReturnsOkResult_WhenTicketsExist()
        {
            // Arrange
            var tickets = new List<Ticket> { CreateSampleTicket(1), CreateSampleTicket(2) };
            _mockRepository.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetAllTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Equal(2, returnedTickets.Count);
        }

        [Fact]
        public async Task GetTicketById_ReturnsOkResult_WhenTicketExists()
        {
            // Arrange
            var ticket = CreateSampleTicket(1);
            _mockRepository.Setup(repo => repo.GetTicketByIdAsync(1)).ReturnsAsync(ticket);

            // Act
            var result = await _controller.GetTicketById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(1, returnedTicket.Id);
        }

        [Fact]
        public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetTicketByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.GetTicketById(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully()
        {
            // Arrange
            var ticket = CreateSampleTicket();
            _mockRepository.Setup(repo => repo.CreateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateTicket(ticket);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task CreateTicket_ReturnsBadRequest_WhenTicketIsNull()
        {
            // Act
            var result = await _controller.CreateTicket(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketsExist()
        {
            // Arrange
            var tickets = new List<Ticket> { CreateSampleTicket(1) };
            _mockRepository.Setup(repo => repo.GetTicketsByStatusAsync("Open")).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetTicketsByStatus("Open");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Single(returnedTickets);
        }

        // ---------- Optional Test Cases ----------

        [Fact]
        public async Task UpdateTicket_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var ticket = CreateSampleTicket(1);
            _mockRepository.Setup(repo => repo.GetTicketByIdAsync(1)).ReturnsAsync(ticket);
            _mockRepository.Setup(repo => repo.UpdateTicketAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTicket(1, ticket);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticket = CreateSampleTicket(99);
            _mockRepository.Setup(repo => repo.GetTicketByIdAsync(99)).ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.UpdateTicket(99, ticket);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ReturnsOkResult_WhenTicketIsDeletedSuccessfully()
        {
            // Arrange
            var ticket = CreateSampleTicket(1);
            _mockRepository.Setup(repo => repo.GetTicketByIdAsync(1)).ReturnsAsync(ticket);
            _mockRepository.Setup(repo => repo.DeleteTicketAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTicket(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.DeleteTicket(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAllTickets_ReturnsEmptyList_WhenNoTicketsExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(new List<Ticket>());

            // Act
            var result = await _controller.GetAllTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Empty(returnedTickets);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsEmptyList_WhenNoMatchingTicketsExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetTicketsByStatusAsync("Closed")).ReturnsAsync(new List<Ticket>());

            // Act
            var result = await _controller.GetTicketsByStatus("Closed");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Empty(returnedTickets);
        }
    }
}
