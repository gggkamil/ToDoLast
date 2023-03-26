namespace ToDoLast.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string JobQuestion { get; set; }
        public string JobAnswer { get; set; }
        public DateTime Date { get { return DateTime.Now; } }
    }
}
