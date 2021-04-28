﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
namespace CollectionsX
{
    public interface ICollectionsObj<T>
    {
        void Set(T obj);
        bool SameValue(T val);
        T Get();

    }
}
