using System.Threading.Tasks;
using AdventCode.Utils;

namespace AdventCode.Tasks
{
    public interface IAdventCodeTask
    {
        int TaskDay { get; }
        Task<string?> GetFirstTaskAnswer();

        Task<string?> GetSecondTaskAnswer();
    }

    public abstract class BaseCodeTask
    {
        private readonly IAdventWebClient _client;
        public BaseCodeTask(IAdventWebClient client)
        {
            _client = client;
        }

        protected async Task<string> GetData(int taskDay)
        {
            var dataInput = await _client.GetDayInput(taskDay);
            if (dataInput == null)
            {
                throw new NoDataException();
            }
            return dataInput;
        }

    }
}