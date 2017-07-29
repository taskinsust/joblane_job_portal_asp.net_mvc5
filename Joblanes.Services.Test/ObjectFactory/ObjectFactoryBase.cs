using Model.JobLanes.Entity.Base;
using NHibernate;
using Services.Joblanes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Joblanes.Services.Test.ObjectFactory
{
    public class ObjectFactoryBase<EntityT> : IObjectFactoryBase
    {
        internal ISession _session;
        public int Level { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int ListStartIndex { get; set; }
        public EntityT Object { get; set; }
        public List<EntityT> ObjectList { get; set; }
        public List<EntityT> SingleObjectList { get; set; }
        IBaseService _baseService;

        private ObjectFactoryBase()
        {
            ObjectList = new List<EntityT>();
            SingleObjectList = new List<EntityT>();
        }
        internal ObjectFactoryBase(IObjectFactoryBase caller, ISession session)
        {
            _session = session;
            Name = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString();
            ShortName = "ShortName_" + Guid.NewGuid().ToString();
            Level = FactoryLevels.GetLevel(this.GetType());
            CheckLevel(this, caller);
            ObjectList = new List<EntityT>();
            SingleObjectList = new List<EntityT>();
            _baseService = new BaseService();
        }

        /// <summary>
        /// This method will prevent declearing chield object factory from parent object factory. Call this method from factory constructor.
        /// </summary>
        /// <param name="thisFactory">Which factory it is?</param>
        /// <param name="caller">Who is instantiating this factory</param>
        public void CheckLevel(IObjectFactoryBase thisFactory, IObjectFactoryBase caller)
        {
            if (caller != null)
            {
                if (caller.Level <= thisFactory.Level)
                {
                    throw new Exception(string.Format("{0} can not create {1} object. Check factory level.", caller.GetType().Name, thisFactory.GetType().Name));
                }
            }
        }

        public List<EntityT> GetLastCreatedObjectList()
        {
            var list = ObjectList.Skip(ListStartIndex).ToList();
            return list;
        }

        public void DeleteAll()
        {
            DeleteObjects(ObjectList);
            DeleteObjects(SingleObjectList);
        }

        public void DeleteObject(EntityT entitie)
        {
            if (entitie != null)
            {
                var tableName = _baseService.GetEntityTableName(entitie.GetType());
                var entity = (IBaseEntity<long>)entitie;
                var qText = "DELETE FROM " + tableName + " WHERE Id = " + entity.Id;
                var query = _session.CreateSQLQuery(qText);
                var effRow = query.ExecuteUpdate();
                SingleObjectList.Remove(entitie);
            }
            entitie = default(EntityT);
        }

        public void DeleteObjects(List<EntityT> entities)
        {
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    var tableName = _baseService.GetEntityTableName(item.GetType());
                    var entity = (IBaseEntity<long>)item;
                    var qText = "DELETE FROM " + tableName + " WHERE Id = " + entity.Id;
                    var query = _session.CreateSQLQuery(qText);
                    var effRow = query.ExecuteUpdate();
                }
                entities.Clear();
                entities = new List<EntityT>();
            }
        }

        public string GetEntityTableName(Type entitie)
        {
            return _baseService.GetEntityTableName(entitie);
        }

        public string GetPcUserName()
        {
            return WindowsIdentity.GetCurrent().Name;
        }
    }
}
