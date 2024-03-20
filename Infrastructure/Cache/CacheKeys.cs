namespace Infrastructure.Cache;

public static class CacheKeys
{
    public const string ClearDb = "FLUSHDB";
    public static class Transport
    {
        public const string Types = "transport-types";
        public const string Makes = "transport-makes";
        public const string BodyTypes = "transport-body-types";
        public const string Models = "transport-models";
    }
    public static class Moderator
    {
        public const string OverviewStatuses = "moderator-overview-statuses";
    }
}