namespace HCProject9.Models
{
    public class SalesListViewModel
    {
        public List<Sales> Sales { get; set; }
        public List<Employee> Employees { get; set; }
        public int EmployeeId { get; set; }
        public double? TotalAmount { get; set; }
    }
}
