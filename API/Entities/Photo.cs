using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
  // make the photo Entity to be called photos in the database
  [Table("photos")] 
  public class Photo
  {
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }
    public AppUser AppUser { get; set; }
    public int AppUserId { get; set; }
  }
}