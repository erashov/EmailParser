using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmailProcessing.DAL.Entities
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string InputMail { get; set; }
        public string InputMailPassword { get; set; }
        public string Subject { get; set; }
        public string OutputMail { get; set; }
        public string ServiceUrl { get; set; }
        public string ImapServer { get; set; }
        public short ImapPort { get; set; }
        public string SmptServer { get; set; }
        public short SmptPort { get; set; }
        public string ProcessName { get; set; }
        public string RegexMask { get; set; }
        public int? RequestTypeId { get; set; }
        public virtual RequestType RequestType { get; set; }
        public List<ParamSetting> ParamSettings { get; set; }

    }
}
