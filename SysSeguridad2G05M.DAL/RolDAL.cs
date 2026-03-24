using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SysSeguridad2G05M.EN;

namespace SysSeguridad2G05M.DAL
{
    public class RolDAL
    {
        public static async Task<int> CrearAsync(Rol pRol)
        { 
            int result = 0;
            using (var dbContexto = new DBContexto())
            { 
                dbContexto.Add(pRol);
                result = await dbContexto.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> ModificarAsync(Rol pRol)
        {
            int result = 0;
            using (var dbContexto = new DBContexto())
            {
                var rol = await dbContexto.Rol.FirstOrDefaultAsync(s => s.Id == pRol.Id);//Select 
                rol.Nombre = pRol.Nombre;
                dbContexto.Update(rol);
                result = await dbContexto.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> EliminarAsync(Rol pRol)
        {
            int result = 0;
            using (var dbContexto = new DBContexto())
            {
                var rol = await dbContexto.Rol.FirstOrDefaultAsync(x => x.Id == pRol.Id);
                dbContexto.Rol.Remove(rol);
                result = await dbContexto.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<Rol> ObtenerPorId(Rol pRol)
        {
            Rol rol = new Rol();
            using (var dbContexto = new DBContexto())
            {
                //Select Id, Nombre From Rol Where Id = 1; 
                rol = await dbContexto.Rol.FirstOrDefaultAsync(s => s.Id == pRol.Id);
            }
            return rol;
        }

        public static async Task<List<Rol>> ObtenerTodosAsync()
        { 
            List<Rol> roles = new List<Rol>();
            using (var dbContexto = new DBContexto())
            { 
                roles = await dbContexto.Rol.ToListAsync();
            }
            return roles;
        }

        internal static IQueryable<Rol> QuerySelect(IQueryable<Rol> pQuery, Rol pRol)
        {
            if (pRol.Id > 0)
                pQuery = pQuery.Where(s => s.Id == pRol.Id);
            if (!string.IsNullOrWhiteSpace(pRol.Nombre))
                pQuery = pQuery.Where(s => s.Nombre.Contains(pRol.Nombre));//like
            pQuery= pQuery.OrderByDescending(s => s.Id).AsQueryable();
            if(pRol.Top_Aux > 0)
                pQuery = pQuery.Take(pRol.Top_Aux).AsQueryable();
            return pQuery;
        }

        /// <summary>
        /// Victor Duran
        /// 18/03/2026
        /// Este metodo se ocupa para hacer las busqueda de uno o varios roles
        /// por medio de condiciones
        /// </summary>
        /// <param name="pRol">Objeto con los datos a buscar</param>
        /// <returns></returns>
        public static async Task<List<Rol>> BuscarAsync(Rol pRol)
        { 
            var roles = new List<Rol>();
            using (var dbContexto = new DBContexto())
            {
                var select = dbContexto.Rol.AsQueryable();
                select = QuerySelect(select, pRol);
                roles = await select.ToListAsync();
            }
            return roles;
        }
    }
}
