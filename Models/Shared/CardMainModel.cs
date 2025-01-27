public class CardMainModel
{
    public int Id { get; set; }
    public string ImgUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsPrimary { get; set; } // oncelikli isaretlenme olayi
    public DateTime ReleaseDate { get; set; }
    public DateTime EndDate { get; set; }
}