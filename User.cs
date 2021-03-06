using System;
using System.Collections.Generic;

public class User
{
    public int UserId { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public virtual IList<TodoItem> TodoItems { get; set; }

    public String GetFullName()
    {
        return this.FirstName + " " + this.LastName;
    }
    public override string ToString()
    {
        return "User [id=" + this.UserId + ", name=" + this.GetFullName() + "]";
    }
}