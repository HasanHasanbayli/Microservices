namespace FreeCourse.Shared.Messages;

public class CreateOrderMessageCommand
{
    public CreateOrderMessageCommand()
    {
        OrderItem = new List<OrderItem>();
    }
    
    public string BuyerId { get; set; }

    public List<OrderItem> OrderItem { get; set; }

    public string Province { get; set; }

    public string District { get; set; }

    public string Street { get; set; }

    public string ZipCode { get; set; }

    public string Line { get; set; }
}