using Microsoft.EntityFrameworkCore;

public interface IDataSource
{
    void GetCustomers();
}

public class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; }

    public string Mobile { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public Group Group { get; set; }

    public List<Order> Orders { get; set; }
}

public class Group
{
    public int GroupId { get; set; }

    public int CustomerId { get; set; }

    public string GroupName { get; set; }

    public List<Comment> Comments { get; set; }

}

public class Comment
{
    public int CommentId { get; set; }

    public int OrderId { get; set; }

    public string Content { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}

public class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string OrderName { get; set; }

    public int Status { get; set; }

    public string Field1 { get; set; }

    public string Field2 { get; set; }

    public string Field3 { get; set; }

    public string Field4 { get; set; }

    public string Field5 { get; set; }

    public string Field6 { get; set; }
   
    public string Field7 { get; set; }
   
    public string Field8 { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<OrderItem> Items { get; set; }

}

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }
}

[Keyless]
public class RawRecord
{
    public int CustomerId1 { get; set; }
    public string Address2 { get; set; }
    public string Email3 { get; set; }
    public string Mobile4 { get; set; }
    public string Name5 { get; set; }
    public int GroupId6 { get; set; }
    public int CustomerId7 { get; set; }
    public string GroupName8 { get; set; }
    public int CommentId9 { get; set; }
    public string Content10 { get; set; }
    public DateTime CreatedAt11 { get; set; }
    public string CreatedBy12 { get; set; }
    public int GroupId13 { get; set; }
    public int OrderId14 { get; set; }
    public DateTime UpdatedAt15 { get; set; }
    public int OrderId16 { get; set; }
    public DateTime CreatedAt17 { get; set; }
    public int CustomerId18 { get; set; }
    public string Field119 { get; set; }
    public string Field220 { get; set; }
    public string Field321 { get; set; }
    public string Field422 { get; set; }
    public string Field523 { get; set; }
    public string Field624 { get; set; }
    public string Field725 { get; set; }
    public string Field826 { get; set; }
    public string OrderName27 { get; set; }
    public int Status28 { get; set; }
    public DateTime UpdatedAt29 { get; set; }
    public int Id30 { get; set; }
    public DateTime CreatedAt031 { get; set; }
    public string Description32 { get; set; }
    public string Name33 { get; set; }
    public int OrderId034 { get; set; }
    public double Price35 { get; set; }
    public int Status036 { get; set; }
}
