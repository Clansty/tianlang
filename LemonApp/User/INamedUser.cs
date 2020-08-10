namespace Clansty.tianlang
{
    interface INamedUser
    {
        string Name { get; }
        string Class { get; }
        int Enrollment { get; }
        string Grade { get; }
        bool Branch { get; }
        string ToXml(string title = "用户信息");
    }
}
