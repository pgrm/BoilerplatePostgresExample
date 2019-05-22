using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReproducePostgresIssue.Entities
{
    public class Queen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int AgeInDays { get; set; }

        public bool HasLifeInsurance { get; set; }

        public int HiveId { get; set; }

        public Hive Hive { get; set; }
    }
}