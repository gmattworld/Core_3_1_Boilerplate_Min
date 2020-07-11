using APP.Repository.EFRepo.EntitiesExt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APP.Repository.EFRepo.Repositories
{
    public class UsersRepository
    {
        public DbContext Context;

        public UsersRepository(DbContext context)
        {
            Context = context;
        }

        public async Task<IUser> InsertAsync(IUser entity)
        {
            Context.Set<IUser>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<IUser>> InsertRangeAsync(IEnumerable<IUser> entities)
        {
            await Context.Set<IUser>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();
            return entities;
        }

        public IEnumerable<IUser> Find(Expression<Func<IUser, bool>> predicate)
        {
            return Context.Set<IUser>().Where(predicate);
        }

        public async Task<IUser> GetAsync(string id)
        {
            var i = await Context.Set<IUser>().FindAsync(id);
            //if (i != null)
            //{
            //	if (i.RecordStatus == RecordStatus.DELETED || i.RecordStatus == RecordStatus.Archive)
            //	{
            //		i = null;
            //	}
            //}
            return i;
        }

        //public async Task<IEnumerable<User>> GetAllAsync()
        //{
        //	return await Context.Set<User>().Where(x => x.RecordStatus != RecordStatus.DELETED && x.RecordStatus != RecordStatus.ARCHIVE).ToListAsync();
        //}

        //public virtual async Task<List<User>> GetAllActiveAsync()
        //{
        //	return await Context.Set<User>().Where(x => x.RecordStatus == RecordStatus.ACTIVE).ToListAsync();
        //}

        public virtual async Task<IUser> UpdateAsync(IUser obj)
        {
            Context.Entry(obj).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return obj;
        }

        //public virtual async Task<int> DeleteAsync(User obj)
        //{
        //	obj.RecordStatus = RecordStatus.DELETED;
        //	Context.Entry(obj).State = EntityState.Modified;
        //	return await Context.SaveChangesAsync();
        //}

        public virtual async Task<int> DeleteFinallyAsync(IUser obj)
        {
            Context.Set<IUser>().Remove(obj);
            return await Context.SaveChangesAsync();
        }

        //public virtual async Task<int> CountAsync()
        //{
        //	return await Query().CountAsync();
        //}

        //public virtual IQueryable<User> Query()
        //{
        //	return Context.Set<User>().Where(x => x.RecordStatus != RecordStatus.DELETED && x.RecordStatus != RecordStatus.ARCHIVE);
        //}

        public virtual async Task<bool> ExistsAsync(string id)
        {
            return await Context.Set<IUser>().AnyAsync(e => e.Id.Equals(id));
        }





        public bool CheckExist(string Username, string Email, out string errorMsg, int Id = 0)
        {
            errorMsg = string.Empty;

            if (CheckUserName(Username, Id))
            {
                errorMsg = string.Format("Username: {0} exist: ", Username);
                return true;
            }

            if (CheckEmail(Email, Id))
            {
                errorMsg = string.Format("Email: {0} exist: ", Email);
                return true;
            }

            return false;
        }

        private bool CheckUserName(string UserName, int Id = 0)
        {
            if (!Id.Equals(0))
            {
                return Find(ex => ex.UserName.Equals(UserName) && !ex.Id.Equals(Id)).Count() > 0;
            }
            return Find(ex => ex.UserName.Equals(UserName)).Count() > 0;
        }

        private bool CheckEmail(string Email, int Id = 0)
        {
            if (!Id.Equals(0))
            {
                return Find(ex => ex.Email.Equals(Email) && !ex.Id.Equals(Id)).Count() > 0;
            }
            return Find(ex => ex.Email.Equals(Email)).Count() > 0;
        }
    }
}
