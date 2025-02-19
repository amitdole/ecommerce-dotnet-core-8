namespace EventBus.Messages.Events
{
    public class BaseIntegrationEvent
    {
        public string CorrelationId { get; private set; }
        public DateTime CreationDate { get; private set; }
        public BaseIntegrationEvent()
        {
            CorrelationId = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
        }
        public BaseIntegrationEvent(Guid corelationId, DateTime createDate)
        {
            CorrelationId = corelationId.ToString();
            CreationDate = createDate;
        }
    }
}