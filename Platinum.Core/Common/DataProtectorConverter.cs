using Anhny010920.Core.Abstractions.Services;
using Anhny010920.Core.Attributes;
using AutoMapper;
using System.Reflection;
using System.Linq;

namespace Platinum.Core.Common
{
    public class DataProtectorConverter<T, K> : ITypeConverter<T, K> where K : new()
    {
        private readonly IEncryptionService encryptionService;
        public DataProtectorConverter(IEncryptionService encryptionService)
        {
            this.encryptionService = encryptionService;
        }

        K ITypeConverter<T, K>.Convert(T source, K destination, ResolutionContext context)
        {
            var sourceProps = source?.GetType().GetProperties();

            K dataItem = new K();
            var desProps = dataItem.GetType().GetProperties();
            foreach (var (parentProperty, childProperty) in sourceProps.SelectMany(parentProperty => desProps.Select(childProperty => (parentProperty, childProperty))))
            {
                var dataProtectedAttribute = childProperty.GetCustomAttribute(typeof(DataProtectedAttribute), true);
                var matchParentAttribute = (MatchParentAttribute)childProperty.GetCustomAttribute(typeof(MatchParentAttribute), true);
                if (matchParentAttribute != null)
                {
                    if (parentProperty.Name == childProperty.Name || matchParentAttribute.ParentPropertyName == parentProperty.Name)
                    {
                        var data = parentProperty.GetValue(source);

                        data = dataProtectedAttribute == null ? data : encryptionService.Encrypt(data.ToString());
                        childProperty.SetValue(dataItem, data);
                        break;
                    }
                }
            }

            return dataItem;
        }
    }
}
