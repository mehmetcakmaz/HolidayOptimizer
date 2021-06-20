using System;

namespace HolidayOptimizer.API.Model.Domain
{
    public class HolidayModel
    {
        public DateTime Date { get; set; }
        public string LocalName { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
    }
}
