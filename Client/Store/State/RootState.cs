
namespace Tomi.Calendar.Mono.Client.Store.State
{
    public abstract record RootState
    {
        public bool IsLoading { get; init; }

        public string? CurrentErrorMessage { get; init; }

        public bool HasCurrentErrors => !string.IsNullOrWhiteSpace(CurrentErrorMessage);
    }
}
