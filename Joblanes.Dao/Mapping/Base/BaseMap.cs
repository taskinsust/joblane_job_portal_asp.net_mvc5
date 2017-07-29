using FluentNHibernate.Mapping;
using Model.JobLanes.Entity.Base;

namespace Dao.Joblanes.Mapping.Base
{
    public class BaseClassMap<TEntityT, TIdT> : ClassMap<TEntityT> where TEntityT : BaseEntity<TIdT>
    {
        public BaseClassMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.BusinessId);
            Version(x => x.VersionNumber);
            Map(x => x.CreationDate);
            Map(x => x.ModificationDate);
            Map(x => x.Status);
            Map(x => x.CreateBy);
            Map(x => x.ModifyBy);
        }
    }
}
