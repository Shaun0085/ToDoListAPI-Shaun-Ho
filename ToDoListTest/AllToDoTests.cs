using Microsoft.Extensions.Logging;
using Moq;
using ToDoListApplication;
using ToDoListDomain;

namespace ToDoListTest
{
    public class AllToDoListTest
    {
        private Mock<IToDoListRepository> _repoMock;
        private IToDoListService _service;
        private ILogger<ToDoListService> _logger;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IToDoListRepository>();
            _service = new ToDoListService(_repoMock.Object, _logger);
        }

        [Test]
        public async Task GetAllItem_Should_Return_All_ToDoItems()
        {
            var dummyData = new List<ToDoListItem>
            {
                new ToDoListItem { ItemId = 1, ItemTitle = "Item A", IsCompleted = false },
                new ToDoListItem { ItemId = 2, ItemTitle = "Item B", IsCompleted = true }
            };
            _repoMock.Setup(r => r.GetAllItem()).ReturnsAsync(dummyData);

            var result = await _service.GetAllItem();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].ItemTitle, Is.EqualTo("Item A"));
        }

        [Test]
        public async Task GetItemById_Should_Return_Single_ToDoItem()
        {
            var dummyItem = new ToDoListItem { ItemId = 5, ItemTitle = "Target Item", IsCompleted = true };
            _repoMock.Setup(r => r.GetItemById(5)).ReturnsAsync(dummyItem);

            var result = await _service.GetItemById(5);

            Assert.IsNotNull(result);
            Assert.That(result.ItemTitle, Is.EqualTo("Target Item"));
        }

        [Test]
        public async Task AddItem_Should_Add_New_ToDoItem()
        {
            var newItemDto = new ToDoListItemDto { ItemTitle = "New Item", IsCompleted = false };
            _repoMock.Setup(r => r.AddItem(It.IsAny<ToDoListItem>()))
                     .Returns((ToDoListItem item) =>
                     {
                         item.ItemId = 10;
                         return Task.FromResult(item);
                     });
            await _service.AddItem(newItemDto);

            _repoMock.Verify(r => r.AddItem(It.Is<ToDoListItem>(i =>
                i.ItemTitle == "New Item" && i.IsCompleted == false)), Times.Once);

            Assert.That(newItemDto.ItemId, Is.EqualTo(10));
            Assert.That(newItemDto.ItemTitle, Is.EqualTo("New Item"));
        }

        [Test]
        public async Task UpdateItem_Should_Updated_Existing_ToDoItem()
        {
            var updateItemDto = new ToDoListItemDto { ItemId = 3, ItemTitle = "Updated Title", IsCompleted = true };
            _repoMock.Setup(r => r.UpdateItem(It.IsAny<ToDoListItem>())).Returns(Task.CompletedTask);
            await _service.UpdateItem(updateItemDto);

            _repoMock.Verify(r => r.UpdateItem(It.Is<ToDoListItem>(i =>
                i.ItemId == 3 && i.ItemTitle == "Updated Title" && i.IsCompleted)), Times.Once);
        }

        [Test]
        public async Task DeleteItem_Should_Delete_Existing_ToDoItem()
        {
            int deleteId = 7;
            _repoMock.Setup(r => r.DeleteItem(deleteId)).Returns(Task.CompletedTask);
            await _service.DeleteItem(deleteId);

            _repoMock.Verify(r => r.DeleteItem(deleteId), Times.Once);
        }
    }
}
