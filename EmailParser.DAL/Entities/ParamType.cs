using System.ComponentModel.DataAnnotations;

namespace EmailParser.DAL.Entities
{
    public class ParamType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}