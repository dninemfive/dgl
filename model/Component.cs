using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.dgl;
public enum Component { R = 0, G = 1, B = 2 }
public static class Components
{
    public static IEnumerable<Component> All => Enum.GetValues<Component>();
}