using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReproducePostgresIssue.Entities
{
    public class Ant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int AgeInDays { get; set; }

        public string FavouriteAntGame { get; set; }

        public int? HiveId { get; set; }

        public Hive Hive { get; set; }

        public bool IsLoyal { get; set; }

        public string Job { get; set; }
    }
}