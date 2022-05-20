using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mptr.bookmark
{
    public readonly record struct Bookmark(string Name, string Url, string IconPath);
}
