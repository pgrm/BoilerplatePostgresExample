using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReproducePostgresIssue.Entities
{
    public class Hive
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int QueenId { get; set; }

        public Queen Queen { get; set; }

        public List<Ant> Ants { get; set; }
    }
}