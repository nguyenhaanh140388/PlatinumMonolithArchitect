using Anhny010920.Core.Models;
using System.Collections.Generic;

namespace Anhny010920.Core.Abstractions.Services
{
    public interface IDataShaper<T>
	{
		IEnumerable<Entity> ShapeData(IEnumerable<T> entities, string fieldsString);
		Entity ShapeData(T entity, string fieldsString);
	}
}
