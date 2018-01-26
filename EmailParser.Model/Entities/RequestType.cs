using System.ComponentModel.DataAnnotations;

namespace EmailProcessing.DAL.Entities
{
    public class RequestType
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string FormatMessage { get; set; }
    }
}