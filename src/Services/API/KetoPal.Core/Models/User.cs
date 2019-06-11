using System;
using System.Collections.Generic;
using System.Linq;

namespace KetoPal.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Preference Preference { get; set; }
        public List<CarbConsumption> CarbConsumption { get; set; }  = new List<CarbConsumption>();
        public double TotalCarbConsumption { get; set; }

        public void RecordConsumption(double carbAmount)
        {
            CarbConsumption.Add(new CarbConsumption()
            {
                Amount = carbAmount,
                ConsumedOn = DateTimeOffset.Now
            });
            TotalCarbConsumption += carbAmount;
        }

        public bool CanConsume(Product product)
        {
            if (Preference != null)
            {
                double consumption = CarbConsumption.Where(x => x.ConsumedOn.Date == DateTimeOffset.Now.Date)
                    .Sum(x => x.Amount);

                double max = Preference.MaxCarbsPerDayInGrams - consumption;

                return product.Carbs <= max;
            }
            return true;
        }
    }
}