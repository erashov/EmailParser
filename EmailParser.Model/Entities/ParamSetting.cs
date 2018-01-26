using System.ComponentModel.DataAnnotations;

namespace EmailProcessing.DAL.Entities
{
    public class ParamSetting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string FullName { get; set; }
        public int? PramTypeId { get; set; }
        public virtual ParamType PramType { get; set; }
        public int? SettingId { get; set; }
        public virtual Setting Setting { get; set; }

    }
}
