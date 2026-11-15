using OnlineKiosk_Logic.Data;
using OnlineKiosk_Logic.Interfaces;
using OnlineKiosk_UnitTestLogic.Mocks;
using OnlineKiosk_Logic;

namespace OnlineKiosk_UnitTestLogic
{
    public class CartServiceUnitTests
    {
        [Fact]
        public void TC_02_01_AddToCart_ProductNotInCart_AddsProductWithQuantityOne()
        {
            // Arrange
            const int VARIANT_ID = 10;
            var storage = new FakeCartStorage();
            var repository = new FakeCartRepository
            {
                VariantToReturn = new ProductVariant(VARIANT_ID, "Standard", 2.50m, 1)
            };
            var service = new CartService(repository, storage);

            // Act
            service.IncreaseQuantity(VARIANT_ID);

            // Assert
            var item = Assert.Single(service.GetCart().Items);
            Assert.Equal(VARIANT_ID, item.VariantId);
            Assert.Equal(1, item.Quantity);
        }

        [Fact]
        public void TC_02_02_AddToCart_SameProductTwice_StoresOneItemWithQuantityTwo()
        {
            // Arrange
            const int VARIANT_ID = 10;
            var storage = new FakeCartStorage();
            var repository = new FakeCartRepository
            {
                VariantToReturn = new ProductVariant(VARIANT_ID, "Standard", 2.50m, 1)
            };
            var service = new CartService(repository, storage);

            // Act
            service.IncreaseQuantity(VARIANT_ID);
            service.IncreaseQuantity(VARIANT_ID);

            // Assert
            var item = Assert.Single(service.GetCart().Items);
            Assert.Equal(VARIANT_ID, item.VariantId);
            Assert.Equal(2, item.Quantity);
        }

        [Fact]
        public void TC_02_03_AddToCart_ProductAlreadyAtMaximumQuantity_DoesNotExceed99()
        {
            // Arrange
            const int VARIANT_ID = 10;
            var storage = new FakeCartStorage(new CartItem(1, VARIANT_ID, 99, 2.50m));
            var service = CreateService(storage);

            // Act
            service.IncreaseQuantity(VARIANT_ID);

            // Assert
            Assert.Equal(99, service.GetCart().Items.Single().Quantity);
        }

        [Fact]
        public void TC_02_04_AddToCart_TwoDifferentProducts_AddsBothSeparatelyWithQuantityOne()
        {
            // Arrange
            const int VARIANT_ID_1 = 10;
            const int VARIANT_ID_2 = 20;
            var storage = new FakeCartStorage();
            var repository = new FakeCartRepository();
            repository.Variants[VARIANT_ID_1] = new ProductVariant(VARIANT_ID_1, "Standard", 2.50m, 1);
            repository.Variants[VARIANT_ID_2] = new ProductVariant(VARIANT_ID_2, "Large", 4.25m, 2);
            var service = new CartService(repository, storage);

            // Act
            service.IncreaseQuantity(VARIANT_ID_1);
            service.IncreaseQuantity(VARIANT_ID_2);

            // Assert
            Assert.Equal(2, service.GetCart().Items.Count);
            Assert.All(service.GetCart().Items, item => Assert.Equal(1, item.Quantity));
            Assert.Contains(service.GetCart().Items, item => item.VariantId == VARIANT_ID_1);
            Assert.Contains(service.GetCart().Items, item => item.VariantId == VARIANT_ID_2);
            Assert.Contains(service.GetCart().Items, item => item.VariantId == VARIANT_ID_2);
        }

        private static CartService CreateService(FakeCartStorage storage)
        {
            return new CartService(new FakeCartRepository(), storage);
        }
    }
}