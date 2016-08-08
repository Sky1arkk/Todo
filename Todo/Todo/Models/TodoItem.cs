using SQLite;

namespace Todo.Models
{
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool Done { get; set; }

        public TodoItem() { }

        public TodoItem(int priority, string name, string note, bool done)
        {
            Priority = priority;
            Name = name;
            Note = note;
            Done = done;
        }
    }
}