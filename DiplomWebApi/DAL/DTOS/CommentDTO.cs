namespace DAL.DTOS
{
    public class CommentDTO
    {
        public string Text { get; set; }
        public string DatePosted { get; set; }
        public string PosterName { get; set; }
    }
    public class CommentAddDTO
    {
        public string Text { get; set; }
    }
}
