using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity
{
    public class FrontsideMenuType : BaseEntityModel
    {
        public int Id { get; set; }

        public int Sort { get; set; }

        public bool IsActive { get; set; }
    }
}