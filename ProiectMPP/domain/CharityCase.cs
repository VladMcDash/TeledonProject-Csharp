namespace ProiectMPP.domain;

using System;

[Serializable]
public class CharityCase {
    public long Id { get; set; }
    public string Name { get; set; }
    public double TotalAmount { get; set; }

    public CharityCase() { }

    public CharityCase(string name, double totalAmount) {
        Name = name;
        TotalAmount = totalAmount;
    }
}