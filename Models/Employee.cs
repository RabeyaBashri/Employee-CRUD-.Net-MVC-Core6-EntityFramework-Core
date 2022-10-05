using System;
using System.Collections.Generic;

namespace EmployeeCRUDCoreWebMVCEF.Models
{
    public partial class Employee
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? HiredOn { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public long? ContactNo { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public long DepartmentId { get; set; }
    }
}
