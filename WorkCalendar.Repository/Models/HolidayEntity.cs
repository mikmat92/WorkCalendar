using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkCalendar.Repository.Models;

public class HolidayEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTimeOffset HolidayDateTimeOffset { get; set; }
    public bool IsReoccurring { get; set; }
}