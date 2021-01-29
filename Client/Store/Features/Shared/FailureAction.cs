namespace Tomi.Calendar.Mono.Client.Store.Features.Shared
{
    public abstract record FailureAction
    {
        protected FailureAction(string errorMessage) => ErrorMessage = errorMessage;
        public string ErrorMessage { get; }
    }
}
