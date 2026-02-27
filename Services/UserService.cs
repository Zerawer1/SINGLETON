namespace SingletonIdGenerator.Services;

public class UserService
{
    private readonly IdGenerator _idGenerator;

    public UserService()
    {
        _idGenerator = IdGenerator.GetInstance();
    }

    public string CreateUser(string username)
    {
        var userId = _idGenerator.NextId("USR");
        return $"Создан пользователь: {username} с ID: {userId}";
    }

    public string CreateMultipleUsers(params string[] usernames)
    {
        var results = new List<string>();
        foreach (var username in usernames)
        {
            results.Add(CreateUser(username));
        }
        return string.Join("\n", results);
    }
}
