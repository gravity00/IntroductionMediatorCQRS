namespace IntroductionMediatorCQRS.Handlers
{
    public class Command : SimpleSoft.Mediator.Command
    {
        public new string CreatedBy
        {
            get => base.CreatedBy;
            set => base.CreatedBy = value;
        }
    }

    public class Command<TResult> : SimpleSoft.Mediator.Command<TResult>
    {
        public new string CreatedBy
        {
            get => base.CreatedBy;
            set => base.CreatedBy = value;
        }
    }
}
