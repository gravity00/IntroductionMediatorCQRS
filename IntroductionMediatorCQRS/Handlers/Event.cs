namespace IntroductionMediatorCQRS.Handlers
{
    public class Event : SimpleSoft.Mediator.Event
    {
        public new string CreatedBy
        {
            get => base.CreatedBy;
            set => base.CreatedBy = value;
        }
    }
}
