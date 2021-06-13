public class TodoItem
{
    public int TodoItemId { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public virtual User AssignedTo { get; set; }

    public override string ToString()
    {
        return "TodoItem [id=" + this.TodoItemId + ", title=" + this.Title + ", isdone=" + this.IsDone + "assigned=" + this.AssignedTo + "]";
    }
}