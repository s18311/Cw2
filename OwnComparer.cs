using System;
using System.Collections.Generic;
using System.Text;

namespace Cw2
{
    class OwnComparer : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y)
        {
            return StringComparer.InvariantCulture.Equals(
                $"{x.FirstName} {x.LastName} {x.Index}",
                $"{y.FirstName} {y.LastName} {y.Index}");
        }

        public int GetHashCode(Student obj)
        {
            return StringComparer.CurrentCultureIgnoreCase.GetHashCode(
                $"{obj.FirstName} {obj.LastName} {obj.Index}");
        }
    }
}
