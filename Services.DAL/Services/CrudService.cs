﻿using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Services
{
    public class CrudService<T> : IService<T> where T : Entity
    {
        public int Create(T entity)
        {
            using (var context = new Context())
            {
                var dbEntity = context.Set<T>().Add(entity);
                context.SaveChanges();
                return dbEntity.Id;
            }
        }

        public bool Delete(int id)
        {
            using (var context = new Context())
            {
                var entity = context.Set<T>().Find(id);
                if (entity == null)
                    return false;

                context.Set<T>().Remove(entity);
                context.SaveChanges();
                return true;
            }
        }

        public IEnumerable<T> Read()
        {
            using (var context = new Context())
            {
                return context.Set<T>().ToList();
            }
        }

        public T Read(int id)
        {
            using (var context = new Context())
            {
                return context.Set<T>().Find(id);
            }
        }

        public bool Update(int id, T entity)
        {
            using (var context = new Context())
            {
                /*var dbEntity = context.Set<T>().Find(id);
                if (dbEntity == null)
                    return false;

                dbEntity.FirstName = entity.FirstName;*/
                entity.Id = id;
                context.Set<T>().Attach(entity);
                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                context.SaveChanges();
                return true;
            }
        }
    }
}
