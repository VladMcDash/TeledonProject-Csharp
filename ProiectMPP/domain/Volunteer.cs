namespace ProiectMPP.domain;
using System;

[Serializable]
public class Volunteer {
    public long Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }

    public Volunteer() { }

    public Volunteer(string username, string password, string name) {
        Username = username;
        Password = password;
        Name = name;
    }
}