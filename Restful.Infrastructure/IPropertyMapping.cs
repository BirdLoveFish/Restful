﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Infrastructure
{
    public interface IPropertyMapping
    {
        Dictionary<string,List<MappedProperty>> MappingDictionary { get; }
    }
}
