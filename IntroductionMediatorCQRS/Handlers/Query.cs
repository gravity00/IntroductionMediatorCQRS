namespace IntroductionMediatorCQRS.Handlers
{
    public class Query<TResult> : SimpleSoft.Mediator.Query<TResult>
    {
        public new string CreatedBy
        {
            get => base.CreatedBy;
            set => base.CreatedBy = value;
        }
    }
}
