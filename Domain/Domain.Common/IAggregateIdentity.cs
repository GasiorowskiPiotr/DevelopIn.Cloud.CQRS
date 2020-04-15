namespace DevelopIn.Cloud.CQRS.Domain.Common
{
    public interface IAggregateIdentity
    {
        string GetStringValue();
    }
}