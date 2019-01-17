using System;

namespace EventCloud.Repository.Entity
{
    public abstract class DateTimeAvailability
    {
    }

    public class SingleDateTimeAvailability : DateTimeAvailability
    {
        internal protected SingleDateTimeAvailability()
        {

        }
    }

    public class RangeDateTimeAvailability : DateTimeAvailability
    {

    }
}