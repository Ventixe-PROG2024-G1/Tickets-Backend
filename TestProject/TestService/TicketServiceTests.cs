using Business.DTOS;
using Business.Services;
using Business.ViewModels;
using Data.Repository;
using Domain.Entity;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.TestService
{
    [TestClass]
    public class TicketServiceTests
    {
        private Mock<ITicketRepository> _ticketRepositoryMock;
        private IMemoryCache _memoryCache;
        private TicketService _ticketService;

        [TestInitialize]
        public void Setup()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();

            // Setup simple in-memory cache for tests
            var memoryCacheOptions = new MemoryCacheOptions();
            _memoryCache = new MemoryCache(memoryCacheOptions);

            _ticketService = new TicketService(_ticketRepositoryMock.Object, _memoryCache);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnCachedTickets_WhenCacheIsSet()
        {
            // Arrange
            var tickets = new List<TicketsViewModel>
            {
                new TicketsViewModel { Id = Guid.NewGuid(), Price = 100, Quantity = 1, TierDescription = "Good view", Tiers = Domain.Enums.TicketTier.Gold }
            };
            _memoryCache.Set("Ticket_All", tickets, TimeSpan.FromMinutes(30));

            // Act
            var result = await _ticketService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tickets.Count, result.Count());

            var expected = tickets.First();
            var actual = result.First();

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.Quantity, actual.Quantity);
            Assert.AreEqual(expected.TierDescription, actual.TierDescription);
            Assert.AreEqual(expected.Tiers, actual.Tiers);

            // Verifiera att repository INTE anropades eftersom cache användes
            _ticketRepositoryMock.Verify(x => x.GetAllAsync(
                It.IsAny<bool>(),
                It.IsAny<System.Linq.Expressions.Expression<Func<TicketEntity, object>>>(),
                It.IsAny<System.Linq.Expressions.Expression<Func<TicketEntity, bool>>>(),
                It.IsAny<System.Linq.Expressions.Expression<Func<TicketEntity, object>>[]>()),
                Times.Never);

        }
        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnTicket_WhenTicketExists()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var ticketEntity = new TicketEntity
            {
                Id = ticketId,
                Price = 200,
                Quantity = 2,
                TierDescription = "VIP",
                Tier = Domain.Enums.TicketTier.VipLounge
            };

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId))
                .ReturnsAsync(ticketEntity);

            // Act
            var result = await _ticketService.GetByIdAsync(ticketId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ticketEntity.Id, result.Id);
            Assert.AreEqual(ticketEntity.Price, result.Price);
            Assert.AreEqual(ticketEntity.Quantity, result.Quantity);
            Assert.AreEqual(ticketEntity.TierDescription, result.TierDescription);
            Assert.AreEqual(ticketEntity.Tier, result.Tiers);
        }
        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId))
                .ReturnsAsync((TicketEntity?)null);

            // Act
            var result = await _ticketService.GetByIdAsync(ticketId);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public async Task CreateAsync_ShouldAddTicket_AndReturnViewModel()
        {
            // Arrange
            var createDto = new CreateTickets
            {
                Price = 150,
                Quantity = 3,
                TierDescription = "Nice seat",
                Tier = Domain.Enums.TicketTier.Silver
            };

            _ticketRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<TicketEntity>()))
                .ReturnsAsync(true);

            _ticketRepositoryMock
                .Setup(x => x.GetAllAsync(
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<TicketEntity, object>>>(),
                    It.IsAny<Expression<Func<TicketEntity, bool>>>(),
                    It.IsAny<Expression<Func<TicketEntity, object>>[]>()))
                .ReturnsAsync(new List<TicketEntity>());

            // Act
            var result = await _ticketService.CreateAsync(createDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(createDto.Price, result.Price);
            Assert.AreEqual(createDto.Quantity, result.Quantity);
            Assert.AreEqual(createDto.TierDescription, result.TierDescription);
            Assert.AreEqual(createDto.Tier, result.Tiers);
        }
        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveTicketAndUpdateCache_WhenSuccessful()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            _ticketRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<Expression<Func<TicketEntity, bool>>>()))
                .ReturnsAsync(true);

            _ticketRepositoryMock
                .Setup(x => x.GetAllAsync(
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<TicketEntity, object>>>(),
                    It.IsAny<Expression<Func<TicketEntity, bool>>>(),
                    It.IsAny<Expression<Func<TicketEntity, object>>[]>()))
                .ReturnsAsync(new List<TicketEntity>());

            // Act
            var result = await _ticketService.DeleteAsync(ticketId);

            // Assert
            Assert.IsTrue(result);
        }
    }

}
