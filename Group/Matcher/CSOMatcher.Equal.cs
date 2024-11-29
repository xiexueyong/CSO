using System;
using CSOEngine.Object;

namespace CSOEngine.Group
{
    
    using System;
using System.Collections.Generic;
using System.Linq;

public partial class CSOMatcher
{
    public override bool Equals(object obj)
    {
        if (object.ReferenceEquals(this, obj))
            return true;

        if (obj == null || this.GetType() != obj.GetType())
            return false;

        CSOMatcher other = obj as CSOMatcher;

        return HashSetEquals(this.allof, other.allof) &&
               HashSetEquals(this.anyof, other.anyof) &&
               HashSetEquals(this.noneof, other.noneof) &&
               FilterEquals(this.filter , other.filter) &&
               this.onlySon == other.onlySon;
    }

    public override int GetHashCode()
    {
        // 初始化 hashCode，使用一个非零的常数
        int hashCode = 17;

        // 合并集合和 filter 的哈希码
        hashCode = hashCode * 31 + GetHashSetHashCode(allof);
        hashCode = hashCode * 31 + GetHashSetHashCode(anyof);
        hashCode = hashCode * 31 + GetHashSetHashCode(noneof);
        hashCode = hashCode * 31 + (filter == null ? 0 : filter.GetHashCode());

        return hashCode;
    }

    private bool HashSetEquals<T>(HashSet<T> set1, HashSet<T> set2)
    {
        if (set1 == null && set2 == null) return true;
        if (set1 == null || set2 == null) return false;
        if (set1.Count != set2.Count) return false;
        return set1.SetEquals(set2);
    }
    private bool FilterEquals(Func<CObject,bool> filter1, Func<CObject,bool> filter2)
    {
        if (filter1 == null && filter2 == null) return true;
        if (filter1 == null || filter2 == null) return false;
        return filter1 == filter2;
    }
    
    private int GetHashSetHashCode<T>(HashSet<T> set)
    {
        if (set == null) return 0;
        int hashCode = 0;
        foreach (var item in set)
        {
            hashCode = hashCode ^ (item?.GetHashCode() ?? 0);
        }
        return hashCode;
    }
}

}