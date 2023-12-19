namespace Platinum.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class MatchedColumnAttribute : Attribute
    {
    }
}
