namespace PaymentMicroServices.Dto
{
    public class CompundDto
    {
        public int Id { get; set; }
        public decimal Principal { get; set; }//p
        public double Rate { get; set; }//r
        public int Time { get; set; } // Time in years//t
        public int CompoundingsPerYear { get; set; } // n  Number of times interest is compounded per year
       // public decimal Interest { get; set; } // a  Calculated compound interest
    }
}
