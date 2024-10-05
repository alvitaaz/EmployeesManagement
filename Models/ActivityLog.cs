using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesManagement.Models
{
    [Table("ActivityLog")]
    public class ActivityLog
    {
        public int LogId { get; set; }          
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime ActionTimes { get; set; }
        public string IPAddress { get; set; }
    }


}
