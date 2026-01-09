using System.ComponentModel.DataAnnotations;

namespace ZenticServer.User;

public class Model
{ // TODO как сделать поля обязательными
    public Model(int _UserId, string Username, string Email)
    {
        UserId = _UserId;
        Username = Username;
        Email = Email;
        Password = "1230";
    }
    public Model()
    { }
    public int UserId  { get; set; }
    public string Username  { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}