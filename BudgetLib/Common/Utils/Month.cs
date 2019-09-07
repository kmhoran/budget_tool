using System;

namespace Common.Utils
{
    public static class Month 
    {
        public static DateTime GetEffectiveDate()
        {
            /* If today's date is before the 10th of the month, 
             * we want to process last month's budget. This will 
             * give us time to finialize transaction record
             */
            var now = DateTime.Now;
            if(now.Day < 10) return now.AddMonths(-1);
            return now;
        }
    }
}
