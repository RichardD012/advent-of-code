namespace AdventCode.Tasks
{
    public interface IAdventCodeTask
    {
        int TaskDay { get; }
        Task<string?> GetFirstTaskAnswerAsync();

        Task<string?> GetSecondTaskAnswerAsync();
    }

    public abstract class BaseCodeTask : IAdventCodeTask
    {
        private readonly IAdventWebClient _client;
        protected abstract string TestData { get; }
        public abstract int TaskDay { get; }

        public BaseCodeTask(IAdventWebClient client)
        {
            _client = client;
        }

        protected async Task<string> GetDataAsync()
        {
            var dataInput = await _client.GetDayInputAsync(TaskDay);
            if (dataInput == null)
            {
                throw new NoDataException();
            }
            return dataInput;
        }

        protected Task<string> GetTestDataAsync()
        {
            return Task.FromResult(TestData);
        }

        protected Task<List<T>> GetTestDataAsListAsync<T>()
        {
            return Task.FromResult(TestData.ToDataList<T>());
        }

        protected async Task<List<T>> GetDataAsListAsync<T>()
        {
            var dataInput = await _client.GetDayInputListAsync<T>(TaskDay);
            return dataInput;
        }

        public abstract Task<string?> GetFirstTaskAnswerAsync();
        public abstract Task<string?> GetSecondTaskAnswerAsync();
    }
}