namespace IndeedClone.Modules.Shared.Enums
{
    public enum AccountStatus
    {
     // # User registered but not verified
        INACTIVE = 0,
     // # Verified & active
        ACTIVE = 1,
     // # Blocked due to 3 failed attempts
        TEMPORARY_BLOCK = 3,
     // # Admin-blocked user
        BLOCK = -3,
     // # Soft deleted
        DELETED = -2,        
    }
}
