namespace ProiectMPP.TeledonProject.domain;

[Serializable]
public class CharityCase : Entity<long>
{
    public string Name { get; set; }
    public double TotalAmount { get; set; }

    public CharityCase() { }

    public CharityCase(string name, double totalAmount)
    {
        Name = name;
        TotalAmount = totalAmount;
    }

    public override string ToString()
    {
        return $"CharityCase{{id={Id}, name='{Name}', totalAmount={TotalAmount}}}";
    }
}