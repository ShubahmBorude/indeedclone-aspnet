namespace IndeedClone.Modules.Shared.Enums
{
    public enum GoogleAuthStatus
    {
        INACTIVE = 0,          // newly created, not active yet
        ACTIVE = 1,            // active
        TEMPORARY_BLOCK = 3,   // temporary block
        BLOCK = -3,            // blocked
        DELETED = -2           // soft deleted
    }
}
