using Moq;

namespace Tests.Helpers
{
    public class DatabaseFaker
    {
        public static Mock<InMemoryDataContext> DataContextMock { get; set; }
        /// <summary>
        /// Returns a new, empty, in-memory database
        /// </summary>
        public static Mock<InMemoryDataContext> GetFake()
        {
            DataContextMock = new Mock<InMemoryDataContext>();
            DataContextMock.CallBase = true;
            return DataContextMock;
        }
    }
}
