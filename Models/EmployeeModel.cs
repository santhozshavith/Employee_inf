namespace Employee_inf.Models
{
    public class EmployeeModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Number { get; set; }
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? Action { get; set; }
        public List<string> Skills { get; set; }
    }
    public class EmployeeResponse
    {
        public List<EmployeeModel> EmployeeId { get; set; } 
        public string Message { get; set; }               
    }
}
