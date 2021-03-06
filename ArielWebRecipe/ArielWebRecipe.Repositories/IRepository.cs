﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArielWebRecipe.Repositories
{
    public interface IRepository<T>
    {
        T Add(T item);

        T Update(int id, T item);

        void Delete(int id);

        void Delete(T item);

        T Get(int id);

        IQueryable<T> All();
    }
}
