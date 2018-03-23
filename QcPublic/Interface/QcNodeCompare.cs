using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
   public class QcNodeComparer:IEqualityComparer<IQcNode>

    {
        public bool Equals(IQcNode x, IQcNode y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.Code == y.Code ;
        }
        public int GetHashCode(IQcNode product)
        {
            if (Object.ReferenceEquals(product, null)) return 0;
            int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();
            int hashProductCode = product.Code.GetHashCode();
            return hashProductName ^ hashProductCode;
        }
    }
}
