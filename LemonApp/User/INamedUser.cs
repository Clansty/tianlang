namespace Clansty.tianlang
{
    interface INamedUser
    {
        string Name { get; }
        int Class { get; }
        int Enrollment { get; }
        string Grade { get; }
        bool Branch { get; }
        string ToXml(string title = "用户信息");
    }
}
