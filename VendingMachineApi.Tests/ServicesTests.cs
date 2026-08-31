using System;
using VendingMachineApi.Application.Services;
using VendingMachineApi.Domain.Exceptions;
using VendingMachineApi.Infrastructure.Catalogs;
using VendingMachineApi.Infrastructure.Persistence;
using Xunit;
using VendingMachineApi.Domain.Exceptions;

namespace VendingMachineApi.Tests
{
    public class ServicesTests
    {
        private VendingMachineService CreateService()
        {
            var repo = new InMemoryVendingMachineRepository();
            var catalog = new InMemoryProductTypeCatalog();
            return new VendingMachineService(repo, catalog);
        }

        [Fact]
        public void CreateMachine_ShouldStoreAndReturnMachine()
        {
            var svc = CreateService();
            var machine = svc.CreateMachine("Machine1", "Room", 5);
            Assert.NotNull(machine);
            Assert.Equal(4, machine.Id.Length);
            Assert.Equal("Machine1", machine.Name);
            Assert.Equal(5, machine.MaxShelves);
            Assert.Equal("Room", machine.Location);
        }

        [Fact]
        public void AddShelf_BeyondMax_ShouldThrow()
        {
            var svc = CreateService();
            var machine = svc.CreateMachine("m", null, 1);
            svc.AddShelf(machine.Id, "drinks", 10);
            var ex = Assert.Throws<DomainException>(() => svc.AddShelf(machine.Id, "snacks", 5));
            Assert.Contains("Maximum", ex.Message);
        }

        [Fact]
        public void LoadInventory_MismatchedType_ShouldThrow()
        {
            var svc = CreateService();
            var m = svc.CreateMachine("m", null, 2);
            var shelf = svc.AddShelf(m.Id, "drinks", 10);
            Assert.Equal(4, shelf.Id.Length);
            var ex = Assert.Throws<DomainException>(() => svc.LoadInventory(m.Id, shelf.Id, 5, "snacks"));
            Assert.Contains("different product type", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void LoadInventory_TooMany_ShouldThrow()
        {
            var svc = CreateService();
            var m = svc.CreateMachine("m", null, 2);
            var shelf = svc.AddShelf(m.Id, "drinks", 10);
            var ex = Assert.Throws<DomainException>(() => svc.LoadInventory(m.Id, shelf.Id, 11, "drinks"));
            Assert.Contains("exceed", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void FullScenario_Works()
        {
            var svc = CreateService();
            // 1 create machine with max 5
            var m = svc.CreateMachine("vm", "loc", 5);
            // 2 add 3 shelves
            var s1 = svc.AddShelf(m.Id, "drinks", 10);
            var s2 = svc.AddShelf(m.Id, "snacks", 20);
            var s3 = svc.AddShelf(m.Id, "drinks", 15);
            Assert.Equal(3, m.Shelves.Count);
            // 3 load 5 bottles water on first
            svc.LoadInventory(m.Id, s1.Id, 5, "drinks");
            Assert.Equal(5, s1.CurrentQuantity);
            // 4 try loading chips on beverage shelf -> failure
            Assert.Throws<DomainException>(() => svc.LoadInventory(m.Id, s1.Id, 1, "chips"));
            // 5 try adding sixth shelf
            svc.AddShelf(m.Id, "cans", 10);
            svc.AddShelf(m.Id, "drinks", 5);
            var ex = Assert.Throws<DomainException>(() => svc.AddShelf(m.Id, "snacks", 1));
            Assert.Contains("Maximum", ex.Message, StringComparison.OrdinalIgnoreCase);
            // 6 load 18 bottles water on shelf that holds 10
            var ex2 = Assert.Throws<DomainException>(() => svc.LoadInventory(m.Id, s1.Id, 18, "drinks"));
            Assert.Contains("exceed", ex2.Message);
        }

        [Fact]
        public void AddShelf_UnsupportedType_ShouldThrow()
        {
            var svc = CreateService();
            var machine = svc.CreateMachine("m", null, 3);

            var ex = Assert.Throws<DomainException>(() => svc.AddShelf(machine.Id, "chips", 5));
            Assert.Contains("supported", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Purchase_EmptyShelf_ShouldThrow()
        {
            var svc = CreateService();
            var m = svc.CreateMachine("m", null, 1);
            var s = svc.AddShelf(m.Id, "drinks", 2);
            var ex = Assert.Throws<DomainException>(() => svc.PurchaseProduct(m.Id, s.Id, "drinks", 1));
            Assert.Contains("Out of stock", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Purchase_DecrementsQuantity()
        {
            var svc = CreateService();
            var m = svc.CreateMachine("m", null, 1);
            var s = svc.AddShelf(m.Id, "drinks", 2);
            svc.LoadInventory(m.Id, s.Id, 2, "drinks");
            svc.PurchaseProduct(m.Id, s.Id, "drinks", 1);
            Assert.Equal(1, s.CurrentQuantity);
        }

        [Fact]
        public void Purchase_InsufficientQuantity_ShouldThrow()
        {
            var svc = CreateService();
            var m = svc.CreateMachine("m", null, 1);
            var s = svc.AddShelf(m.Id, "drinks", 10);
            svc.LoadInventory(m.Id, s.Id, 3, "drinks");

            var ex = Assert.Throws<DomainException>(() => svc.PurchaseProduct(m.Id, s.Id, "drinks", 5));
            Assert.Contains("Out of stock", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}


